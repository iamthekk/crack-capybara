using System;
using System.Collections.Generic;
using System.Linq;
using Dxx.Guild;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;
using Proto.Guild;
using Proto.Social;
using UnityEngine;

namespace HotFix
{
	public class SocialityDataModule : IDataModule
	{
		public bool IsFinishedForRank
		{
			get
			{
				return this.m_isFinishedForRank;
			}
		}

		public bool IsHaveGuild
		{
			get
			{
				this.m_isHaveGuild = false;
				if (GuildSDKManager.Instance.GuildInfo != null)
				{
					this.m_isHaveGuild = !string.IsNullOrEmpty(GuildSDKManager.Instance.GuildInfo.GuildID);
				}
				return this.m_isHaveGuild;
			}
		}

		public int InteractSocketCount
		{
			get
			{
				return this.m_interactSocketCount;
			}
		}

		public int GetName()
		{
			return 123;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingRankData, new HandlerEvent(this.OnEventCheckLoadingRankData));
			manager.RegisterEvent(LocalMessageName.CC_SocialityDataModule_LoadingRankData, new HandlerEvent(this.OnEventLoadingRankData));
			manager.RegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingGuildData, new HandlerEvent(this.OnEventCheckLoadingGuildData));
			manager.RegisterEvent(LocalMessageName.CC_SocialityDataModule_AddInteractiveSocketCount, new HandlerEvent(this.OnEventAddInteractiveSocketCount));
			manager.RegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingInteractiveData, new HandlerEvent(this.OnEventCheckLoadingInteractiveData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingRankData, new HandlerEvent(this.OnEventCheckLoadingRankData));
			manager.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_LoadingRankData, new HandlerEvent(this.OnEventLoadingRankData));
			manager.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingGuildData, new HandlerEvent(this.OnEventCheckLoadingGuildData));
			manager.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_AddInteractiveSocketCount, new HandlerEvent(this.OnEventAddInteractiveSocketCount));
			manager.UnRegisterEvent(LocalMessageName.CC_SocialityDataModule_CheckLoadingInteractiveData, new HandlerEvent(this.OnEventCheckLoadingInteractiveData));
		}

		public void Reset()
		{
		}

		private void OnEventCheckLoadingRankData(object sender, int type, BaseEventArgs eventargs)
		{
			this.CheckLoadRankDatas();
		}

		private void OnEventLoadingRankData(object sender, int type, BaseEventArgs eventargs)
		{
			this.LoadRankDatas();
		}

		private void OnEventCheckLoadingGuildData(object sender, int type, BaseEventArgs eventargs)
		{
			this.LoadGuildDatas();
		}

		private void OnEventCheckLoadingInteractiveData(object sender, int type, BaseEventArgs eventargs)
		{
			this.LoadInteractiveDatas();
		}

		private void OnEventAddInteractiveSocketCount(object sender, int type, BaseEventArgs eventargs)
		{
			this.m_interactSocketCount++;
		}

		private void CheckLoadRankDatas()
		{
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - this.m_lastRefreshTimeForRank < this.m_spanTimeForRank)
			{
				return;
			}
			this.LoadRankDatas();
		}

		private void LoadRankDatas()
		{
			if (GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC - this.m_lastRefreshTimeForRank < this.m_spanTimeForRank)
			{
				this.m_page++;
			}
			else
			{
				this.m_isFinishedForRank = false;
				this.m_ranks.Clear();
				this.m_page = 1;
			}
			if (this.m_isFinishedForRank)
			{
				return;
			}
			NetworkUtils.Sociality.DoSocialPowerRankRequest(this.m_page, delegate(bool isOk, SocialPowerRankResponse resp)
			{
				if (!isOk)
				{
					return;
				}
				long localUTC = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC;
				this.m_lastRefreshTimeForRank = localUTC;
				bool flag = this.m_page >= Singleton<GameConfig>.Instance.Sociality_RankMaxPage;
				bool flag2;
				if (resp.Rank != null && resp.Rank.Count > 0)
				{
					this.m_ranks.AddRange(resp.Rank);
					flag2 = true;
				}
				else
				{
					flag = true;
					flag2 = this.m_isFinishedForRank != flag;
				}
				this.m_isFinishedForRank = flag;
				if (flag2)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_RefreshRankData, null);
				}
			});
		}

		private void LoadGuildDatas()
		{
			this.m_guilds.Clear();
			GuildNetUtil.Guild.DoRequest_GetGuildMemberList(GuildSDKManager.Instance.GuildInfo.GuildID, delegate(bool isOK, GuildGetMemberListResponse rep)
			{
				if (!isOK)
				{
					return;
				}
				GameApp.Data.GetDataModule(DataName.LoginDataModule);
				RepeatedField<GuildMemberInfoDto> guildMemberInfoDtos = rep.GuildMemberInfoDtos;
				for (int i = 0; i < guildMemberInfoDtos.Count; i++)
				{
					GuildMemberInfoDto guildMemberInfoDto = guildMemberInfoDtos[i];
					if (guildMemberInfoDto != null)
					{
						this.m_guilds.Add(guildMemberInfoDto);
					}
				}
				IOrderedEnumerable<GuildMemberInfoDto> orderedEnumerable = from dto in this.m_guilds
					orderby dto.BattlePower descending, dto.Position
					select dto;
				this.m_guilds = orderedEnumerable.ToList<GuildMemberInfoDto>();
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_RefreshGuildData, null);
			}, false);
		}

		private void LoadInteractiveDatas()
		{
			this.m_interacts.Clear();
			NetworkUtils.Sociality.DoInteractListRequest(delegate(bool isOk, InteractListResponse rep)
			{
				if (!isOk)
				{
					return;
				}
				if (rep.Interacts == null)
				{
					return;
				}
				this.m_interactSocketCount = 0;
				long localUTC = GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalUTC;
				for (int i = 0; i < rep.Interacts.Count; i++)
				{
					InteractDto interactDto = rep.Interacts[i];
					if (interactDto != null)
					{
						SocialityInteractiveData socialityInteractiveData = new SocialityInteractiveData();
						socialityInteractiveData.RefreshData(interactDto, localUTC);
						this.m_interacts.Add(socialityInteractiveData);
					}
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_SocialityDataModule_RefreshInteractiveData, null);
			});
		}

		public int GetInteractiveCount()
		{
			int num = 0;
			for (int i = 0; i < this.m_interacts.Count; i++)
			{
				SocialityInteractiveData socialityInteractiveData = this.m_interacts[i];
				if (socialityInteractiveData != null && !socialityInteractiveData.m_status)
				{
					num++;
				}
			}
			return num + this.m_interactSocketCount;
		}

		public List<PowerRankDto> m_ranks = new List<PowerRankDto>();

		private long m_lastRefreshTimeForRank;

		private int m_page;

		private long m_spanTimeForRank = 7200L;

		private bool m_isFinishedForRank;

		private bool m_isHaveGuild;

		public List<GuildMemberInfoDto> m_guilds = new List<GuildMemberInfoDto>();

		public List<SocialityInteractiveData> m_interacts = new List<SocialityInteractiveData>();

		[SerializeField]
		private int m_interactSocketCount;
	}
}
