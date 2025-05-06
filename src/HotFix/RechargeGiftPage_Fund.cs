using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace HotFix
{
	public class RechargeGiftPage_Fund : RechargeGiftPageBase
	{
		public override IAPRechargeGiftViewModule.EPageType PageType
		{
			get
			{
				return IAPRechargeGiftViewModule.EPageType.Fund;
			}
		}

		protected override void OnPageInit()
		{
			for (int i = 0; i < this.fundNodeCtrls.Count; i++)
			{
				UIRechargeGiftFundNodeCtrl uirechargeGiftFundNodeCtrl = this.fundNodeCtrls[i];
				uirechargeGiftFundNodeCtrl.SetIndex(i);
				uirechargeGiftFundNodeCtrl.Init();
			}
		}

		protected override void OnPageDeInit()
		{
			for (int i = 0; i < this.fundNodeCtrls.Count; i++)
			{
				this.fundNodeCtrls[i].DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.fundNodeCtrls.Count; i++)
			{
				this.fundNodeCtrls[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnShow()
		{
			this.scroll.verticalNormalizedPosition = 1f;
		}

		protected override void OnRefresh()
		{
			for (int i = 0; i < this.fundNodeCtrls.Count; i++)
			{
				this.fundNodeCtrls[i].Refresh();
			}
		}

		public ScrollRect scroll;

		public List<UIRechargeGiftFundNodeCtrl> fundNodeCtrls;
	}
}
