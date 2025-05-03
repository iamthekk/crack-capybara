using System;

namespace HotFix
{
	public class GameEventDataFishing : GameEventData
	{
		public int npcId { get; private set; }

		public FishType fishType { get; private set; }

		public GameEventDataFishing(GameEventPoolData poolData, int npcId, FishType fishType)
		{
			this.poolData = poolData;
			this.npcId = npcId;
			this.fishType = fishType;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Fishing;
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
			return "Fishing";
		}
	}
}
