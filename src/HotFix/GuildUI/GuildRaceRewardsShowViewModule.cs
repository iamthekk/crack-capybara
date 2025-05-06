using System;
using System.Collections.Generic;
using Dxx.Guild;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRewardsShowViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.PopCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnViewOpen(object data)
		{
			this.RefreshUI();
		}

		protected override void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			this.DeInitAllScrollUI();
		}

		public void RefreshUI()
		{
			this.m_seqPool.Clear(false);
			if (this.mDataList.Count <= 0)
			{
				this.BuildDataList();
			}
			this.Scroll.SetListItemCount(this.mDataList.Count + 2, true);
			this.Scroll.RefreshAllShowItems();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

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
			GuildRaceRewardsShowItemData guildRaceRewardsShowItemData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("RewardItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			GuildRaceRewardsShowItem guildRaceRewardsShowItem = this.TryGetUI(instanceID);
			GuildRaceRewardsShowItem component = loopListViewItem.GetComponent<GuildRaceRewardsShowItem>();
			if (guildRaceRewardsShowItem == null)
			{
				guildRaceRewardsShowItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			guildRaceRewardsShowItem.SetData(guildRaceRewardsShowItemData);
			guildRaceRewardsShowItem.SetActive(true);
			guildRaceRewardsShowItem.RefreshUI();
			return loopListViewItem;
		}

		private GuildRaceRewardsShowItem TryGetUI(int key)
		{
			GuildRaceRewardsShowItem guildRaceRewardsShowItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildRaceRewardsShowItem))
			{
				return guildRaceRewardsShowItem;
			}
			return null;
		}

		private GuildRaceRewardsShowItem TryAddUI(int key, LoopListViewItem2 loopitem, GuildRaceRewardsShowItem ui)
		{
			ui.Init();
			GuildRaceRewardsShowItem guildRaceRewardsShowItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildRaceRewardsShowItem))
			{
				if (guildRaceRewardsShowItem == null)
				{
					guildRaceRewardsShowItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, GuildRaceRewardsShowItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		private void BuildDataList()
		{
			this.mDataList.Clear();
			List<GuildRace_level> raceLevelAllTab = GuildProxy.Table.GetRaceLevelAllTab();
			for (int i = 0; i < raceLevelAllTab.Count; i++)
			{
				GuildRace_level guildRace_level = raceLevelAllTab[i];
				GuildRaceRewardsShowItemData guildRaceRewardsShowItemData = new GuildRaceRewardsShowItemData();
				guildRaceRewardsShowItemData.RaceDan = guildRace_level.Level;
				string[] array = guildRace_level.rewards;
				for (int j = 0; j < array.Length; j++)
				{
					GuildItemData guildItemData = GuildProxy.Table.StringToGuildItemData(array[j]);
					if (guildItemData != null)
					{
						guildRaceRewardsShowItemData.Rewards.Add(guildItemData);
					}
				}
				array = guildRace_level.winRewards;
				for (int k = 0; k < array.Length; k++)
				{
					GuildItemData guildItemData2 = GuildProxy.Table.StringToGuildItemData(array[k]);
					if (guildItemData2 != null)
					{
						guildRaceRewardsShowItemData.RewardsWin.Add(guildItemData2);
					}
				}
				this.mDataList.Add(guildRaceRewardsShowItemData);
			}
			this.mDataList.Sort(new Comparison<GuildRaceRewardsShowItemData>(this.SortByDan));
		}

		private int SortByDan(GuildRaceRewardsShowItemData x, GuildRaceRewardsShowItemData y)
		{
			return y.RaceDan.CompareTo(x.RaceDan);
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceRewardsShow();
		}

		private void OnPopClick(int obj)
		{
			this.ClickCloseThis();
		}

		public UIGuildPopCommon PopCommon;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 1;

		private const int mSBottomCount = 1;

		private const int mSExCount = 2;

		private List<GuildRaceRewardsShowItemData> mDataList = new List<GuildRaceRewardsShowItemData>();

		private Dictionary<int, GuildRaceRewardsShowItem> mUICtrlDic = new Dictionary<int, GuildRaceRewardsShowItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
