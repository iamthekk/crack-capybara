using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonTicketBuyTipModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.addButton.onClick.AddListener(new UnityAction(this.AddCountOne));
			this.subButton.onClick.AddListener(new UnityAction(this.SubCountOne));
			this.addButtonTen.onClick.AddListener(new UnityAction(this.AddCountTen));
			this.subButtonTen.onClick.AddListener(new UnityAction(this.SubCountTen));
		}

		public override void OnOpen(object data)
		{
			this.isClickBuy = false;
			this.openData = (CommonTicketBuyTipModule.OpenData)data;
			this.dataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.itemTable = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.openData.TicketType);
			this.uiItemInfoButton.Init();
			this.uiItemInfoButton.SetOnClick(new Action(this.OnBuyButtonClick));
			this.uiItemInfoButton.SetItemIcon(2);
			this.SetBuyCount(1, false);
			this.RefreshView();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.uiItemInfoButton.DeInit();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnIAPSuccess));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.SC_IAPBuySupplyGiftSuccess, new HandlerEvent(this.OnIAPSuccess));
		}

		private void RefreshView()
		{
			int maxTicketCount = this.dataModule.GetTicketTableData(this.openData.TicketType).MaxTicketCount;
			bool flag = this.IsHaveUpperLimit();
			this.upperLimitText.SetText(8904, string.Format("{0}/{1}", maxTicketCount, maxTicketCount));
			this.upperLimitText.gameObject.SetActive(flag);
			this.uiItemInfoButton.SetActive(!flag);
			this.RefreshTicketIcon();
			this.RefreshBuyCount();
		}

		private void RefreshTicketIcon()
		{
			string text = GameApp.Table.GetAtlasPath(this.itemTable.atlasID);
			this.ticketIcon.SetImage(text, this.itemTable.icon);
			Quality_itemQuality elementById = GameApp.Table.GetManager().GetQuality_itemQualityModelInstance().GetElementById(this.itemTable.quality);
			if (elementById != null)
			{
				text = GameApp.Table.GetAtlasPath(elementById.atlasId);
				this.ticketIconBg.SetImage(text, elementById.bgSpriteName);
			}
		}

		private void RefreshBuyCount()
		{
			int diamonds = GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Diamonds;
			this.buyCountText.text = this.curTicketCount.ToString();
			this.uiItemInfoButton.SetInfoText(string.Format("{0}X{1}", Singleton<LanguageManager>.Instance.GetInfoByID("8901"), this.curTicketCount));
			this.buyItemInfo.text = string.Format("{0}x{1}", Singleton<LanguageManager>.Instance.GetInfoByID(this.itemTable.nameID), this.curTicketCount);
			string text = ((diamonds >= this.curTicketPrice) ? this.curTicketPrice.ToString() : string.Format("<color=#ff0000ff>{0}</color>", this.curTicketPrice));
			this.uiItemInfoButton.SetCountText(text, false);
			bool flag = this.curTicketCount <= 0;
			this.uiItemInfoButton.SetGrayState(flag);
		}

		private ViewName GetName()
		{
			return ViewName.CommonTicketBuyTipModule;
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

		private void AddCountOne()
		{
			this.AddCount(1);
		}

		private void SubCountOne()
		{
			this.AddCount(-1);
		}

		private void AddCountTen()
		{
			this.AddCount(10);
		}

		private void SubCountTen()
		{
			this.AddCount(-10);
		}

		private void AddCount(int count)
		{
			this.SetBuyCount(this.curBuyCount + count, true);
			this.RefreshBuyCount();
		}

		private void SetBuyCount(int buyCount, bool showUpperLimitTip)
		{
			int canBuyCount = this.dataModule.GetCanBuyCount(this.openData.TicketType);
			bool flag = buyCount > canBuyCount;
			bool flag2 = this.IsHaveUpperLimit();
			if (flag)
			{
				this.curBuyCount = (flag2 ? 1 : canBuyCount);
				if (showUpperLimitTip)
				{
					GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("8902"));
				}
			}
			else if (buyCount < 1)
			{
				this.curBuyCount = 1;
			}
			else
			{
				this.curBuyCount = buyCount;
			}
			this.dataModule.GetTicketDataByBuyCount(this.openData.TicketType, this.curBuyCount, flag2, out this.curTicketPrice, out this.curTicketCount);
		}

		private void OnBuyButtonClick()
		{
			if (this.isClickBuy)
			{
				return;
			}
			this.isClickBuy = true;
			NetworkUtils.Ticket.DoShopBuyTicketsRequest(this.openData.TicketType, this.curTicketCount, delegate(bool isOk, ShopBuyTicketsResponse response)
			{
				this.isClickBuy = false;
				if (isOk)
				{
					this.CloseSelf();
				}
			});
		}

		private bool IsHaveUpperLimit()
		{
			return this.dataModule.GetCanBuyCount(this.openData.TicketType) <= 0;
		}

		private void OnIAPSuccess(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshView();
		}

		private const int MinBuyCount = 1;

		[SerializeField]
		private CustomImage ticketIconBg;

		[SerializeField]
		private CustomImage ticketIcon;

		[SerializeField]
		private UIPopCommon uiPopCommon;

		[SerializeField]
		private CustomButton addButton;

		[SerializeField]
		private CustomButton subButton;

		[SerializeField]
		private CustomButton addButtonTen;

		[SerializeField]
		private CustomButton subButtonTen;

		[SerializeField]
		private CustomText buyCountText;

		[SerializeField]
		private CustomText upperLimitText;

		[SerializeField]
		private CustomText buyItemInfo;

		[SerializeField]
		private UIItemInfoButton uiItemInfoButton;

		private CommonTicketBuyTipModule.OpenData openData;

		private TicketDataModule dataModule;

		private Item_Item itemTable;

		private int curBuyCount;

		private int curTicketPrice;

		private int curTicketCount;

		private bool isClickBuy;

		public struct OpenData
		{
			public UserTicketKind TicketType { readonly get; private set; }

			public void SetData(UserTicketKind ticketType)
			{
				this.TicketType = ticketType;
			}
		}
	}
}
