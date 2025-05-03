using System;
using Dxx.Chat;
using Framework;
using Framework.Logic.UI;
using HotFix.GuildUI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public abstract class ChatItem_UserChatBase : ChatItemBase
	{
		protected override void ChatUI_OnInit()
		{
			if (this.UserIcon != null)
			{
				this.UserIcon.Init();
			}
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Init();
			}
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.UserIcon != null)
			{
				this.UserIcon.DeInit();
			}
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.DeInit();
			}
		}

		public override void SetData(ChatData data)
		{
			this.Data = data;
		}

		public virtual void OnClickHeadIcon(object headicon)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData((long)this.Data.userId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public override void RefreshUI()
		{
			if (this.Data == null)
			{
				this.RefreshUIAsNull();
				return;
			}
			this.RefreshUIBase();
			this.RefreshUIContent();
		}

		public abstract void RefreshUIContent();

		public virtual void RefreshUIBase()
		{
			if (this.UserIcon != null)
			{
				this.UserIcon.Refresh(this.Data.avatar, this.Data.avatarFrame, new Action<object>(this.OnClickHeadIcon));
			}
			this.TextNick.text = this.Data.GetShowNick();
			Vector2 vector = this.TextNick.rectTransform.sizeDelta;
			vector.x = this.TextNick.preferredWidth + 2f;
			this.TextNick.rectTransform.sizeDelta = vector;
			this.TextTitle.text = this.Data.GetShowTitle();
			vector = this.TextTitle.rectTransform.sizeDelta;
			vector.x = this.TextTitle.preferredWidth + 2f;
			this.TextTitle.rectTransform.sizeDelta = vector;
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.SetAndFresh(this.Data.title);
			}
		}

		public virtual void RefreshUIAsNull()
		{
			this.TextNick.text = "";
			this.TextTitle.text = "";
			if (this.TitleCtrl != null)
			{
				this.TitleCtrl.Hide();
			}
		}

		public abstract override void RefreshSize();

		public override void SetSizeRate(float show)
		{
			base.RTFRoot.sizeDelta = new Vector2(base.RTFRoot.sizeDelta.x, (float)this.RootSizeHeight * Mathf.Clamp01(show));
			Action<ChatItemBase> onSizeChange = this.OnSizeChange;
			if (onSizeChange == null)
			{
				return;
			}
			onSizeChange(this);
		}

		public CustomText TextNick;

		public CustomText TextTitle;

		public UIGuildHead UserIcon;

		[Header("称号")]
		public UITitleCtrl TitleCtrl;
	}
}
