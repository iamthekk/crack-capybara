using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventOpenMiniGameUI : BaseEventArgs
	{
		public MiniGameType miniGameType { get; private set; }

		public int miniGameId { get; private set; }

		public int serverSeed { get; private set; }

		public int rewardRate { get; private set; }

		public void SetData(MiniGameType type, int gameId, int seed, int rate)
		{
			this.miniGameType = type;
			this.miniGameId = gameId;
			this.serverSeed = seed;
			this.rewardRate = rate;
		}

		public override void Clear()
		{
		}
	}
}
