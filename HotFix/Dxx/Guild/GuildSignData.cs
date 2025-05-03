using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildSignData
	{
		public int SignCount;

		public int MaxSignCount;

		public GuildItemData SignCost;

		public List<GuildItemData> Rewards = new List<GuildItemData>();
	}
}
