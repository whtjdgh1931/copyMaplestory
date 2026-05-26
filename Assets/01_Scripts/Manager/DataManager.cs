using GoogleSheetsToUnity;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 데이터 관리를 위한 매니저
/// </summary>
public class DataManager : MonoBehaviour
{
    [SerializeField] private string _sheetID;
    [SerializeField] private string _sheetName;

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
