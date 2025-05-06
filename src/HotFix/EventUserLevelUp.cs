using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventUserLevelUp : BaseEventArgs
	{
		public override void Clear()
		{
			this.OldLevel = null;
			this.NewLevel = null;
			this.Rewards = null;
		}

		public UserLevel OldLevel;

		public UserLevel NewLevel;

		public IList<RewardDto> Rewards;
	}
}
