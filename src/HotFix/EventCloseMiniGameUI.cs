using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventCloseMiniGameUI : BaseEventArgs
	{
		public MiniGameType miniGameType { get; private set; }

		public List<GameEventMiniGameData> rewardList { get; private set; }

		public void SetData(MiniGameType type, List<GameEventMiniGameData> list)
		{
			this.miniGameType = type;
			this.rewardList = list;
		}

		public override void Clear()
		{
		}
	}
}
