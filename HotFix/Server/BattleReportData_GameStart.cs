using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReportData_GameStart : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.GameStart;
			}
		}

		public override string ToString()
		{
			string text = string.Format("[Member Count:{0}]:\n", this.m_members.Count);
			for (int i = 0; i < this.m_members.Count; i++)
			{
				text += this.m_members[i].ToString();
			}
			return text;
		}

		public List<GameStartMemberData> m_members = new List<GameStartMemberData>();
	}
}
