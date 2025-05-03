using System;
using Framework.EventSystem;
using Proto.Guild;

namespace HotFix
{
	public class EventArgsBattleGuildBossEnter : BaseEventArgs
	{
		public override void Clear()
		{
			this.Response = null;
		}

		public GuildBossEndBattleResponse Response;
	}
}
