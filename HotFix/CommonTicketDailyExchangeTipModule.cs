using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.User;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class CommonTicketDailyExchangeTipModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
		}

		public override void OnOpen(object data)
		{
			this.openData = (CommonTicketDailyExchangeTipModule.OpenData)data;
			this.ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.ticketDailyExchangeDataModule = GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule);
			this.itemTable = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.openData.TicketType);
			this.InitAll();
			this.RefreshAll();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.FreshGiftTimeAndBtn();
		}

		public override void OnClose()
		{
			this.DeInitBuy();
			this.DeInitAd();
			this.DeInitGift();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Currency_Update));
			manager.RegisterEvent(LocalMessageName.CC_Ticket_DayChangeUpdated, new HandlerEvent(this.OnDayChangeUpdated));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Currency_Update));
			manager.UnRegisterEvent(LocalMessageName.CC_Ticket_DayChangeUpdated, new HandlerEvent(this.OnDayChangeUpdated));
		}

		private void InitAll()
		{
			this.InitBuy();
			this.InitAd();
			this.InitGift();
		}

		private void RefreshAll()
		{
			this.FreshTicketBuy();
			this.FreshTicketAd();
			this.FreshGift();
		}

		private void RefreshUITicketItem(CommonTicketDailyExchangeTipModule.UITicketItem uiTicketItem)
		{
			string text = GameApp.Table.GetAtlasPath(this.itemTable.atlasID);
			if (!uiTicketItem.dontFreshIcon)
			{
				uiTicketItem.ticketIcon.SetImage(text, this.itemTable.icon);
			}
			Quality_itemQuality elementById = GameApp.Table.GetManager().GetQuality_itemQualityModelInstance().GetElementById(this.itemTable.quality);
			if (elementById != null)
			{
				text = GameApp.Table.GetAtlasPath(elementById.atlasId);
				uiTicketItem.ticketIconBg.SetImage(text, elementById.bgSpriteName);
			}
			uiTicketItem.buyCountText.text = "x" + uiTicketItem.data_CurTicketCount.ToString();
			uiTicketItem.leftTimesText.SetText("Common_Ticket_DailyExchange_LeftTimes", uiTicketItem.data_LeftBuyTimes.ToString());
		}

		private ViewName GetName()
		{
			return ViewName.CommonTicketDailyExchangeTipModule;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.CloseSelf();
			}
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		private bool IsHaveUpperLimit(CommonTicketDailyExchangeTipModule.UITicketItem uiTicketItem)
		{
			return uiTicketItem.data_LeftBuyTimes <= 0;
		}

		private void OnClockTicket(int itemId, Vector3 clickPos)
		{
			ItemData itemData = new ItemData();
			itemData.SetID(itemId);
			PropData propData = itemData.ToPropData();
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = propData,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, clickPos, 70f);
		}

		private void Event_Currency_Update(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshBuyBtn();
		}

		private void OnDayChangeUpdated(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshAll();
		}

		private void InitBuy()
		{
			this.uiTicketBuy.temp_IsClickBuy = false;
			this.uiTicketBuy.btnBuy.Init();
			this.uiTicketBuy.btnBuy.SetOnClick(new Action(this.OnBuyButtonClick));
			this.uiTicketBuy.btnBuy.SetItemIcon(2);
			this.uiTicketBuy.ticketBtn.m_onClick = delegate
			{
				this.OnClockTicket((int)this.openData.TicketType, this.uiTicketBuy.ticketBtn.transform.position);
			};
		}

		private void DeInitBuy()
		{
			this.uiTicketBuy.ticketBtn.m_onClick = null;
			this.uiTicketBuy.btnBuy.DeInit();
		}

		private void SetBuyCount(CommonTicketDailyExchangeTipModule.UITicketBuy uiTicketItem, int buyCount, bool showUpperLimitTip)
		{
			uiTicketItem.data_LeftBuyTimes = this.ticketDataModule.GetCanBuyCount(this.openData.TicketType);
			bool flag = buyCount > uiTicketItem.data_LeftBuyTimes;
			bool flag2 = this.IsHaveUpperLimit(uiTicketItem);
			if (flag)
			{
				uiTicketItem.data_CurBuyCount = (flag2 ? uiTicketItem.Cfg_MinBuyCount : uiTicketItem.data_LeftBuyTimes);
				if (showUpperLimitTip)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("8902"));
				}
			}
			else if (buyCount < uiTicketItem.Cfg_MinBuyCount)
			{
				uiTicketItem.data_CurBuyCount = uiTicketItem.Cfg_MinBuyCount;
			}
			else
			{
				uiTicketItem.data_CurBuyCount = buyCount;
			}
			this.ticketDataModule.GetTicketDataByBuyCount(this.openData.TicketType, uiTicketItem.data_CurBuyCount, flag2, out uiTicketItem.data_CurTicketPrice, out uiTicketItem.data_CurTicketCount);
		}

		private void FreshTicketBuy()
		{
			this.SetBuyCount(this.uiTicketBuy, this.uiTicketBuy.Cfg_MinBuyCount, false);
			this.RefreshUITicketItem(this.uiTicketBuy);
			this.RefreshBuyBtn();
		}

		private void RefreshBuyBtn()
		{
			string text = ((GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Diamonds >= this.uiTicketBuy.data_CurTicketPrice) ? this.uiTicketBuy.data_CurTicketPrice.ToString() : string.Format("<color=#ff0000ff>{0}</color>", this.uiTicketBuy.data_CurTicketPrice));
			this.uiTicketBuy.btnBuy.SetCountText(text, false);
			this.uiTicketBuy.btnBuy.SetGrayState(this.IsHaveUpperLimit(this.uiTicketBuy));
		}

		private void OnBuyButtonClick()
		{
			if (this.uiTicketBuy.temp_IsClickBuy)
			{
				return;
			}
			this.uiTicketBuy.temp_IsClickBuy = true;
			NetworkUtils.Ticket.DoShopBuyTicketsRequest(this.openData.TicketType, this.uiTicketBuy.data_CurTicketCount, delegate(bool isOk, ShopBuyTicketsResponse response)
			{
				this.uiTicketBuy.temp_IsClickBuy = false;
				if (isOk)
				{
					this.FreshTicketBuy();
				}
			});
		}

		private void InitAd()
		{
			this.uiTicketAd.temp_IsClickBuy = false;
			this.uiTicketAd.btnAd.m_onClick = new Action(this.OnAdButtonClick);
			this.uiTicketAd.ticketBtn.m_onClick = delegate
			{
				this.OnClockTicket((int)this.openData.TicketType, this.uiTicketAd.ticketBtn.transform.position);
			};
		}

		private void DeInitAd()
		{
			this.uiTicketAd.btnAd.m_onClick = null;
			this.uiTicketAd.ticketBtn.m_onClick = null;
		}

		private void SetAdCount(CommonTicketDailyExchangeTipModule.UITicketItem uiTicketItem)
		{
			uiTicketItem.data_LeftBuyTimes = this.ticketDailyExchangeDataModule.GetLeftAdWatchTimes(this.openData.TicketType);
			uiTicketItem.data_CurTicketCount = this.ticketDailyExchangeDataModule.GetNextAdWatchGetNum(this.openData.TicketType);
		}

		private void FreshTicketAd()
		{
			this.SetAdCount(this.uiTicketAd);
			this.RefreshUITicketItem(this.uiTicketAd);
			this.RefreshAdBtn();
		}

		private void RefreshAdBtn()
		{
			this.SetAdBtnGrayState(this.IsHaveUpperLimit(this.uiTicketAd));
		}

		private void SetAdBtnGrayState(bool isGray)
		{
			this.uiTicketAd.btnAd.enabled = !isGray;
			if (isGray)
			{
				this.uiTicketAd.uiGraysAd.SetUIGray();
				this.uiTicketAd.redNode.gameObject.SetActive(false);
				return;
			}
			this.uiTicketAd.uiGraysAd.Recovery();
			this.uiTicketAd.redNode.gameObject.SetActive(true);
		}

		private void OnAdButtonClick()
		{
			if (this.uiTicketAd.temp_IsClickBuy)
			{
				return;
			}
			int adId = this.ticketDailyExchangeDataModule.GetAdID(this.openData.TicketType);
			Action<bool, ADGetRewardResponse> <>9__1;
			AdBridge.PlayRewardVideo(adId, delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					this.uiTicketAd.temp_IsClickBuy = true;
					int num = 1;
					int adId2 = adId;
					Action<bool, ADGetRewardResponse> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(bool isOk, ADGetRewardResponse response)
						{
							this.uiTicketAd.temp_IsClickBuy = false;
							if (isOk)
							{
								this.FreshTicketAd();
							}
						});
					}
					NetworkUtils.TicketDailyExchange.ADGetRewardRequest(num, adId2, action);
				}
			});
		}

		private void InitGift()
		{
			this.uiGift.temp_IsClickBuy = false;
			this.uiGift.btnGift.m_onClick = new Action(this.OnGiftButtonClick);
			this.uiGift.giftItem.Init();
			this.FreshGiftItem();
		}

		private void DeInitGift()
		{
			this.uiGift.btnGift.m_onClick = null;
			this.uiGift.giftItem.DeInit();
		}

		private void FreshGift()
		{
			this.FreshGiftTitle();
			this.FreshGiftItem();
			this.FreshGiftTimeAndBtn();
		}

		private void FreshGiftTitle()
		{
			this.uiGift.title.text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Ticket_DailyExchange_DailyTile");
		}

		private void FreshGiftTimeAndBtn()
		{
			long freeAdLifeReFreshTime = this.ticketDailyExchangeDataModule.GetFreeAdLifeReFreshTime(this.openData.TicketType);
			if (freeAdLifeReFreshTime > DxxTools.Time.ServerTimestamp)
			{
				this.SetGiftBtnGrayState(true);
				this.uiGift.leftTimeObj.SetActive(true);
				int num = Mathf.Max(0, Mathf.CeilToInt((float)(freeAdLifeReFreshTime - DxxTools.Time.ServerTimestamp)));
				this.uiGift.leftTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("Common_Ticket_DailyExchange_DailyLeftTime", new object[] { DxxTools.FormatFullTimeWithDay((long)num) });
				return;
			}
			this.uiGift.leftTimeObj.SetActive(false);
			this.SetGiftBtnGrayState(false);
		}

		private void FreshGiftItem()
		{
			string[] array = GameApp.Table.GetManager().GetGameConfig_Config(2009).Value.Split(',', StringSplitOptions.None);
			ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
			this.uiGift.giftItem.SetData(itemData.ToPropData());
			this.uiGift.giftItem.OnRefresh();
		}

		private void SetGiftBtnGrayState(bool isGray)
		{
			this.uiGift.btnGift.enabled = !isGray;
			if (isGray)
			{
				this.uiGift.btnGetText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collected");
				this.uiGift.uiGraysGift.SetUIGray();
				this.uiGift.redNode.gameObject.SetActive(false);
				return;
			}
			this.uiGift.btnGetText.text = Singleton<LanguageManager>.Instance.GetInfoByID("btn_txt_state_collect");
			this.uiGift.uiGraysGift.Recovery();
			this.uiGift.redNode.gameObject.SetActive(true);
		}

		private void OnGiftButtonClick()
		{
			if (this.uiGift.temp_IsClickBuy)
			{
				return;
			}
			this.uiGift.temp_IsClickBuy = true;
			NetworkUtils.TicketDailyExchange.ADGetRewardRequest(0, 0, delegate(bool isOk, ADGetRewardResponse response)
			{
				this.uiGift.temp_IsClickBuy = false;
				if (isOk)
				{
					this.FreshGift();
				}
			});
		}

		[SerializeField]
		private UIPopCommon uiPopCommon;

		[SerializeField]
		private CommonTicketDailyExchangeTipModule.UITicketBuy uiTicketBuy;

		[SerializeField]
		private CommonTicketDailyExchangeTipModule.UITicketAd uiTicketAd;

		[SerializeField]
		private CommonTicketDailyExchangeTipModule.UIGift uiGift;

		private CommonTicketDailyExchangeTipModule.OpenData openData;

		private TicketDataModule ticketDataModule;

		private TicketDailyExchangeDataModule ticketDailyExchangeDataModule;

		private Item_Item itemTable;

		public struct OpenData
		{
			public UserTicketKind TicketType { readonly get; private set; }

			public void SetData(UserTicketKind ticketType)
			{
				this.TicketType = ticketType;
			}
		}

		[Serializable]
		public abstract class UITicketBase
		{
			[HideInInspector]
			public bool temp_IsClickBuy;
		}

		[Serializable]
		public abstract class UITicketItem : CommonTicketDailyExchangeTipModule.UITicketBase
		{
			public CustomButton ticketBtn;

			public CustomImage ticketIconBg;

			public CustomImage ticketIcon;

			public CustomText buyCountText;

			public CustomText leftTimesText;

			public bool dontFreshIcon;

			[HideInInspector]
			public int data_LeftBuyTimes;

			[HideInInspector]
			public int data_CurTicketCount;
		}

		[Serializable]
		public class UITicketBuy : CommonTicketDailyExchangeTipModule.UITicketItem
		{
			public UIItemInfoButton btnBuy;

			public int Cfg_MinBuyCount = 1;

			[HideInInspector]
			public int data_CurBuyCount;

			[HideInInspector]
			public int data_CurTicketPrice;
		}

		[Serializable]
		public class UITicketAd : CommonTicketDailyExchangeTipModule.UITicketItem
		{
			public CustomButton btnAd;

			public UIGrays uiGraysAd;

			public RedNodeOneCtrl redNode;
		}

		[Serializable]
		public class UIGift : CommonTicketDailyExchangeTipModule.UITicketBase
		{
			public CustomText title;

			public GameObject leftTimeObj;

			public CustomText leftTime;

			public UIItem giftItem;

			public CustomButton btnGift;

			public CustomText btnGetText;

			public UIGrays uiGraysGift;

			public RedNodeOneCtrl redNode;
		}
	}
}
