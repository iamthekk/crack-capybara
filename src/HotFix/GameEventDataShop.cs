using System;

namespace HotFix
{
	public class GameEventDataShop : GameEventData
	{
		public GameEventDataShop(GameEventPoolData poolData, int npcId)
		{
			this.poolData = poolData;
			this.npcId = npcId;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Shop;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override GameEventData GetNext(int index)
		{
			return null;
		}

		public override string GetInfo()
		{
			return "Shop";
		}

		public int npcId;
	}
}
