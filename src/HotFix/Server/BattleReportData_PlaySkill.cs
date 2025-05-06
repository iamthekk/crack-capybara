using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReportData_PlaySkill : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.PlaySkill;
			}
		}

		public int m_memberInstanceID { get; set; }

		public int m_skillId { get; set; }

		public CritType m_critType { get; set; }

		public List<int> m_targetList { get; set; }

		public int m_curCD { get; set; }

		public int m_maxCD { get; set; }

		public SkillCastType m_castType { get; set; }

		public override string ToString()
		{
			string text = string.Concat(new string[]
			{
				string.Format("m_memberInstanceID:{0}, ", this.m_memberInstanceID),
				string.Format("m_skillId:{0}, ", this.m_skillId),
				string.Format("m_critType:{0}, ", this.m_critType),
				string.Format("m_curCD:{0}, ", this.m_curCD),
				string.Format("m_maxCD:{0} ", this.m_maxCD)
			});
			text += " m_targetList:";
			for (int i = 0; i < this.m_targetList.Count; i++)
			{
				text = text + this.m_targetList[i].ToString() + ",";
			}
			return text + string.Format("m_currentCD:{0}, m_maxCD:{1}, \n", this.m_curCD, this.m_maxCD);
		}
	}
}
