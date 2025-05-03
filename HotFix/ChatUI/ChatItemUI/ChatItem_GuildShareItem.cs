using System;
using Dxx.Chat;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_GuildShareItem : ChatItem_UserChatBase
	{
		protected override void ChatUI_OnInit()
		{
			base.ChatUI_OnInit();
			this.ShareItem.Init();
		}

		protected override void ChatUI_OnUnInit()
		{
			this.ShareItem.DeInit();
			base.ChatUI_OnUnInit();
		}

		public override void SetData(ChatData data)
		{
			base.SetData(data);
			this.ShareItem.SetData(data);
		}

		public override void RefreshUIContent()
		{
			this.ShareItem.RefreshUI();
		}

		public override void RefreshSize()
		{
			this.RefreshSizeAsShareItem();
		}

		private void RefreshSizeAsShareItem()
		{
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			Vector2 vector;
			vector..ctor(base.RTFRoot.sizeDelta.x, 300f);
			if (this.ExtureHeight > 0)
			{
				vector.y += (float)this.ExtureHeight;
			}
			base.RTFRoot.sizeDelta = vector;
			this.RootSizeHeight = (int)vector.y;
			if (sizeDelta != vector)
			{
				Action<ChatItemBase> onSizeChange = this.OnSizeChange;
				if (onSizeChange == null)
				{
					return;
				}
				onSizeChange(this);
			}
		}

		public RectTransform RTFContent;

		public ChatCell_ShareItem ShareItem;
	}
}
