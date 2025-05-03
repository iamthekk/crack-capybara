using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatEmojiUI : ChatProxy.ChatProxy_BaseBehaviour
	{
		public RectTransform RTFRoot
		{
			get
			{
				return base.rectTransform;
			}
		}

		public ChatEmojiData Data
		{
			get
			{
				return this.mData;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.Button.m_onClick = new Action(this.OnClickEmoji);
		}

		protected override void ChatUI_OnUnInit()
		{
			if (this.Button != null)
			{
				this.Button.m_onClick = null;
			}
		}

		public void SetData(ChatEmojiData data)
		{
			this.mData = data;
		}

		public void RefreshUI()
		{
			if (this.mData == null)
			{
				return;
			}
			string atlas = this.mData.Atlas;
			string icon = this.mData.Icon;
			this.EmojiImage.SetImage(atlas, icon);
		}

		public void Resize()
		{
		}

		private void OnClickEmoji()
		{
			Action<ChatEmojiUI> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public CustomButton Button;

		public CustomImage EmojiImage;

		private ChatEmojiData mData;

		public Action<ChatEmojiUI> OnClick;
	}
}
