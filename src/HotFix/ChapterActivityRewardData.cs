using System;

namespace HotFix
{
	public class ChapterActivityRewardData
	{
		public ChapterActivityRewardData(ulong rowId, ChapterActivityKind kind, int actId)
		{
			this.RowId = rowId;
			this.Kind = kind;
			this.ActivityId = actId;
		}

		public ulong RowId;

		public ChapterActivityKind Kind;

		public int ActivityId;
	}
}
