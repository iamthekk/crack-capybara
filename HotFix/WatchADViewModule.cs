using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class WatchADViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			int tableID = this.iapDataModule.MonthCard.GetTableID(IAPMonthCardData.CardType.NoAd);
			string text = "uiprivilegecard_buyText";
			this.buttonBuy.SetData(tableID, text, new Action<bool>(this.OnBuyResult), null);
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("uiwatchad_title");
			this.buttonCancel.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnCloseSelf));
			this.buttonWatch.onClick.AddListener(new UnityAction(this.OnWatchAd));
		}

		public override void OnOpen(object data)
		{
			if (data == null)
			{
				return;
			}
			WatchADViewModule.OpenData openData = data as WatchADViewModule.OpenData;
			if (openData != null)
			{
				this.openData = openData;
			}
			if (this.openData == null)
			{
				return;
			}
			Shop_Ad shop_Ad = GameApp.Table.GetManager().GetShop_Ad(this.openData.adId);
			if (shop_Ad == null)
			{
				HLog.LogError(string.Format("Shop_Ad not found id={0}", this.openData.adId));
				return;
			}
			this.textContent.text = Singleton<LanguageManager>.Instance.GetInfoByID(shop_Ad.languageId);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.openData != null)
			{
				this.RefreshAdTime();
			}
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonCancel.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnCloseSelf));
			this.buttonWatch.onClick.RemoveListener(new UnityAction(this.OnWatchAd));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void RefreshAdTime()
		{
			if (this.openData != null)
			{
				bool flag = this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.NoAd);
				int maxTimes = this.adDataModule.GetMaxTimes(this.openData.adId);
				int watchTimes = this.adDataModule.GetWatchTimes(this.openData.adId);
				long watchCD = this.adDataModule.GetWatchCD(this.openData.adId);
				if (!flag && watchCD > 0L && watchTimes < maxTimes)
				{
					this.getObj.SetActiveSafe(false);
					this.cdObj.SetActiveSafe(true);
					this.textCD.text = Singleton<LanguageManager>.Instance.GetTime(watchCD);
					return;
				}
				this.getObj.SetActiveSafe(true);
				this.cdObj.SetActiveSafe(false);
			}
		}

		private void OnBuyResult(bool isSuccess)
		{
			this.adObj.SetActiveSafe(!isSuccess);
		}

		private void OnCloseSelf()
		{
			if (GameApp.View.IsOpened(ViewName.WatchAdViewModule))
			{
				GameApp.View.CloseView(ViewName.WatchAdViewModule, null);
			}
		}

		private void OnWatchAd()
		{
			if (this.openData != null)
			{
				int maxTimes = this.adDataModule.GetMaxTimes(this.openData.adId);
				if (this.adDataModule.GetWatchTimes(this.openData.adId) >= maxTimes)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("uihangup_noad");
					GameApp.View.ShowStringTip(infoByID);
					return;
				}
				if (this.iapDataModule.MonthCard.IsActivePrivilege(CardPrivilegeKind.NoAd))
				{
					Action onJump = this.openData.onJump;
					if (onJump != null)
					{
						onJump();
					}
				}
				else if (this.adDataModule.GetWatchCD(this.openData.adId) > 0L)
				{
					string infoByID2 = Singleton<LanguageManager>.Instance.GetInfoByID("ad_cd_tip");
					GameApp.View.ShowStringTip(infoByID2);
				}
				else
				{
					Action onWatch = this.openData.onWatch;
					if (onWatch != null)
					{
						onWatch();
					}
				}
			}
			this.OnCloseSelf();
		}

		public CustomText textTitle;

		public CustomText textContent;

		public CustomButton buttonCancel;

		public CustomButton buttonWatch;

		public CustomButton buttonClose;

		public PurchaseButtonCtrl buttonBuy;

		public GameObject adObj;

		public GameObject getObj;

		public GameObject cdObj;

		public CustomText textCD;

		private WatchADViewModule.OpenData openData;

		private AdDataModule adDataModule;

		private IAPDataModule iapDataModule;

		public class OpenData
		{
			public int adId;

			public Action onWatch;

			public Action onJump;
		}
	}
}
