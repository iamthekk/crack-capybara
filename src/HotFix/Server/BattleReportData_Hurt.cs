using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReportData_Hurt : BaseBattleReportData
	{
		public override BattleReportType m_type
		{
			get
			{
				return BattleReportType.Hurt;
			}
		}

		public List<BattleReportData_HurtOneData> reportDatas
		{
			get
			{
				return this.m_hurtOneDatas;
			}
		}

		public void AddData(BattleReportData_HurtOneData data)
		{
			this.m_hurtOneDatas.Add(data);
		}

		public List<BattleReportData_HurtOneData> GetList()
		{
			return this.m_hurtOneDatas;
		}

		public override string ToString()
		{
			string text = string.Format("[Count：{0}]：\n", this.m_hurtOneDatas.Count);
			for (int i = 0; i < this.m_hurtOneDatas.Count; i++)
			{
				text += this.m_hurtOneDatas[i].ToString();
			}
			return text;
		}

		private List<BattleReportData_HurtOneData> m_hurtOneDatas = new List<BattleReportData_HurtOneData>();
	}
}
