using System;

namespace HotFix.GuildUI
{
	public class UIGuildHead : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.HeadUI.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			this.HeadUI.DeInit();
		}

		public void Refresh(int avatarId, int frameId)
		{
			this.HeadUI.RefreshData(avatarId, frameId);
			this.HeadUI.OnClick = new Action<UIAvatarCtrl>(this.DefaultClickUserIcon);
		}

		public void Refresh(int avatarId, int frameId, Action<object> onHeadClick)
		{
			this.HeadUI.RefreshData(avatarId, frameId);
			this.HeadUI.OnClick = onHeadClick;
		}

		public void SetDefaultClick(long userid)
		{
			this.mUserId = userid;
			this.HeadUI.OnClick = new Action<UIAvatarCtrl>(this.DefaultClickUserIcon);
		}

		private void DefaultClickUserIcon(UIAvatarCtrl ctrl)
		{
			if (this.mUserId != 0L)
			{
				GuildProxy.UI.OpenOtherPlayer(this.mUserId);
			}
		}

		public UIAvatarCtrl HeadUI;

		private long mUserId;
	}
}
