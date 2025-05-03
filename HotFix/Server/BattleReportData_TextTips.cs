using System;

namespace Server
{
	public class BattleReportData_TextTips : BaseBattleReportData
	{
		public int TargetInstanceID { get; set; }

		public string TextID { get; set; } = string.Empty;

		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.TextTips;
			}
		}

		public override string ToString()
		{
			return string.Format("TargetInstaceID：{0}, ", this.TargetInstanceID) + "TextID：" + this.TextID + ", \n";
		}
	}
}
