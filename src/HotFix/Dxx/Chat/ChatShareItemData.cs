using System;

namespace Dxx.Chat
{
	public class ChatShareItemData
	{
		public static int Sort(ChatShareItemData x, ChatShareItemData y)
		{
			return y.SortVal.CompareTo(x.SortVal);
		}

		public int Type;

		public long RowID;

		public int ItemID;

		public int Count;

		public long SortVal;

		public object RawData;
	}
}
