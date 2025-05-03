using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIItemCurrencyCollection : CustomBehaviour
	{
		public List<RectTransform> layoutTrans { get; private set; } = new List<RectTransform>();

		public List<ItemCurrencyUICtrl> itemCurrencyUICtrls { get; private set; } = new List<ItemCurrencyUICtrl>();

		public Dictionary<int, ItemCurrencyUICtrl> ItemBinds { get; private set; } = new Dictionary<int, ItemCurrencyUICtrl>();

		protected override void OnInit()
		{
			this.layoutTrans.Clear();
			List<LayoutGroup> list = new List<LayoutGroup>();
			base.gameObject.GetComponentsInChildren<LayoutGroup>(true, list);
			for (int i = 0; i < list.Count; i++)
			{
				this.layoutTrans.Add(list[i].transform as RectTransform);
			}
			base.gameObject.GetComponentsInChildren<ItemCurrencyUICtrl>(true, this.itemCurrencyUICtrls);
			foreach (ItemCurrencyUICtrl itemCurrencyUICtrl in this.itemCurrencyUICtrls)
			{
				itemCurrencyUICtrl.Init();
				itemCurrencyUICtrl.OnClick = new Action<ItemCurrencyUICtrl>(this.OnClickItem);
			}
			this.FreshDataBinds(true);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			this.FreshAllUI();
		}

		protected override void OnDeInit()
		{
			foreach (ItemCurrencyUICtrl itemCurrencyUICtrl in this.itemCurrencyUICtrls)
			{
				itemCurrencyUICtrl.OnClick = null;
				itemCurrencyUICtrl.DeInit();
			}
			this.itemCurrencyUICtrls.Clear();
			this.ItemBinds.Clear();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
		}

		private void OnClickItem(ItemCurrencyUICtrl itemCtrl)
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

		private void Event_ItemUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsItemUpdate eventArgsItemUpdate = eventArgs as EventArgsItemUpdate;
			if (eventArgsItemUpdate != null)
			{
				this.FreshUI(eventArgsItemUpdate.itemId, true);
			}
		}

		public void FreshDataBinds(bool freshAllUI = true)
		{
			this.ItemBinds.Clear();
			for (int i = 0; i < this.itemCurrencyUICtrls.Count; i++)
			{
				if (this.itemCurrencyUICtrls[i].ItemId > 0)
				{
					this.ItemBinds.Add(this.itemCurrencyUICtrls[i].ItemId, this.itemCurrencyUICtrls[i]);
					this.itemCurrencyUICtrls[i].gameObject.SetActiveSafe(true);
				}
				else
				{
					this.itemCurrencyUICtrls[i].gameObject.SetActiveSafe(false);
				}
			}
			if (freshAllUI)
			{
				this.FreshAllUI();
			}
		}

		public void FreshUI(int itemId, bool freshAllLayout = true)
		{
			ItemCurrencyUICtrl itemCurrencyUICtrl;
			if (this.ItemBinds.TryGetValue(itemId, out itemCurrencyUICtrl))
			{
				itemCurrencyUICtrl.FreshAll();
			}
			if (freshAllLayout)
			{
				this.FreshAllLayout();
			}
		}

		public void FreshAllUI()
		{
			foreach (ItemCurrencyUICtrl itemCurrencyUICtrl in this.ItemBinds.Values)
			{
				itemCurrencyUICtrl.FreshAll();
			}
			this.FreshAllLayout();
		}

		public void FreshAllLayout()
		{
			for (int i = 0; i < this.layoutTrans.Count; i++)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(this.layoutTrans[i]);
			}
		}
	}
}
