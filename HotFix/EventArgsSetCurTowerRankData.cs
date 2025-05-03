using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsSetCurTowerRankData : BaseEventArgs
	{
		public int TowerRank { get; set; }

		public override void Clear()
		{
			this.TowerRank = 0;
		}
	}
}
