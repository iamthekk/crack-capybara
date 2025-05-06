using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class LearnSkill_Legend : LearnSkillBase
	{
		protected override void OnSetTableData(BattleMain_skill tableData)
		{
			this.m_maxLevel = this.m_skillList.Count;
			if (this.m_maxLevel != 1)
			{
				base.LogError(string.Format("OnSetTableData() error, m_maxLevel:{0} != 1", this.m_maxLevel));
			}
		}

		public override int GetCurrentSkillId()
		{
			if (this.m_currentLevel == 0)
			{
				base.LogError(string.Format("GetCurrentSkillId() error, skillType:{0} m_currentLevel:{1} == 0", this.m_skillType, this.m_currentLevel));
				return 0;
			}
			if (this.m_currentLevel <= this.m_maxLevel)
			{
				return this.m_skillList[this.m_currentLevel - 1];
			}
			base.LogError(string.Format("GetCurrentSkillId() error, skillType:{0} m_currentLevel:{1} > {2}", this.m_skillType, this.m_currentLevel, this.m_maxLevel));
			return 0;
		}

		public override void LearnSkill()
		{
			this.m_currentLevel = this.m_maxLevel;
		}

		public override int GetNextSkillId(out bool isMax)
		{
			isMax = true;
			return this.m_skillList[0];
		}

		public override bool IsAlreadyBreak()
		{
			return this.m_currentLevel > 0;
		}

		public override List<int> GetLearnedSkills()
		{
			List<int> list = new List<int>();
			if (this.m_currentLevel > 0)
			{
				list.Add(this.m_skillList[0]);
			}
			return list;
		}
	}
}
