using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIActivityCurrency : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.updateItemObj();
			this.m_item.Init();
			this.m_item.OnClick = new Action<ItemCurrencyUICtrl>(this.OnClickOther);
			this.m_diamondUI.Init();
			this.m_diamondUI.CurrencyType = CurrencyType.Diamond;
			this.m_diamondUI.OnClick = new Action<CurrencyType>(this.OnClickDiamonds);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			this.OnRefreshUI();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_topRightTrans);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_topRightTrans);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			this.m_item.OnClick = null;
			this.m_item.DeInit();
			if (this.m_diamondUI != null)
			{
				this.m_diamondUI.DeInit();
			}
		}

		public void SetItemId(int itemId = 0)
		{
			this.m_otherItemId = itemId;
			this.m_item.ItemId = itemId;
			this.updateItemObj();
		}

		private void updateItemObj()
		{
			this.m_item.SetFresh(this.m_otherItemId, "{0}");
		}

		private void OnClickOther(ItemCurrencyUICtrl itemCtrl)
		{
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemCtrl.ItemId));
			ItemData itemData = new ItemData(itemCtrl.ItemId, itemDataCountByid);
			ItemInfoOpenData itemInfoOpenData = new ItemInfoOpenData();
			itemInfoOpenData.m_propData = itemData.ToPropData();
			itemInfoOpenData.m_openDataType = ItemInfoOpenDataType.eBag;
			itemInfoOpenData.m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume);
			Vector3 position = itemCtrl.Icon.transform.position;
			DxxTools.UI.ShowItemInfo(itemInfoOpenData, position, 30f);
		}

		private void OnClickDiamonds(CurrencyType type)
		{
			Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop, null);
		}

		private void Event_ItemUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsItemUpdate eventArgsItemUpdate = eventArgs as EventArgsItemUpdate;
			if (eventArgsItemUpdate != null && eventArgsItemUpdate.itemId == this.m_otherItemId)
			{
				this.updateItemNum();
			}
		}

		private void Event_SetDiamond(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsLong eventArgsLong = eventArgs as EventArgsLong;
			this.setDiamond(eventArgsLong.Value);
		}

		private void Event_Update(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.OnRefreshUI();
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshUI();
		}

		private void updateItemNum()
		{
			this.m_item.FreshNum();
		}

		private void setDiamond(long value)
		{
			this.m_diamondUI.SetText(DxxTools.FormatNumber(value));
		}

		public void OnRefreshUI()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.setDiamond((long)dataModule.userCurrency.Diamonds);
			this.updateItemNum();
		}

		public RectTransform m_topRightTrans;

		public CurrencyUICtrl m_diamondUI;

		public ItemCurrencyUICtrl m_item;

		private int m_otherItemId;
	}
}
