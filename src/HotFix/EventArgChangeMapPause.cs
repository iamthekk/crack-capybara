using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgChangeMapPause : BaseEventArgs
	{
		public bool isPause { get; private set; }

		public int mapId { get; private set; }

		public void SetData(bool pause, int map)
		{
			this.isPause = pause;
			this.mapId = map;
		}

		public override void Clear()
		{
		}
	}
}
