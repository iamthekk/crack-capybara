using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshIAPBattlePassScore : BaseEventArgs
	{
		public void SetData(int score)
		{
			this.Score = score;
		}

		public override void Clear()
		{
			this.Score = -1;
		}

		public int Score;
	}
}
