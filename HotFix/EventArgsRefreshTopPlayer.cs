using System;
using Framework.EventSystem;
using Proto.LeaderBoard;

namespace HotFix
{
	public class EventArgsRefreshTopPlayer : BaseEventArgs
	{
		public RankUserDto topPlayer { get; private set; }

		public void SetData(RankUserDto dto)
		{
			this.topPlayer = dto;
		}

		public override void Clear()
		{
		}
	}
}
