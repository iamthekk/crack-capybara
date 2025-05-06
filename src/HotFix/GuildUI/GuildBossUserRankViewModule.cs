using System;
using System.Collections.Generic;
using Dxx.Guild;
using SuperScrollView;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildBossUserRankViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
			this.MyRankItem.Init();
			this.ObjEmptyList.SetActive(false);
			this.Scroll.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetItemByIndex), null);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.LoadAndShowRank();
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
		}

		protected override void OnViewDelete()
		{
			this.DeInitAllScrollUI();
			this.MyRankItem.DeInit();
			base.OnViewDelete();
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
			GuildBossRankData guildBossRankData = this.mDataList[index];
			LoopListViewItem2 loopListViewItem = listView.NewListViewItem("RankItem");
			int instanceID = loopListViewItem.gameObject.GetInstanceID();
			UIGuildBossUserDamageRankItem uiguildBossUserDamageRankItem = this.TryGetUI(instanceID);
			UIGuildBossUserDamageRankItem component = loopListViewItem.GetComponent<UIGuildBossUserDamageRankItem>();
			if (uiguildBossUserDamageRankItem == null)
			{
				uiguildBossUserDamageRankItem = this.TryAddUI(instanceID, loopListViewItem, component);
			}
			uiguildBossUserDamageRankItem.SetData(guildBossRankData);
			uiguildBossUserDamageRankItem.SetActive(true);
			uiguildBossUserDamageRankItem.RefreshUI();
			return loopListViewItem;
		}

		private UIGuildBossUserDamageRankItem TryGetUI(int key)
		{
			UIGuildBossUserDamageRankItem uiguildBossUserDamageRankItem;
			if (this.mUICtrlDic.TryGetValue(key, out uiguildBossUserDamageRankItem))
			{
				return uiguildBossUserDamageRankItem;
			}
			return null;
		}

		private UIGuildBossUserDamageRankItem TryAddUI(int key, LoopListViewItem2 loopitem, UIGuildBossUserDamageRankItem ui)
		{
			ui.Init();
			UIGuildBossUserDamageRankItem uiguildBossUserDamageRankItem;
			if (this.mUICtrlDic.TryGetValue(key, out uiguildBossUserDamageRankItem))
			{
				if (uiguildBossUserDamageRankItem == null)
				{
					uiguildBossUserDamageRankItem = ui;
					this.mUICtrlDic[key] = ui;
				}
				return ui;
			}
			this.mUICtrlDic.Add(key, ui);
			return ui;
		}

		private void DeInitAllScrollUI()
		{
			foreach (KeyValuePair<int, UIGuildBossUserDamageRankItem> keyValuePair in this.mUICtrlDic)
			{
				keyValuePair.Value.DeInit();
			}
			this.mUICtrlDic.Clear();
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType kind)
		{
			GuildProxy.UI.CloseUIGuildBossChallengeRecord();
		}

		private void LoadAndShowRank()
		{
			this.m_seqPool.Clear(false);
			this.RefreshList();
			this.RefreshMyRank();
		}

		private void RefreshList()
		{
			this.mDataList.Clear();
			List<GuildBossRankData> myGuildBossRankList = base.SDK.GuildActivity.GetMyGuildBossRankList();
			this.mDataList.AddRange(myGuildBossRankList);
			this.ObjEmptyList.SetActive(this.mDataList.Count <= 0);
			this.Scroll.SetListItemCount(this.mDataList.Count + 2, true);
			this.Scroll.RefreshAllShowItems();
			DxxTools.UI.DoMoveRightToScreenAnim(this.m_seqPool.Get(), this.Scroll.ItemList, 0f, 0.05f, 0.2f, 9);
		}

		private void RefreshMyRank()
		{
			GuildBossRankData guildBossRankData = base.SDK.GuildActivity.GetMyGuildBossRankMyData();
			if (guildBossRankData == null || guildBossRankData.UserData == null)
			{
				guildBossRankData = new GuildBossRankData();
				guildBossRankData.UserData = new GuildUserShareData();
				guildBossRankData.UserData.CloneFrom(base.SDK.User.MyUserData);
				guildBossRankData.Rank = guildBossRankData.Rank;
				guildBossRankData.Damage = guildBossRankData.Damage;
			}
			this.MyRankItem.SetData(guildBossRankData);
			this.MyRankItem.RefreshUI();
		}

		public UIPopCommon PopCommon;

		public GameObject ObjEmptyList;

		public UIGuildBossUserDamageRankItem MyRankItem;

		[Header("滚动区域")]
		public LoopListView2 Scroll;

		private const int mSTopCount = 1;

		private const int mSBottomCount = 1;

		private const int mSExCount = 2;

		private List<GuildBossRankData> mDataList = new List<GuildBossRankData>();

		private Dictionary<int, UIGuildBossUserDamageRankItem> mUICtrlDic = new Dictionary<int, UIGuildBossUserDamageRankItem>();

		private SequencePool m_seqPool = new SequencePool();
	}
}
