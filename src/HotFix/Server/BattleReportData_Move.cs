using System;

namespace Server
{
	public class BattleReportData_Move : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.Move;
			}
		}

		public int m_memberInstanceID { get; set; }

		public int m_skillId { get; set; }

		public int m_targetInstanceID { get; set; }

		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("m_memberInstanceID:{0}, ", this.m_memberInstanceID),
				string.Format("m_skillId:{0},", this.m_skillId),
				string.Format("m_targetInstanceID:{0},", this.m_targetInstanceID),
				string.Format("m_moveFrame:{0},", this.m_moveFrame),
				string.Format("m_isMoveBack:{0}", this.m_isMoveBack)
			});
		}

		public int m_moveFrame = 3;

		public bool m_isMoveBack;
	}
}
