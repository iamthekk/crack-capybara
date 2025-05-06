using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIPrivilegeCardBottomItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.buttonGet.onClick.AddListener(new UnityAction(this.OnClickCollect));
			for (int i = 0; i < this.buyGetItems.Count; i++)
			{
				this.buyGetItems[i].Init();
			}
			for (int j = 0; j < this.dayGetItems.Count; j++)
			{
				this.dayGetItems[j].Init();
			}
		}

		protected override void OnDeInit()
		{
			this.buttonGet.onClick.RemoveListener(new UnityAction(this.OnClickCollect));
			for (int i = 0; i < this.buyGetItems.Count; i++)
			{
				this.buyGetItems[i].DeInit();
			}
			for (int j = 0; j < this.dayGetItems.Count; j++)
			{
				this.dayGetItems[j].DeInit();
			}
		}

		public void SetData(IAPMonthCardData.CardType type)
		{
			this.cardType = type;
			int tableID = this.iapDataModule.MonthCard.GetTableID(this.cardType);
			this.monthCard = GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById(tableID);
			if (this.monthCard == null)
			{
				return;
			}
			List<PropData> list = this.monthCard.products.ToPropDataList();
			List<PropData> list2 = this.monthCard.productsPerDay.ToPropDataList();
			this.buyGetObj.SetActiveSafe(list.Count > 0);
			this.dayGetObj.SetActiveSafe(list2.Count > 0);
			for (int i = 0; i < this.buyGetItems.Count; i++)
			{
				this.buyGetItems[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < list.Count; j++)
			{
				if (j < this.buyGetItems.Count)
				{
					UIItem uiitem = this.buyGetItems[j];
					uiitem.gameObject.SetActiveSafe(true);
					uiitem.SetData(list[j]);
					uiitem.OnRefresh();
				}
			}
			for (int k = 0; k < this.dayGetItems.Count; k++)
			{
				this.dayGetItems[k].gameObject.SetActiveSafe(false);
			}
			for (int l = 0; l < list2.Count; l++)
			{
				if (l < this.dayGetItems.Count)
				{
					UIItem uiitem2 = this.dayGetItems[l];
					uiitem2.gameObject.SetActiveSafe(true);
					uiitem2.SetData(list2[l]);
					uiitem2.OnRefresh();
				}
			}
		}

		public void Refresh()
		{
			if (this.monthCard == null)
			{
				return;
			}
			bool flag = this.iapDataModule.MonthCard.IsAlwaysActive(this.cardType);
			bool flag2 = this.iapDataModule.MonthCard.IsActivation(this.cardType);
			bool flag3 = this.iapDataModule.MonthCard.IsCanReward(this.cardType);
			int num = this.iapDataModule.MonthCard.GetLastDay(this.cardType) + 1;
			this.timeObj.SetActiveSafe(!flag && flag2);
			this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_time", new object[] { num });
			string text = "uiprivilegecard_buyText";
			if (flag2)
			{
				if (flag3)
				{
					this.textGet.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collect");
				}
				else
				{
					this.textGet.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_collected");
				}
				if (!flag)
				{
					text = "uiprivilegecard_renewalText";
				}
			}
			this.buttonBuyCtrl.SetData(this.monthCard.id, text, new Action<bool>(this.OnBuyResult), null);
			this.buttonGet.gameObject.SetActiveSafe(flag3);
			this.buttonBuyCtrl.gameObject.SetActiveSafe(!flag3);
			if (flag2 && flag)
			{
				this.buttonBuyCtrl.SetBought(Singleton<LanguageManager>.Instance.GetInfoByID("uiprivilegecard_bought"));
				this.gray.SetUIGray();
			}
			else
			{
				this.gray.Recovery();
			}
			if (this.cardType == IAPMonthCardData.CardType.AutoMining)
			{
				bool flag4 = Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Mining, false);
				this.tipObj.SetActiveSafe(!flag3 && !flag4);
				if (!flag4)
				{
					this.buttonBuyCtrl.gameObject.SetActiveSafe(false);
					return;
				}
			}
			else
			{
				this.tipObj.SetActiveSafe(false);
			}
		}

		private void OnClickCollect()
		{
			if (this.monthCard == null)
			{
				return;
			}
			if (!this.iapDataModule.MonthCard.IsCanReward(this.cardType))
			{
				return;
			}
			NetworkUtils.Purchase.SendMonthCardGetRewardRequest(this.monthCard.id, delegate(bool isOk, MonthCardGetRewardResponse resp)
			{
				if (isOk)
				{
					this.Refresh();
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				}
			});
		}

		private void OnBuyResult(bool isResult)
		{
			this.Refresh();
		}

		public GameObject buyGetObj;

		public GameObject dayGetObj;

		public GameObject timeObj;

		public CustomText textTime;

		public UIGrays gray;

		public PurchaseButtonCtrl buttonBuyCtrl;

		public CustomButton buttonGet;

		public CustomText textGet;

		public List<UIItem> buyGetItems;

		public List<UIItem> dayGetItems;

		public GameObject tipObj;

		private IAPMonthCardData.CardType cardType;

		private IAP_MonthCard monthCard;

		private IAPDataModule iapDataModule;
	}
}
