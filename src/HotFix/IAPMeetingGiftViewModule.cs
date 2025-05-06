using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPMeetingGiftViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.dayItems.Count; i++)
			{
				this.dayItems[i].Init();
			}
			for (int j = 0; j < this.buttonItems.Count; j++)
			{
				this.buttonItems[j].Init();
				this.buttonItems[j].SetData(j, new Action<int>(this.OnSelectTag));
			}
			HeroDataModule dataModule = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			this.memberId = dataModule.MainCardData.m_memberID;
		}

		public override void OnOpen(object data)
		{
			this.selectIndex = 0;
			this.modelItem.Init();
			this.modelItem.OnShow();
			List<IAP_PushPacks> gifts = this.iapDataModule.MeetingGift.GetGifts();
			for (int i = 0; i < this.buttonItems.Count; i++)
			{
				if (i < gifts.Count)
				{
					IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(gifts[i].id);
					if (elementById != null && elementById.price1 > 0f)
					{
						string priceForPlatformID = GameApp.Purchase.Manager.GetPriceForPlatformID(elementById.platformID);
						this.buttonItems[i].SetPrice(priceForPlatformID);
					}
				}
			}
			this.OnSelectTag(this.selectIndex);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			for (int i = 0; i < this.dayItems.Count; i++)
			{
				this.dayItems[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.modelItem.OnHide(false);
			this.modelItem.DeInit();
		}

		public override void OnDelete()
		{
			this.buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			for (int i = 0; i < this.dayItems.Count; i++)
			{
				this.dayItems[i].DeInit();
			}
			for (int j = 0; j < this.buttonItems.Count; j++)
			{
				this.buttonItems[j].DeInit();
			}
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_MeetingGift_RefreshUI, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_MeetingGift_ChangDay, new HandlerEvent(this.OnEventChangDay));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_MeetingGift_RefreshUI, new HandlerEvent(this.OnEventRefreshUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_MeetingGift_ChangDay, new HandlerEvent(this.OnEventChangDay));
		}

		private void OnEventRefreshUI(object sender, int type, BaseEventArgs eventArgs)
		{
			this.Refresh();
		}

		private void OnEventChangDay(object sender, int type, BaseEventArgs eventArgs)
		{
			RedPointController.Instance.ReCalc("IAPGift.Meeting", true);
			this.Refresh();
		}

		private void Refresh()
		{
			List<IAP_PushPacks> gifts = this.iapDataModule.MeetingGift.GetGifts();
			if (this.selectIndex >= gifts.Count)
			{
				return;
			}
			IAP_PushPacks iap_PushPacks = gifts[this.selectIndex];
			this.textTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(iap_PushPacks.nameID);
			this.textValue.text = string.Format("{0}%", iap_PushPacks.valueNum);
			for (int i = 0; i < this.nodes.Count; i++)
			{
				this.nodes[i].SetActiveSafe(i == this.selectIndex);
			}
			if (this.selectIndex < this.parents.Count)
			{
				this.modelItem.transform.SetParentNormal(this.parents[this.selectIndex], false);
			}
			for (int j = 0; j < this.dayItems.Count; j++)
			{
				int num = j + 1;
				this.dayItems[j].SetData(iap_PushPacks, num);
			}
			bool flag = this.iapDataModule.MeetingGift.IsBought(iap_PushPacks.id);
			this.payButtonCtrl.gameObject.SetActiveSafe(!flag);
			this.payButtonCtrl.SetData(iap_PushPacks.id, "", new Action<bool>(this.OnPaySuccess), new Action(this.OnCloseReward));
			int num2 = 0;
			ClothesData clothesData = new ClothesData();
			switch (this.selectIndex)
			{
			case 0:
				clothesData.HeadId = 1004;
				break;
			case 1:
				num2 = 210304;
				break;
			case 2:
				clothesData.BodyId = 1016;
				break;
			}
			bool flag2 = this.iapDataModule.MeetingGift.IsBoughtAny();
			for (int k = 0; k < this.buttonItems.Count; k++)
			{
				if (flag2)
				{
					this.buttonItems[k].gameObject.SetActiveSafe(true);
				}
				else if (k != 0)
				{
					this.buttonItems[k].gameObject.SetActiveSafe(false);
				}
				if (k < gifts.Count)
				{
					bool flag3 = this.iapDataModule.MeetingGift.IsCanGet(gifts[k].id);
					this.buttonItems[k].SetRedPoint(flag3);
				}
				else
				{
					this.buttonItems[k].SetRedPoint(false);
				}
			}
			Dictionary<SkinType, SkinData> skinDatas = clothesData.GetSkinDatas();
			this.modelItem.ShowPlayerModel("UIMeetingGift", this.memberId, num2, skinDatas, 0);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.MeetingGiftViewModule, null);
			if (this.iapDataModule.MeetingGift.IsAllEnd())
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_MeetingGift_RefreshEnter, null);
			}
		}

		private void OnSelectTag(int index)
		{
			this.selectIndex = index;
			for (int i = 0; i < this.buttonItems.Count; i++)
			{
				this.buttonItems[i].SetSelect(this.selectIndex);
			}
			this.Refresh();
		}

		private void OnPaySuccess(bool isOk)
		{
			if (isOk)
			{
				this.Refresh();
			}
		}

		private void OnCloseReward()
		{
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false) && Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen() && !GameApp.View.IsOpened(ViewName.FunctionOpenViewModule))
			{
				GameApp.View.OpenView(ViewName.FunctionOpenViewModule, null, 2, null, null);
			}
		}

		public List<GameObject> nodes;

		public List<Transform> parents;

		public List<UIMeetingDayItem> dayItems;

		public CustomText textTitle;

		public CustomText textValue;

		public UIModelItem modelItem;

		public PurchaseButtonCtrl payButtonCtrl;

		public List<UIMeetingButtonItem> buttonItems;

		public CustomButton buttonClose;

		private IAPDataModule iapDataModule;

		private int selectIndex;

		private int memberId;

		private const int SHOW_WEAPON_ID = 210304;

		private const int SHOW_HEAD_ID = 1004;

		private const int SHOW_BODY_ID = 1016;
	}
}
