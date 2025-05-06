using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class ServerRoleItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.btnItem.m_onClick = new Action(this.OnItemClick);
			if (this.uiAvatar != null)
			{
				this.uiAvatar.Init();
				this.uiAvatar.SetEnableButton(true);
			}
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
			if (this.uiAvatar != null)
			{
				this.uiAvatar.DeInit();
			}
		}

		private void SetAvatar(int avatarID, int avatarFrameID)
		{
			if (this.uiAvatar == null)
			{
				return;
			}
			this.uiAvatar.RefreshData(avatarID, avatarFrameID);
		}

		public void OnItemClick()
		{
			Action<ServerRoleItem> action = this.onClickAction;
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
			uint serverZoneIdByServerId = GameApp.Data.GetDataModule(DataName.SelectServerDataModule).GetServerZoneIdByServerId(data.roleDetail.ServerId);
			ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)serverZoneIdByServerId);
			uint num = data.roleDetail.ServerId - (uint)elementById.range[0] + 1U;
			this.textServer.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.serverPrefix, new object[] { num });
			this.textCombat.text = DxxTools.FormatNumber((long)data.roleDetail.Power);
			this.textName.text = data.roleDetail.NickName.ToString();
			this.textTime.text = Singleton<LanguageManager>.Instance.GetGoTime((long)data.roleDetail.LastLoginPass);
			this.SetAvatar((int)data.roleDetail.Avatar, (int)data.roleDetail.AvatarFrame);
		}

		public CustomButton btnItem;

		public CustomText textServer;

		public CustomText textName;

		public CustomText textTime;

		public CustomText textCombat;

		public UIAvatarCtrl uiAvatar;

		[NonSerialized]
		public Action<ServerRoleItem> onClickAction;

		[NonSerialized]
		public int index;

		[NonSerialized]
		public SelectServerViewModule.ServerItemNodeData data;
	}
}
