using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsLoadTableFinish : BaseEventArgs
	{
		public void SetData(bool isFinished)
		{
			this.isFinished = isFinished;
		}

		public override void Clear()
		{
		}

		public bool isFinished;
	}
}
