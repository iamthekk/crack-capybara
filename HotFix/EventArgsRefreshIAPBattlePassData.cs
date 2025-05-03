using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshIAPBattlePassData : BaseEventArgs
	{
		public void SetData(IAPBattlePassDto data)
		{
			this.BattlePassDto = data;
		}

		public override void Clear()
		{
			this.BattlePassDto = null;
		}

		public IAPBattlePassDto BattlePassDto;
	}
}
