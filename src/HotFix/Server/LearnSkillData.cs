using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Server
{
	public class LearnSkillData
	{
		public int m_id
		{
			get
			{
				return this.m_learnSkill.m_id;
			}
		}

		public int m_currentLevel
		{
			get
			{
				return this.m_learnSkill.m_currentLevel;
			}
		}

		public int m_maxLevel
		{
			get
			{
				return this.m_learnSkill.m_maxLevel;
			}
		}

		public void SetTableData(BattleMain_skill tableData)
		{
			this.m_skillType = (LearnSkillType)tableData.skillType;
			LearnSkillType skillType = this.m_skillType;
			if (skillType != LearnSkillType.Normal)
			{
				if (skillType != LearnSkillType.Legend)
				{
					HLog.LogError(string.Format("LearnSkillData.SetTableData() error, skillType:{0}", this.m_skillType));
					return;
				}
				this.m_learnSkill = new LearnSkill_Legend();
			}
			else
			{
				this.m_learnSkill = new LearnSkill_Normal();
			}
			this.m_learnSkill.SetTableData(tableData);
		}

		public void LearnSkill()
		{
			this.m_learnSkill.LearnSkill();
		}

		public void SetLevel(int level)
		{
			this.m_learnSkill.SetLevel(level);
		}

		public int GetNextSkillId(out bool isMax)
		{
			return this.m_learnSkill.GetNextSkillId(out isMax);
		}

		public int GetCurrentSkillId()
		{
			return this.m_learnSkill.GetCurrentSkillId();
		}

		public bool IsAlreadyBreak()
		{
			return this.m_learnSkill.IsAlreadyBreak();
		}

		public List<int> GetLearnedSkills()
		{
			return this.m_learnSkill.GetLearnedSkills();
		}

		private LearnSkillBase m_learnSkill;

		public LearnSkillType m_skillType;
	}
}
