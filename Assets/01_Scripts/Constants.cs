using UnityEngine;

public static class Constants 
{
		#region float
		public const float FLOAT_TeleportMinRange = 0.5f;

		public const float FLOAT_CheckFloat = 0.1f;
		#endregion

		#region tag

		/// <summary>
		/// 그라운드 타일맵 태그
		/// </summary>
		public const string TAG_Ground = "Ground";

		public const string TAG_Player = "Player";

		#endregion

		#region layer
		public const int LAYER_Ground = 3;
		public const int LAYER_Player = 7;
		#endregion

		#region SheetName
		//공통,Sheet 이름
		public const string ID = "ID";
		public const string Name = "Name";
		public const string FistRow = "1";	


		public const string PlayerSheet = "Player";
		public const string MonsterSheet = "Monster";
		public const string ItemSheet = "Item";

	//Player
	public const string Player_MoveSpeed = "_moveSpeed";

        //Monster

    //Item


    #endregion
}
