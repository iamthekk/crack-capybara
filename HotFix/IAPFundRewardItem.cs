using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPFundRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.item)
			{
				this.item.Init();
			}
		}

		protected override void OnDeInit()
		{
			if (this.item)
			{
				this.item.DeInit();
			}
		}

		public void SetData(PropData propData)
		{
			this.item.SetData(propData);
		}

		public void Refresh()
		{
			this.item.OnRefresh();
		}

		public void SetState(IAPLevelFund.RewardState rewardState)
		{
			if (this.maskObj)
			{
				this.maskObj.SetActiveSafe(rewardState != IAPLevelFund.RewardState.CanCollect);
			}
			if (this.lockObj)
			{
				this.lockObj.SetActiveSafe(rewardState == IAPLevelFund.RewardState.Lock);
			}
			if (this.collectedObj)
			{
				this.collectedObj.SetActiveSafe(rewardState == IAPLevelFund.RewardState.Collected);
			}
			if (this.canCollectObj)
			{
				this.canCollectObj.SetActiveSafe(rewardState == IAPLevelFund.RewardState.CanCollect);
			}
			if (this.redNode)
			{
				this.redNode.SetActiveSafe(rewardState == IAPLevelFund.RewardState.CanCollect);
			}
		}

		public UIItem item;

		public GameObject lockObj;

		public GameObject maskObj;

		public GameObject collectedObj;

		public GameObject canCollectObj;

		public GameObject redNode;
	}
}
