using System;

namespace Server
{
	public class BattleReportData_GameOver : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.GameOver;
			}
		}

		public OutResultData m_resultData { get; set; }

		public override string ToString()
		{
			return string.Format("{0}", this.m_resultData);
		}

		public bool m_isWin;
	}
}
