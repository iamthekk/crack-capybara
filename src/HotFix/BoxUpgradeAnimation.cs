using System;

namespace HotFix
{
	public static class BoxUpgradeAnimation
	{
		public static string GetBirthAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "2_Green_Birth";
			case 3:
				return "3_Blue_Birth";
			case 4:
				return "4_Purple_Birth";
			case 5:
				return "5_Golden_Birth";
			case 6:
				return "6_Red_Birth";
			default:
				return "1_White_Birth";
			}
		}

		public static string GetIdleAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "2_Green_Idle";
			case 3:
				return "3_Blue_Idle";
			case 4:
				return "4_Purple_Idle";
			case 5:
				return "5_Golden_Idle";
			case 6:
				return "6_Red_Idle";
			default:
				return "1_White_Idle";
			}
		}

		public static string GetClickAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "2_Green_Click";
			case 3:
				return "3_Blue_Click";
			case 4:
				return "4_Purple_Click";
			case 5:
				return "5_Golden_Click";
			case 6:
				return "6_Red_Click";
			default:
				return "1_White_Click";
			}
		}

		public static string GetClickUpAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "2_Green_ClickUp";
			case 3:
				return "3_Blue_ClickUp";
			case 4:
				return "4_Purple_ClickUp";
			case 5:
				return "5_Golden_ClickUp";
			case 6:
				return "6_Red_ClickUp";
			default:
				return "1_White_ClickUp";
			}
		}

		public static string GetOpenAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "2_Green_Open";
			case 3:
				return "3_Blue_Open";
			case 4:
				return "4_Purple_Open";
			case 5:
				return "5_Golden_Open";
			case 6:
				return "6_Red_Open";
			default:
				return "1_White_Open";
			}
		}

		public const string White_Birth = "1_White_Birth";

		public const string White_Click = "1_White_Click";

		public const string White_ClickUp = "1_White_ClickUp";

		public const string White_Idle = "1_White_Idle";

		public const string White_Open = "1_White_Open";

		public const string Green_Birth = "2_Green_Birth";

		public const string Green_Click = "2_Green_Click";

		public const string Green_ClickUp = "2_Green_ClickUp";

		public const string Green_Idle = "2_Green_Idle";

		public const string Green_Open = "2_Green_Open";

		public const string Blue_Birth = "3_Blue_Birth";

		public const string Blue_Click = "3_Blue_Click";

		public const string Blue_ClickUp = "3_Blue_ClickUp";

		public const string Blue_Idle = "3_Blue_Idle";

		public const string Blue_Open = "3_Blue_Open";

		public const string Purple_Birth = "4_Purple_Birth";

		public const string Purple_Click = "4_Purple_Click";

		public const string Purple_ClickUp = "4_Purple_ClickUp";

		public const string Purple_Idle = "4_Purple_Idle";

		public const string Purple_Open = "4_Purple_Open";

		public const string Golden_Birth = "5_Golden_Birth";

		public const string Golden_Click = "5_Golden_Click";

		public const string Golden_ClickUp = "5_Golden_ClickUp";

		public const string Golden_Idle = "5_Golden_Idle";

		public const string Golden_Open = "5_Golden_Open";

		public const string Red_Birth = "6_Red_Birth";

		public const string Red_Click = "6_Red_Click";

		public const string Red_ClickUp = "6_Red_ClickUp";

		public const string Red_Idle = "6_Red_Idle";

		public const string Red_Open = "6_Red_Open";
	}
}
