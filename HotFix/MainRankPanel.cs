using System;
using System.Collections.Generic;
using System.Linq;
using Dxx.Guild;
using Framework;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI.SuperScrollUI;
using LocalModels.Bean;
using Proto.ActTime;
using Proto.Guild;
using Proto.LeaderBoard;
using Proto.Tower;
using SuperScrollView;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class MainRankPanel : CustomBehaviour
	{
		private List<BaseRankData> RankDtoList { get; } = new List<BaseRankData>();

		private long LastLoadRankDataTime
		{
			get
			{
				if (this.clearCacheTimeOnOpen)
				{
					return this.m_LastLoadRankDataTime;
				}
				return long.Parse(Utility.PlayerPrefs.GetUserString("CommonMainRank_LastLoadRankDataTime", "0"));
			}
			set
			{
				if (this.clearCacheTimeOnOpen)
				{
					this.m_LastLoadRankDataTime = value;
					return;
				}
				Utility.PlayerPrefs.SetUserString("CommonMainRank_LastLoadRankDataTime", value.ToString());
			}
		}

		private void ClearCachedOpenTime()
		{
			GameApp.Data.GetDataModule(DataName.RankDataModule).ClearLastLoadedUtc(this.m_RankOpenData.RankType);
			this.m_LastLoadRankDataTime = 0L;
		}

		private bool IsOpen()
		{
			return base.gameObject.activeSelf;
		}

		protected override void OnInit()
		{
			this.orderScrollLayoutGroup.enabled = false;
			this.rankRewardTemplate.gameObject.SetActive(false);
			this.rankItemSelf.Init();
			this.rankItemSelf.Hide();
			this.topNodeCtrl.Init();
			if (this.firstInit)
			{
				this.firstInit = false;
				this.orderScroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnRankListGetItemByIndex), null);
			}
			this.orderScroll.mOnBeginDragAction = new Action(this.OnRankListBeginDrag);
			this.orderScroll.mOnDragingAction = new Action(this.OnRankListDragging);
			this.orderScroll.mOnEndDragAction = new Action(this.OnRankListEndDrag);
		}

		protected override void OnDeInit()
		{
			if (this.clearCacheTimeOnOpen)
			{
				this.ClearCachedOpenTime();
			}
			this.orderScroll.mOnBeginDragAction = null;
			this.orderScroll.mOnDragingAction = null;
			this.orderScroll.mOnEndDragAction = null;
			this.rankItemSelf.DeInit();
			this.topNodeCtrl.DeInit();
			for (int i = 0; i < this.rewardNodeList.Count; i++)
			{
				this.rewardNodeList[i].DeInit();
			}
			foreach (CustomBehaviour customBehaviour in this.rankNodeList.Values)
			{
				customBehaviour.DeInit();
			}
		}

		public void Open(RankViewType rankViewType, RankOpenData openData, Action<bool> onGetDataCallback = null)
		{
			this.m_rankDataModule = GameApp.Data.GetDataModule(DataName.RankDataModule);
			this.m_RankViewType = rankViewType;
			this.m_RankOpenData = openData;
			this.m_OnGetDataCallback = onGetDataCallback;
			this.obj_Order.SetActiveSafe(false);
			this.obj_Reward.SetActiveSafe(false);
			this.obj_Empty.SetActiveSafe(false);
			if (rankViewType == RankViewType.Reward)
			{
				if (onGetDataCallback != null)
				{
					onGetDataCallback(true);
				}
				this.RefreshAllReward(true, true);
				return;
			}
			this.curRankPage = 1;
			this.orderScroll.MovePanelToItemIndex(0, 0f);
			this.CheckLoadRankDataList();
		}

		private void OnStartLoadRankDataList(bool showNetLoading)
		{
			if (!this.IsOpen())
			{
				return;
			}
			this.OnBeginLoadRank(showNetLoading);
		}

		private void OnEnLoadRankDataList(bool success, bool isNoMoreData, bool isPlayAnim)
		{
			if (!this.IsOpen())
			{
				return;
			}
			this.OnEndLoadRank(success, isNoMoreData, isPlayAnim);
		}

		public void SetContentVisible(bool visible)
		{
			this.contentRoot.SetActiveSafe(visible);
		}

		private void SetRewardByCount(int count)
		{
			for (int i = 0; i < this.rewardNodeList.Count; i++)
			{
				this.rewardNodeList[i].gameObject.SetActive(false);
			}
			for (int j = 0; j < count; j++)
			{
				if (j < this.rewardNodeList.Count)
				{
					this.rewardNodeList[j].gameObject.SetActive(true);
				}
				else
				{
					UIRankRewardCtrl uirankRewardCtrl = Object.Instantiate<UIRankRewardCtrl>(this.rankRewardTemplate, this.node_Reward_Content);
					this.rewardNodeList.Add(uirankRewardCtrl);
					uirankRewardCtrl.Init();
				}
			}
		}

		private void CheckLoadRankDataList()
		{
			if (DxxTools.Time.ServerTimestamp - this.m_rankDataModule.GetLastLoadedUtc(this.m_RankOpenData.RankType) < this.rankDataCacheTime)
			{
				this.OnEnLoadRankDataList(true, false, true);
				return;
			}
			this.curRankPage = 1;
			this.LoadRankDataList(false);
		}

		private void LoadRankDataList(bool isNextPage)
		{
			if (this.curRankPage >= this.GetRankMaxPage())
			{
				this.OnEnLoadRankDataList(true, false, !isNextPage);
				return;
			}
			this.OnStartLoadRankDataList(isNextPage);
			int num = ((isNextPage && this.curRankPage < this.GetRankMaxPage() && this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count >= this.GetRankSingleCount()) ? (this.curRankPage + 1) : this.curRankPage);
			switch (this.m_RankOpenData.RankType)
			{
			case RankType.WorldBoss:
			case RankType.NewWorld:
				NetworkUtils.DoRankRequest(this.m_RankOpenData.RankType, num, isNextPage, true, new Action<int, bool, bool, LeaderBoardResponse>(this.OnNetwork_GetWorldBossRankPage));
				return;
			case RankType.WeekActivity:
			{
				ActRankOpenData actRankOpenData = (ActRankOpenData)this.m_RankOpenData;
				NetworkUtils.ActivityWeek.RequestActTimeRank(num, isNextPage, actRankOpenData.ActId, true, new Action<int, bool, bool, ActTimeRankResponse>(this.OnNetwork_GetWeekActivityRankPage));
				return;
			}
			case RankType.RogueDungeon:
				NetworkUtils.RogueDungeon.DoHellRankRequest(num, isNextPage, true, new Action<int, bool, bool, HellRankResponse>(this.OnNetwork_GeRogueDungeonRankPage));
				return;
			case RankType.GuildBossRank:
				GuildNetUtil.Guild.DoRequest_GetGuildBossGuildRankList(num, isNextPage, true, 1, new Action<int, bool, bool, GuildBossBattleGRankResponse>(this.OnNetwork_GetGuildBossRankPage));
				return;
			case RankType.GuildBossSelfRank:
				GuildNetUtil.Guild.DoRequest_GetGuildBossGuildRankList(num, isNextPage, true, 2, new Action<int, bool, bool, GuildBossBattleGRankResponse>(this.OnNetwork_GetGuildBossSelfRankPage));
				return;
			default:
				return;
			}
		}

		private void OnLoadRankTypePage(bool success, int nextPage, bool isNextPage, List<BaseRankData> rankList)
		{
			if (!success)
			{
				this.OnEnLoadRankDataList(false, false, !isNextPage);
				return;
			}
			bool flag = false;
			if (nextPage == 1)
			{
				this.m_rankDataModule.ClearLastRankData(this.m_RankOpenData.RankType);
			}
			GameApp.Data.GetDataModule(DataName.RankDataModule).SetLastLoadedUtc(this.m_RankOpenData.RankType, DxxTools.Time.ServerTimestamp);
			this.LastLoadRankDataTime = DxxTools.Time.ServerTimestamp;
			if (rankList.Count > 0)
			{
				flag = isNextPage && this.curRankPage == nextPage;
				this.curRankPage = nextPage;
				this.m_rankDataModule.SetLaseRankDataAdd(this.m_RankOpenData.RankType, rankList);
			}
			this.OnEnLoadRankDataList(true, flag, !isNextPage);
		}

		private void RefreshAllRank(bool isPlayAnim)
		{
			this.obj_Order.SetActiveSafe(true);
			if (this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count > 0)
			{
				List<BaseRankData> lastRankData = this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType);
				this.obj_Empty.SetActiveSafe(false);
				this.orderScroll.SetListItemCount(lastRankData.Count + 1, false);
				this.orderScroll.RefreshAllShowItems();
				this.FreshTopRank();
				this.FreshSelfRank(isPlayAnim);
				if (isPlayAnim)
				{
					this.PlayShowAnim();
					return;
				}
			}
			else
			{
				this.obj_Empty.SetActiveSafe(true);
				this.orderScroll.SetListItemCount(0, true);
				this.orderScroll.RefreshAllShowItems();
				this.FreshTopRank();
				this.FreshSelfRank(isPlayAnim);
			}
		}

		private void RefreshAllReward(bool isShowRankList, bool isPlayAnim)
		{
			this.obj_Reward.SetActiveSafe(true);
			this.FreshTopRank();
			this.FreshSelfRank(isPlayAnim);
			this.rewards.Clear();
			if (isShowRankList)
			{
				switch (this.m_RankOpenData.RankType)
				{
				case RankType.WorldBoss:
				{
					WorldBossDataModule dataModule = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
					int num = 0;
					this.rewards.AddRange(dataModule.RefreshRoundReward(ref num));
					break;
				}
				case RankType.WeekActivity:
				{
					ActRankOpenData actRankOpenData = (ActRankOpenData)this.m_RankOpenData;
					CommonActivity_CommonActivity commonActivityData = GameApp.Table.GetManager().GetCommonActivity_CommonActivity(actRankOpenData.ActId);
					List<CommonActivity_RankObj> list = (from x in GameApp.Table.GetManager().GetCommonActivity_RankObjModelInstance().GetAllElements()
						where x.randID == commonActivityData.RankID
						select x).ToList<CommonActivity_RankObj>();
					for (int i = 0; i < list.Count; i++)
					{
						RankReward rankReward = default(RankReward);
						rankReward.RankStart = ((i > 0) ? (list[i - 1].rank + 1) : list[i].rank);
						rankReward.RankEnd = list[i].rank;
						rankReward.Data = new PropData[list[i].reward.Length];
						for (int j = 0; j < list[i].reward.Length; j++)
						{
							string[] array = list[i].reward[j].Split(',', StringSplitOptions.None);
							ItemData itemData = new ItemData(int.Parse(array[0]), long.Parse(array[1]));
							rankReward.Data[j] = itemData.ToPropData();
						}
						this.rewards.Add(rankReward);
					}
					break;
				}
				case RankType.GuildBossRank:
				case RankType.GuildBossSelfRank:
				{
					GuildBossInfo guildBoss = GuildSDKManager.Instance.GuildActivity.GuildBoss;
					if (guildBoss != null)
					{
						GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(guildBoss.GuildDan);
						if (guildBossDanTable != null)
						{
							List<GuildBOSS_guildBossSeasonReward> list2 = GuildProxy.Table.OnGetSeasonRewardByGuildDan(guildBossDanTable.matchgroup);
							for (int k = 0; k < list2.Count; k++)
							{
								RankReward rankReward2 = default(RankReward);
								rankReward2.RankStart = int.Parse(list2[k].ranking[0]);
								rankReward2.RankEnd = int.Parse(list2[k].ranking[1]);
								rankReward2.Data = new PropData[list2[k].weeklyRewards.Length];
								for (int l = 0; l < list2[k].weeklyRewards.Length; l++)
								{
									string[] array2 = list2[k].weeklyRewards[l].Split(',', StringSplitOptions.None);
									ItemData itemData2 = new ItemData(int.Parse(array2[0]), long.Parse(array2[1]));
									rankReward2.Data[l] = itemData2.ToPropData();
								}
								this.rewards.Add(rankReward2);
							}
						}
					}
					break;
				}
				case RankType.NewWorld:
				{
					List<NewWorld_newWorldRank> list3 = GameApp.Table.GetManager().GetNewWorld_newWorldRankElements().ToList<NewWorld_newWorldRank>();
					list3.Sort((NewWorld_newWorldRank a, NewWorld_newWorldRank b) => a.id.CompareTo(b.id));
					for (int m = 0; m < list3.Count; m++)
					{
						RankReward rankReward3 = default(RankReward);
						rankReward3.RankStart = ((m > 0) ? (list3[m - 1].rank + 1) : list3[m].rank);
						rankReward3.RankEnd = list3[m].rank;
						rankReward3.Data = new PropData[list3[m].reward.Length];
						for (int n = 0; n < list3[m].reward.Length; n++)
						{
							string[] array3 = list3[m].reward[n].Split(',', StringSplitOptions.None);
							ItemData itemData3 = new ItemData(int.Parse(array3[0]), long.Parse(array3[1]));
							rankReward3.Data[n] = itemData3.ToPropData();
						}
						this.rewards.Add(rankReward3);
					}
					break;
				}
				}
			}
			this.SetRewardByCount(this.rewards.Count);
			for (int num2 = 0; num2 < this.rewards.Count; num2++)
			{
				this.rewardNodeList[num2].Init();
				this.rewardNodeList[num2].SetFresh(this.m_RankOpenData.RankType, this.rewards[num2]);
			}
			this.rewardScroll.verticalNormalizedPosition = 1f;
			if (isPlayAnim)
			{
				this.PlayShowAnim();
			}
		}

		private void PlayShowAnim()
		{
			this.seqPool.Clear(false);
			if (this.m_RankViewType == RankViewType.Reward)
			{
				for (int i = 0; i < this.rewards.Count; i++)
				{
					Transform transform = this.rewardNodeList[i].transform;
					DxxTools.UI.DoMoveRightToScreenAnim(this.seqPool.Get(), transform.GetChild(0) as RectTransform, 0f, 0.05f * (float)i, 0.3f, 9);
				}
				return;
			}
			for (int j = 0; j < this.orderScroll.ShownItemCount; j++)
			{
				LoopListViewItem2 shownItemByIndex = this.orderScroll.GetShownItemByIndex(j);
				if (!(shownItemByIndex == null))
				{
					UIBaseRankItemCtrl component = shownItemByIndex.GetComponent<UIBaseRankItemCtrl>();
					if (component != null)
					{
						component.PlayAni(0f, 0.2f, 0.05f * (float)j);
					}
				}
			}
		}

		private void FreshTopRank()
		{
			List<BaseRankData> lastRankData = this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType);
			this.topNodeCtrl.Refresh(this.m_RankOpenData.RankType, lastRankData);
		}

		private void FreshSelfRank(bool isPlayAnim)
		{
			if (isPlayAnim)
			{
				this.rankItemSelf.Hide();
			}
			this.rankItemSelf.SetFreshMy(this.m_RankOpenData.RankType);
		}

		private void OnBeginLoadRank(bool isShowNetLoading)
		{
			this.loadingStatus = MainRankPanel.LoadingStatus.Loading;
			this.netLoading.SetActive(isShowNetLoading);
		}

		private void OnEndLoadRank(bool res, bool isNoMoreData, bool isPlayAnim)
		{
			this.loadingStatus = (isNoMoreData ? MainRankPanel.LoadingStatus.Loaded : MainRankPanel.LoadingStatus.None);
			this.netLoading.SetActive(false);
			if (res)
			{
				Action<bool> onGetDataCallback = this.m_OnGetDataCallback;
				if (onGetDataCallback != null)
				{
					onGetDataCallback(true);
				}
				this.RefreshAllRank(isPlayAnim);
				return;
			}
			Action<bool> onGetDataCallback2 = this.m_OnGetDataCallback;
			if (onGetDataCallback2 == null)
			{
				return;
			}
			onGetDataCallback2(false);
		}

		private void OnRankListBeginDrag()
		{
		}

		private void OnRankListDragging()
		{
			if (this.loadingStatus != MainRankPanel.LoadingStatus.ReadyLoad && this.loadingStatus != MainRankPanel.LoadingStatus.None)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.orderScroll.GetShownItemByItemIndex(this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex2 = this.orderScroll.GetShownItemByItemIndex(this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count - 1);
			if (shownItemByItemIndex2 == null)
			{
				return;
			}
			if (this.orderScroll.GetItemCornerPosInViewPort(shownItemByItemIndex2, 0).y + this.orderScroll.ViewPortSize >= this.loadingTipItemHeight)
			{
				if (this.loadingStatus != MainRankPanel.LoadingStatus.None)
				{
					return;
				}
				this.loadingStatus = MainRankPanel.LoadingStatus.ReadyLoad;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
			else
			{
				if (this.loadingStatus != MainRankPanel.LoadingStatus.ReadyLoad)
				{
					return;
				}
				this.loadingStatus = MainRankPanel.LoadingStatus.None;
				this.UpdateLoadingTip(shownItemByItemIndex);
				return;
			}
		}

		private void OnRankListEndDrag()
		{
			if (this.loadingStatus != MainRankPanel.LoadingStatus.ReadyLoad)
			{
				return;
			}
			LoopListViewItem2 shownItemByItemIndex = this.orderScroll.GetShownItemByItemIndex(this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count);
			if (shownItemByItemIndex == null)
			{
				return;
			}
			this.orderScroll.OnItemSizeChanged(shownItemByItemIndex.ItemIndex);
			this.UpdateLoadingTip(shownItemByItemIndex);
			this.LoadRankDataList(true);
		}

		private LoopListViewItem2 OnRankListGetItemByIndex(LoopListView2 listView, int index)
		{
			if (index < 0)
			{
				return null;
			}
			List<BaseRankData> lastRankData = this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType);
			if (lastRankData.Count <= 0)
			{
				return null;
			}
			LoopListViewItem2 loopListViewItem;
			if (index == lastRankData.Count)
			{
				loopListViewItem = listView.NewListViewItem("LoadingNode");
				this.UpdateLoadingTip(loopListViewItem);
				return loopListViewItem;
			}
			if (index > lastRankData.Count)
			{
				return null;
			}
			BaseRankData baseRankData = lastRankData[index];
			if (baseRankData == null)
			{
				return null;
			}
			loopListViewItem = listView.NewListViewItem(this.scrollItemName_Other);
			CustomBehaviour component;
			this.rankNodeList.TryGetValue(loopListViewItem.gameObject.GetInstanceID(), out component);
			if (component == null)
			{
				component = loopListViewItem.gameObject.GetComponent<UIBaseRankItemCtrl>();
				component.Init();
				this.rankNodeList[loopListViewItem.gameObject.GetInstanceID()] = component;
			}
			if (index == this.m_rankDataModule.GetLastRankData(this.m_RankOpenData.RankType).Count - 1)
			{
				loopListViewItem.Padding = 0f;
			}
			UIBaseRankItemCtrl uibaseRankItemCtrl = component as UIBaseRankItemCtrl;
			if (uibaseRankItemCtrl != null)
			{
				int num = index + 1;
				uibaseRankItemCtrl.Init();
				uibaseRankItemCtrl.SetFresh(this.m_RankOpenData.RankType, baseRankData, num);
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
			if (this.loadingStatus == MainRankPanel.LoadingStatus.None)
			{
				component.SetActive(false);
				item.CachedRectTransform.SetSizeWithCurrentAnchors(1, 0f);
				return;
			}
			component.SetActive(true);
			component.SetScroll(this.orderScroll);
			if (this.loadingStatus == MainRankPanel.LoadingStatus.Loaded)
			{
				component.SetAsNoMoreData();
			}
			else
			{
				component.SetAsLoading();
			}
			item.CachedRectTransform.SetSizeWithCurrentAnchors(1, this.loadingTipItemHeight);
		}

		protected virtual int GetRankMaxPage()
		{
			if (this.m_RankOpenData.RankType == RankType.WorldBoss)
			{
				return Singleton<GameConfig>.Instance.WorldBoss_RankMaxPage;
			}
			return Singleton<GameConfig>.Instance.Sociality_RankMaxPage;
		}

		protected virtual int GetRankSingleCount()
		{
			if (this.m_RankOpenData.RankType == RankType.WorldBoss)
			{
				return Singleton<GameConfig>.Instance.WorldBoss_RankSingleCount;
			}
			return Singleton<GameConfig>.Instance.Sociality_RankSingleCount;
		}

		protected virtual void OnNetwork_GetGuildBossSelfRankPage(int nextPage, bool isNextPage, bool success, GuildBossBattleGRankResponse resp)
		{
			if (!success)
			{
				this.OnLoadRankTypePage(false, nextPage, isNextPage, null);
				return;
			}
			List<BaseRankData> list = new List<BaseRankData>();
			if (resp.Dtos != null && resp.Dtos.Count > 0)
			{
				int num = (nextPage - 1) * this.GetRankSingleCount();
				for (int i = 0; i < resp.Dtos.Count; i++)
				{
					BaseRankData baseRankData = new BaseRankData();
					int num2 = num + i + 1;
					baseRankData.SetGuildRank(this.m_RankOpenData.RankType, resp.Dtos[i], num2);
					list.Add(baseRankData);
				}
			}
			this.OnLoadRankTypePage(true, nextPage, isNextPage, list);
		}

		protected virtual void OnNetwork_GetGuildBossRankPage(int nextPage, bool isNextPage, bool success, GuildBossBattleGRankResponse resp)
		{
			if (!success)
			{
				this.OnLoadRankTypePage(false, nextPage, isNextPage, null);
				return;
			}
			List<BaseRankData> list = new List<BaseRankData>();
			if (resp.Dtos != null && resp.Dtos.Count > 0)
			{
				int num = (nextPage - 1) * this.GetRankSingleCount();
				for (int i = 0; i < resp.Dtos.Count; i++)
				{
					BaseRankData baseRankData = new BaseRankData();
					int num2 = num + i + 1;
					baseRankData.SetGuildRank(this.m_RankOpenData.RankType, resp.Dtos[i], num2);
					list.Add(baseRankData);
				}
			}
			this.OnLoadRankTypePage(true, nextPage, isNextPage, list);
		}

		protected virtual void OnNetwork_GetWorldBossRankPage(int nextPage, bool isNextPage, bool success, LeaderBoardResponse resp)
		{
			if (!success)
			{
				this.OnLoadRankTypePage(false, nextPage, isNextPage, null);
				return;
			}
			List<BaseRankData> list = new List<BaseRankData>();
			if (resp.Ranks != null && resp.Ranks.Count > 0)
			{
				for (int i = 0; i < resp.Ranks.Count; i++)
				{
					BaseRankData baseRankData = new BaseRankData();
					baseRankData.SetUserRank(this.m_RankOpenData.RankType, resp.Ranks[i]);
					list.Add(baseRankData);
				}
			}
			this.OnLoadRankTypePage(true, nextPage, isNextPage, list);
		}

		protected virtual void OnNetwork_GetWeekActivityRankPage(int nextPage, bool isNextPage, bool success, ActTimeRankResponse resp)
		{
			if (!success)
			{
				this.OnLoadRankTypePage(false, nextPage, isNextPage, null);
				return;
			}
			List<BaseRankData> list = new List<BaseRankData>();
			if (resp.Rank != null && resp.Rank.Count > 0)
			{
				for (int i = 0; i < resp.Rank.Count; i++)
				{
					BaseRankData baseRankData = new BaseRankData();
					baseRankData.SetActRank(this.m_RankOpenData.RankType, resp.Rank[i]);
					list.Add(baseRankData);
				}
			}
			this.OnLoadRankTypePage(true, nextPage, isNextPage, list);
		}

		protected virtual void OnNetwork_GeRogueDungeonRankPage(int nextPage, bool isNextPage, bool result, HellRankResponse resp)
		{
			if (!result)
			{
				this.OnLoadRankTypePage(false, nextPage, isNextPage, null);
				return;
			}
			List<BaseRankData> list = new List<BaseRankData>();
			if (resp.Rank != null && resp.Rank.Count > 0)
			{
				int num = (nextPage - 1) * this.GetRankSingleCount();
				for (int i = 0; i < resp.Rank.Count; i++)
				{
					BaseRankData baseRankData = new BaseRankData();
					int num2 = num + i + 1;
					baseRankData.SetRogueRank(this.m_RankOpenData.RankType, resp.Rank[i], num2);
					list.Add(baseRankData);
				}
			}
			this.OnLoadRankTypePage(true, nextPage, isNextPage, list);
		}

		public GameObject contentRoot;

		private bool clearCacheTimeOnOpen = true;

		[Header("排行榜")]
		[SerializeField]
		private GameObject obj_Order;

		[SerializeField]
		private LoopListView2 orderScroll;

		[SerializeField]
		private LayoutGroup orderScrollLayoutGroup;

		[SerializeField]
		private GameObject netLoading;

		[SerializeField]
		private UISelfRankItemCtrl rankItemSelf;

		[SerializeField]
		private UIRankTopNodeCtrl topNodeCtrl;

		[SerializeField]
		private GameObject obj_Empty;

		[Header("奖励预览")]
		[SerializeField]
		private GameObject obj_Reward;

		[SerializeField]
		private ScrollRect rewardScroll;

		[SerializeField]
		private UIRankRewardCtrl rankRewardTemplate;

		[SerializeField]
		private Transform node_Reward_Content;

		public string scrollItemName_Other = "Rank_Other";

		private MainRankPanel.LoadingStatus loadingStatus;

		private readonly float loadingTipItemHeight = 100f;

		private Dictionary<int, CustomBehaviour> rankNodeList = new Dictionary<int, CustomBehaviour>();

		private List<UIRankRewardCtrl> rewardNodeList = new List<UIRankRewardCtrl>();

		private readonly SequencePool seqPool = new SequencePool();

		private RankOpenData m_RankOpenData;

		private RankViewType m_RankViewType;

		private List<RankReward> rewards = new List<RankReward>();

		private readonly long rankDataCacheTime = 60L;

		private int curRankPage = 1;

		private long m_LastLoadRankDataTime;

		private RankDataModule m_rankDataModule;

		private Action<bool> m_OnGetDataCallback;

		private bool firstInit = true;

		private enum LoadingStatus
		{
			None,
			ReadyLoad,
			Loading,
			Loaded
		}
	}
}
