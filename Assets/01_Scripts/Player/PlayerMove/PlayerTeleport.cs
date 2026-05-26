using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 텔레포트 관련 클래스(순간이동, 강제 이동 등에 사용)
/// </summary>
public class PlayerTeleport : MonoBehaviour
{
		/// <summary>
		/// 텔레포트 방향 벡터
		/// </summary>
		private Vector2Int _teleportVector;
		public Vector2Int TeleportVector { get { return _teleportVector; } set {  _teleportVector = value; } }

		/// <summary>
		/// 리지드바디
		/// </summary>
		[SerializeField] private Rigidbody2D _rb;
		public Rigidbody2D Rb=>_rb;

		/// <summary>
		/// 초기화용 매서드
		/// </summary>
		public void Initialize()
		{
				if(_rb == null)
				{  
						_rb = GetComponent<Rigidbody2D>(); 
				}
		}

		/// <summary>
		/// 텔레포트, 강제이동 실행 매서드
		/// </summary>
		/// <param name="dirVector">텔레포트 방향</param>
		/// <param name="teleportRange">텔레포트 거리</param>
		/// <param name="searchAngle">텔레포트 서치 범위</param>
		/// <param name="teleportOffset">텔레포트 이동 시 y축 보정</param>
		public void Teleport(Vector2Int dirVector,float teleportRange,float searchAngle,float teleportOffset)
		{
				_teleportVector = dirVector;

				Vector2? posVector = CheckTilemap(dirVector, teleportRange, searchAngle);
				

				if(posVector!=null)
				{
						_rb.position = posVector.Value + new Vector2(0,teleportOffset);

						GameManager.Instance.Player.PlayerMove.SyncPosition(_rb.position);
				}

				PlayTeleportEffect();
		}

		/// <summary>
		/// 타일맵 서치
		/// </summary>
		/// <param name="dirVector">텔레포트 방향</param>
		/// <param name="teleportRange">텔레포트 거리</param>
		/// <param name="searchAngle">텔레포트 서치 범위</param>
		public Vector2? CheckTilemap(Vector2Int dirVector,float teleportRange,float searchAngle)
		{
				Tilemap currentMap = GameManager.Instance.MapManager.CurrentTileMap;
				Vector2 currentPos = _rb.position;
				Vector2 checkDir = new Vector2(dirVector.x, dirVector.y).normalized;

				//탐색 범위 설정
				Vector3Int minCell = currentMap.WorldToCell(currentPos - new Vector2(teleportRange, teleportRange));
				Vector3Int maxCell = currentMap.WorldToCell(currentPos + new Vector2(teleportRange, teleportRange));

				// 타겟 위치와 타겟까지의 거리
				Vector2? targetPos = null;
				float targetDistance = -1f;
				float maxTeleportRange = teleportRange * teleportRange;
				float minTeleportRange = Constants.FLOAT_TeleportMinRange * Constants.FLOAT_TeleportMinRange;


				// 타일맵 순회
				for (int x = minCell.x; x <= maxCell.x; x++)
				{
						for (int y = minCell.y; y <= maxCell.y; y++)
						{
								Vector3Int cellPos = new Vector3Int(x, y, 0);
								

								//타일 존재 여부 검사
								if (currentMap.HasTile(cellPos))
								{
										//거리 검사
										Vector2 tileWorldPos = currentMap.GetCellCenterWorld(cellPos);
										Vector2 dirToTile = tileWorldPos - currentPos;

										float distanceToTile = dirToTile.sqrMagnitude;

										if(distanceToTile <= maxTeleportRange && distanceToTile > minTeleportRange)
										{
												//각도 검사
												float angle = Vector2.Angle(checkDir, dirToTile);

												if(angle <= searchAngle/2f)
												{
														if(distanceToTile >targetDistance)
														{
																//서치 타일 갱신
																targetPos = tileWorldPos;
																targetDistance = distanceToTile;
														}
												}
										}

										
								}

						}
				}

				return targetPos;
		}
		
		/// <summary>
		/// 텔레포트 이펙트 재생
		/// </summary>
		public void PlayTeleportEffect()
		{

		}

		
}
