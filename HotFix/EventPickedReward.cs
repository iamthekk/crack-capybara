using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventPickedReward : BaseEventArgs
	{
		public string UID { get; private set; }

		public void SetData(string uid)
		{
			this.UID = uid;
		}

		public override void Clear()
		{
			this.UID = null;
		}
	}
}
