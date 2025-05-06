using System;
using LocalModels.Bean;

namespace HotFix
{
	public class PaySlotRewardData
	{
		public ChapterMiniGame_paySlotReward Config { get; private set; }

		public string RewardType { get; private set; }

		public NodeAttParam NodeAttParam { get; private set; }

		public GameEventSkillBuildData SkillBuildData { get; private set; }

		public bool IsSkill
		{
			get
			{
				return this.RewardType == "2";
			}
		}

		public bool IsAttribute
		{
			get
			{
				return this.RewardType == "1";
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.RewardType == "0";
			}
		}

		public PaySlotRewardData(ChapterMiniGame_paySlotReward config)
		{
			this.Config = config;
			this.RewardType = ((config.param.Length != 0) ? config.param[0] : "0");
			if (this.IsAttribute)
			{
				this.NodeAttParam = MiniGameUtils.GetNodeAttParam(config.param, ChapterDropSource.Event, 1);
			}
		}

		public void SetSkillBuild(GameEventSkillBuildData skillBuild)
		{
			this.SkillBuildData = skillBuild;
		}
	}
}
