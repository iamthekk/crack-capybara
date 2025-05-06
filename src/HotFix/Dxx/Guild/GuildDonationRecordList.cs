using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildDonationRecordList
	{
		public IList<GuildDonationRecord> Records
		{
			get
			{
				return this.mRecords;
			}
		}

		public long GetMaxMsgID()
		{
			if (this.mRecords.Count <= 0)
			{
				return 0L;
			}
			return this.mRecords[this.mRecords.Count - 1].msgId;
		}

		public void AddRecords(List<GuildDonationRecord> records)
		{
			this.mRecords.AddRange(records);
			this.CurrentPage++;
			this.mRecords.Sort(new Comparison<GuildDonationRecord>(GuildDonationRecord.SortByMsgID));
		}

		private List<GuildDonationRecord> mRecords = new List<GuildDonationRecord>();

		public int CurrentPage = 1;
	}
}
