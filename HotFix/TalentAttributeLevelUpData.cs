using System;
using Server;

namespace HotFix
{
	public class TalentAttributeLevelUpData
	{
		public TalentAttributeLevelUpData(int talentStep, MergeAttributeData mergeAttributeData, int levelLimit, long[] levelUpCost)
		{
			this.talentStep = talentStep;
			this.talentAttributeKey = mergeAttributeData.Header;
			this.talentAttributeValue = mergeAttributeData.Value;
			this.maxLevel = levelLimit;
			this.levelUpCost = levelUpCost;
		}

		public void UpdateLevel(int lv)
		{
			this.curLevel = lv;
		}

		public int talentStep;

		public string talentAttributeKey;

		public FP talentAttributeValue;

		public int maxLevel;

		public int curLevel;

		public long[] levelUpCost;
	}
}
