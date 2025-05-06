using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class LearnSkill_Normal : LearnSkillBase
	{
		protected override void OnSetTableData(BattleMain_skill tableData)
		{
			this.m_maxLevel = this.m_skillList.Count;
		}

		public override int GetCurrentSkillId()
		{
			if (this.m_currentLevel == 0 || this.m_currentLevel > this.m_maxLevel)
			{
				base.LogError(string.Format("GetCurrentSkillId() error, skillType:{0} m_currentLevel:{1} error", this.m_skillType, this.m_currentLevel));
				return 0;
			}
			return this.m_skillList[this.m_currentLevel - 1];
		}

		public override void LearnSkill()
		{
			this.m_currentLevel++;
		}

		public override int GetNextSkillId(out bool isMax)
		{
			isMax = false;
			if (this.m_currentLevel >= this.m_maxLevel)
			{
				base.LogError(string.Format("GetNextSkillId m_currentLevel:{0} >= m_maxLevel:{1}", this.m_currentLevel, this.m_maxLevel));
				return 0;
			}
			if (this.m_skillList.Count > 0)
			{
				if (this.m_currentLevel == this.m_maxLevel - 1)
				{
					isMax = true;
				}
				return this.m_skillList[this.m_currentLevel];
			}
			base.LogError(string.Format("GetNextSkillId[{0}] error m_skillList.Count == 0.", this.m_id));
			return 0;
		}

		public override bool IsAlreadyBreak()
		{
			return this.m_currentLevel >= this.m_maxLevel;
		}

		public override List<int> GetLearnedSkills()
		{
			List<int> list = new List<int>();
			if (this.m_currentLevel == 0)
			{
				return list;
			}
			if (this.m_currentLevel <= this.m_maxLevel)
			{
				list.Add(this.m_skillList[this.m_currentLevel - 1]);
			}
			else
			{
				base.LogError(string.Format("GetLearnedSkills[{0}] Normal error m_currentLevel:{1} > {2}.", this.m_id, this.m_currentLevel, this.m_maxLevel));
			}
			return list;
		}
	}
}
