using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Proto.ServerList;
using SuperScrollView;
using UnityEngine;

namespace HotFix
{
	public class SelectServerViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.tabItem.gameObject.SetActive(false);
			this.selectServerDataModule = GameApp.Data.GetDataModule(DataName.SelectServerDataModule);
			this.InitLoopListServerTab();
			this.InitLoopListServerItem();
		}

		public override void OnOpen(object data)
		{
			this.UpdateTabData();
			this.CreateTabItems();
			this.tabGroup.CollectChildButtons();
			this.tabGroup.ChooseButtonName(this.tabList[0].name);
			uint serverZoneIdByServerId = this.selectServerDataModule.GetServerZoneIdByServerId(GameApp.NetWork.m_serverID);
			ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)serverZoneIdByServerId);
			if (elementById != null)
			{
				uint num = GameApp.NetWork.m_serverID - (uint)elementById.range[0] + 1U;
				this.textCurServer.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.serverPrefix, new object[] { num });
				return;
			}
			this.textCurServer.text = "";
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.DeInitLoopListServerItem();
			this.DeInitLoopListServerTab();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.tabGroup.OnSwitch = new Action<CustomChooseButton>(this.OnSwitchPage);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_SelectServer_DataChanged, new HandlerEvent(this.OnServerDataChange));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.uiPopCommon.OnClick = null;
			this.tabGroup.OnSwitch = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_SelectServer_DataChanged, new HandlerEvent(this.OnServerDataChange));
		}

		private void CreateTabItems()
		{
			for (int i = 0; i < this.tabDatas.Count; i++)
			{
				CustomChooseButton customChooseButton = Object.Instantiate<CustomChooseButton>(this.tabItem, this.tabParent, false);
				customChooseButton.transform.localScale = Vector3.one;
				customChooseButton.gameObject.SetActive(true);
				customChooseButton.name = this.tabDatas[i].zoneId.ToString();
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.tabDatas[i].nameId);
				customChooseButton.m_selectText.text = infoByID;
				customChooseButton.m_unSelectText.text = infoByID;
				customChooseButton.SetSelect(false);
				this.tabList.Add(customChooseButton);
			}
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			this.OnBtnCloseClick();
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.SelectServerViewModule, null);
		}

		private void OnSwitchPage(CustomChooseButton button)
		{
			if (button == null)
			{
				return;
			}
			int num = int.Parse(button.name);
			if (num == this.curTabType)
			{
				return;
			}
			this.curTabType = num;
			this.UpdateView();
		}

		private void UpdateView()
		{
			this.UpdateServerTabDatas();
			this.RefreshLoopListServerTab();
		}

		private void UpdateTabData()
		{
			List<uint> list = this.selectServerDataModule.ServerGroupMap.Keys.ToList<uint>();
			this.tabDatas.Clear();
			this.tabDatas.Add(new SelectServerViewModule.TabData
			{
				zoneId = 0U,
				sortId = 0,
				nameId = "select_server_recent_login"
			});
			for (int i = 0; i < list.Count; i++)
			{
				uint num = list[i];
				ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)num);
				SelectServerViewModule.TabData tabData = new SelectServerViewModule.TabData
				{
					zoneId = num,
					sortId = elementById.sortId,
					nameId = elementById.nameId
				};
				this.tabDatas.Add(tabData);
			}
			this.tabDatas.Sort((SelectServerViewModule.TabData a, SelectServerViewModule.TabData b) => a.sortId.CompareTo(b.sortId));
		}

		private void UpdateServerTabDatas()
		{
			this.serverTabNodeDatas.Clear();
			this.serverTabNodeDatas.Add(new SelectServerViewModule.ServerTabNodeData
			{
				isTopSpace = true
			});
			if (this.curTabType == 0)
			{
				List<uint> roleZoneList = this.selectServerDataModule.GetRoleZoneList();
				for (int i = 0; i < roleZoneList.Count; i++)
				{
					this.serverTabNodeDatas.Add(new SelectServerViewModule.ServerTabNodeData
					{
						zoneId = roleZoneList[i]
					});
				}
			}
			else
			{
				List<ServerGroupData> groupList = this.selectServerDataModule.GetGroupList(this.curTabType);
				for (int j = 0; j < groupList.Count; j++)
				{
					this.serverTabNodeDatas.Add(new SelectServerViewModule.ServerTabNodeData
					{
						groupData = groupList[j]
					});
				}
			}
			this.serverTabNodeDatas.Add(new SelectServerViewModule.ServerTabNodeData
			{
				isBottomSpace = true
			});
		}

		private void UpdateRoleList(uint zoneId)
		{
			this.serverItemNodeDatas.Clear();
			this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
			{
				isTopSpace = true
			});
			List<RoleDetailDto> zoneRoleList = this.selectServerDataModule.GetZoneRoleList(zoneId);
			for (int i = 0; i < zoneRoleList.Count; i++)
			{
				this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
				{
					roleDetail = zoneRoleList[i]
				});
			}
			this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
			{
				isBottomSpace = true
			});
		}

		private void UpdateServerInfoList(ServerGroupData groupData)
		{
			List<ServerInfoData> serverInfoList = this.selectServerDataModule.GetServerInfoList(groupData);
			this.serverItemNodeDatas.Clear();
			this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
			{
				isTopSpace = true
			});
			for (int i = 0; i < serverInfoList.Count; i++)
			{
				this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
				{
					ServerInfoData = serverInfoList[i]
				});
			}
			this.serverItemNodeDatas.Add(new SelectServerViewModule.ServerItemNodeData
			{
				isBottomSpace = true
			});
		}

		private void OnServerDataChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.UpdateView();
		}

		[GameTestMethod("界面", "打开选服界面", "", 0)]
		public static void OpenUI()
		{
			SelectServerDataModule dataModule = GameApp.Data.GetDataModule(DataName.SelectServerDataModule);
			if (dataModule.isDataInit)
			{
				GameApp.View.OpenView(ViewName.SelectServerViewModule, null, 1, null, null);
				dataModule.PullServerData(null, true);
				return;
			}
			dataModule.PullServerData(delegate(bool result)
			{
				if (result)
				{
					GameApp.View.OpenView(ViewName.SelectServerViewModule, null, 1, null, null);
				}
			}, false);
		}

		private void InitLoopListServerItem()
		{
			this.loopListContent.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetServerItemByIndex), null);
		}

		private void DeInitLoopListServerItem()
		{
			foreach (CustomBehaviour customBehaviour in this.serverLoopItemDict.Values)
			{
				if (customBehaviour != null)
				{
					customBehaviour.DeInit();
				}
			}
			this.serverLoopItemDict.Clear();
		}

		private void RefreshLoopListServerItem()
		{
			List<SelectServerViewModule.ServerItemNodeData> cacheServerListData = this.GetCacheServerListData();
			int num = 0;
			this.loopListContent.SetListItemCount(cacheServerListData.Count, true);
			this.loopListContent.RefreshAllShownItem();
			this.loopListContent.MovePanelToItemIndex(num, 0f);
		}

		private List<SelectServerViewModule.ServerItemNodeData> GetCacheServerListData()
		{
			return this.serverItemNodeDatas;
		}

		private LoopListViewItem2 OnGetServerItemByIndex(LoopListView2 view, int index)
		{
			List<SelectServerViewModule.ServerItemNodeData> cacheServerListData = this.GetCacheServerListData();
			if (index < 0 || index >= cacheServerListData.Count)
			{
				return null;
			}
			SelectServerViewModule.ServerItemNodeData serverItemNodeData = cacheServerListData[index];
			LoopListViewItem2 loopListViewItem;
			if (serverItemNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.serverLoopItemDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.serverLoopItemDict[instanceID] = component;
				}
			}
			else if (serverItemNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.serverLoopItemDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.serverLoopItemDict[instanceID2] = component2;
				}
			}
			else if (serverItemNodeData.roleDetail != null)
			{
				loopListViewItem = view.NewListViewItem("ServerContentItem1");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.serverLoopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					ServerRoleItem component3 = loopListViewItem.gameObject.GetComponent<ServerRoleItem>();
					component3.Init();
					this.serverLoopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				ServerRoleItem serverRoleItem = customBehaviour3 as ServerRoleItem;
				if (serverRoleItem != null)
				{
					serverRoleItem.onClickAction = new Action<ServerRoleItem>(this.OnServerRoleItemClick);
					serverRoleItem.SetData(index, serverItemNodeData);
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("ServerContentItem2");
				int instanceID4 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour4;
				this.serverLoopItemDict.TryGetValue(instanceID4, out customBehaviour4);
				if (customBehaviour4 == null)
				{
					ServerInfoItem component4 = loopListViewItem.gameObject.GetComponent<ServerInfoItem>();
					component4.Init();
					this.serverLoopItemDict[instanceID4] = component4;
					customBehaviour4 = component4;
				}
				ServerInfoItem serverInfoItem = customBehaviour4 as ServerInfoItem;
				if (serverInfoItem != null)
				{
					serverInfoItem.onClickAction = new Action<ServerInfoItem>(this.OnServerInfoItemClick);
					serverInfoItem.SetData(index, serverItemNodeData);
				}
			}
			return loopListViewItem;
		}

		private void OnServerRoleItemClick(ServerRoleItem item)
		{
			if (item == null || item.data == null || item.data.roleDetail == null)
			{
				return;
			}
			uint serverId = item.data.roleDetail.ServerId;
			this.JumpServer(serverId);
		}

		private void OnServerInfoItemClick(ServerInfoItem item)
		{
			if (item == null || item.data == null || item.data.ServerInfoData == null)
			{
				return;
			}
			if (GameApp.Data.GetDataModule(DataName.SelectServerDataModule).GetServerStatus(item.data.ServerInfoData.serverId) == 2)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("select_server_full_tip");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			uint serverId = item.data.ServerInfoData.serverId;
			this.JumpServer(serverId);
		}

		private void JumpServer(uint serverId)
		{
			if (serverId == GameApp.NetWork.m_serverID)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("select_server_always_in_server");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			SelectServerDataModule.SetJumpServerData(serverId);
			Singleton<GameManager>.Instance.ResetSpeed();
			DxxTools.Time.ResetServerTime();
			Singleton<GameConfig>.Instance.IsPopNotice = true;
			GameApp.View.TryDestroyAllCacheUI();
			GameApp.View.CloseAllView(new int[] { 101, 102, 106 });
			GameApp.State.ActiveState(StateName.LoginState);
		}

		private void InitLoopListServerTab()
		{
			this.loopListTab.InitListView(0, new Func<LoopListView2, int, LoopListViewItem2>(this.OnGetServerTabByIndex), null);
		}

		private void DeInitLoopListServerTab()
		{
			foreach (CustomBehaviour customBehaviour in this.tabLoopItemDict.Values)
			{
				if (customBehaviour != null)
				{
					customBehaviour.DeInit();
				}
			}
			this.tabLoopItemDict.Clear();
		}

		private void RefreshLoopListServerTab()
		{
			this.curServerGroupIndex = -1;
			List<SelectServerViewModule.ServerTabNodeData> cacheTabListData = this.GetCacheTabListData();
			int num = 1;
			if (this.curTabType == 0)
			{
				uint serverZoneIdByServerId = this.selectServerDataModule.GetServerZoneIdByServerId(GameApp.NetWork.m_serverID);
				for (int i = 0; i < cacheTabListData.Count; i++)
				{
					if (cacheTabListData[i].zoneId == serverZoneIdByServerId)
					{
						num = i;
						break;
					}
				}
			}
			this.loopListTab.SetListItemCount(cacheTabListData.Count, true);
			this.loopListTab.RefreshAllShownItem();
			this.loopListTab.MovePanelToItemIndex(num, 0f);
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.tabLoopItemDict)
			{
				if (keyValuePair.Value is ServerTabItem)
				{
					ServerTabItem serverTabItem = keyValuePair.Value as ServerTabItem;
					if (serverTabItem.isActiveAndEnabled && serverTabItem.index == num)
					{
						serverTabItem.OnItemClick();
						break;
					}
				}
			}
		}

		private List<SelectServerViewModule.ServerTabNodeData> GetCacheTabListData()
		{
			return this.serverTabNodeDatas;
		}

		private void OnServerTabItemClick(ServerTabItem tabItem)
		{
			if (tabItem == null || tabItem.index == this.curServerGroupIndex)
			{
				return;
			}
			this.curServerGroupIndex = tabItem.index;
			foreach (KeyValuePair<int, CustomBehaviour> keyValuePair in this.tabLoopItemDict)
			{
				if (keyValuePair.Value is ServerTabItem)
				{
					(keyValuePair.Value as ServerTabItem).UpdateSelect(tabItem.index);
				}
			}
			if (tabItem.data.zoneId > 0U)
			{
				this.UpdateRoleList(tabItem.data.zoneId);
			}
			else if (tabItem.data.groupData != null)
			{
				this.selectServerDataModule.PullServerStateData(tabItem.data.groupData.groupId);
				this.UpdateServerInfoList(tabItem.data.groupData);
			}
			this.RefreshLoopListServerItem();
		}

		private LoopListViewItem2 OnGetServerTabByIndex(LoopListView2 view, int index)
		{
			List<SelectServerViewModule.ServerTabNodeData> cacheTabListData = this.GetCacheTabListData();
			if (index < 0 || index >= cacheTabListData.Count)
			{
				return null;
			}
			SelectServerViewModule.ServerTabNodeData serverTabNodeData = cacheTabListData[index];
			LoopListViewItem2 loopListViewItem;
			if (serverTabNodeData.isTopSpace)
			{
				loopListViewItem = view.NewListViewItem("TopSpace");
				int instanceID = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour;
				this.tabLoopItemDict.TryGetValue(instanceID, out customBehaviour);
				if (customBehaviour == null)
				{
					UIItemSpace component = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component.Init();
					this.tabLoopItemDict[instanceID] = component;
				}
			}
			else if (serverTabNodeData.isBottomSpace)
			{
				loopListViewItem = view.NewListViewItem("BottomSpace");
				int instanceID2 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour2;
				this.tabLoopItemDict.TryGetValue(instanceID2, out customBehaviour2);
				if (customBehaviour2 == null)
				{
					UIItemSpace component2 = loopListViewItem.gameObject.GetComponent<UIItemSpace>();
					component2.Init();
					this.tabLoopItemDict[instanceID2] = component2;
				}
			}
			else
			{
				loopListViewItem = view.NewListViewItem("ServerTabItem1");
				int instanceID3 = loopListViewItem.gameObject.GetInstanceID();
				CustomBehaviour customBehaviour3;
				this.tabLoopItemDict.TryGetValue(instanceID3, out customBehaviour3);
				if (customBehaviour3 == null)
				{
					ServerTabItem component3 = loopListViewItem.gameObject.GetComponent<ServerTabItem>();
					component3.Init();
					this.tabLoopItemDict[instanceID3] = component3;
					customBehaviour3 = component3;
				}
				ServerTabItem serverTabItem = customBehaviour3 as ServerTabItem;
				if (serverTabItem != null)
				{
					serverTabItem.onClickAction = new Action<ServerTabItem>(this.OnServerTabItemClick);
					serverTabItem.SetData(index, serverTabNodeData);
				}
			}
			return loopListViewItem;
		}

		public CustomText textCurServer;

		public CustomChooseButtonGroup tabGroup;

		public Transform tabParent;

		public CustomChooseButton tabItem;

		public UIPopCommon uiPopCommon;

		public LoopListView2 loopListTab;

		public LoopListView2 loopListContent;

		private List<CustomChooseButton> tabList = new List<CustomChooseButton>();

		private List<SelectServerViewModule.TabData> tabDatas = new List<SelectServerViewModule.TabData>();

		private List<SelectServerViewModule.ServerTabNodeData> serverTabNodeDatas = new List<SelectServerViewModule.ServerTabNodeData>();

		private List<SelectServerViewModule.ServerItemNodeData> serverItemNodeDatas = new List<SelectServerViewModule.ServerItemNodeData>();

		private SelectServerDataModule selectServerDataModule;

		private int curTabType = -1;

		private Dictionary<int, CustomBehaviour> serverLoopItemDict = new Dictionary<int, CustomBehaviour>();

		private Dictionary<int, CustomBehaviour> tabLoopItemDict = new Dictionary<int, CustomBehaviour>();

		private int curServerGroupIndex = -1;

		public class TabData
		{
			public uint zoneId;

			public int sortId;

			public string nameId;
		}

		public class ServerTabNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public uint zoneId;

			public ServerGroupData groupData;
		}

		public class ServerItemNodeData
		{
			public bool isTopSpace;

			public bool isBottomSpace;

			public RoleDetailDto roleDetail;

			public ServerInfoData ServerInfoData;
		}
	}
}
