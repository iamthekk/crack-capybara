using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels.Bean;

namespace Server
{
	public abstract class LearnSkillBase
	{
		public void SetTableData(BattleMain_skill tableData)
		{
			this.m_id = tableData.ID;
			this.m_skillType = (LearnSkillType)tableData.skillType;
			this.m_skillList = tableData.skillList.ToList<int>();
			this.m_currentLevel = 0;
			this.OnSetTableData(tableData);
		}

		protected abstract void OnSetTableData(BattleMain_skill tableData);

		public abstract int GetCurrentSkillId();

		public abstract int GetNextSkillId(out bool isMax);

		public abstract void LearnSkill();

		public abstract bool IsAlreadyBreak();

		public abstract List<int> GetLearnedSkills();

		public void SetLevel(int level)
		{
			if (level < 1 || level > this.m_maxLevel)
			{
				HLog.LogError(string.Format("LearnSkillData.SetLevel({0}) error, maxLevel{1} skillId{2} skillType:{3}", new object[] { level, this.m_maxLevel, this.m_id, this.m_skillType }));
				return;
			}
			this.m_currentLevel = level;
		}

		protected void LogError(string value)
		{
			HLog.LogError(string.Format("[{0}][id:{1}]:{2}", base.GetType(), this.m_id, value));
		}

		public int m_id;

		public LearnSkillType m_skillType;

		public List<int> m_skillList;

		public int m_currentLevel;

		public int m_maxLevel;
	}
}
