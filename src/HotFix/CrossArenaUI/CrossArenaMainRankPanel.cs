using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI.SuperScrollUI;
using Proto.CrossArena;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.CrossArenaUI
{
	public class CrossArenaMainRankPanel : CustomBehaviour
	{
		private int TopItemIndex
		{
			get
			{
				return 0;
			}
		}

		private int LoadingItemIndex
		{
			get
			{
				return this.MemberDataList.Count + 1;
			}
		}

		private int BottomItemIndex
		{
			get
			{
				return this.MemberDataList.Count + 1 + 1;
			}
		}

		private int TotalShowItemCount
		{
			get
			{
				return this.MemberDataList.Count + 3;
			}
		}

		protected override void OnInit()
		{
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Scroll.mOnBeginDragAction = new Action(this.OnDragStart);
			this.Scroll.mOnDragingAction = new Action(this.OnDraging);
			this.Scroll.mOnEndDragAction = new Action(this.OnDragEnd);
			this.Scroll.ScrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScrollValueChange));
			this.MyRank.Init();
			this.MyRank.SetHide();
			this.MyRank.MyRank.OnShowPlayerInfo = new Action<long>(this.OnClickShowPlayerInfo);
			this.Obj_NetLoading.SetActive(false);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			foreach (KeyValuePair<int, CrossArenaRankMemberItem> keyValuePair in this.UICtrlDic)
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.DeInit();
				}
			}
			this.UICtrlDic.Clear();
			if (this.MyRank != null)
			{
				this.MyRank.DeInit();
			}
		}

		private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0 || index > this.TotalShowItemCount)
			{
				return null;
			}
			if (index == this.TopItemIndex)
			{
				return listView.NewListViewItem("CrossArena_TopEmpty");
			}
			LoopListViewItem2 loopListViewItem;
			if (index == this.LoadingItemIndex)
			{
				loopListViewItem = listView.NewListViewItem("CrossArena_LoadingItem");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			if (index == this.BottomItemIndex)
			{
				return listView.NewListViewItem("CrossArena_BottomEmpty");
			}
			int num = index - 1;
			CrossArenaRankMember crossArenaRankMember = this.MemberDataList[num];
			loopListViewItem = listView.NewListViewItem("CrossArena_RankItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			CrossArenaRankMemberItem crossArenaRankMemberItem = this.TryGetUI(instanceID);
			if (crossArenaRankMemberItem == null)
			{
				CrossArenaRankMemberItem component = loopListViewItem.GetComponent<CrossArenaRankMemberItem>();
				crossArenaRankMemberItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			crossArenaRankMemberItem.SetData(num, crossArenaRankMember);
			crossArenaRankMemberItem.RefreshUI();
			return loopListViewItem;
		}

		private CrossArenaRankMemberItem TryGetUI(int key)
		{
			CrossArenaRankMemberItem crossArenaRankMemberItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRankMemberItem))
			{
				return crossArenaRankMemberItem;
			}
			return null;
		}

		private CrossArenaRankMemberItem TryAddUI(int key, LoopListViewItem2 loopitem, CrossArenaRankMemberItem ui)
		{
			ui.Init();
			ui.OnShowPlayerInfo = new Action<long>(this.OnClickShowPlayerInfo);
			CrossArenaRankMemberItem crossArenaRankMemberItem;
			if (this.UICtrlDic.TryGetValue(key, out crossArenaRankMemberItem))
			{
				if (crossArenaRankMemberItem == null)
				{
					crossArenaRankMemberItem = ui;
					this.UICtrlDic[key] = ui;
				}
				return ui;
			}
			this.UICtrlDic.Add(key, ui);
			return ui;
		}

		public void RefreshOnViewOpen()
		{
			this.m_seqPool.Clear(false);
			this.mCurPage = 1;
			this.MemberDataList.Clear();
			this.mMemberDic.Clear();
			this.RefreshUI();
			this.MyRank.SetHide();
			this.LoadPageData();
		}

		public void ResetData()
		{
			this.m_seqPool.Clear(false);
			this.mCurPage = 1;
			this.MemberDataList.Clear();
			this.mMemberDic.Clear();
			this.MyRank.SetHide();
			this.Scroll.SetListItemCount(0, true);
		}

		private void LoadPageData()
		{
			if (this.mCurPage < 1)
			{
				this.mCurPage = 1;
			}
			this.Obj_NetLoading.SetActive(true);
			NetworkUtils.CrossArena.DoCrossArenaRankRequest(this.mCurPage, delegate(bool result, CrossArenaRankResponse resp)
			{
				if (this == null || base.gameObject == null || !base.gameObject.activeSelf)
				{
					return;
				}
				this.Obj_NetLoading.SetActive(false);
				if (result)
				{
					List<CrossArenaRankMember> list = CrossArenaRankMember.ToList(resp.Rank);
					this.CombineMemberList(list);
					if (resp.Rank.Count > 0 && (ulong)resp.TotalCount > (ulong)((long)this.MemberDataList.Count))
					{
						this.mCurPage++;
					}
					else
					{
						this.mHasAllServerData = true;
					}
					this.RefreshUI();
				}
			});
		}

		private void CombineMemberList(List<CrossArenaRankMember> list)
		{
			long userId = GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
			GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			for (int i = 0; i < list.Count; i++)
			{
				CrossArenaRankMember crossArenaRankMember = list[i];
				if (crossArenaRankMember != null)
				{
					CrossArenaRankMember crossArenaRankMember2;
					if (this.mMemberDic.TryGetValue(crossArenaRankMember.UserID, out crossArenaRankMember2))
					{
						crossArenaRankMember2.CloneFrom(crossArenaRankMember);
					}
					else
					{
						this.mMemberDic[crossArenaRankMember.UserID] = crossArenaRankMember;
						this.MemberDataList.Add(crossArenaRankMember);
					}
				}
			}
		}

		public void RefreshUI()
		{
			this.MyRank.PlayShow();
			if (this.mHasAllServerData)
			{
				this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.Loaded;
			}
			this.Scroll.SetListItemCount(this.TotalShowItemCount, true);
			this.Scroll.RefreshAllShowItems();
			this.PlayScale();
		}

		private void PlayScale()
		{
			this.m_seqPool.Clear(false);
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.3f, 9);
		}

		private void OnClickShowPlayerInfo(long userid)
		{
			if (userid == 0L)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(userid);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnScrollValueChange(Vector2 arg0)
		{
			if (this.mLoadingTipStatus == CrossArenaMainRankPanel.LoadingTipStatus.WaitLoad && this.Scroll.ScrollRect.verticalNormalizedPosition > -0.0001f)
			{
				this.Scroll.ScrollRect.verticalNormalizedPosition = 0f;
				this.ReqGuildList();
			}
		}

		private void ReqGuildList()
		{
			if (this.mLoadingTipStatus == CrossArenaMainRankPanel.LoadingTipStatus.RequireData)
			{
				return;
			}
			bool flag = this.mHasAllServerData;
			this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.RequireData;
			NetworkUtils.CrossArena.DoCrossArenaRankRequest(this.mCurPage, delegate(bool result, CrossArenaRankResponse resp)
			{
				this.Obj_NetLoading.SetActive(false);
				if (result)
				{
					if (resp.Rank.Count > 0)
					{
						this.mCurPage++;
					}
					else
					{
						this.mHasAllServerData = true;
					}
					List<CrossArenaRankMember> list = CrossArenaRankMember.ToList(resp.Rank);
					this.CombineMemberList(list);
					this.RefreshListAfterGetDataList();
				}
			});
		}

		private void OnDragStart()
		{
		}

		private void OnDraging()
		{
			this.RefreshScrollOnDraging();
		}

		private void OnDragEnd()
		{
			this.RefreshScrollAfterDragEnd();
		}

		private void RefreshScrollOnDraging()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.None && this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.Scroll.GetShownItemByItemIndex(this.LoadingItemIndex);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.Scroll.GetShownItemByItemIndex(this.BottomItemIndex);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.Scroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.Scroll.ViewPortSize >= 5f)
			{
				if (this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.None)
				{
					return;
				}
				this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease)
				{
					return;
				}
				this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void RefreshScrollAfterDragEnd()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.None && this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.Scroll.GetShownItemByItemIndex(this.LoadingItemIndex);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.Scroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			if (this.mLoadingTipStatus != CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease)
			{
				return;
			}
			if (this.Scroll.GetItemCornerPosInViewPort(shownItemByItemIndex, 0).y + this.Scroll.ViewPortSize < 0f)
			{
				if (this.mHasAllServerData)
				{
					this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.Loaded;
				}
				else
				{
					this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.None;
				}
				this.Scroll.SetListItemCount(this.TotalShowItemCount, false);
				this.Scroll.RefreshAllShownItem();
				return;
			}
			if (this.mHasAllServerData)
			{
				this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.Loaded;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.WaitLoad;
			this.UpdateLoadingTip(shownItemByItemIndex);
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			SuperScrollBottomLoadingItem component = item.GetComponent<SuperScrollBottomLoadingItem>();
			if (component == null)
			{
				HLog.LogError(string.Format("UpdateLoadingTip : >>>{0}", component == null));
				return;
			}
			switch (this.mLoadingTipStatus)
			{
			case CrossArenaMainRankPanel.LoadingTipStatus.None:
				component.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			case CrossArenaMainRankPanel.LoadingTipStatus.WaitRelease:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case CrossArenaMainRankPanel.LoadingTipStatus.WaitLoad:
			case CrossArenaMainRankPanel.LoadingTipStatus.RequireData:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsLoading();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			case CrossArenaMainRankPanel.LoadingTipStatus.Loaded:
				component.SetActive(true);
				component.SetScroll(this.Scroll);
				component.SetAsNoMoreData();
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.mLoadingTipItemHeight);
				return;
			default:
				return;
			}
		}

		private void RefreshListAfterGetDataList()
		{
			if (this.Scroll.ShownItemCount == 0)
			{
				return;
			}
			if (this.mLoadingTipStatus == CrossArenaMainRankPanel.LoadingTipStatus.RequireData)
			{
				if (this.mHasAllServerData)
				{
					this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.Loaded;
				}
				else
				{
					this.mLoadingTipStatus = CrossArenaMainRankPanel.LoadingTipStatus.None;
				}
				this.Scroll.SetListItemCount(this.TotalShowItemCount, false);
				this.Scroll.RefreshAllShownItem();
			}
		}

		public LoopListView2 Scroll;

		public CrossArenaMainMyRank MyRank;

		public GameObject Obj_NetLoading;

		private Dictionary<long, CrossArenaRankMember> mMemberDic = new Dictionary<long, CrossArenaRankMember>();

		public List<CrossArenaRankMember> MemberDataList = new List<CrossArenaRankMember>();

		public Dictionary<int, CrossArenaRankMemberItem> UICtrlDic = new Dictionary<int, CrossArenaRankMemberItem>();

		private int mCurPage = 1;

		private const int mTopExtureCount = 1;

		private const int mBottomLoadingItemCount = 1;

		private const int mBottomExtureCount = 1;

		private const int mExtureCount = 3;

		private SequencePool m_seqPool = new SequencePool();

		private CrossArenaMainRankPanel.LoadingTipStatus mLoadingTipStatus;

		private float mLoadingTipItemHeight = 100f;

		private bool mHasAllServerData;

		public enum LoadingTipStatus
		{
			None,
			WaitRelease,
			WaitLoad,
			RequireData,
			Loaded
		}
	}
}
