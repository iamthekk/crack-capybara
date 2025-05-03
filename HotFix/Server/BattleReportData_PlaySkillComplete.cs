using System;

namespace Server
{
	public class BattleReportData_PlaySkillComplete : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.PlaySkillComplete;
			}
		}

		public int m_memberInstanceID { get; set; }

		public int m_skillId { get; set; }

		public int m_displayFrame { get; set; }

		public override string ToString()
		{
			return string.Format("m_memberInstanceID = {0}   m_skillId = {1}\n m_displayFrame = {2}", this.m_memberInstanceID, this.m_skillId, this.m_displayFrame);
		}
	}
}
