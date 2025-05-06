using System;

namespace Server
{
	public class BattleReportData_LegacySkillSummonDisplay : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.LegacySkillSummonDisplay;
			}
		}

		public int m_memberInstanceID { get; set; }

		public int m_skillId { get; set; }

		public void SetData(int memberInstanceID, int skillId, int legacyAppearFrame)
		{
			if (legacyAppearFrame <= 0)
			{
				HLog.LogError("传承技能召唤模型Appear动画帧数错误，请检查GameSkill表");
				legacyAppearFrame = 30;
			}
			this.m_displayFrame = 18 + legacyAppearFrame;
			this.m_memberInstanceID = memberInstanceID;
			this.m_skillId = skillId;
		}

		public override string ToString()
		{
			return string.Format("m_memberInstanceID:{0}, ", this.m_memberInstanceID) + string.Format("m_skillId:{0}, ", this.m_skillId) + string.Format("m_displayFrame:{0}", this.m_displayFrame);
		}

		public int m_displayFrame;
	}
}
