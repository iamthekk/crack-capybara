using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsCrossArenaRefreshOppList : BaseEventArgs
	{
		public override void Clear()
		{
			this.Members = null;
		}

		public int RefreshCount;

		public List<CrossArenaRankMember> Members;
	}
}
