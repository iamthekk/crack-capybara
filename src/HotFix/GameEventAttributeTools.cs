using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public static class GameEventAttributeTools
	{
		public static NodeAttParam ToNodeAttParam(this MergeAttributeData data)
		{
			GameEventAttType gameEventAttType = GameEventAttType.None;
			string header = data.Header;
			if (!(header == "Attack%"))
			{
				if (!(header == "Defence%"))
				{
					if (header == "HPMax%")
					{
						gameEventAttType = GameEventAttType.HPMaxPercent;
					}
				}
				else
				{
					gameEventAttType = GameEventAttType.DefencePercent;
				}
			}
			else
			{
				gameEventAttType = GameEventAttType.AttackPercent;
			}
			return new NodeAttParam(gameEventAttType, (double)data.Value.AsFloat(), ChapterDropSource.Event, 1);
		}

		public static List<NodeAttParam> ToNodeAttParams(this List<MergeAttributeData> list)
		{
			List<NodeAttParam> list2 = new List<NodeAttParam>();
			for (int i = 0; i < list.Count; i++)
			{
				NodeAttParam nodeAttParam = list[i].ToNodeAttParam();
				list2.Add(nodeAttParam);
			}
			return list2;
		}
	}
}
