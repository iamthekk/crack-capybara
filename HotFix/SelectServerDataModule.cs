using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.ServerList;

namespace HotFix
{
	public class SelectServerDataModule : IDataModule
	{
		private static uint jumpServerId { get; set; } = 0U;

		private static string m_account { get; set; } = "";

		private static string m_deviceID { get; set; } = "";

		private static string m_account2 { get; set; } = "";

		public static void SetJumpServerData(uint serverId)
		{
			SelectServerDataModule.jumpServerId = serverId;
			SelectServerDataModule.m_account = GameApp.NetWork.m_account;
			SelectServerDataModule.m_deviceID = GameApp.NetWork.m_deviceID;
			SelectServerDataModule.m_account2 = GameApp.NetWork.m_account2;
		}

		public static void ClearJumpServerData()
		{
			SelectServerDataModule.jumpServerId = 0U;
			SelectServerDataModule.m_account = "";
			SelectServerDataModule.m_deviceID = "";
			SelectServerDataModule.m_account2 = "";
		}

		public static uint GetJumpServerId(string account, string account2, string deviceId)
		{
			if (string.IsNullOrEmpty(account))
			{
				return SelectServerDataModule.jumpServerId;
			}
			if (account.Equals(SelectServerDataModule.m_account))
			{
				return SelectServerDataModule.jumpServerId;
			}
			return 0U;
		}

		public bool isDataInit { get; private set; }

		public List<RoleDetailDto> RoleList { get; private set; } = new List<RoleDetailDto>();

		public Dictionary<uint, ZoneInfoDto> ServerGroupMap { get; private set; } = new Dictionary<uint, ZoneInfoDto>();

		public Dictionary<uint, ServerInfoDto> ServerStatusDict { get; private set; } = new Dictionary<uint, ServerInfoDto>();

		public uint GetServerZoneIdByServerId(uint serverId)
		{
			uint num = 0U;
			IList<ServerList_serverList> allElements = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				ServerList_serverList serverList_serverList = allElements[i];
				if (serverList_serverList != null && (long)serverList_serverList.range[0] <= (long)((ulong)serverId) && (ulong)serverId <= (ulong)((long)serverList_serverList.range[1]))
				{
					return (uint)serverList_serverList.id;
				}
			}
			return num;
		}

		public List<RoleDetailDto> GetZoneRoleList(uint zoneId)
		{
			List<RoleDetailDto> list = new List<RoleDetailDto>();
			if (this.RoleList != null)
			{
				for (int i = 0; i < this.RoleList.Count; i++)
				{
					RoleDetailDto roleDetailDto = this.RoleList[i];
					if (this.GetServerZoneIdByServerId(roleDetailDto.ServerId).Equals(zoneId))
					{
						list.Add(roleDetailDto);
					}
				}
			}
			list.Sort((RoleDetailDto a, RoleDetailDto b) => a.LastLoginPass.CompareTo(b.LastLoginPass));
			return list;
		}

		public List<uint> GetRoleZoneList()
		{
			List<uint> list = new List<uint>();
			if (this.RoleList != null)
			{
				for (int i = 0; i < this.RoleList.Count; i++)
				{
					RoleDetailDto roleDetailDto = this.RoleList[i];
					uint serverZoneIdByServerId = this.GetServerZoneIdByServerId(roleDetailDto.ServerId);
					if (serverZoneIdByServerId > 0U && !list.Contains(serverZoneIdByServerId))
					{
						list.Add(serverZoneIdByServerId);
					}
				}
			}
			list.Sort((uint a, uint b) => a.CompareTo(b));
			return list;
		}

