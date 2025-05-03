using System;
using System.Collections.Generic;

namespace Server
{
	public class BattleReport
	{
		public bool Enable
		{
			get
			{
				return this.m_enable;
			}
		}

		public void SetEnable(bool enable)
		{
			this.m_enable = enable;
		}

		public void AddReport(BaseBattleReportData data)
		{
			if (!this.m_enable)
			{
				return;
			}
			if (data == null)
			{
				return;
			}
			this.m_datas.Add(data);
		}

		public void DebugReportByRound(int round)
		{
			BattleLogHelper.DebugReportByRound(round, this.m_datas);
		}

		public void RemoveReport(BaseBattleReportData data)
		{
			if (!this.m_enable)
			{
				return;
			}
			this.m_datas.Remove(data);
		}

		public void RemoveAllReport()
		{
			this.m_datas.Clear();
		}

		public List<BaseBattleReportData> GetReportDatas(int startIndex, int currentFrame)
		{
			this.datas.Clear();
			if (this.m_datas.Count <= startIndex)
			{
				return this.datas;
			}
			int num = startIndex;
			while (num < this.m_datas.Count && this.m_datas[num].m_frame <= currentFrame)
			{
				this.datas.Add(this.m_datas[num]);
				num++;
			}
			return this.datas;
		}

		public int GetReportDatas(int startIndex, int currentFrame, Action<BaseBattleReportData> callback)
		{
			if (this.m_datas.Count <= startIndex)
			{
				return 0;
			}
			int num = 0;
			for (int i = startIndex; i < this.m_datas.Count; i++)
			{
				BaseBattleReportData baseBattleReportData = this.m_datas[i];
				if (baseBattleReportData.m_frame > currentFrame)
				{
					break;
				}
				num++;
				callback(baseBattleReportData);
			}
			return num;
		}

		private void SortReportData(List<BaseBattleReportData> datas)
		{
			datas.Sort((BaseBattleReportData x, BaseBattleReportData y) => x.m_frame.CompareTo(y.m_frame));
		}

		public List<BaseBattleReportData> m_datas = new List<BaseBattleReportData>();

		private bool m_enable = true;

		private List<BaseBattleReportData> datas = new List<BaseBattleReportData>();
	}
}
