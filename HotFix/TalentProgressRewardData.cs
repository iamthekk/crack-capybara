using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class TalentProgressRewardData
	{
		public TalentProgressRewardData(TalentNew_talent data)
		{
			this.id = data.id;
			this.exp = data.talentLevel;
			this.evolution = data.evolution;
			this.rewardType = data.rewardType;
			this.reward = data.reward;
			this.atlasId = data.iconAtlasID;
			this.iconId = data.iconID;
			this.talentName = data.talentName;
			this.talentDesc = data.talentDesc;
			if (this.evolution > 0)
			{
				TalentNew_talentEvolution elementById = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(this.evolution);
				this.rewardShowType = elementById.type;
				return;
			}
			this.rewardShowType = data.rewardType + 100;
		}

		public int id;

		public int exp;

		public int evolution;

		public int rewardShowType;

		public int rewardType;

		public string reward;

		public int atlasId;

		public string iconId;

		public string talentName;

		public string talentDesc;
	}
}
