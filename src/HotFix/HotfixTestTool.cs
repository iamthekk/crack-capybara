using System;
using System.Collections.Generic;
using Framework.Logic.GameTestTools;

namespace HotFix
{
	public class HotfixTestTool
	{
		public static void RegistGameTest()
		{
		}

		public static void UnRegistGameTest()
		{
		}

		public static void RebuildTestList()
		{
		}

		public static List<GameTestGUIInfo> GetTestList()
		{
			if (HotfixTestTool.mTestList.Count <= 0)
			{
				HotfixTestTool.RebuildTestList();
			}
			List<GameTestGUIInfo> list = new List<GameTestGUIInfo>();
			for (int i = 0; i < HotfixTestTool.mTestList.Count; i++)
			{
				GameTestMethodVO gameTestMethodVO = HotfixTestTool.mTestList[i];
				if (gameTestMethodVO != null)
				{
					list.Add(gameTestMethodVO.MakeInfo());
				}
			}
			return list;
		}

		private static void OnDoTest(GameTestGUIInfo info)
		{
			if (info == null)
			{
				return;
			}
			int num = 0;
			while (num < HotfixTestTool.mTestList.Count && !HotfixTestTool.mTestList[num].TryDoTest(info))
			{
				num++;
			}
		}

		private static List<GameTestMethodVO> mTestList = new List<GameTestMethodVO>();
	}
}
