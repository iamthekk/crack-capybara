using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using LocalModels.Model;
using UnityEngine;

namespace HotFix
{
	public class ActivityShopViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.ActivityShopModule;
		}

		public override void OnCreate(object data)
		{
			this.Obj_NetLoading.SetActiveSafe(false);
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.buttonMask.m_onClick = new Action(this.OnClickClose);
			this.currencyCtrl.Init();
		}

		public override void OnDelete()
		{
			this.Button_Close.m_onClick = null;
			this.buttonMask.m_onClick = null;
			this.currencyCtrl.DeInit();
			foreach (UIActivityShopItem uiactivityShopItem in this.shopItemCacheList)
			{
				uiactivityShopItem.m_BuyButton.m_onClick = null;
				uiactivityShopItem.DeInit();
				Object.Destroy(uiactivityShopItem.gameObject);
			}
			this.shopItemCacheList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
		}

		public override void OnOpen(object data)
		{
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			ActivityWeekEntryViewModule viewModule = GameApp.View.GetViewModule(ViewName.ActivityWeekEntryModule);
			if (viewModule != null)
			{
				viewModule.SetBgVisible(false);
			}
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("week_activity_shop");
			string[] array = GameApp.Table.GetManager().GetGameConfig_Config(902).Value.Split(',', StringSplitOptions.None);
			for (int i = 0; i < this.currencyCtrl.itemCurrencyUICtrls.Count; i++)
			{
				int num;
				if (i < array.Length)
				{
					num = int.Parse(array[i]);
				}
				else
				{
					num = 0;
				}
				this.currencyCtrl.itemCurrencyUICtrls[i].SetData(num, "{0}");
			}
			this.currencyCtrl.FreshDataBinds(true);
			IList<CommonActivity_DropObj> allElements = GameApp.Table.GetManager().GetCommonActivity_DropObjModelInstance().GetAllElements();
			float time = Time.time;
			foreach (UIActivityShopItem uiactivityShopItem in this.shopItemCacheList)
			{
				uiactivityShopItem.gameObject.SetActive(false);
			}
			this.shopItemDic.Clear();
			for (int j = 0; j < allElements.Count; j++)
			{
				this.CreateShopItem(allElements[j], j, time);
			}
		}

		public override void OnClose()
		{
		}

		private void CreateShopItem(CommonActivity_DropObj table, int index, float statTime)
		{
			UIActivityShopItem uiactivityShopItem;
			if (index < this.shopItemCacheList.Count)
			{
				uiactivityShopItem = this.shopItemCacheList[index];
			}
			else
			{
				uiactivityShopItem = Object.Instantiate<UIActivityShopItem>(this.Item_Prefab, this.Item_Parent);
				this.shopItemCacheList.Add(uiactivityShopItem);
			}
			if (!uiactivityShopItem.gameObject.activeSelf)
			{
				uiactivityShopItem.gameObject.SetActive(true);
			}
			this.shopItemDic.Add(uiactivityShopItem, table);
			uiactivityShopItem.Init();
			string[] array = table.ObjGoods[0].Split(',', StringSplitOptions.None);
			ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
			uiactivityShopItem.item.SetData(itemData.ToPropData());
			uiactivityShopItem.item.onClick = new Action<UIItem, PropData, object>(this.OnClickItem);
			uiactivityShopItem.item.OnRefresh();
			uiactivityShopItem.itemName.text = Singleton<LanguageManager>.Instance.GetInfoByID(table.ObjName);
			int bugId = table.ObjPrice1;
			this.FreshShopItemCost(uiactivityShopItem, table);
			uiactivityShopItem.m_BuyButton.m_onClick = delegate
			{
				if (this.propDataModule.GetItemDataCountByid((ulong)((long)bugId)) >= (long)table.ObjPrice2)
				{
					NetworkUtils.ActivityWeek.RequestActiveDropBuy(0, table.Id, null);
					return;
				}
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("100000110"));
			};
			uiactivityShopItem.gameObject.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(statTime, index, 100025);
		}

		private void FreshShopItemCost(UIActivityShopItem shopItem, CommonActivity_DropObj table)
		{
			Item_ItemModel item_ItemModelInstance = GameApp.Table.GetManager().GetItem_ItemModelInstance();
			int objPrice = table.ObjPrice1;
			Item_Item elementById = item_ItemModelInstance.GetElementById(objPrice);
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)objPrice));
			shopItem.buyCostImage.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			shopItem.buyCostText.text = "x" + table.ObjPrice2.ToString();
			shopItem.buyCostText.color = ((itemDataCountByid < (long)table.ObjPrice2) ? this.colorPriceUnEnough : Color.white);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(this.GetName(), null);
			ActivityWeekEntryViewModule viewModule = GameApp.View.GetViewModule(ViewName.ActivityWeekEntryModule);
			if (viewModule == null)
			{
				return;
			}
			viewModule.SetBgVisible(true);
		}

		private void OnClickItem(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, item.transform.position, 70f);
		}

		private void Event_ItemUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsItemUpdate eventArgsItemUpdate = eventArgs as EventArgsItemUpdate;
			if (eventArgsItemUpdate != null)
			{
				foreach (KeyValuePair<UIActivityShopItem, CommonActivity_DropObj> keyValuePair in this.shopItemDic)
				{
					if (keyValuePair.Value.ObjPrice1 == eventArgsItemUpdate.itemId)
					{
						this.FreshShopItemCost(keyValuePair.Key, keyValuePair.Value);
					}
				}
			}
		}

		public CustomButton buttonMask;

		public UIItemCurrencyCollection currencyCtrl;

		public Color colorPriceUnEnough = Color.red;

		public CustomText Text_Title;

		public CustomButton Button_Close;

		public GameObject Obj_NetLoading;

		public UIActivityShopItem Item_Prefab;

		public RectTransform Item_Parent;

		private PropDataModule propDataModule;

		private Dictionary<UIActivityShopItem, CommonActivity_DropObj> shopItemDic = new Dictionary<UIActivityShopItem, CommonActivity_DropObj>();

		private List<UIActivityShopItem> shopItemCacheList = new List<UIActivityShopItem>();
	}
}
