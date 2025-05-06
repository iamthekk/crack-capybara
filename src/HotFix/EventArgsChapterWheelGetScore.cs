using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsChapterWheelGetScore : BaseEventArgs
	{
		public int OldScore { get; private set; }

		public int NewScore { get; private set; }

		public void SetData(int oldScore, int newScore)
		{
			this.OldScore = oldScore;
			this.NewScore = newScore;
		}

		public override void Clear()
		{
		}
	}
}
