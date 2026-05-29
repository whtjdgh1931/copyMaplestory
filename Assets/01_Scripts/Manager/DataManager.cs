using GoogleSheetsToUnity;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// 데이터 관리를 위한 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [Header("Spreadsheets Setting")]
    [SerializeField] private string _sheetID;

    /// <summary>
    /// 시트 이름과 시트 처리를 연결하는 딕셔너리
    /// </summary>
    private Dictionary<string, Action<GstuSpreadSheet>> _sheetMapping;

    /// <summary>
    /// sheet key값 모음
    /// </summary>
    private string[] _targetSheets;

    /// <summary>
    /// 플레이어 데이터
    /// </summary>
    public PlayerMoveData CachedPlayerData { get; private set; } = new PlayerMoveData();

    /// <summary>
    /// 몬스터 데이터 테이블
    /// </summary>
    public Dictionary<int, MonsterData> MonsterTable { get; private set; } = new Dictionary<int, MonsterData>();

    /// <summary>
    /// 아이템 데이터 테이블
    /// </summary>
    public Dictionary<int, ItemData> ItemTable { get; private set; } = new Dictionary<int, ItemData>();

    private int _currentLoadCount = 0;

    /// <summary>
    /// 모든 데이터 로드 시 발행할 이벤트
    /// </summary>
    public event Action OnAllDataLoaded;

    /// <summary>
    /// 초기화
    /// </summary>
    public void Initialize()
    {
        _sheetMapping = new Dictionary<string, Action<GstuSpreadSheet>>();

        //자동 매핑
        var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        foreach (var method in methods)
        {
            var attr = method.GetCustomAttribute<SheetTargetAttribute>();
            if (attr != null)
            {
                var action = (Action<GstuSpreadSheet>)Delegate.CreateDelegate(typeof(Action<GstuSpreadSheet>), this, method);
                _sheetMapping.Add(attr.SheetName, action);
            }
        }

        _targetSheets = _sheetMapping.Keys.ToArray();
        ReadAllData();

    }

    /// <summary>
    /// Data를 읽기 위한 메서드
    /// </summary>
    public void ReadAllData()
    {
        _currentLoadCount = 0;
        MonsterTable.Clear();
        ItemTable.Clear();

        foreach (string sheetName in _targetSheets)
        {
            SpreadsheetManager.Read(new GSTU_Search(_sheetID, sheetName), (ss) =>
            {
                if (_sheetMapping.TryGetValue(sheetName, out var processAction))
                {
                    processAction.Invoke(ss);
                }
                CheckLoadComplete();
            });
        }
    }

    private void CheckLoadComplete()
    {
        _currentLoadCount++;
        if (_currentLoadCount >= _targetSheets.Length)
        {
            Debug.Log("DataManager : 파싱 완료");
            OnAllDataLoaded?.Invoke();
        }
    }


    /// <summary>
    /// 자동 파싱
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="targetObject"></param>
    /// <param name="ss"></param>
    /// <param name="rowId"></param>
    private void AutoParseRowData<T>(T targetObject, GstuSpreadSheet ss, string rowId)
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        foreach (var field in fields)
        {
            var attr = field.GetCustomAttribute<SheetColumnAttribute>();
            if (attr == null)
            {
                continue;
            }

            string cellValue = ss[rowId, attr.ColumnName]?.value;
            if (string.IsNullOrEmpty(cellValue))
            {
                continue;
            }

            if (field.FieldType == typeof(float))
            {
                field.SetValue(targetObject, ParseFloatSafe(cellValue));
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(targetObject, ParseIntSafe(cellValue));
            }
            else if (field.FieldType == typeof(string))
            {
                field.SetValue(targetObject, cellValue);
            }
        }
    }

    #region ParseData
    [SheetTarget(Constants.PlayerSheet)]
    private void ParsePlayerData(GstuSpreadSheet ss)
    => AutoParseRowData(CachedPlayerData, ss, Constants.FistRow);

    [SheetTarget(Constants.MonsterSheet)]
    private void ParseMonsterData(GstuSpreadSheet ss)
    {
        if(!ss.columns.ContainsKey(Constants.ID))
        {
            return;
        }
        foreach (var cell in ss.columns[Constants.ID])
        {
            if(cell.value == Constants.ID || string.IsNullOrEmpty(cell.value))
            {
                continue;
            }

            MonsterData monsterData = new MonsterData();
            AutoParseRowData(monsterData, ss, cell.rowId);
            if(!MonsterTable.ContainsKey(monsterData.id))
            {
                MonsterTable.Add(monsterData.id, monsterData);
            }
        }
    }

    [SheetTarget(Constants.ItemSheet)]
    private void ParseItemData(GstuSpreadSheet ss)
    {
        if(!ss.columns.ContainsKey(Constants.ID))
        {
            return;

        }
        foreach(var cell in ss.columns[Constants.ID])
        {
            if(cell.value == Constants.ID || string.IsNullOrEmpty(cell.value))
            {
                continue;
            }

            ItemData itemData = new ItemData();
            AutoParseRowData(itemData,ss,cell.rowId);
            if(!ItemTable.ContainsKey(itemData.id))
            {
                ItemTable.Add(itemData.id, itemData);
            }
        }
    }
    #endregion

#region ParseType
    public MonsterData GetMonsterData(int monsterId) => MonsterTable.TryGetValue(monsterId, out MonsterData data) ? data : null;
    public ItemData GetItemData(int itemId) => ItemTable.TryGetValue(itemId, out ItemData data) ? data : null;
    private float ParseFloatSafe(string value, float defaultValue = 0f) => float.TryParse(value, out float result) ? result : defaultValue;
    private int ParseIntSafe(string value, int defalutValue = 0) => int.TryParse(value, out int result) ? result : defalutValue;
    #endregion
}
