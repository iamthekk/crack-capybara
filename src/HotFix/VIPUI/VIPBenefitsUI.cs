﻿using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using SuperScrollView;

namespace HotFix.VIPUI
{
	public class VIPBenefitsUI : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_seqPool.Clear(false);
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			this.DeInitAllItem();
		}

		private void DeInitAllItem()
		{
			foreach (KeyValuePair<int, VIPBenefitsItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			VIPDataModule.VIPData vipdata = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("Item");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			VIPBenefitsItem vipbenefitsItem = this.TryGetUI(instanceID);
			VIPBenefitsItem component = loopListViewItem.gameObject.GetComponent<VIPBenefitsItem>();
			if (vipbenefitsItem == null)
			{
				vipbenefitsItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			vipbenefitsItem.SetData(vipdata);
			vipbenefitsItem.SetActive(true);
			vipbenefitsItem.RefreshUI();
			return loopListViewItem;
		}

		private VIPBenefitsItem TryGetUI(int key)
		{
			VIPBenefitsItem vipbenefitsItem;
			if (this.UICtrlDic.TryGetValue(key, out vipbenefitsItem))
			{
				return vipbenefitsItem;
			}
			return null;
		}

		private VIPBenefitsItem TryAddUI(int key, LoopListViewItem2 loopitem, VIPBenefitsItem ui)
		{
			ui.Init();
			VIPBenefitsItem vipbenefitsItem;
			if (this.UICtrlDic.TryGetValue(key, out vipbenefitsItem))
			{
				if (vipbenefitsItem == null)
				{
					vipbenefitsItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void SetData(VIPDataModule.VIPDatas data)
		{
			this.mCurShowVIPData = data;
		}

		public void RefreshUI()
		{
			this.InternalRefreshUI();
		}

		private void InternalRefreshUI()
		{
			if (this.mCurShowVIPData == null)
			{
				HLog.LogError("VIPBenefitsUI InternalRefreshUI() mCurShowVIPData is null!");
				return;
			}
			this.m_seqPool.Clear(false);
			this.mDataList.Clear();
			this.mDataList.AddRange(this.mCurShowVIPData.m_vipDatas);
			this.Scroll.SetListItemCount(this.mDataList.Count, true);
			this.Scroll.RefreshAllShowItems();
		}

		public LoopListView2 Scroll;

		private List<VIPDataModule.VIPData> mDataList = new List<VIPDataModule.VIPData>();

		public Dictionary<int, VIPBenefitsItem> UICtrlDic = new Dictionary<int, VIPBenefitsItem>();

		private VIPDataModule.VIPDatas mCurShowVIPData;

		private VIPDataModule mVIPDataModule;

		private SequencePool m_seqPool = new SequencePool();
	}
}
