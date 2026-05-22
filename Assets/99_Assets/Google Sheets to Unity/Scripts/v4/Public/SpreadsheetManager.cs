using System.Collections;
using System.Text;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using TinyJSON;
using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest를 사용하기 위해 추가

public delegate void OnSpreedSheetLoaded(GstuSpreadSheet sheet);

namespace GoogleSheetsToUnity
{
    /// <summary>
    /// Partial class for the spreadsheet manager to handle all Public functions
    /// </summary>
    public partial class SpreadsheetManager
    {
        static GoogleSheetsToUnityConfig _config;
        /// <summary>
        /// Reference to the config for access to the auth details
        /// </summary>
        public static GoogleSheetsToUnityConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = (GoogleSheetsToUnityConfig)Resources.Load("GSTU_Config");
                }

                return _config;
            }
            set { _config = value; }
        }

        /// <summary>
        /// Read a public accessable spreadsheet
        /// </summary>
        /// <param name="searchDetails"></param>
        /// <param name="callback">event that will fire after reading is complete</param>
        public static void ReadPublicSpreadsheet(GSTU_Search searchDetails, OnSpreedSheetLoaded callback)
        {
            if (string.IsNullOrEmpty(Config.API_Key))
            {
                Debug.Log("Missing API Key, please enter this in the confie settings");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
            sb.Append("/" + searchDetails.sheetId);
            sb.Append("/values");
            sb.Append("/" + searchDetails.worksheetName + "!" + searchDetails.startCell + ":" + searchDetails.endCell);
            sb.Append("?key=" + Config.API_Key);

            // 수정: WWW 객체 대신 sb.ToString()으로 URL 문자열 자체를 넘겨줍니다.
            if (Application.isPlaying)
            {
                new Task(Read(sb.ToString(), searchDetails.titleColumn, searchDetails.titleRow, callback));
            }
#if UNITY_EDITOR
            else
            {
                EditorCoroutineRunner.StartCoroutine(Read(sb.ToString(), searchDetails.titleColumn, searchDetails.titleRow, callback));
            }
#endif
        }

        /// <summary>
        /// Wait for the Web request to complete and then process the results
        /// </summary>
        /// <param name="url">The URL to request</param>
        /// <param name="titleColumn"></param>
        /// <param name="titleRow"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        // 수정: WWW 대신 string url을 받도록 변경
        static IEnumerator Read(string url, string titleColumn, int titleRow, OnSpreedSheetLoaded callback)
        {
            // 수정: UnityWebRequest 사용 및 using 문으로 메모리 누수 방지
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // 서버 응답 대기
                yield return request.SendWebRequest();

                // 네트워크 오류 또는 HTTP 오류 처리
                if (request.result == UnityWebRequest.Result.ConnectionError ||
                    request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Spreadsheet Load Error: " + request.error);

                    if (callback != null)
                    {
                        callback(null); // 에러 발생 시 null 반환
                    }
                    yield break;
                }

                // 성공 시 JSON 파싱
                ValueRange rawData = JSON.Load(request.downloadHandler.text).Make<ValueRange>();
                GSTU_SpreadsheetResponce responce = new GSTU_SpreadsheetResponce(rawData);

                GstuSpreadSheet spreadSheet = new GstuSpreadSheet(responce, titleColumn, titleRow);

                if (callback != null)
                {
                    callback(spreadSheet);
                }
            }
        }
    }
}