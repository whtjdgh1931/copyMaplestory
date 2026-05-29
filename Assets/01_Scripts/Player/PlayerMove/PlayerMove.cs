using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    /// <summary>
    /// 리지드바디
    /// </summary>
    [SerializeField] private Rigidbody2D _rb;
    public Rigidbody2D Rb=>_rb;

    /// <summary>
    /// 플레이어 콜라이더
    /// </summary>
    [SerializeField] private Collider2D _playerCollider;
    public Collider2D PlayerCollider => _playerCollider;

		#region move
		[Header("Move Setting")]

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
    /// 바라보는 방향(오른쪽을 바라보는지) 1f면 오른쪽, -1f면 왼쪽
    /// </summary>
    [SerializeField] private float _isRight;

    /// <summary>
    /// 이동속도
    /// </summary>
    [SerializeField] private float _moveSpeed;
    #endregion

    #region jump
    [Header("Jump Setting")]


    /// <summary>
    /// 최대 점프 회수
    /// </summary>
    [SerializeField] private int _jumpCnt;

		/// <summary>
		/// 현재 점프 카운트
		/// </summary>
		[SerializeField] private int _currentJumpCnt;

		/// <summary>
		/// 일반 점프 높이
		/// </summary>
		[SerializeField] private float _normalJumpPower;

    /// <summary>
    /// 위 점프 파워
    /// </summary>
    [SerializeField] private float _highJumpPower;

    /// <summary>
    /// 더블 점프 정면 파워
    /// </summary>
    [SerializeField] private float _doubleJumpForwardPower;

    /// <summary>
    /// 더블 점프 위 파워
    /// </summary>
    [SerializeField] private float _doubleJumpUpPower;

    /// <summary>
    ///  밑점 충돌 무시 시간
    /// </summary>
    [SerializeField] private float _downJumpSeconds;

    /// <summary>
    /// 공중 속도 관성
    /// </summary>
    [SerializeField] private float _airControlFloat;
    
    /// <summary>
    /// 밑점 중인지
    /// </summary>
    [SerializeField] private bool _isDownJumping;


    
		#endregion

		/// <summary>
		/// 초기화용 메서드
		/// </summary>
		public void Initialize()
    {
        if(_rb == null)
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        if (_playerCollider == null)
        {
            _playerCollider = GetComponent<Collider2D>();
        }

        _currentPosition = _rb.position;

        GameManager.Instance.DataManager.OnAllDataLoaded += CALLBACK_OnAllDataLoaded;

        if(GameManager.Instance.DataManager.CachedPlayerData.MoveSpeed >0)
        {
            CALLBACK_OnAllDataLoaded();
        }
    }

		public void Update()
    {
        CheckGround();
        Move(_moveVector);
		}

    public void InputMoveVector(Vector2 moveVector)
    {
        if (Mathf.Abs(moveVector.x) > Constants.FLOAT_CheckFloat)
        {
            //좌우 이동 
            _moveVector.x = moveVector.x > 0 ? 1f : -1f;
        }
        else
        {
            //입력 없으면 정지
            _moveVector.x = 0f;
        }
       
    }


		/// <summary>
		/// 이동 관련 매서드
		/// </summary>
		/// <param name="moveVector"></param>
		public void Move(Vector2 moveVector)
    {

        //바라보는 방향 저장
        if(Mathf.Abs(moveVector.x)> Constants.FLOAT_CheckFloat)
        {
            _isRight = Mathf.Sign(moveVector.x);
        }

        //바닥
        if(_currentJumpCnt == 0 && !_isDownJumping)
        {
            _rb.linearVelocity = new Vector2(moveVector.normalized.x * _moveSpeed, _rb.linearVelocityY);
        }
        //공중
        else
        {
            //입력이 있을 시 궤도 수정
            if(Mathf.Abs(moveVector.x)>Constants.FLOAT_CheckFloat)
            {
                float targetSpeedX = moveVector.normalized.x * _moveSpeed;
                float airControlPower = _moveSpeed * _airControlFloat;

                float newX = Mathf.MoveTowards(_rb.linearVelocityX, targetSpeedX, airControlPower * Time.deltaTime);
                _rb.linearVelocity = new Vector2(newX, _rb.linearVelocityY);
            }
        }

    }



    /// <summary>
    /// 점프
    /// </summary>
		public void Jump()
		{
        //이미 최대 점프 회수면 리턴
        if(_currentJumpCnt >= _jumpCnt)
        {
            return;
        }

        //바닥에 있을 때
				if(_currentJumpCnt ==0)
        {
            if(_verticalDirection<-0.5f)
            {
                StartCoroutine(DownJumpCoroutine());
                _currentJumpCnt++;
                return;
            }

            //윗점
            float appliedJumpPower = _verticalDirection > Constants.FLOAT_CheckFloat ? _highJumpPower : _normalJumpPower;
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, 0f);
            _rb.AddForce(Vector2.up * appliedJumpPower, ForceMode2D.Impulse);

            _currentJumpCnt++;
        }
        //2차 점프
        else
        {
            //방향키 검사
            if(Mathf.Abs(_moveVector.x)>Constants.FLOAT_CheckFloat)
            {
                _isRight = Mathf.Sign(_moveVector.x);
            }
    
            _rb.linearVelocity = Vector2.zero;

            Vector2 doubleJumpForce = new Vector2(_isRight * _doubleJumpForwardPower, _doubleJumpUpPower);
            _rb.AddForce(doubleJumpForce, ForceMode2D.Impulse);

            _currentJumpCnt++;
        }
		}

    /// <summary>
    /// 밑점 로직
    /// </summary>
    /// <returns></returns>
    private IEnumerator DownJumpCoroutine()
    {
        if (_isDownJumping)
        {
            yield break;
        }
        _isDownJumping = true;
        Physics2D.IgnoreLayerCollision(gameObject.layer, Constants.LAYER_Ground, true);
        _rb.linearVelocity = new Vector2(_rb.linearVelocityX, -1f * _normalJumpPower);

        yield return new WaitForSeconds(_downJumpSeconds);

        Physics2D.IgnoreLayerCollision(gameObject.layer, Constants.LAYER_Ground, false);
        _isDownJumping=false;
    }

    /// <summary>
    /// BoxCast를 이용한 바닥 감지
    /// </summary>
    private void CheckGround()
    {
        // 캐릭터가 점프 중일 때는 바닥 감지를 무시
        if (_rb.linearVelocityY > 0.05f || _isDownJumping)
        {
            return;
        }


				Vector2 boundsCenter = _playerCollider.bounds.center;
				Vector2 boundsSize = _playerCollider.bounds.size;

				// 캐릭터의 콜라이더 크기만 한 박스를 아래(Vector2.down)로 쏘아서 바닥 레이어와 닿는지 검사
				int groundLayerMask = 1 << Constants.LAYER_Ground;
				RaycastHit2D hit = Physics2D.BoxCast(boundsCenter, boundsSize, 0f, Vector2.down, 0.05f, groundLayerMask);


				// 바닥이 감지되었다면 점프 카운트 초기화
				if (hit.collider != null)
				{
						_currentJumpCnt = 0;
				}
		}


		/// <summary>
		/// 텔레포트나 강제 이동 후 위치 동기화
		/// </summary>
		/// <param name="syncVector"></param>
		public void SyncPosition(Vector2 syncVector)
    {
        _currentPosition = syncVector;
        _rb.position = syncVector;
    }

    public void ReturnToPool()
    {
        GameManager.Instance.DataManager.OnAllDataLoaded -= CALLBACK_OnAllDataLoaded;
    }


    /// <summary>
    /// 데이터 완료 시 실행할 매서드
    /// </summary>
    public void CALLBACK_OnAllDataLoaded()
    {
        PlayerMoveData data = GameManager.Instance.DataManager.CachedPlayerData;

        DataBinder.Bind(data, this);

        Debug.Log("Data Bind");
    }
}
