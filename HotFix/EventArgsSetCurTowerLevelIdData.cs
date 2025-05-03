using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsSetCurTowerLevelIdData : BaseEventArgs
	{
		public int CompleteTowerLevelId { get; set; }

		public int ClaimedRewardTowerId { get; set; }

		public override void Clear()
		{
			this.CompleteTowerLevelId = 0;
			this.ClaimedRewardTowerId = 0;
		}
	}
}
