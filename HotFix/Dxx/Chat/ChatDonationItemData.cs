using System;

namespace Dxx.Chat
{
	public class ChatDonationItemData
	{
		public static int Sort(ChatDonationItemData x, ChatDonationItemData y)
		{
			return y.SortVal.CompareTo(x.SortVal);
		}

		public int Type;

		public long RowID;

		public int ItemID;

		public int Count;

		public int SortVal;
	}
}
