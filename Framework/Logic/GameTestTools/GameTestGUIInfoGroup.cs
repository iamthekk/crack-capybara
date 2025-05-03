using System;
using System.Collections.Generic;

namespace Framework.Logic.GameTestTools
{
	public class GameTestGUIInfoGroup
	{
		public static int SortGroup(GameTestGUIInfoGroup x, GameTestGUIInfoGroup y)
		{
			return x.Head.CompareTo(y.Head);
		}

		public string Head;

		public List<GameTestGUIInfo> List = new List<GameTestGUIInfo>();
	}
}
