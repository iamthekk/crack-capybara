using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using SuperScrollView;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildRaceRankViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.PopCommon.OnClick = new Action<int>(this.OnPopClick);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
			this.Button_Rewards.onClick.AddListener(new UnityAction(this.OpenRewardsUI));
			this.Text_Next.text = "";
			this.Text_Rank.text = "";
			this.MyGuild.Init();
		}

		protected override void OnViewOpen(object data)
		{
			this.GetListFromServer();
		}

		protected override void OnViewClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void OnViewDelete()
		{
			CustomButton button_Rewards = this.Button_Rewards;
			if (button_Rewards != null)
			{
				button_Rewards.onClick.RemoveListener(new UnityAction(this.OpenRewardsUI));
			}
			GuildRaceRankItem myGuild = this.MyGuild;
			if (myGuild != null)
			{
				myGuild.Init();
			}
			this.DeInitAllScrollUI();
		}

		public void GetListFromServer()
		{
			this.MakeRankList();
			this.RefreshUI();
		}

		private void MakeRankList()
		{
			this.mDataList.Clear();
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance != null && instance.CurrentRaceKind == GuildRaceStageKind.Battle)
			{
				List<GuildRaceGuildVSRecord> allRecords = instance.AllRecords;
				IList<GuildRaceGuild> allGuildOfGroup = base.SDK.GuildActivity.GuildRace.AllGuildOfGroup;
				for (int i = 0; i < allGuildOfGroup.Count; i++)
				{
					GuildRaceGuild guildRaceGuild = this.CopyGuildRace(allGuildOfGroup[i]);
					if (guildRaceGuild != null)
					{
						this.SubBattleScore(guildRaceGuild, allRecords);
						this.mDataList.Add(guildRaceGuild);
					}
				}
				this.mDataList.Sort(new Comparison<GuildRaceGuild>(GuildRaceGuild.SortRank));
				return;
			}
			this.mDataList.AddRange(base.SDK.GuildActivity.GuildRace.AllGuildOfGroup);
		}

		private GuildRaceGuild CopyGuildRace(GuildRaceGuild raceguild)
		{
			if (raceguild == null)
			{
				return null;
			}
			return new GuildRaceGuild
			{
				RaceDan = raceguild.RaceDan,
				RaceScore = raceguild.RaceScore,
				TotalPower = raceguild.TotalPower,
				ShareData = raceguild.ShareData
			};
		}

		private void SubBattleScore(GuildRaceGuild raceguild, List<GuildRaceGuildVSRecord> records)
		{
			if (raceguild == null || records == null || records.Count <= 0)
			{
				return;
			}
			if (GuildRaceBattleController.Instance == null)
			{
				return;
			}
			GuildRaceBattleController instance = GuildRaceBattleController.Instance;
			if (instance.BattleProcess == null)
			{
				return;
			}
			ulong guildID_Ulong = raceguild.GuildID_Ulong;
			string guildID = raceguild.GuildID;
			GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
			for (int i = 0; i < records.Count; i++)
			{
				GuildRaceGuildVSRecord guildRaceGuildVSRecord2 = records[i];
				if (guildRaceGuildVSRecord2 != null && (guildRaceGuildVSRecord2.GuildID1_Ulong == guildID_Ulong || guildRaceGuildVSRecord2.GuildID2_Ulong == guildID_Ulong))
				{
					guildRaceGuildVSRecord = guildRaceGuildVSRecord2;
					break;
				}
			}
			if (guildRaceGuildVSRecord == null)
			{
				return;
			}
			int curUserPKIndex = instance.BattleProcess.CurUserPKIndex;
			List<GuildRaceUserVSRecord> resultList = guildRaceGuildVSRecord.ResultList;
			int num = 0;
			for (int j = curUserPKIndex; j < resultList.Count; j++)
			{
				GuildRaceUserVSRecord guildRaceUserVSRecord = resultList[j];
				if (guildRaceUserVSRecord != null)
				{
					GuildRaceMember winRaceUser = guildRaceUserVSRecord.GetWinRaceUser();
					if (winRaceUser != null && winRaceUser.GuildID == guildID)
					{
						GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(winRaceUser.Position);
						if (raceBaseTable != null)
						{
							num += raceBaseTable.TypeIntArray[1];
						}
					}
				}
			}
			if (num > 0)
			{
				raceguild.RaceScore -= num;
				if (raceguild.RaceScore < 0)
				{
					raceguild.RaceScore = 0;
				}
			}
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
			GuildRaceGuild guildRaceGuild = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("RankItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			GuildRaceRankItem guildRaceRankItem = this.TryGetUI(instanceID);
			GuildRaceRankItem component = loopListViewItem.GetComponent<GuildRaceRankItem>();
			if (guildRaceRankItem == null)
			{
				guildRaceRankItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			guildRaceRankItem.SetData(index + 1, guildRaceGuild);
			guildRaceRankItem.SetActive(true);
			guildRaceRankItem.RefreshUI();
			return loopListViewItem;
		}

		private GuildRaceRankItem TryGetUI(int key)
		{
			GuildRaceRankItem guildRaceRankItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildRaceRankItem))
			{
				return guildRaceRankItem;
			}
			return null;
		}

		private GuildRaceRankItem TryAddUI(int key, LoopListViewItem2 loopitem, GuildRaceRankItem ui)
		{
			ui.Init();
			GuildRaceRankItem guildRaceRankItem;
			if (this.mUICtrlDic.TryGetValue(key, out guildRaceRankItem))
			{
				if (guildRaceRankItem == null)
				{
					guildRaceRankItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, GuildRaceRankItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		public void RefreshUI()
		{
			this.m_seqPool.Clear(false);
			this.Scroll.SetListItemCount(this.mDataList.Count + 2, true);
			this.Scroll.RefreshAllShowItems();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
			string guildID = base.SDK.GuildInfo.GuildID;
			int num = this.mDataList.Count;
			GuildRaceGuild guildRaceGuild = null;
			for (int i = 0; i < this.mDataList.Count; i++)
			{
				if (guildID == this.mDataList[i].ShareData.GuildID)
				{
					num = i + 1;
					guildRaceGuild = this.mDataList[i];
				}
			}
			if (guildRaceGuild == null)
			{
				this.Text_Next.text = "";
				this.Text_Rank.text = GuildProxy.Language.GetInfoByID1("400409", "?");
				this.MyGuild.RefreshAsNull();
				return;
			}
			this.Text_Rank.text = GuildProxy.Language.GetInfoByID1("400409", num);
			GuildRace_level raceLevelTab = GuildProxy.Table.GetRaceLevelTab(guildRaceGuild.RaceDan);
			this.Text_Next.text = GuildProxy.Language.GetInfoByID1_LogError(400440, raceLevelTab.upNum);
			this.MyGuild.SetData(num, guildRaceGuild);
			this.MyGuild.RefreshUI();
		}

		private void OpenRewardsUI()
		{
			GuildProxy.UI.OpenGuildRaceRewardsShow(null);
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseGuildRaceRank();
		}

		private void OnPopClick(int obj)
		{
			this.ClickCloseThis();
		}

		public UIGuildPopCommon PopCommon;

		public CustomText Text_Next;

		public CustomText Text_Rank;

		public CustomButton Button_Rewards;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 1;

		private const int mSBottomCount = 1;

		private const int mSExCount = 2;

		private List<GuildRaceGuild> mDataList = new List<GuildRaceGuild>();

		private Dictionary<int, GuildRaceRankItem> mUICtrlDic = new Dictionary<int, GuildRaceRankItem>();

		public GuildRaceRankItem MyGuild;

		private SequencePool m_seqPool = new SequencePool();
	}
}
