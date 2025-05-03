using System;

namespace Server
{
	public class BattleReportData_Revive : BaseBattleReportData
	{
		public int TargetInstanceID { get; private set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.Revive;
			}
		}

		public BattleReportData_Revive SetData(int targetInstanceId, FP reviveHp)
		{
			this.TargetInstanceID = targetInstanceId;
			this.reviveHp = reviveHp;
			return this;
		}

		public override string ToString()
		{
			return string.Format("[Revive] TargetInstanceID:{0} ReviveHp:{1}", this.TargetInstanceID, this.reviveHp);
		}

		public FP reviveHp;
	}
}
