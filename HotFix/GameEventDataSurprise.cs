using System;

namespace HotFix
{
	public class GameEventDataSurprise : GameEventData
	{
		public GameEventDataSurprise(GameEventPoolData poolData)
		{
			this.poolData = poolData;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Surprise;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Normal;
		}

		public override string GetInfo()
		{
			return "Surprise";
		}

		public override GameEventData GetNext(int index)
		{
			return null;
		}
	}
}
