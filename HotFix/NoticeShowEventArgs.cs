using System;
using Framework.EventSystem;

namespace HotFix
{
	public class NoticeShowEventArgs : BaseEventArgs
	{
		public override void Clear()
		{
			this.Index = -1;
		}

		public int Index;
	}
}
