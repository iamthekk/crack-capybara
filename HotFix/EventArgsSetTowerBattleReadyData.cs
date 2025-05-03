using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsSetTowerBattleReadyData : BaseEventArgs
	{
		public int BattleLevelId { get; private set; }

		public bool IsReset { get; private set; } = true;

		public int BattleResult { get; private set; }

		public void SetData(int battleLevelId, bool isReset, int battleResult)
		{
			this.BattleLevelId = battleLevelId;
			this.IsReset = isReset;
			this.BattleResult = battleResult;
		}

		public override void Clear()
		{
			this.SetData(0, true, 0);
		}
	}
}
