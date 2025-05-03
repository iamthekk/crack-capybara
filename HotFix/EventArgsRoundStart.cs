using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRoundStart : BaseEventArgs
	{
		public int CurRound { get; private set; }

		public int MaxRound { get; private set; }

		public void SetData(int curRound, int maxRound)
		{
			this.CurRound = curRound;
			this.MaxRound = maxRound;
		}

		public override void Clear()
		{
		}
	}
}
