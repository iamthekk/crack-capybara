using System;

namespace HotFix
{
	public static class MainChestType
	{
		public static string GetBoxSkinName(int boxId)
		{
			string text;
			switch (boxId)
			{
			case 1:
				text = "Chest_01";
				break;
			case 2:
				text = "Chest_02";
				break;
			case 3:
				text = "Chest_03";
				break;
			case 4:
				text = "Chest_04";
				break;
			case 5:
				text = "Chest_05";
				break;
			default:
				text = "Chest_01";
				break;
			}
			return text;
		}

		public const int Default = 2;

		public const int Bronze = 1;

		public const int Silver = 2;

		public const int Gold = 3;

		public const int Pet = 4;

		public const int Diamond = 5;

		public const string AnimationAppear = "Appear";

		public const string AnimationIdle = "Idle";

		public const string AnimationOpen = "Open";

		public const string AnimationOpen2 = "Open2";

		public const string AnimationOpen_Idle = "Open_Idle";
	}
}
