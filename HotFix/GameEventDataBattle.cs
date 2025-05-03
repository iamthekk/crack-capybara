using System;

namespace HotFix
{
	public class GameEventDataBattle : GameEventData
	{
		public int groupIndex { get; private set; }

		public GameEventDataBattle(GameEventPoolData poolData, int groupIndex)
		{
			this.poolData = poolData;
			this.groupIndex = groupIndex;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Battle;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
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

		public override string GetInfo()
		{
			return "Battle";
		}
	}
}
