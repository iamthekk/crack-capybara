using System;
using System.Collections.Generic;
using Framework.EventSystem;
using LocalModels.Bean;

namespace HotFix
{
	public class EventArgsChapterActivityGetScoreAni : BaseEventArgs
	{
		public ulong RowId { get; private set; }

		public int OldScore { get; private set; }

		public int NewScore { get; private set; }

		public int OldTotalScore { get; private set; }

		public int NewTotalScore { get; private set; }

		public int OldIndex { get; private set; }

		public int NewIndex { get; private set; }

		public List<ItemData> rewards { get; private set; }

		public Dictionary<int, ChapterActivity_ChapterObj> chapterObjDic { get; private set; }

		public void SetData(ulong rowId, int oldCurScore, int newCurScore, int oldTotalScore, int newTotalScore, int oldIndex, int newIndex, List<ItemData> rewardList, Dictionary<int, ChapterActivity_ChapterObj> dic)
		{
			this.RowId = rowId;
			this.OldScore = oldCurScore;
			this.NewScore = newCurScore;
			this.OldTotalScore = oldTotalScore;
			this.NewTotalScore = newTotalScore;
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
			this.rewards = rewardList;
			this.chapterObjDic = dic;
		}

		public override void Clear()
		{
		}
	}
}
