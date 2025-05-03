using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgSelectSurprise : BaseEventArgs
	{
		public GameEventPushType pushType { get; private set; }

		public object selectData { get; private set; }

		public void SetData(GameEventPushType type, object data)
		{
			this.pushType = type;
			this.selectData = data;
		}

		public override void Clear()
		{
		}
	}
}
