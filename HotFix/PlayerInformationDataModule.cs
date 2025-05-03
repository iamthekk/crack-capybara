using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.User;

namespace HotFix
{
	public class PlayerInformationDataModule : IDataModule
	{
		public int GetName()
		{
			return 125;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_PlayerInformationData_SetData, new HandlerEvent(this.OnEventSetData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_PlayerInformationData_SetData, new HandlerEvent(this.OnEventSetData));
		}

		public void AddOrUpdateCachePlayerInfo(List<PlayerInfoDto> playerInfos)
		{
			if (playerInfos == null)
			{
				return;
			}
			foreach (PlayerInfoDto playerInfoDto in playerInfos)
			{
				this.AddOrUpdateCachePlayerInfo(playerInfoDto);
			}
		}

		public void AddOrUpdateCachePlayerInfo(PlayerInfoDto playerInfo)
		{
			if (playerInfo == null)
			{
				return;
			}
			DxxTools.Game.VersionMatch(playerInfo);
			this.m_cachePlayerInfoDict[playerInfo.UserId] = playerInfo;
			EventArgsSetPlayerInformationData instance = Singleton<EventArgsSetPlayerInformationData>.Instance;
			instance.SetData(playerInfo.UserId, playerInfo);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_PlayerInformationData_DataUpdate, instance);
		}

		public bool TryGetCachePlayerInfo(long playerID, out PlayerInfoDto playerInfo)
		{
			playerInfo = null;
			if (playerID == 0L || this.m_cachePlayerInfoDict == null)
			{
				return false;
			}
			if (this.m_cachePlayerInfoDict.TryGetValue(playerID, out playerInfo))
			{
				if (DxxTools.Time.ServerTimestamp - (long)playerInfo.Timestamp > 10L)
				{
					NetworkUtils.User.DoUserGetPlayerInfoRequest(new List<long> { playerID }, null);
				}
				return true;
			}
			NetworkUtils.User.DoUserGetPlayerInfoRequest(new List<long> { playerID }, null);
			return false;
		}

		public void Reset()
		{
			this.m_cachePlayerInfoDict.Clear();
		}

		private void OnEventSetData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetPlayerInformationData eventArgsSetPlayerInformationData = eventargs as EventArgsSetPlayerInformationData;
			if (eventArgsSetPlayerInformationData == null)
			{
				return;
			}
			if (eventArgsSetPlayerInformationData.m_response == null)
			{
				return;
			}
			this.m_cachePlayerInfoDict[eventArgsSetPlayerInformationData.m_response.UserId] = eventArgsSetPlayerInformationData.m_response;
		}

		public Dictionary<long, PlayerInfoDto> m_cachePlayerInfoDict = new Dictionary<long, PlayerInfoDto>();

		public const long needUpdateInterval = 10L;
	}
}
