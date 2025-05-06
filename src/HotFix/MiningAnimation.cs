using System;

namespace HotFix
{
	public static class MiningAnimation
	{
		public static string GetNormalAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "Anim_UIFX_KPBL_Green_Normal";
			case 3:
				return "Anim_UIFX_KPBL_Blue_Normal";
			case 4:
				return "Anim_UIFX_KPBL_Purple_Normal";
			case 5:
				return "Anim_UIFX_KPBL_Golden_Normal";
			case 6:
				return "Anim_UIFX_KPBL_Red_Normal";
			default:
				return "Anim_UIFX_KPBL_White_Normal";
			}
		}

		public static string GetClickAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "Anim_UIFX_KPBL_Green_Click";
			case 3:
				return "Anim_UIFX_KPBL_Blue_Click";
			case 4:
				return "Anim_UIFX_KPBL_Purple_Click";
			case 5:
				return "Anim_UIFX_KPBL_Golden_Click";
			case 6:
				return "Anim_UIFX_KPBL_Red_Click";
			default:
				return "Anim_UIFX_KPBL_White_Click";
			}
		}

		public static string GetOpenAnim(int quality)
		{
			switch (quality)
			{
			case 2:
				return "Anim_UIFX_KPBL_Green_Open";
			case 3:
				return "Anim_UIFX_KPBL_Blue_Open";
			case 4:
				return "Anim_UIFX_KPBL_Purple_Open";
			case 5:
				return "Anim_UIFX_KPBL_Golden_Open";
			case 6:
				return "Anim_UIFX_KPBL_Red_Open";
			default:
				return "Anim_UIFX_KPBL_White_Open";
			}
		}

		public const string Mining = "ChuiZi";

		public const string Gravel = "SuiShi";

		public const string Light_White = "Guang_Bai";

		public const string Light_Green = "Guang_Lv";

		public const string Light_Blue = "Guang_Lan";

		public const string Light_Purple = "Guang_Zi";

		public const string Light_Golden = "Guang_Jin";

		public const string Light_Red = "Guang_Hong";

		public const string Bomb_Idle = "ZhaDanA_2";

		public const string Bomb_Ready = "ZhaDanA_3";

		public const string Bomb_Blow = "ZhaDanA_5";

		public const string Next_Floor_Start = "Start";

		public const string Next_Floor_End = "End";

		public const string Next_Floor_Loop = "Loop";

		public const string White_Normal = "Anim_UIFX_KPBL_White_Normal";

		public const string White_Click = "Anim_UIFX_KPBL_White_Click";

		public const string White_Open = "Anim_UIFX_KPBL_White_Open";

		public const string Green_Normal = "Anim_UIFX_KPBL_Green_Normal";

		public const string Green_Click = "Anim_UIFX_KPBL_Green_Click";

		public const string Green_Open = "Anim_UIFX_KPBL_Green_Open";

		public const string Blue_Normal = "Anim_UIFX_KPBL_Blue_Normal";

		public const string Blue_Click = "Anim_UIFX_KPBL_Blue_Click";

		public const string Blue_Open = "Anim_UIFX_KPBL_Blue_Open";

		public const string Purple_Normal = "Anim_UIFX_KPBL_Purple_Normal";

		public const string Purple_Click = "Anim_UIFX_KPBL_Purple_Click";

		public const string Purple_Open = "Anim_UIFX_KPBL_Purple_Open";

		public const string Golden_Normal = "Anim_UIFX_KPBL_Golden_Normal";

		public const string Golden_Click = "Anim_UIFX_KPBL_Golden_Click";

		public const string Golden_Open = "Anim_UIFX_KPBL_Golden_Open";

		public const string Red_Normal = "Anim_UIFX_KPBL_Red_Normal";

		public const string Red_Click = "Anim_UIFX_KPBL_Red_Click";

		public const string Red_Open = "Anim_UIFX_KPBL_Red_Open";
	}
}
