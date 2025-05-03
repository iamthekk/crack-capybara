using System;

namespace Server
{
	public abstract class BaseBattleReportData
	{
		public abstract BattleReportType m_type { get; }

		public new abstract string ToString();

		public int m_frame = -1;

		public int m_round = -1;
	}
}
