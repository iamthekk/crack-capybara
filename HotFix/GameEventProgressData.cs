using System;

namespace HotFix
{
	public class GameEventProgressData
	{
		public GameEventType type { get; private set; }

		public int stage { get; private set; }

		public GameEventProgressData(GameEventType type, int stage)
		{
			this.type = type;
			this.stage = stage;
		}
	}
}
