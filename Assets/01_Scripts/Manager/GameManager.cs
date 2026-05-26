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
            
return _instance;
        }


    }

    [SerializeField]
		/// <summary>
		/// 맵 관리 매니저
		/// </summary>
		private MapManager _mapManager;
    public MapManager MapManager => _mapManager;

    [SerializeField]
    /// <summary>
    /// 입력 관련 매니저
    /// </summary>
    private InputManager _inputManager;
    public InputManager InputManager=>_inputManager;

		[SerializeField]
		/// <summary>
		/// 플레이어
		/// </summary>
		private Player _player;
    public Player Player => _player;



    /// <summary>
    /// 초기화 여부
    /// </summary>
    private bool _isInit = false;



		public void Awake()
		{
				if(_isInit)
        {
            return;
        }
        ManagerCheck();
        Initialize();
		}

    /// <summary>
    /// 비어있는 매니저가 있는지 검사
    /// </summary>
    public void ManagerCheck()
    {
        //맵 매니저 검사
        if(_mapManager == null)
        {
            _mapManager = GetComponent<MapManager>();
            if(_mapManager == null)
            {
                _mapManager = _instance.gameObject.AddComponent<MapManager>();
            }
        }

        //인풋 매니저 검사
        if(_inputManager == null)
        {
           _inputManager = GetComponent<InputManager>();
            if(_inputManager == null)
            {
                _inputManager = _instance.gameObject.AddComponent<InputManager>();
            }
        }

        //플레이어 검사
        if(_player == null)
        {
            _player = FindAnyObjectByType<Player>();
        }
    }


		/// <summary>
		/// 초기화용 매서드
		/// </summary>
		public void Initialize()
    {
        // 이미 초기화 되었다면 리턴
        if(_isInit)
        {
            return;
        }

        //맵 매니저 초기화
        _mapManager.Initialize();


        //인풋 매니저 초기화
        _inputManager.Initialize();


        //플레이어 초기화
        _player.Initialize();

        _isInit = true;
    }
}
