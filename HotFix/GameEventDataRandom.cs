using System;
using Server;

namespace HotFix
{
	public class GameEventDataRandom : GameEventData
	{
		public GameEventDataRandom(GameEventPoolData poolData)
		{
			this.poolData = poolData;
			this.sysRandom = Singleton<GameEventController>.Instance.GetGroupRandom();
			if (this.sysRandom == null)
			{
				this.sysRandom = new XRandom(poolData.randomSeed);
			}
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Random;
		}

		public override GameEventNodeOptionType GetNodeOptionType()
		{
			return GameEventNodeOptionType.Option;
		}

		public override string GetInfo()
		{
			return "Random";
		}

		public override GameEventData GetNext(int index)
		{
			if (this.children.Count == 0)
			{
				return null;
			}
			int num = this.sysRandom.Range(0, this.children.Count);
			GameEventData gameEventData = this.children[num];
			if (gameEventData != null && gameEventData.GetNodeOptionType() == GameEventNodeOptionType.Option)
			{
				gameEventData = gameEventData.GetNext(0);
			}
			return gameEventData;
		}

		private readonly XRandom sysRandom;
	}
}