		public List<ServerGroupData> GetGroupList(int serverZoneType)
		{
			List<ServerGroupData> list = new List<ServerGroupData>();
			ZoneInfoDto zoneInfoDto;
			if (this.ServerGroupMap.TryGetValue((uint)serverZoneType, out zoneInfoDto))
			{
				uint maxServer = zoneInfoDto.MaxServer;
				ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById(serverZoneType);
				IList<ServerList_serverGrop> allElements = GameApp.Table.GetManager().GetServerList_serverGropModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					ServerList_serverGrop serverList_serverGrop = allElements[i];
					if (elementById.range[0] <= serverList_serverGrop.range[0] && serverList_serverGrop.range[1] <= elementById.range[1] && (ulong)maxServer >= (ulong)((long)serverList_serverGrop.range[0]))
					{
						list.Add(new ServerGroupData
						{
							zoneId = (uint)serverZoneType,
							groupId = (uint)serverList_serverGrop.id,
							maxServerId = maxServer,
							sortId = serverList_serverGrop.sortId
						});
					}
				}
			}
			list.Sort((ServerGroupData a, ServerGroupData b) => b.sortId.CompareTo(a.sortId));
			return list;
		}

		public List<ServerInfoData> GetServerInfoList(ServerGroupData groupData)
		{
			List<ServerInfoData> list = new List<ServerInfoData>();
			ServerList_serverGrop elementById = GameApp.Table.GetManager().GetServerList_serverGropModelInstance().GetElementById((int)groupData.groupId);
			uint maxServerId = groupData.maxServerId;
			for (int i = elementById.range[0]; i <= elementById.range[1]; i++)
			{
				if ((long)i <= (long)((ulong)maxServerId))
				{
					list.Add(new ServerInfoData
					{
						zoneId = groupData.zoneId,
						serverId = (uint)i,
						groupId = groupData.groupId
					});
				}
			}
			list.Sort((ServerInfoData a, ServerInfoData b) => b.serverId.CompareTo(a.serverId));
			return list;
		}

		public int GetServerStatus(uint serverId)
		{
			ServerInfoDto serverInfoDto;
			if (this.ServerStatusDict.TryGetValue(serverId, out serverInfoDto))
			{
				return (int)serverInfoDto.Status;
			}
			return 0;
		}

		public int GetName()
		{
			return 162;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void PullServerData(Action<bool> callback, bool isShowMask)
		{
			NetworkUtils.UserGetLastLoginRequest(delegate(bool result, UserGetLastLoginResponse resp)
			{
				if (result)
				{
					this.isDataInit = true;
					this.UpdateData(resp);
					Action<bool> callback2 = callback;
					if (callback2 != null)
					{
						callback2(true);
					}
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_SelectServer_DataChanged, null);
					return;
				}
				Action<bool> callback3 = callback;
				if (callback3 == null)
				{
					return;
				}
				callback3(false);
			}, isShowMask);
		}

		public void PullServerStateData(uint groupId)
		{
			NetworkUtils.FindServerListRequest(groupId, delegate(bool result, FindServerListResponse resp)
			{
				if (result)
				{
					foreach (KeyValuePair<uint, ServerGroupDto> keyValuePair in resp.ServerInfoDto.ServerList)
					{
						ServerGroupDto value = keyValuePair.Value;
						for (int i = 0; i < value.ServerInfoDto.Count; i++)
						{
							this.ServerStatusDict[value.ServerInfoDto[i].ServerId] = value.ServerInfoDto[i];
						}
					}
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_SelectServer_StatusChanged, null);
				}
			});
		}

		private void UpdateData(UserGetLastLoginResponse resp)
		{
			if (resp == null)
			{
				return;
			}
			this.RoleList.Clear();
			this.ServerGroupMap.Clear();
			for (int i = 0; i < resp.RoleList.Count; i++)
			{
				this.RoleList.Add(resp.RoleList[i]);
			}
			foreach (KeyValuePair<uint, ZoneInfoDto> keyValuePair in resp.ServerList)
			{
				this.ServerGroupMap.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public void Reset()
		{
			this.isDataInit = false;
			this.RoleList.Clear();
			this.ServerGroupMap.Clear();
		}
	}
}
