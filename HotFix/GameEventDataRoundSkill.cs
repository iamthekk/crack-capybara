using System;

namespace HotFix
{
	public class GameEventDataRoundSkill : GameEventData
	{
		public GameEventDataRoundSkill(GameEventPoolData poolData, int sourceId, int round, int randomNum, int selectNum)
		{
			this.poolData = poolData;
			this.sourceId = sourceId;
			this.round = round;
			this.randomNum = randomNum;
			this.selectNum = selectNum;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.RoundSkill;
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
			return string.Format("RoundSkill_{0}_{1}_{2}_{3}", new object[] { this.sourceId, this.round, this.randomNum, this.selectNum });
		}

		public int sourceId;

		public int round;

		public int randomNum;

		public int selectNum;
	}
}
