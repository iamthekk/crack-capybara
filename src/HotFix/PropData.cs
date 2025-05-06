using System;

namespace HotFix
{
	[Serializable]
	public class PropData
	{
		public override string ToString()
		{
			return string.Format("PropData rowId:{0} id:{1} count:{2} level:{3} exp:{4}", new object[] { this.rowId, this.id, this.count, this.level, this.exp });
		}

		public ulong rowId;

		public uint id;

		public ulong count;

		public uint level;

		public uint exp;
	}
}
