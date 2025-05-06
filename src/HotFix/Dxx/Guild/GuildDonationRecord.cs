using System;

namespace Dxx.Guild
{
	public class GuildDonationRecord
	{
		public static int SortByMsgID(GuildDonationRecord x, GuildDonationRecord y)
		{
			return x.msgId.CompareTo(y.msgId);
		}

		public long msgId;

		public long timestamp;

		public int type;

		public int itemId;

		public int itemCount;

		public long fromUserId;

		public string fromNickName;

		public long toUserId;

		public string toUserNickName;
	}
}
