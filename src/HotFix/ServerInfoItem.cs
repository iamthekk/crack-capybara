using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class ServerInfoItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnItemClick);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_SelectServer_StatusChanged, new HandlerEvent(this.OnServerStatusChange));
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_SelectServer_StatusChanged, new HandlerEvent(this.OnServerStatusChange));
		}

		public void OnItemClick()
		{
			Action<ServerInfoItem> action = this.onClickAction;
			if (action == null)
			{
				return;
			}
			action(this);
		}

		public void SetData(int index, SelectServerViewModule.ServerItemNodeData data)
		{
			this.index = index;
			this.data = data;
			ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)data.ServerInfoData.zoneId);
			uint num = data.ServerInfoData.serverId - (uint)elementById.range[0] + 1U;
			this.textServer.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.serverPrefix, new object[] { num });
			this.RefreshStatus();
		}

		private void RefreshStatus()
		{
			int serverStatus = GameApp.Data.GetDataModule(DataName.SelectServerDataModule).GetServerStatus(this.data.ServerInfoData.serverId);
			this.goNew.SetActive(serverStatus == 1);
			this.goFull.SetActive(serverStatus == 2);
		}

		private void OnServerStatusChange(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshStatus();
		}

		public CustomButton btnItem;

		public CustomText textServer;

		public GameObject goNew;

		public GameObject goFull;

		[NonSerialized]
		public Action<ServerInfoItem> onClickAction;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public SelectServerViewModule.ServerItemNodeData data;
	}
}
