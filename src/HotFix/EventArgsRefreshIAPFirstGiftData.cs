using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshIAPFirstGiftData : BaseEventArgs
	{
		public void SetData(bool firstRechargeReward, uint totalRecharge)
		{
			this.m_firstRechargeReward = firstRechargeReward;
			this.m_totalRecharge = (int)totalRecharge;
		}

		public override void Clear()
		{
		}

		public bool m_firstRechargeReward;

		public int m_totalRecharge;
	}
}
