using System;


/// <summary>
/// 임시 몬스터 데이터 형식
/// </summary>
[Serializable]
public class MonsterData
{
		public int id;
		public string name;
		public float hp;
		public float atk;
		public float speed;
		public float level;
		public float exp;
}

/// <summary>
/// 임시 아이템 데이터 형식
/// </summary>
[Serializable]
public class ItemData
{
		public int id;
		public string name;
		public string description;
}

/// <summary>
/// 플레이어 이동 관련 데이터
/// </summary>
[Serializable]
public class PlayerMoveData
{
		#region Move

		/// <summary>
		/// 이동 속도
		/// </summary>
		[SheetColumn(Constants.Player_MoveSpeed)] public float MoveSpeed;

		#endregion

		#region Jump

		/// <summary>
		/// 최대 점프 횟수
		/// </summary>
		public int JumpCnt;

		/// <summary>
		/// 일반 점프 높이
		/// </summary>
		public float NormalJumpPower;

		/// <summary>
		/// 위 점프 높이
		/// </summary>
		public float HighJumpPower;

		/// <summary>
		/// 더블 점프 x축 파워
		/// </summary>
		public float DoubleJumpFowardPower;

		/// <summary>
		/// 더블 점프 y축 파워
		/// </summary>
		public float DoubleJumpUpPower;
		#endregion
}



/// <summary>
/// 순간이동이나 강제이동 관련 데이터
/// </summary>
[Serializable]
public class TeleportData
{
		/// <summary>
		/// 텔레포트 거리
		/// </summary>
		public float TeleportRange;

		/// <summary>
		/// 텔레포트 각도
		/// </summary>
		public float TeleportAngle;


}