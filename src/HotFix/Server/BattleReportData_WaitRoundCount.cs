using System;

namespace Server
{
	public class BattleReportData_WaitRoundCount : BaseBattleReportData
	{
		public int TargetInstanceID { get; set; }

		public int WaitRoundCount { get; set; }

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.WaitRoundCount;
			}
		}

		public override string ToString()
		{
			return string.Format("TargetInstaceID：{0}, ", this.TargetInstanceID) + string.Format("WaitRoundCount：{0}, ", this.WaitRoundCount) + "\n";
		}
	}
}
