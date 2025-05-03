using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Proto.Common;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class SocialityRankPanelGroup : BaseSocialityPanel
	{
		protected override void OnInit()
		{
			this.m_socialityDataModule = GameApp.Data.GetDataModule(DataName.SocialityDataModule);
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_scroll.InitListView(this.m_datas.Count + 1, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.m_scroll.mOnBeginDragAction = new Action(this.OnBeginDrag);
			this.m_scroll.mOnDragingAction = new Action(this.OnDraging);
			this.m_scroll.mOnEndDragAction = new Action(this.OnEndDrag);
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.m_nodes)
			{
				if (!(keyValuePair.Value == null))
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.m_nodes.Clear();
			this.m_datas.Clear();
			this.m_scroll.mOnBeginDragAction = null;
			this.m_scroll.mOnDragingAction = null;
			this.m_scroll.mOnEndDragAction = null;
			this.m_socialityDataModule = null;
			this.m_loginDataModule = null;
		}

		public override void OnShow()
		{
			this.m_datas.Clear();
			for (int i = 0; i < this.m_socialityDataModule.m_ranks.Count; i++)
			{
				PowerRankDto powerRankDto = this.m_socialityDataModule.m_ranks[i];
				if (powerRankDto != null && powerRankDto.UserId != this.m_loginDataModule.userId)
				{
					this.m_datas.Add(powerRankDto);
				}
			}
			if (this.m_socialityDataModule.IsFinishedForRank)
			{
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.Finished;
			}
			else
			{
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.None;
			}
			this.m_netLoading.SetActive(this.m_datas.Count == 0);
			this.m_scroll.SetListItemCount(this.m_datas.Count + 1, true);
			this.m_scroll.RefreshAllShowItems();
			this.PlayScale();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshRankData, new HandlerEvent(this.OnRefreshUI));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_CheckLoadingRankData, null);
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_RefreshRankData, new HandlerEvent(this.OnRefreshUI));
			this.m_sequencePool.Clear(false);
		}

		private void OnRefreshUI(object sender, int type, BaseEventArgs eventargs)
		{
			int count = this.m_datas.Count;
			this.m_datas.Clear();
			for (int i = 0; i < this.m_socialityDataModule.m_ranks.Count; i++)
			{
				PowerRankDto powerRankDto = this.m_socialityDataModule.m_ranks[i];
				if (powerRankDto != null && powerRankDto.UserId != this.m_loginDataModule.userId)
				{
					this.m_datas.Add(powerRankDto);
				}
			}
			this.m_isLoadingRequest = false;
			if (this.m_socialityDataModule.IsFinishedForRank)
			{
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.Finished;
			}
			else
			{
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.None;
			}
			bool activeSelf = this.m_netLoading.activeSelf;
			this.m_netLoading.SetActive(false);
			this.m_scroll.SetListItemCount(this.m_datas.Count + 1, false);
			this.m_scroll.RefreshAllShowItems();
			if (activeSelf)
			{
				this.PlayScale();
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem;
			if (index == this.m_datas.Count)
			{
				loopListViewItem = listView.NewListViewItem("LoadNode");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			PowerRankDto powerRankDto = this.m_datas[index];
			if (powerRankDto == null)
			{
				return null;
			}
			loopListViewItem = listView.NewListViewItem("Node");
			CustomBehaviour component;
			this.m_nodes.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<SocialityRankNode>();
				component.Init();
				this.m_nodes[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			if (index == this.m_datas.Count - 1)
			{
				loopListViewItem.Padding = 0f;
			}
			SocialityRankNode socialityRankNode = component as SocialityRankNode;
			if (socialityRankNode != null)
			{
				socialityRankNode.RefreshData(powerRankDto, index);
			}
			return loopListViewItem;
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			CustomBehaviour component;
			this.m_nodes.TryGetValue(item.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = item.GetComponent<SocialityRankLoadNode>();
				component.Init();
				this.m_nodes[item.gameObject.GetInstanceID()] = component;
			}
			SocialityRankLoadNode socialityRankLoadNode = component as SocialityRankLoadNode;
			if (socialityRankLoadNode == null)
			{
				return;
			}
			switch (this.mLoadingTipStatus)
			{
			case SocialityRankPanelGroup.LoadingTipStatus.None:
				socialityRankLoadNode.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			case SocialityRankPanelGroup.LoadingTipStatus.WaitRelease:
				socialityRankLoadNode.SetActive(true);
				socialityRankLoadNode.SetActiveNextTxt(true);
				socialityRankLoadNode.SetActiveFinishedTxt(false);
				socialityRankLoadNode.SetActiveLoading(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case SocialityRankPanelGroup.LoadingTipStatus.WaitLoad:
				socialityRankLoadNode.SetActive(true);
				socialityRankLoadNode.SetActiveNextTxt(false);
				socialityRankLoadNode.SetActiveFinishedTxt(false);
				socialityRankLoadNode.SetActiveLoading(true);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case SocialityRankPanelGroup.LoadingTipStatus.Finished:
				socialityRankLoadNode.SetActive(true);
				socialityRankLoadNode.SetActiveNextTxt(false);
				socialityRankLoadNode.SetActiveFinishedTxt(true);
				socialityRankLoadNode.SetActiveLoading(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		private void OnBeginDrag()
		{
		}

		private void OnDraging()
		{
			if (this.m_socialityDataModule.IsFinishedForRank)
			{
				return;
			}
			if (this.m_scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.None && this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.WaitRelease && this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.Finished)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.m_scroll.GetShownItemByItemIndex(this.m_datas.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.m_scroll.GetShownItemByItemIndex(this.m_datas.Count - 1);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.m_scroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.m_scroll.ViewPortSize >= this.mLoadingTipItemHeight)
			{
				if (this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.None)
				{
					return;
				}
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.WaitRelease;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.WaitRelease)
				{
					return;
				}
				this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void OnEndDrag()
		{
			if (this.m_scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.None && this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.WaitRelease && this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.Finished)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.m_scroll.GetShownItemByItemIndex(this.m_datas.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.m_scroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (this.mLoadingTipStatus != SocialityRankPanelGroup.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			this.mLoadingTipStatus = SocialityRankPanelGroup.LoadingTipStatus.WaitLoad;
			this.UpdateLoadingTip(shownItemByItemIndex);
			if (this.m_isLoadingRequest)
			{
				return;
			}
			this.m_isLoadingRequest = true;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_LoadingRankData, null);
		}

		private void PlayScale()
		{
			this.m_sequencePool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_sequencePool.Get(), this.m_scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private SocialityRankPanelGroup.LoadingTipStatus mLoadingTipStatus;

		private float mLoadingTipItemHeight = 100f;

		public LoopListView2 m_scroll;

		public GameObject m_netLoading;

		private List<PowerRankDto> m_datas = new List<PowerRankDto>();

		private Dictionary<int, CustomBehaviour> m_nodes = new Dictionary<int, CustomBehaviour>();

		public bool m_isLoadingRequest;

		private SequencePool m_sequencePool = new SequencePool();

		private SocialityDataModule m_socialityDataModule;

		private LoginDataModule m_loginDataModule;

		public enum LoadingTipStatus
		{
			None,
			WaitRelease,
			WaitLoad,
			Finished
		}
	}
}
