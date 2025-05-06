using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgSurprise : BaseEventArgs
	{
		public int surpriseId { get; private set; }

		public int randomSeed { get; private set; }

		public void SetData(int id, int seed)
		{
			this.surpriseId = id;
			this.randomSeed = seed;
		}

		public override void Clear()
		{
		}
	}
}
