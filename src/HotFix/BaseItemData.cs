using System;

namespace HotFix
{
	[Serializable]
	public class BaseItemData
	{
		public override string ToString()
		{
			return string.Format("[ rowid = {0} ,id = {1} , count = {2} ]", this.rowId, this.id, this.count);
		}

		public ulong rowId;

		public uint id;

		public ulong count;
	}
}
