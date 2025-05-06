using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildBossRankViewModule : GuildProxy.GuildProxy_BaseView
	{
		private GuildBossInfo guildBossInfo
		{
			get
			{
				return base.SDK.GuildActivity.GuildBoss;
			}
		}

		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.RankPanel.Init();
			this.RankPanel.SetContentVisible(false);
			this.Button_Mask.m_onClick = new Action(this.OnClickClose);
			this.Button_Close.m_onClick = new Action(this.OnClickClose);
			this.Button_GuildRank.onClick.AddListener(new UnityAction(this.OnClickGuildRank));
			this.Button_GuildRankReward.onClick.AddListener(new UnityAction(this.OnClickRankReward));
			this.Button_SelfRank.onClick.AddListener(new UnityAction(this.OnClickSelfRank));
		}

		private void OnClickGuildRank()
		{
			this.OpenRankPanel(RankViewType.Guild, false);
			this.RefreshTitleInfo(RankViewType.Guild);
		}

		private void OnClickRankReward()
		{
			if (GuildSDKManager.Instance.GuildActivity.MyGuildRankData == null)
			{
				GuildNetUtil.Guild.DoRequest_GetGuildBossGuildRankList(1, false, true, 1, delegate(int nextPage, bool isNextPage, bool success, GuildBossBattleGRankResponse resp)
				{
					List<BaseRankData> list = new List<BaseRankData>();
					if (resp.Dtos != null && resp.Dtos.Count > 0)
					{
						for (int i = 0; i < resp.Dtos.Count; i++)
						{
							BaseRankData baseRankData = new BaseRankData();
							baseRankData.SetGuildRank(RankType.GuildBossRank, resp.Dtos[i], i + 1);
							list.Add(baseRankData);
						}
					}
					GameApp.Data.GetDataModule(DataName.RankDataModule).SetLaseRankDataAdd(RankType.GuildBossRank, list);
					this.OpenRankPanel(RankViewType.Reward, false);
					this.RefreshTitleInfo(RankViewType.Reward);
				});
				return;
			}
			this.OpenRankPanel(RankViewType.Reward, false);
			this.RefreshTitleInfo(RankViewType.Reward);
		}

		private void OnClickSelfRank()
		{
			this.OpenRankPanel(RankViewType.Order, false);
			this.RefreshTitleInfo(RankViewType.Order);
		}

		private void OnClickClose()
		{
			GuildProxy.UI.CloseUIGuildBossGuildRank();
		}

		private void OpenRankPanel(RankViewType viewType, bool force = false)
		{
			if (this.m_RankViewType == viewType && !force)
			{
				return;
			}
			this.m_RankViewType = viewType;
			this.Button_GuildRank.SetSelect(viewType == RankViewType.Guild);
			this.Button_GuildRankReward.SetSelect(viewType == RankViewType.Reward);
			this.Button_SelfRank.SetSelect(viewType == RankViewType.Order);
			RankOpenData rankOpenData = new RankOpenData();
			rankOpenData.RankType = ((viewType == RankViewType.Order) ? RankType.GuildBossSelfRank : RankType.GuildBossRank);
			this.RankPanel.Open(viewType, rankOpenData, new Action<bool>(this.OnOpedRankPanel));
		}

		private void OnOpedRankPanel(bool success)
		{
			if (success)
			{
				this.RankPanel.SetContentVisible(true);
				return;
			}
			GameApp.View.CloseView(ViewName.GuildBossGuildRankViewModule, null);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.m_isShowBtn = false;
			if (this.guildBossInfo == null)
			{
				return;
			}
			if (this.guildBossInfo != null)
			{
				Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(140);
				if (guildConstTable != null && this.guildBossInfo.BossData.BossStep >= guildConstTable.TypeInt)
				{
					this.m_isShowBtn = true;
				}
			}
			this.Button_GuildRank.gameObject.SetActiveSafe(this.m_isShowBtn);
			this.Button_SelfRank.gameObject.SetActiveSafe(this.m_isShowBtn);
			this.Button_GuildRankReward.gameObject.SetActiveSafe(this.m_isShowBtn);
			this.RankPanel.SetContentVisible(false);
			GuildBossRankViewModule.GuildBossRankData guildBossRankData = null;
			if (data != null)
			{
				guildBossRankData = data as GuildBossRankViewModule.GuildBossRankData;
			}
			if (guildBossRankData == null)
			{
				return;
			}
			this.RefreshTitleInfo(guildBossRankData.ViewType);
			this.OpenRankPanel(guildBossRankData.ViewType, true);
		}

		private void RefreshTitleInfo(RankViewType viewType)
		{
			if (viewType == RankViewType.Order)
			{
				this.Text_RankTip.gameObject.SetActiveSafe(false);
				return;
			}
			if (this.m_isShowBtn && this.guildBossInfo.GuildDan > 0)
			{
				this.Text_RankTip.gameObject.SetActiveSafe(true);
				GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(this.guildBossInfo.GuildDan);
				int promotionRank = guildBossDanTable.PromotionRank;
				this.Text_RankTip.text = ((promotionRank > 0) ? Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_PromotionWillUp", new object[] { guildBossDanTable.PromotionRank }) : Singleton<LanguageManager>.Instance.GetInfoByID("WorldBoss_Rank_PromotionWillDown", new object[] { guildBossDanTable.DemoteRank }));
				return;
			}
			this.Text_RankTip.gameObject.SetActiveSafe(false);
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			RankDataModule dataModule = GameApp.Data.GetDataModule(DataName.RankDataModule);
			if (dataModule != null)
			{
				dataModule.ClearLastLoadedUtc(RankType.GuildBossRank);
			}
			if (dataModule != null)
			{
				dataModule.ClearLastLoadedUtc(RankType.GuildBossSelfRank);
			}
			if (dataModule != null)
			{
				dataModule.ClearLastRankData(RankType.GuildBossRank);
			}
			if (dataModule != null)
			{
				dataModule.ClearLastRankData(RankType.GuildBossSelfRank);
			}
			GuildSDKManager.Instance.GuildActivity.SetMyGuildRankEmpty();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.Button_Close.m_onClick = null;
			this.Button_GuildRank.onClick.RemoveListener(new UnityAction(this.OnClickGuildRank));
			this.Button_GuildRankReward.onClick.RemoveListener(new UnityAction(this.OnClickRankReward));
			this.Button_SelfRank.onClick.RemoveListener(new UnityAction(this.OnClickSelfRank));
			this.RankPanel.DeInit();
		}

		public CustomButton Button_Mask;

		public CustomButton Button_Close;

		public CustomText Text_RankTip;

		public MainRankPanel RankPanel;

		public CustomChooseButton Button_GuildRank;

		public CustomChooseButton Button_GuildRankReward;

		public CustomChooseButton Button_SelfRank;

		private RankViewType m_RankViewType;

		private bool m_isShowBtn;

		public class GuildBossRankData
		{
			public RankViewType ViewType = RankViewType.Guild;
		}
	}
}
