using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using SuperScrollView;

namespace HotFix.VIPUI
{
	public class VIPInfoUI : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mVIPDataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			this.ButtonLeft.m_onClick = new Action(this.ShowLevelDown);
			this.ButtonRight.m_onClick = new Action(this.ShowLevelUp);
			this.RedNodeLeft.SetType(240);
			this.RedNodeLeft.Value = 0;
			this.RedNodeRight.SetType(240);
			this.RedNodeRight.Value = 0;
			LoopListViewInitParam loopListViewInitParam = new LoopListViewInitParam();
			loopListViewInitParam.mSmoothDumpRate = 0.1f;
			loopListViewInitParam.mSnapVecThreshold = 300f;
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), loopListViewInitParam);
			this.Scroll.ScrollRect.enabled = false;
		}

		protected override void OnDeInit()
		{
			this.DeInitAllItem();
		}

		public void OnViewOpen(int viplevel)
		{
			this.m_seqPool.Clear(false);
			this.mCurIndex = viplevel - 1;
			if (this.mCurIndex < 0)
			{
				this.mCurIndex = 0;
			}
		}

		public void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		private void DeInitAllItem()
		{
			foreach (KeyValuePair<int, VIPNodeUI> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count)
			{
				return null;
			}
			VIPDataModule.VIPDatas vipdatas = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("VIPNode");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			VIPNodeUI vipnodeUI = this.TryGetUI(instanceID);
			VIPNodeUI component = loopListViewItem.gameObject.GetComponent<VIPNodeUI>();
			if (vipnodeUI == null)
			{
				vipnodeUI = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			vipnodeUI.SetData(vipdatas);
			vipnodeUI.SetActive(true);
			vipnodeUI.RefreshUI();
			return loopListViewItem;
		}

		private VIPNodeUI TryGetUI(int key)
		{
			VIPNodeUI vipnodeUI;
			if (this.mUICtrlDic.TryGetValue(key, out vipnodeUI))
			{
				return vipnodeUI;
			}
			return null;
		}

		private VIPNodeUI TryAddUI(int key, LoopListViewItem2 loopitem, VIPNodeUI ui)
		{
			ui.Init();
			VIPNodeUI vipnodeUI;
			if (this.mUICtrlDic.TryGetValue(key, out vipnodeUI))
			{
				if (vipnodeUI == null)
				{
					vipnodeUI = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		public void RefreshUI()
		{
			this.mDataList.Clear();
			this.mDataList.AddRange(this.mVIPDataModule.GetAllVIPDatas());
			this.Scroll.SetListItemCount(this.mDataList.Count, true);
			this.Scroll.MovePanelToItemIndex(this.mCurIndex, 0f);
			this.RefreshLeftAndRightButton();
		}

		public void ShowLevelDown()
		{
			if (this.mCurIndex <= 0)
			{
				return;
			}
			this.mCurIndex--;
			this.Scroll.SetSnapTargetItemIndex(this.mCurIndex, -1f);
			this.RefreshLeftAndRightButton();
		}

		public void ShowLevelUp()
		{
			if (this.mCurIndex + 1 >= this.mVIPDataModule.MaxVIPLevel)
			{
				return;
			}
			this.mCurIndex++;
			this.Scroll.SetSnapTargetItemIndex(this.mCurIndex, -1f);
			this.RefreshLeftAndRightButton();
		}

		private void RefreshLeftAndRightButton()
		{
			int maxVIPLevel = this.mVIPDataModule.MaxVIPLevel;
			this.ButtonLeft.gameObject.SetActive(this.mCurIndex > 0);
			this.ButtonRight.gameObject.SetActive(this.mCurIndex + 1 < maxVIPLevel);
			bool flag = this.mCurIndex > 0 && this.IsCanGetRewardsLeft(this.mCurIndex + 1);
			bool flag2 = this.mCurIndex < maxVIPLevel && this.IsCanGetRewardsRight(this.mCurIndex + 1);
			this.RedNodeLeft.Value = (flag ? 1 : 0);
			this.RedNodeRight.Value = (flag2 ? 1 : 0);
		}

		private bool IsCanGetRewardsLeft(int viplevel)
		{
			for (int i = viplevel - 1; i >= 1; i--)
			{
				if (this.mVIPDataModule.IsCanGetReward(i))
				{
					return true;
				}
			}
			return false;
		}

		private bool IsCanGetRewardsRight(int viplevel)
		{
			int maxVIPLevel = this.mVIPDataModule.MaxVIPLevel;
			for (int i = viplevel + 1; i <= maxVIPLevel; i++)
			{
				if (this.mVIPDataModule.IsCanGetReward(i))
				{
					return true;
				}
			}
			return false;
		}

		public CustomButton ButtonLeft;

		public CustomButton ButtonRight;

		public RedNodeOneCtrl RedNodeLeft;

		public RedNodeOneCtrl RedNodeRight;

		public LoopListView2 Scroll;

		private List<VIPDataModule.VIPDatas> mDataList = new List<VIPDataModule.VIPDatas>();

		private Dictionary<int, VIPNodeUI> mUICtrlDic = new Dictionary<int, VIPNodeUI>();

		private int mCurIndex;

		private VIPDataModule mVIPDataModule;

		private SequencePool m_seqPool = new SequencePool();
	}
}
