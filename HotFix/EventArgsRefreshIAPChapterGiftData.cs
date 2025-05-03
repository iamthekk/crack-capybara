using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsRefreshIAPChapterGiftData : BaseEventArgs
	{
		public MapField<uint, ulong> ChapterGiftTime { get; private set; }

		public void SetData(MapField<uint, ulong> chapterGiftTime)
		{
			this.ChapterGiftTime = chapterGiftTime;
		}

		public override void Clear()
		{
			this.ChapterGiftTime = null;
		}
	}
}
