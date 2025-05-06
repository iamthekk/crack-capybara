using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgChangeMap : BaseEventArgs
	{
		public int mapId { get; private set; }

		public void SetData(int newMapId)
		{
			this.mapId = newMapId;
		}

		public override void Clear()
		{
		}
	}
}
