using System;

namespace Server
{
	public class BattleReportData_RoundEnd : BaseBattleReportData
	{
		public int CurRound { get; private set; }

		public int MaxRound { get; private set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.RoundEnd;
			}
		}

		public void SetData(int curRound, int maxRound)
		{
			this.CurRound = curRound;
			this.MaxRound = maxRound;
		}

		public override string ToString()
		{
			return string.Format("[BattleReportData_RoundEnd] CurRound:{0} MaxRound:{1}\n", this.CurRound, this.MaxRound);
		}
	}
}
