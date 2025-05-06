using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgRandomBox : BaseEventArgs
	{
		public GameEventBoxViewModule.RandomBoxData randomBoxData { get; private set; }

		public void SetData(GameEventBoxViewModule.RandomBoxData data)
		{
			this.randomBoxData = data;
		}

		public override void Clear()
		{
		}
	}
}
