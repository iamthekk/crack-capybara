using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ItemResourcesViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.ScrollView_Content.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemIndex), null);
		}

		private void OnClickClose()
		{
			GameApp.View.CloseView(ViewName.ItemResourcesViewModule, null);
		}

		public override void OnOpen(object data)
		{
			this.m_itemId = (int)data;
			this.m_itemResourcesCfg = GameApp.Table.GetManager().GetItemResources_itemget(this.m_itemId);
			if (this.m_itemResourcesCfg == null)
			{
				return;
			}
			this.OnRefreshView();
		}

		private void OnRefreshView()
		{
			this.Text_Title.text = Singleton<LanguageManager>.Instance.GetInfoByID("item_resources_1");
			this.Text_ContentTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID("item_resources_2");
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.m_itemId));
			this.Text_ItemNum.text = string.Format(Singleton<LanguageManager>.Instance.GetInfoByID("item_resources_3", new object[] { DxxTools.FormatNumber(itemDataCountByid) }), Array.Empty<object>());
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(this.m_itemId);
			if (item_Item != null)
			{
				this.Text_ItemName.text = Singleton<LanguageManager>.Instance.GetInfoByID(item_Item.nameID);
			}
			PropData propData = new PropData();
			propData.id = (uint)this.m_itemId;
			propData.count = (ulong)itemDataCountByid;
			UIItem component = this.GoodsItem.GetComponent<UIItem>();
			if (component != null)
			{
				component.Init();
			}
			if (component != null)
			{
				component.SetData(propData);
			}
			if (component != null)
			{
				component.OnRefresh();
			}
			this.OnRefreshList();
		}

		private void OnRefreshList()
		{
			this.m_itemDataList.Clear();
			this.itemGetIdList.Clear();
			if (this.m_itemId == 28)
			{
				if (LoginDataModule.IsTestB())
				{
					this.itemGetIdList.Add(24);
					this.itemGetIdList.Add(31);
				}
				else
				{
					this.itemGetIdList.Add(23);
					this.itemGetIdList.Add(24);
				}
			}
			else if (this.m_itemId == 29)
			{
				if (LoginDataModule.IsTestB())
				{
					this.itemGetIdList.Add(25);
					this.itemGetIdList.Add(26);
					this.itemGetIdList.Add(32);
				}
				else
				{
					this.itemGetIdList.Add(25);
					this.itemGetIdList.Add(26);
				}
			}
			else
			{
				for (int i = 0; i < this.m_itemResourcesCfg.itemGet.Length; i++)
				{
					this.itemGetIdList.Add(int.Parse(this.m_itemResourcesCfg.itemGet[i]));
				}
			}
			for (int j = 0; j < this.itemGetIdList.Count; j++)
			{
				int num = this.itemGetIdList[j];
				ItemResources_jumpResource itemResources_jumpResource = GameApp.Table.GetManager().GetItemResources_jumpResource(num);
				if (itemResources_jumpResource != null)
				{
					int jumpType = itemResources_jumpResource.jumpType;
					int paramsId = itemResources_jumpResource.jumpId;
					if (jumpType == ItemResourcesType.Gift.GetHashCode())
					{
						List<IAP_pushIap> list = (from x in GameApp.Table.GetManager().GetIAP_pushIapElements()
							where x.@group == paramsId
							select x).ToList<IAP_pushIap>();
						PushGiftDataModule dataModule = GameApp.Data.GetDataModule(DataName.PushGiftDataModule);
						if (dataModule != null && dataModule.OnGetSupplyData(list[0].conditionParams) != null && Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Shop, false))
						{
							this.m_itemDataList.Add(num);
						}
					}
					else
					{
						this.m_itemDataList.Add(num);
					}
				}
			}
			this.ScrollView_Content.SetListItemCount(this.m_itemDataList.Count + 1, false);
			this.ScrollView_Content.RefreshAllShowItems();
		}

		private LoopListViewItem2 OnGetItemIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			if (index > this.itemGetIdList.Count)
			{
				return null;
			}
			if (index == 0)
			{
				LoopListViewItem2 loopListViewItem = listView.NewListViewItem("ItemSpace");
				CustomBehaviour customBehaviour;
				this.m_Items.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.m_Items[loopListViewItem.gameObject.GetInstanceID()] = component;
				}
				return loopListViewItem;
			}
			int num = this.m_itemDataList[index - 1];
			if (GameApp.Table.GetManager().GetItemResources_jumpResource(num).jumpType == ItemResourcesType.Gift.GetHashCode())
			{
				LoopListViewItem2 loopListViewItem2 = listView.NewListViewItem("ItemResourcesItem2");
				CustomBehaviour component2;
				this.m_Items.TryGetValue(loopListViewItem2.gameObject.GetInstanceID(), out component2);
				if (component2 == null)
				{
					component2 = loopListViewItem2.gameObject.GetComponent<ItemResourcesItem>();
					component2.Init();
					this.m_Items[loopListViewItem2.gameObject.GetInstanceID()] = component2;
				}
				ItemResourcesItem component3 = component2.GetComponent<ItemResourcesItem>();
				if (component3 != null)
				{
					component3.SetData(this.m_itemResourcesCfg, num, new Action<bool>(this.OnBuySupplyGiftSuccess), new Action(this.OnBuySupplyGiftClose));
				}
				return loopListViewItem2;
			}
			LoopListViewItem2 loopListViewItem3 = listView.NewListViewItem("ItemResourcesItem1");
			CustomBehaviour component4;
			this.m_Items.TryGetValue(loopListViewItem3.gameObject.GetInstanceID(), out component4);
			if (component4 == null)
			{
				component4 = loopListViewItem3.gameObject.GetComponent<ItemResourcesItem>();
				component4.Init();
				this.m_Items[loopListViewItem3.gameObject.GetInstanceID()] = component4;
			}
			ItemResourcesItem component5 = component4.GetComponent<ItemResourcesItem>();
			if (component5 != null)
			{
				component5.SetData(this.m_itemResourcesCfg, num, new Action<bool>(this.OnBuySupplyGiftSuccess), new Action(this.OnBuySupplyGiftClose));
			}
			return loopListViewItem3;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void OnBuySupplyGiftSuccess(bool isOk)
		{
		}

		private void OnBuySupplyGiftClose()
		{
			this.OnRefreshList();
		}

		private void OnTicketUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)this.m_itemId));
			this.Text_ItemNum.text = Singleton<LanguageManager>.Instance.GetInfoByID("item_resources_3", new object[] { DxxTools.FormatNumber(itemDataCountByid) });
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_Items)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_Items.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.UnRegisterEvents(null);
			this.Button_Close.onClick.AddListener(new UnityAction(this.OnClickClose));
			this.Button_Mask.onClick.AddListener(new UnityAction(this.OnClickClose));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.Button_Close.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			this.Button_Mask.onClick.RemoveListener(new UnityAction(this.OnClickClose));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Item_Update.GetHashCode(), new HandlerEvent(this.OnTicketUpdate));
		}

		public CustomText Text_Title;

		public CustomText Text_ItemName;

		public CustomText Text_ItemNum;

		public CustomText Text_ContentTitle;

		public LoopListView2 ScrollView_Content;

		public GameObject GoodsItem;

		[Header("按钮")]
		public CustomButton Button_Close;

		public CustomButton Button_Mask;

		private int m_itemId;

		private ItemResources_itemget m_itemResourcesCfg;

		private Dictionary<int, CustomBehaviour> m_Items = new Dictionary<int, CustomBehaviour>();

		private List<int> m_itemDataList = new List<int>();

		private List<int> itemGetIdList = new List<int>();
	}
}
