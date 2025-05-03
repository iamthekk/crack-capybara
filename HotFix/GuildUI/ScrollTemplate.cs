using System;
using System.Collections.Generic;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	internal class ScrollTemplate
	{
		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count + 2)
			{
				return null;
			}
			if (index < 1 || index + 1 >= this.mDataList.Count + 2)
			{
				return listView.NewListViewItem("EmptyItem");
			}
			index--;
			ScrollTemplateItemData scrollTemplateItemData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("CorssArenaRecord_Item");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			ScrollTemplateItemUI scrollTemplateItemUI = this.TryGetUI(instanceID);
			ScrollTemplateItemUI component = loopListViewItem.GetComponent<ScrollTemplateItemUI>();
			if (scrollTemplateItemUI == null)
			{
				scrollTemplateItemUI = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			scrollTemplateItemUI.SetData(scrollTemplateItemData);
			scrollTemplateItemUI.SetActive(true);
			scrollTemplateItemUI.RefreshUI();
			return loopListViewItem;
		}

		private ScrollTemplateItemUI TryGetUI(int key)
		{
			ScrollTemplateItemUI scrollTemplateItemUI;
			if (this.mUICtrlDic.TryGetValue(key, out scrollTemplateItemUI))
			{
				return scrollTemplateItemUI;
			}
			return null;
		}

		private ScrollTemplateItemUI TryAddUI(int key, LoopListViewItem2 loopitem, ScrollTemplateItemUI ui)
		{
			ui.Init();
			ScrollTemplateItemUI scrollTemplateItemUI;
			if (this.mUICtrlDic.TryGetValue(key, out scrollTemplateItemUI))
			{
				if (scrollTemplateItemUI == null)
				{
					scrollTemplateItemUI = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, ScrollTemplateItemUI> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public void OnInit()
		{
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public void OnDeInit()
		{
			this.DeInitAllScrollUI();
		}

		public void RefreshUI()
		{
			this.mDataList.Clear();
			this.Scroll.SetListItemCount(this.mDataList.Count + 2, true);
			this.Scroll.RefreshAllShowItems();
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 1;

		private const int mSBottomCount = 1;

		private const int mSExCount = 2;

		private List<ScrollTemplateItemData> mDataList = new List<ScrollTemplateItemData>();

		private Dictionary<int, ScrollTemplateItemUI> mUICtrlDic = new Dictionary<int, ScrollTemplateItemUI>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
