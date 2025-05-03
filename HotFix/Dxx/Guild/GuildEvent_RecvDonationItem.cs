using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_RecvDonationItem : GuildBaseEvent
	{
		public List<GuildItemData> Rewards { get; set; }

		public override void Clear()
		{
			this.Rewards = null;
		}
	}
}
