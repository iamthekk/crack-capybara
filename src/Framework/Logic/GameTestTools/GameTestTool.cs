using System;
using System.Collections.Generic;

namespace Framework.Logic.GameTestTools
{
	public class GameTestTool
	{
		public static void SetHotFixTestList(List<GameTestGUIInfo> list, Action<GameTestGUIInfo> action)
		{
		}

		public static void SetCSharpsTestList(List<GameTestGUIInfo> list, Action<GameTestGUIInfo> action)
		{
		}

		public static void SetEditorTestList(List<GameTestGUIInfo> list, Action<GameTestGUIInfo> action)
		{
		}

		private static void AddTestToListGroup(List<GameTestGUIInfo> list, List<GameTestGUIInfoGroup> grouplist)
		{
			for (int i = 0; i < list.Count; i++)
			{
				GameTestGUIInfo gameTestGUIInfo = list[i];
				GameTestGUIInfoGroup gameTestGUIInfoGroup = null;
				for (int j = 0; j < grouplist.Count; j++)
				{
					GameTestGUIInfoGroup gameTestGUIInfoGroup2 = grouplist[j];
					if (gameTestGUIInfoGroup2.Head == gameTestGUIInfo.Head)
					{
						gameTestGUIInfoGroup = gameTestGUIInfoGroup2;
						break;
					}
				}
				if (gameTestGUIInfoGroup == null)
				{
					gameTestGUIInfoGroup = new GameTestGUIInfoGroup
					{
						Head = gameTestGUIInfo.Head
					};
					grouplist.Add(gameTestGUIInfoGroup);
				}
				gameTestGUIInfoGroup.List.Add(gameTestGUIInfo);
			}
		}

		public static List<GameTestGUIInfoGroup> HotFixTestList = new List<GameTestGUIInfoGroup>();

		public static Action<GameTestGUIInfo> OnHotFixTestAction;

		public static List<GameTestGUIInfoGroup> CSharpsTestList = new List<GameTestGUIInfoGroup>();

		public static Action<GameTestGUIInfo> OnCSharpsTestAction;

		public static List<GameTestGUIInfoGroup> EditorTestList = new List<GameTestGUIInfoGroup>();

		public static Action<GameTestGUIInfo> OnEditorTestAction;
	}
}
