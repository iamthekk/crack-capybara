using System;
using Dxx.Guild;
using Framework.Logic.UI;
using HotFix.GuildUI.GuildLevelInfoUI;
using Proto.Guild;

namespace HotFix.GuildUI
{
	public class GuildLevelInfoViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.btnMask.m_onClick = new Action(this.ClickCloseThis);
			this.m_closeBt.m_onClick = new Action(this.ClickCloseThis);
			this.guildInfo.Init();
			this.currentLevel.Init();
			this.nextLevel.OnClickToLevelUP = new Action(this.OnTryLevelUP);
			this.nextLevel.Init();
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.Refresh();
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			GuildLevelInfo_Info guildLevelInfo_Info = this.guildInfo;
			if (guildLevelInfo_Info != null)
			{
				guildLevelInfo_Info.DeInit();
			}
			this.guildInfo = null;
			GuildLevelInfo_CurrentLevel guildLevelInfo_CurrentLevel = this.currentLevel;
			if (guildLevelInfo_CurrentLevel != null)
			{
				guildLevelInfo_CurrentLevel.DeInit();
			}
			this.currentLevel = null;
			GuildLevelInfo_NextLevel guildLevelInfo_NextLevel = this.nextLevel;
			if (guildLevelInfo_NextLevel != null)
			{
				guildLevelInfo_NextLevel.DeInit();
			}
			this.nextLevel = null;
		}

		private void Refresh()
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			this.guildInfo.RefreshUI();
			this.currentLevel.SetCurrentLevel(guildData.GuildLevel);
			this.currentLevel.RefreshUI();
			this.nextLevel.RefreshUI();
		}

		private void OnTryLevelUP()
		{
			GuildNetUtil.Guild.DoRequest_GuildLevelUp(delegate(bool result, GuildLevelUpResponse resp)
			{
				if (result)
				{
					this.Refresh();
					GuildProxy.UI.OpenUIGuildLevelUp(null);
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshHall);
				}
			});
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseUIGuildLevelInfo();
		}

		public CustomButton btnMask;

		public CustomButton m_closeBt;

		public GuildLevelInfo_Info guildInfo;

		public GuildLevelInfo_CurrentLevel currentLevel;

		public GuildLevelInfo_NextLevel nextLevel;
	}
}
