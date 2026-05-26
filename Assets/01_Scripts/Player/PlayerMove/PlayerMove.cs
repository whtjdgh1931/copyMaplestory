using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 리지드바디
    /// </summary>
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb=>_rb;

    /// <summary>
    /// 이동 방향 벡터
    /// </summary>
    [SerializeField] private Vector2 _moveVector;
    public Vector2 MoveVector { get { return _moveVector; } set { _moveVector = value;  }  }

    [SerializeField] private float _verticalDirection;
    public float VerticalDirection { get { return _verticalDirection; } set { _verticalDirection = value; } }

    /// <summary>
    /// 이동 전 위치
    /// </summary>
    [SerializeField] private Vector2 _currentPosition;

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField] private float _moveSpeed;

    /// <summary>
    /// 점프 높이
    /// </summary>
    [SerializeField] private float _jumpPower;

    /// <summary>
    /// 최대 점프 회수
    /// </summary>
    [SerializeField] private int _jumpCount;

    /// <summary>
    /// 초기화용 메서드
    /// </summary>
    public void Initialize()
    {
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        _currentPosition = _rb.position;
    }

		public void Update()
    {

        Move(_moveVector);
		}


		/// <summary>
		/// 이동 관련 매서드
		/// </summary>
		/// <param name="moveVector"></param>
		public void Move(Vector2 moveVector)
    {
        _currentPosition.x += moveVector.normalized.x * _moveSpeed * Time.deltaTime;
        
				_rb.MovePosition(_currentPosition);
    }


    /// <summary>
    /// 텔레포트나 강제 이동 후 위치 동기화
    /// </summary>
    /// <param name="syncVector"></param>
    public void SyncPosition(Vector2 syncVector)
    {
        _currentPosition = syncVector;
    }

}
