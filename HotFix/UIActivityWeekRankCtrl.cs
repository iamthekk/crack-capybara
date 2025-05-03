using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI.SuperScrollUI;
using Proto.ActTime;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class UIActivityWeekRankCtrl : CustomBehaviour
	{
		public List<ActTimeRankDto> RankDtoList { get; } = new List<ActTimeRankDto>();

		private bool IsOpen()
		{
			return this.isOpen;
		}

		protected override void OnInit()
		{
			this.isOpen = true;
			this.rankItemSelf.Init();
			foreach (UIActivityWeekRankItem uiactivityWeekRankItem in this.rankItemTopList)
			{
				uiactivityWeekRankItem.Init();
			}
			if (this.initScroll)
			{
				this.rankItemScroll.SetListItemCount(0, true);
			}
			else
			{
				this.initScroll = true;
				this.rankItemScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnRankListGetItemByIndex), null);
			}
			this.rankItemScroll.mOnBeginDragAction = new Action(this.OnRankListBeginDrag);
			this.rankItemScroll.mOnDragingAction = new Action(this.OnRankListDraging);
			this.rankItemScroll.mOnEndDragAction = new Action(this.OnRankListEndDrag);
			base.SetActive(true);
			this.SetContentVisible(false);
		}

		protected override void OnDeInit()
		{
			this.isOpen = false;
			if (this.rankDataModule != null)
			{
				this.rankDataModule.ResetLoadRankDataTime();
			}
			this.rankItemScroll.mOnBeginDragAction = null;
			this.rankItemScroll.mOnDragingAction = null;
			this.rankItemScroll.mOnEndDragAction = null;
			this.rankItemSelf.DeInit();
			foreach (UIActivityWeekRankItem uiactivityWeekRankItem in this.rankItemTopList)
			{
				uiactivityWeekRankItem.DeInit();
			}
		}

		public void Open(ActivityWeekViewModule.ActViewType viewType, UIActivityWeekRankItem.RankType rankType, int actId, Action<bool> onGetDataCallback = null)
		{
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.rankDataModule = GameApp.Data.GetDataModule(DataName.ActivityTimeRankDataModule);
			this.m_ActViewType = viewType;
			this.m_RankType = rankType;
			this.m_ActId = actId;
			this.m_OnGetDataCallback = onGetDataCallback;
			this.curRankPage = 1;
			this.rankItemScroll.MovePanelToItemIndex(0, 0f);
			this.SetContentVisible(false);
			this.obj_Empty.SetActiveSafe(false);
			this.CheckLoadRankDataList(delegate
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnBeginLoadRank(true);
			}, delegate(bool res)
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnEndLoadRank(res, true, true);
			});
		}

		public void SetContentVisible(bool visible)
		{
			this.seqPool.Clear(false);
			this.contentRoot.SetActiveSafe(visible);
		}

		public void CheckLoadRankDataList(Action onLoading, Action<bool> onLoadEnd)
		{
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - this.rankDataModule.LastLoadRankDataTime >= this.rankDataCacheTime)
			{
				this.curRankPage = 1;
				this.LoadRankDataList(false, onLoading, delegate(bool res, bool isUpdateData)
				{
					Action<bool> onLoadEnd3 = onLoadEnd;
					if (onLoadEnd3 == null)
					{
						return;
					}
					onLoadEnd3(res);
				});
				return;
			}
			Action<bool> onLoadEnd2 = onLoadEnd;
			if (onLoadEnd2 == null)
			{
				return;
			}
			onLoadEnd2(true);
		}

		public void LoadRankDataList(bool isNextPage, Action onLoading, Action<bool, bool> onLoadEnd)
		{
			if (this.curRankPage < Singleton<GameConfig>.Instance.ActivityWeek_RankMaxPage)
			{
				if (onLoading != null)
				{
					onLoading();
				}
				int nextPage = ((isNextPage && this.curRankPage < Singleton<GameConfig>.Instance.ActivityWeek_RankMaxPage && this.RankDtoList.Count >= Singleton<GameConfig>.Instance.ActivityWeek_RankSingleCount) ? (this.curRankPage + 1) : this.curRankPage);
				NetworkUtils.ActivityWeek.RequestActTimeRank(nextPage, isNextPage, this.m_ActId, false, delegate(int _, bool _, bool isOk, ActTimeRankResponse resp)
				{
					if (!isOk)
					{
						Action<bool, bool> onLoadEnd3 = onLoadEnd;
						if (onLoadEnd3 == null)
						{
							return;
						}
						onLoadEnd3(false, false);
						return;
					}
					else
					{
						bool flag = false;
						if (nextPage == 1)
						{
							this.RankDtoList.Clear();
						}
						if (resp.Rank != null && resp.Rank.Count > 0)
						{
							flag = this.curRankPage != nextPage;
							this.RankDtoList.AddRange(resp.Rank);
							this.curRankPage = nextPage;
						}
						Action<bool, bool> onLoadEnd4 = onLoadEnd;
						if (onLoadEnd4 == null)
						{
							return;
						}
						onLoadEnd4(true, flag);
						return;
					}
				});
				return;
			}
			Action<bool, bool> onLoadEnd2 = onLoadEnd;
			if (onLoadEnd2 == null)
			{
				return;
			}
			onLoadEnd2(true, false);
		}

		private void RefreshAll(bool isPlayAnim)
		{
			if (this.RankDtoList.Count > 0)
			{
				this.obj_Empty.SetActiveSafe(false);
				this.rankItemScroll.SetListItemCount(this.RankDtoList.Count + 1, false);
				this.rankItemScroll.RefreshAllShowItems();
				this.FreshTopRank();
				this.FreshSelfRank();
				if (isPlayAnim)
				{
					this.PlayShowAnim();
					return;
				}
			}
			else
			{
				this.obj_Empty.SetActiveSafe(true);
				this.rankItemScroll.SetListItemCount(0, true);
				this.rankItemScroll.RefreshAllShowItems();
				this.FreshTopRank();
				this.FreshSelfRank();
			}
		}

		private void PlayShowAnim()
		{
			this.seqPool.Clear(false);
			for (int i = 0; i < this.rankItemScroll.ShownItemCount; i++)
			{
				LoopListViewItem2 shownItemByIndex = this.rankItemScroll.GetShownItemByIndex(i);
				if (!(shownItemByIndex == null))
				{
					RectTransform cachedRectTransform = shownItemByIndex.CachedRectTransform;
					DxxTools.UI.DoMoveRightToScreenAnim(this.seqPool.Get(), cachedRectTransform.GetChild(0) as RectTransform, 0f, 0.1f * (float)i, 0.2f, 9);
				}
			}
		}

		public void FreshTopRank()
		{
			for (int i = 0; i < this.rankItemTopList.Count; i++)
			{
				UIActivityWeekRankItem uiactivityWeekRankItem = this.rankItemTopList[i];
				bool flag = i >= this.RankDtoList.Count;
				uiactivityWeekRankItem.SetActive(!flag);
				if (!flag)
				{
					int num = i;
					uiactivityWeekRankItem.RefreshData(this.m_RankType, this.RankDtoList[i], num + 1);
				}
			}
		}

		private void FreshSelfRank()
		{
			this.RefreshSelfRank(this.rankItemSelf, this.m_RankType);
		}

		public void RefreshSelfRank(UIActivityWeekRankItem selfItem, UIActivityWeekRankItem.RankType rankType)
		{
			ActTimeRankDto actTimeRankDto = new ActTimeRankDto
			{
				UserId = this.loginDataModule.userId,
				NickName = this.loginDataModule.NickName,
				Avatar = this.loginDataModule.Avatar,
				AvatarFrame = this.loginDataModule.AvatarFrame,
				TitleId = this.loginDataModule.AvatarTitle,
				Score = this.rankDataModule.Data.RankScore
			};
			int rankIndex = this.rankDataModule.Data.RankIndex;
			selfItem.RefreshData(rankType, actTimeRankDto, rankIndex);
		}

		private void OnBeginLoadRank(bool isShowNetLoading)
		{
			this.loadingStatus = UIActivityWeekRankCtrl.LoadingStatus.Loading;
			this.netLoading.SetActive(isShowNetLoading);
		}

		private void OnEndLoadRank(bool res, bool isUpdateData, bool isPlayAnim)
		{
			this.loadingStatus = (isUpdateData ? UIActivityWeekRankCtrl.LoadingStatus.None : UIActivityWeekRankCtrl.LoadingStatus.Loaded);
			this.netLoading.SetActive(false);
			if (res)
			{
				Action<bool> onGetDataCallback = this.m_OnGetDataCallback;
				if (onGetDataCallback != null)
				{
					onGetDataCallback(true);
				}
				if (this.m_ActViewType != ActivityWeekViewModule.ActViewType.RankOrderType)
				{
					return;
				}
				this.SetContentVisible(true);
				this.RefreshAll(isPlayAnim);
				return;
			}
			else
			{
				Action<bool> onGetDataCallback2 = this.m_OnGetDataCallback;
				if (onGetDataCallback2 == null)
				{
					return;
				}
				onGetDataCallback2(false);
				return;
			}
		}

		private void OnRankListBeginDrag()
		{
		}

		private void OnRankListDraging()
		{
			if (this.loadingStatus != UIActivityWeekRankCtrl.LoadingStatus.ReadyLoad && this.loadingStatus != UIActivityWeekRankCtrl.LoadingStatus.None)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.rankItemScroll.GetShownItemByItemIndex(this.RankDtoList.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.rankItemScroll.GetShownItemByItemIndex(this.RankDtoList.Count - 1);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.rankItemScroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.rankItemScroll.ViewPortSize >= this.loadingTipItemHeight)
			{
				if (this.loadingStatus != UIActivityWeekRankCtrl.LoadingStatus.None)
				{
					return;
				}
				this.loadingStatus = UIActivityWeekRankCtrl.LoadingStatus.ReadyLoad;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.loadingStatus != UIActivityWeekRankCtrl.LoadingStatus.ReadyLoad)
				{
					return;
				}
				this.loadingStatus = UIActivityWeekRankCtrl.LoadingStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void OnRankListEndDrag()
		{
			if (this.loadingStatus != UIActivityWeekRankCtrl.LoadingStatus.ReadyLoad)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.rankItemScroll.GetShownItemByItemIndex(this.RankDtoList.Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.rankItemScroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			this.UpdateLoadingTip(shownItemByItemIndex);
			this.LoadRankDataList(true, delegate
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnBeginLoadRank(false);
			}, delegate(bool res, bool isUpdateData)
			{
				if (!this.IsOpen())
				{
					return;
				}
				this.OnEndLoadRank(res, isUpdateData, false);
			});
		}

		private LoopListViewItem2 OnRankListGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem;
			if (index == this.RankDtoList.Count)
			{
				loopListViewItem = listView.NewListViewItem("LoadingNode");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			ActTimeRankDto actTimeRankDto = this.RankDtoList[index];
			if (actTimeRankDto == null)
			{
				return null;
			}
			loopListViewItem = listView.NewListViewItem("UIActivityWeekRankItem");
			CustomBehaviour component;
			this.rankNodeList.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<UIActivityWeekRankItem>();
				component.Init();
				this.rankNodeList[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			if (index == this.RankDtoList.Count - 1)
			{
				loopListViewItem.Padding = 0f;
			}
			UIActivityWeekRankItem uiactivityWeekRankItem = component as UIActivityWeekRankItem;
			if (uiactivityWeekRankItem != null)
			{
				int num = index + 1;
				uiactivityWeekRankItem.RefreshData(this.m_RankType, actTimeRankDto, num);
			}
			return loopListViewItem;
		}

		private void UpdateLoadingTip(LoopListViewItem2 item)
		{
			if (item == null)
			{
				return;
			}
			SuperScrollBottomLoadingItem component = item.gameObject.GetComponent<SuperScrollBottomLoadingItem>();
			if (component == null)
			{
				return;
			}
			if (this.loadingStatus == UIActivityWeekRankCtrl.LoadingStatus.None)
			{
				component.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			}
			component.SetActive(true);
			component.SetScroll(this.rankItemScroll);
			if (this.loadingStatus == UIActivityWeekRankCtrl.LoadingStatus.Loaded)
			{
				component.SetAsNoMoreData();
			}
			else
			{
				component.SetAsLoading();
			}
			item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.loadingTipItemHeight);
		}

		public GameObject contentRoot;

		[SerializeField]
		private LoopListView2 rankItemScroll;

		[SerializeField]
		private GameObject netLoading;

		[SerializeField]
		private List<UIActivityWeekRankItem> rankItemTopList;

		[SerializeField]
		private UIActivityWeekRankItem rankItemSelf;

		[SerializeField]
		private GameObject obj_Empty;

		private UIActivityWeekRankCtrl.LoadingStatus loadingStatus;

		private readonly float loadingTipItemHeight = 100f;

		private Dictionary<int, CustomBehaviour> rankNodeList = new Dictionary<int, CustomBehaviour>();

		private int m_ActId;

		private ActTimeRankDataModule rankDataModule;

		private LoginDataModule loginDataModule;

		private readonly SequencePool seqPool = new SequencePool();

		private readonly long rankDataCacheTime = 60L;

		private int curRankPage = 1;

		private ActivityWeekViewModule.ActViewType m_ActViewType;

		private UIActivityWeekRankItem.RankType m_RankType;

		private Action<bool> m_OnGetDataCallback;

		private bool isOpen;

		private bool initScroll;

		private enum LoadingStatus
		{
			None,
			ReadyLoad,
			Loading,
			Loaded
		}
	}
}
