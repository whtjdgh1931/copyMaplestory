using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


/// <summary>
/// 키보드 마우스 입력 관련 매니저
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput _inputManager;
    [SerializeField] private Player _player;

    /// <summary>
    /// 벡터 체크를 위한 플롯값
    /// </summary>
		[SerializeField] private float _checkfloat;

		/// <summary>
		///  초기화용 메서드
		/// </summary>
		public void Initialize()
    {
        //인풋매니저가 비어있다면 설정
        if(_inputManager == null)
        {
            _inputManager = GetComponent<PlayerInput>();
        }
        _player = GameManager.Instance.Player;
    }

    public void OnMove(InputValue inputValue)
    {
				Vector2 input = inputValue.Get<Vector2>();
				Vector2 currentMove = _player.PlayerMove.MoveVector;

				if (Mathf.Abs(input.x) > _checkfloat)
				{
						//좌우 이동 
						currentMove.x = input.x > 0 ? 1f : -1f;
				}
				else
				{
						//입력 없으면 정지
						currentMove.x = 0f;
				}

				_player.PlayerMove.MoveVector = currentMove;
		}

    public void OnUpDown(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();

        if(Mathf.Abs(input.y) > _checkfloat)
        {
            _player.PlayerMove.VerticalDirection = input.y > 0 ?  1f : -1f;
        }
        else
        {
						_player.PlayerMove.VerticalDirection = 0f;
				}
    }

    public void OnJump()
    {
        Vector2 dirVector = _player.PlayerMove.MoveVector;
        Vector2Int dirVectorInt = Vector2Int.zero;
        if(dirVector.x != 0f)
        {
						dirVectorInt.x = dirVector.x > 0f ? 1 : -1;
        }
        
        if(dirVector.y != 0f)
        {
            dirVectorInt.y = dirVector.y > 0f ? 1 : -1;
        }
        
        _player.PlayerTeleport.Teleport(dirVectorInt,10f,45f,2f);
    }

}
