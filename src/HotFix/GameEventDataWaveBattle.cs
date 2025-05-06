using System;
using System.Collections.Generic;

namespace HotFix
{
	public class GameEventDataWaveBattle : GameEventData
	{
		public GameEventDataWaveBattle(GameEventPoolData poolData, string groupArr)
		{
			this.poolData = poolData;
			this.groupList = groupArr.GetListInt(',');
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.WaveBattle;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override string GetInfo()
		{
			return "WaveBattle";
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

		public List<int> groupList;
	}
}
