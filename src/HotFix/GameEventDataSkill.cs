using System;

namespace HotFix
{
	public class GameEventDataSkill : GameEventData
	{
		public GameEventDataSkill(GameEventPoolData poolData, int sourceId, int randomNum, int selectNum, int skillBuildId)
		{
			this.poolData = poolData;
			this.sourceId = sourceId;
			this.randomNum = randomNum;
			this.selectNum = selectNum;
			this.skillBuildId = skillBuildId;
		}

		public override GameEventNodeType GetNodeType()
		{
			return GameEventNodeType.Skill;
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
			return string.Format("Skill_{0}_{1}_{2}", this.sourceId, this.randomNum, this.selectNum);
		}

		public int sourceId;

		public int randomNum;

		public int selectNum;

		public int skillBuildId;
	}
}
