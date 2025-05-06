using System;

namespace HotFix
{
	public class GameEventDataBox : GameEventData
	{
		public GameEventDataBox(GameEventPoolData poolData)
		{
			this.poolData = poolData;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Box;
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
			return "Box";
		}
	}
}
