using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsTowerChallengeEnd : BaseEventArgs
	{
		public int LevelId { get; set; }

		public int Result { get; set; }

		public override void Clear()
		{
			this.Result = 0;
		}
	}
}
