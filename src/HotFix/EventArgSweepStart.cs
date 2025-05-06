using System;
using Framework.EventSystem;
using Proto.Chapter;

namespace HotFix
{
	public class EventArgSweepStart : BaseEventArgs
	{
		public StartChapterSweepResponse response { get; private set; }

		public void SetData(StartChapterSweepResponse resp)
		{
			this.response = resp;
		}

		public override void Clear()
		{
		}
	}
}
