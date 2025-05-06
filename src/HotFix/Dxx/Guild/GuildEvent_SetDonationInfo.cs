using System;

namespace Dxx.Guild
{
	public class GuildEvent_SetDonationInfo : GuildBaseEvent
	{
		public override void Clear()
		{
			this.DonationInfo = null;
		}

		public GuildDonationInfo DonationInfo;
	}
}
