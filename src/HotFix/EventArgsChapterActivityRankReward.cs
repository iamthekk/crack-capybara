using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsChapterActivityRankReward : BaseEventArgs
	{
		public ChapterActiveRankInfo RankReward { get; private set; }

		public void SetData(ChapterActiveRankInfo rankReward)
		{
			this.RankReward = rankReward;
		}

		public override void Clear()
		{
		}
	}
}
