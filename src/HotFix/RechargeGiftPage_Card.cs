using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using Proto.Pay;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class RechargeGiftPage_Card : RechargeGiftPageBase
	{
		public override IAPRechargeGiftViewModule.EPageType PageType
		{
			get
			{
				return IAPRechargeGiftViewModule.EPageType.Card;
			}
		}

		private IAPDataModule iapDataModule
		{
			get
			{
				return GameApp.Data.GetDataModule(DataName.IAPDataModule);
			}
		}

		protected override void OnPageInit()
		{
			this.buttonCollectAll.onClick.AddListener(new UnityAction(this.OnClickCollectAll));
			for (int i = 0; i < this.cardNodeCtrls.Count; i++)
			{
				UIRechargeGiftCardNodeBase uirechargeGiftCardNodeBase = this.cardNodeCtrls[i];
				uirechargeGiftCardNodeBase.SetIndex(i);
				uirechargeGiftCardNodeBase.Init();
			}
		}

		protected override void OnPageDeInit()
		{
			this.buttonCollectAll.onClick.RemoveListener(new UnityAction(this.OnClickCollectAll));
			for (int i = 0; i < this.cardNodeCtrls.Count; i++)
			{
				this.cardNodeCtrls[i].DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.cardNodeCtrls.Count; i++)
			{
				this.cardNodeCtrls[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnShow()
		{
			this.scroll.verticalNormalizedPosition = 1f;
		}

		protected override void OnRefresh()
		{
			List<IAPMonthCardData.CardType> list = DxxTools.EnumToList<IAPMonthCardData.CardType>();
			this.isReward = false;
			for (int i = 0; i < list.Count; i++)
			{
				this.isReward = this.iapDataModule.MonthCard.IsCanReward(list[i]);
				if (this.isReward)
				{
					break;
				}
			}
			if (this.isReward)
			{
				this.btnGray.Recovery();
			}
			else
			{
				this.btnGray.SetUIGray();
			}
			if (this.iapDataModule.MonthCard.IsCanReward(IAPMonthCardData.CardType.Free))
			{
				this.freeCardCtrl.transform.SetAsFirstSibling();
			}
			else
			{
				this.freeCardCtrl.transform.SetAsLastSibling();
			}
			for (int j = 0; j < this.cardNodeCtrls.Count; j++)
			{
				this.cardNodeCtrls[j].Refresh();
			}
		}

		private void OnClickCollectAll()
		{
			if (this.isReward)
			{
				NetworkUtils.Purchase.SendMonthCardGetRewardRequest(0, delegate(bool isOk, MonthCardGetRewardResponse resp)
				{
					if (isOk)
					{
						this.OnRefresh();
						DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
					}
				});
			}
		}

		public ScrollRect scroll;

		public List<UIRechargeGiftCardNodeBase> cardNodeCtrls;

		public CustomButton buttonCollectAll;

		public UIGrays btnGray;

		public UIRechargeGiftCardNodeBase freeCardCtrl;

		private bool isReward;
	}
}
