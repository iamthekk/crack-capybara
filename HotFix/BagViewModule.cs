using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BagViewModule : BaseViewModule
	{
		public override void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Bag_UpdateList, new HandlerEvent(this.EventUpdateList));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Bag_UpdateList, new HandlerEvent(this.EventUpdateList));
		}

		private void EventUpdateList(object sender, int type, BaseEventArgs eventArgs)
		{
			PropShowType propShowType = this.mShowType;
			this.mShowType = PropShowType.eNull;
			this.refreshScroll(propShowType);
		}

		public override void OnCreate(object data)
		{
			this.Obj_Empty.SetActive(false);
			this.m_uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.Button_All.onClick.AddListener(new UnityAction(this.OnClickAllBt));
			this.Button_Equip.onClick.AddListener(new UnityAction(this.OnClickEquipBt));
			this.Button_Item.onClick.AddListener(new UnityAction(this.OnClickPropBt));
			this.Scroll.InitGridView(0, new Func<LoopGridView, int, int, int, LoopGridViewItem>(this.OnGetItemByIndex), null, null);
			this.initScroll();
		}

		private void initScroll()
		{
			this.buttonsAllNormal();
			this.mShowType = PropShowType.eAll;
			this.refreshScroll(this.mShowType);
		}

		private void buttonsAllNormal()
		{
			this.Button_All.SetSelect(this.mShowType == PropShowType.eAll);
			this.Button_Item.SetSelect(this.mShowType == PropShowType.eProp);
			this.Button_Equip.SetSelect(this.mShowType == PropShowType.eEquip);
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as BagViewModule.OpenData;
			this.mShowType = PropShowType.eNull;
			this.refreshScroll(PropShowType.eAll);
		}

		private void refreshScroll(PropShowType showType)
		{
			if (this.mShowType == showType)
			{
				return;
			}
			this.mShowType = showType;
			this.buttonsAllNormal();
			this.mDataList = this.getProps(showType);
			this.Obj_Empty.SetActive(this.mDataList.Count == 0);
			this.Scroll.SetListItemCount(this.mDataList.Count, true);
			this.Scroll.RefreshAllShownItem();
			this.PlayScale();
		}

		private List<PropData> getProps(PropShowType showType)
		{
			return GameApp.Data.GetDataModule<PropDataModule>(118).GetBagList(showType);
		}

		private LoopGridViewItem OnGetItemByIndex(LoopGridView view, int index, int row, int column)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			PropData propData = this.mDataList[index];
			LoopGridViewItem loopGridViewItem = view.NewListViewItem("Item");
			int instanceID = loopGridViewItem.gameObject.GetInstanceID();
			UIItem uiitem = this.TryGetUI(instanceID);
			if (uiitem == null)
			{
				uiitem = this.TryAddUI(instanceID, loopGridViewItem, loopGridViewItem.GetComponent<UIItem>());
			}
			uiitem.SetData(propData);
			uiitem.SetActive(true);
			uiitem.OnRefresh();
			return loopGridViewItem;
		}

		private UIItem TryGetUI(int key)
		{
			UIItem uiitem;
			if (this.UICtrlDic.TryGetValue(key, out uiitem))
			{
				return uiitem;
			}
			return null;
		}

		private UIItem TryAddUI(int key, LoopGridViewItem loopitem, UIItem ui)
		{
			ui.Init();
			ui.SetCountShowType(UIItem.CountShowType.MissOne);
			ui.onClick = new Action<UIItem, PropData, object>(this.onClickItem);
			UIItem uiitem;
			if (this.UICtrlDic.TryGetValue(key, out uiitem))
			{
				if (uiitem == null)
				{
					uiitem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnClickCloseBt();
			}
		}

		private void OnClickCloseBt()
		{
			if (this.m_openData != null)
			{
				MoreExtensionViewModule.TryBackOpenView(this.m_openData.srcViewName);
			}
			GameApp.View.CloseView(ViewName.BagViewModule, null);
		}

		private void OnClickPropBt()
		{
			this.refreshScroll(PropShowType.eProp);
		}

		private void OnClickEquipBt()
		{
			this.refreshScroll(PropShowType.eEquip);
		}

		private void OnClickAllBt()
		{
			this.refreshScroll(PropShowType.eAll);
		}

		private void onClickItem(UIItem item, PropData data, object arg)
		{
			RectTransform rectTransform = item.transform.GetChild(0) as RectTransform;
			float num = rectTransform.rect.height * rectTransform.localScale.y * 0.5f + 10f;
			Vector3 position = rectTransform.position;
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eBag,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, position, num);
		}

		public override void OnClose()
		{
			if (this.m_openData != null)
			{
				Action onCloseCallback = this.m_openData.onCloseCallback;
				if (onCloseCallback != null)
				{
					onCloseCallback();
				}
				this.m_openData = null;
			}
			this.m_seqPool.Clear(false);
			foreach (KeyValuePair<int, UIItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
			this.mDataList.Clear();
		}

		public override void OnDelete()
		{
			this.m_seqPool.Clear(false);
			this.m_uiPopCommon.OnClick = null;
			CustomChooseButton button_All = this.Button_All;
			if (button_All != null)
			{
				button_All.onClick.RemoveListener(new UnityAction(this.OnClickAllBt));
			}
			CustomChooseButton button_Equip = this.Button_Equip;
			if (button_Equip != null)
			{
				button_Equip.onClick.RemoveListener(new UnityAction(this.OnClickEquipBt));
			}
			CustomChooseButton button_Item = this.Button_Item;
			if (button_Item == null)
			{
				return;
			}
			button_Item.onClick.RemoveListener(new UnityAction(this.OnClickPropBt));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			for (int i = 0; i < this.Scroll.ItemTotalCount; i++)
			{
				LoopGridViewItem shownItemByItemIndex = this.Scroll.GetShownItemByItemIndex(i);
				if (!(shownItemByItemIndex == null))
				{
					Transform child = shownItemByItemIndex.CachedRectTransform.GetChild(0);
					if (!(child == null))
					{
						DxxTools.UI.DoScaleAnim(this.m_seqPool.Get(), child, 0f, this.itemScale, 0.02f * (float)i, 0.2f, 0);
					}
				}
			}
		}

		public UIPopCommon m_uiPopCommon;

		public CustomChooseButton Button_All;

		public CustomChooseButton Button_Equip;

		public CustomChooseButton Button_Item;

		public GameObject Obj_Empty;

		public float itemScale = 0.8f;

		public LoopGridView Scroll;

		private PropShowType mShowType;

		private List<PropData> mDataList = new List<PropData>();

		public Dictionary<int, UIItem> UICtrlDic = new Dictionary<int, UIItem>();

		private SequencePool m_seqPool = new SequencePool();

		private BagViewModule.OpenData m_openData;

		public class OpenData
		{
			public ViewName srcViewName;

			public Action onCloseCallback;
		}
	}
}
