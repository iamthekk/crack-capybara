using System;

namespace HotFix
{
	public class GameEventDataFishingGame : GameEventData
	{
		public GameEventDataFishingGame(GameEventPoolData poolData)
		{
			this.poolData = poolData;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.FishingGame;
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
