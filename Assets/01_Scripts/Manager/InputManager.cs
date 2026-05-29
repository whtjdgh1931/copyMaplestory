using System;
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
    /// 이동 관련 인풋 발생 시 발행
    /// </summary>
    public event Action<Vector2> OnMoveInput;

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
		_player.Move(input);
				
				
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
		#region teleport
		Vector2 dirVector = _player.PlayerMove.MoveVector;
		Vector2Int dirVectorInt = Vector2Int.zero;
		if (dirVector.x != 0f)
		{
			dirVectorInt.x = dirVector.x > 0f ? 1 : -1;
		}

		if (dirVector.y != 0f)
		{
			dirVectorInt.y = dirVector.y > 0f ? 1 : -1;
		}

		_player.PlayerTeleport.Teleport(dirVectorInt, 10f, 45f, 2f);
		#endregion
		//_player.PlayerMove.Jump();

		}

}
