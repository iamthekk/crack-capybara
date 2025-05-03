using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgEventPause : BaseEventArgs
	{
		public bool isPause { get; private set; }

		public void SetData(bool pause)
		{
			this.isPause = pause;
		}

		public override void Clear()
		{
		}
	}
}
