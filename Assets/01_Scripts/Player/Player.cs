using UnityEngine;

/// <summary>
/// 기본 플레이어 부모 클래스
/// </summary>
public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 이동 관련 컴포넌트
    /// </summary>
    [SerializeField] private PlayerMove _playerMove;
    public PlayerMove PlayerMove => _playerMove;

    /// <summary>
    /// 플레이어 텔레포트나 강제이동을 위한 컴포넌트
    /// </summary>
    [SerializeField] private PlayerTeleport _playerTeleport;
    public PlayerTeleport PlayerTeleport => _playerTeleport;

    /// <summary>
    /// 컴포넌트 체크 메서드
    /// </summary>
    public void CheckComponent()
    {
				if (_playerMove == null)
				{
						_playerMove = GetComponent<PlayerMove>();
				}

        if (_playerTeleport == null)
        {
            _playerTeleport = GetComponent<PlayerTeleport>();
        }
		}


    /// <summary>
    /// 초기화용 메서드
    /// </summary>
    public void Initialize()
    {
        CheckComponent();

        //이동 초기화
        _playerMove.Initialize();

        //텔레포트 초기화
        _playerTeleport.Initialize();



    }

}
