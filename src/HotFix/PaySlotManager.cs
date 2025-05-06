using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class PaySlotManager
	{
		public PaySlotManager(int seed, int groupId)
		{
			foreach (ChapterMiniGame_paySlotReward chapterMiniGame_paySlotReward in GameApp.Table.GetManager().GetChapterMiniGame_paySlotRewardElements())
			{
				if (chapterMiniGame_paySlotReward.groupId == groupId)
				{
					this.randomPool.Add(chapterMiniGame_paySlotReward);
				}
			}
			this.xRandom = new XRandom(seed);
		}

		public List<PaySlotRewardData> GetShowReward(int count)
		{
			List<PaySlotRewardData> list = new List<PaySlotRewardData>();
			Random random = new Random((int)DateTime.Now.Ticks);
			for (int i = 0; i < count; i++)
			{
				int num = random.Next(this.randomPool.Count);
				ChapterMiniGame_paySlotReward chapterMiniGame_paySlotReward = this.randomPool[num];
				if (i == 1)
				{
					chapterMiniGame_paySlotReward = this.randomPool[0];
				}
				PaySlotRewardData paySlotRewardData = new PaySlotRewardData(chapterMiniGame_paySlotReward);
				if (paySlotRewardData.IsSkill)
				{
					int num2;
					GameEventSkillBuildData gameEventSkillBuildData;
					if (chapterMiniGame_paySlotReward.param.Length > 1 && int.TryParse(chapterMiniGame_paySlotReward.param[1], out num2))
					{
						gameEventSkillBuildData = this.CreateShowSkill((SkillBuildSourceType)num2, random);
					}
					else
					{
						gameEventSkillBuildData = this.CreateShowSkill(SkillBuildSourceType.Normal, random);
					}
					if (gameEventSkillBuildData != null)
					{
						paySlotRewardData.SetSkillBuild(gameEventSkillBuildData);
					}
				}
				list.Add(paySlotRewardData);
			}
			return list;
		}

		private GameEventSkillBuildData CreateShowSkill(SkillBuildSourceType source, Random showRandom)
		{
			List<GameEventSkillBuildData> skillPool = Singleton<GameEventController>.Instance.GetSkillPool(source);
			int num = showRandom.Next(skillPool.Count);
			if (num < skillPool.Count)
			{
				return skillPool[num];
			}
			return null;
		}

		public PaySlotRewardData RandomResult()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.randomPool.Count; i++)
			{
				list.Add(this.randomPool[i].weight);
			}
			int weightedRandomSelection = RandUtils.GetWeightedRandomSelection(list, this.xRandom);
			PaySlotRewardData paySlotRewardData = new PaySlotRewardData(this.randomPool[weightedRandomSelection]);
			if (paySlotRewardData.IsSkill)
			{
				int num = this.xRandom.NextInt();
				GameEventSkillBuildData skillBuildData = MiniGameUtils.GetSkillBuildData(paySlotRewardData.Config.param, num);
				paySlotRewardData.SetSkillBuild(skillBuildData);
			}
			return paySlotRewardData;
		}

		private List<ChapterMiniGame_paySlotReward> randomPool = new List<ChapterMiniGame_paySlotReward>();

		private XRandom xRandom;
	}
}
