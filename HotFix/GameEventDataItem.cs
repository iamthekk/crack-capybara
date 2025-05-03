using System;
using System.Collections.Generic;

namespace HotFix
{
	public class GameEventDataItem : GameEventData
	{
		public GameEventDataItem(GameEventPoolData poolData, List<NodeItemParam> list)
		{
			this.poolData = poolData;
			this.paramList = list;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Item;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override string GetInfo()
		{
			return "Items";
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count > 0 && index < this.children.Count)
			{
				GameEventData gameEventData = this.children[index];
				while (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
				{
					gameEventData = gameEventData.GetNext(0);
				}
				return gameEventData;
			}
			return null;
		}

		public List<NodeItemParam> GetParamList()
		{
			return this.paramList;
		}

		private List<NodeItemParam> paramList;
	}
}
