using System;

namespace Server
{
	public class BattleReportData_RoundStart : BaseBattleReportData
	{
		public int CurRound { get; private set; }

		public int MaxRound { get; private set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.RoundStart;
			}
		}

		public void SetData(int curRound, int maxRound)
		{
			this.CurRound = curRound;
			this.MaxRound = maxRound;
		}

		public override string ToString()
		{
			return string.Format("CurRound:{0},  ", this.CurRound) + string.Format("MaxRound:{0}", this.MaxRound) + "\n";
		}
	}
}
