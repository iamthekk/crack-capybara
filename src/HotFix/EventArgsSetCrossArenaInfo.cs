using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsSetCrossArenaInfo : BaseEventArgs
	{
		public void SetData(uint dan, uint rank, uint score, uint total, uint seasonId)
		{
			this.Dan = dan;
			this.Rank = rank;
			this.Score = score;
			this.TotalMemberCount = total;
			this.CurSeasonId = seasonId;
		}

		public void SetData(uint dan, uint rank, uint score, uint total, uint seasonId, uint groupId)
		{
			this.SetData(dan, rank, score, total, seasonId);
			this.GroupId = groupId;
		}

		public override void Clear()
		{
			this.Dan = 0U;
			this.Rank = 0U;
			this.Score = 0U;
			this.TotalMemberCount = 1U;
			this.CurSeasonId = 0U;
		}

		public uint Dan;

		public uint Rank;

		public uint Score;

		public uint TotalMemberCount;

		public uint CurSeasonId;

		public uint GroupId;
	}
}
