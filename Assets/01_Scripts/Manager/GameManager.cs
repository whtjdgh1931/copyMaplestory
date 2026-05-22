using UnityEngine;

/// <summary>
/// 전체 매니저 및 플레이어 관리
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
if(_instance == null)
            {
                _instance = FindAnyObjectByType<GameManager>();
                if(_instance == null)
                {
                    GameObject gameManagerObject = new GameObject();
                    _instance = gameManagerObject.AddComponent<GameManager>();
                }
            }
            _instance._isInit();
return _instance;
        }


    }



    /// <summary>
    /// 초기화 여부
    /// </summary>
    private bool _isInit = false;


   

    /// <summary>
    /// 초기화용 매서드
    /// </summary>
public void Init()
    {

    }
}
