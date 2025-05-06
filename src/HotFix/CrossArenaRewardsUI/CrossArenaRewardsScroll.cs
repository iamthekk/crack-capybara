using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using SuperScrollView;

namespace HotFix.CrossArenaRewardsUI
{
	public class CrossArenaRewardsScroll : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Scroll = base.gameObject.GetComponent<LoopListView2>();
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
			foreach (KeyValuePair<int, CrossArenaRewardsItem> keyValuePair in this.UICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.UICtrlDic.Clear();
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= this.mDataList.Count + 1)
			{
				return null;
			}
			if (index == 0)
			{
				return listView.NewListViewItem("TopEmpty");
			}
			index--;
			CrossArenaRankRewards crossArenaRankRewards = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("CorssArenaRewards_Item");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			CrossArenaRewardsItem crossArenaRewardsItem = this.TryGetUI(instanceID);
			CrossArenaRewardsItem component = loopListViewItem.gameObject.GetComponent<CrossArenaRewardsItem>();
			if (crossArenaRewardsItem == null)
			{
				crossArenaRewardsItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			crossArenaRewardsItem.SetData(crossArenaRankRewards, this.mRewardsKind);
			crossArenaRewardsItem.SetActive(true);
			crossArenaRewardsItem.RefreshUI();
			return loopListViewItem;
		}

		private CrossArenaRewardsItem TryGetUI(int key)
		{
			CrossArenaRewardsItem crossArenaRewardsItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRewardsItem))
			{
				return crossArenaRewardsItem;
			}
			return null;
		}

		private CrossArenaRewardsItem TryAddUI(int key, LoopListViewItem2 loopitem, CrossArenaRewardsItem ui)
		{
			ui.Init();
			CrossArenaRewardsItem crossArenaRewardsItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRewardsItem))
			{
				if (crossArenaRewardsItem == null)
				{
					crossArenaRewardsItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void SetData(CrossArenaRewards rewards, CrossArenaRewardsKind kind)
		{
			this.mData = rewards;
			this.mRewardsKind = kind;
			this.mDataList.Clear();
			if (this.mData != null && this.mData.RankRewards != null)
			{
				this.mDataList.AddRange(this.mData.RankRewards);
			}
			this.RefreshUI();
		}

		private void RefreshUI()
		{
			this.Scroll.SetListItemCount(this.mDataList.Count + 1, false);
			this.Scroll.MovePanelToItemIndex(0, 0f);
			this.PlayScale();
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.3f, 9);
		}

		public LoopListView2 Scroll;

		private List<CrossArenaRankRewards> mDataList = new List<CrossArenaRankRewards>();

		public Dictionary<int, CrossArenaRewardsItem> UICtrlDic = new Dictionary<int, CrossArenaRewardsItem>();

		private CrossArenaRewards mData;

		private CrossArenaRewardsKind mRewardsKind;

		private const int TopEmptyCount = 1;

		private SequencePool m_seqPool = new SequencePool();
	}
}
