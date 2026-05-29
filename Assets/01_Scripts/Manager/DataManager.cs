using GoogleSheetsToUnity;
using UnityEngine;
using System.Collections.Generic;
using System;

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
    private string[] _targetSheet;

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
        _sheetMapping = new Dictionary<string, Action<GstuSpreadSheet>>
        {
            
        };
    }

    /// <summary>
    /// Data를 읽기 위한 메서드
    /// </summary>
    public void ReadData()
    {
        SpreadsheetManager.Read(new GSTU_Search(_sheetID, _sheetName), PrintData);

    }

    private void PrintData(GstuSpreadSheet gstuSpreadSheet)
    {
        Debug.Log(gstuSpreadSheet["1", "Name"].value);
        List<GSTU_Cell> nameList = new List<GSTU_Cell>();
        nameList = gstuSpreadSheet.columns["Name"];

        foreach(var item in nameList)
        {
            Debug.Log(item.value);
        }
    }



}
