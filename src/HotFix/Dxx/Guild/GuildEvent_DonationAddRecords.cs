using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_DonationAddRecords : GuildBaseEvent
	{
		public override void Clear()
		{
			this.Records = null;
		}

		public int type;

		public List<GuildDonationRecord> Records;
	}
}
