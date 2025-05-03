using System;
using Dxx.Chat;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI
{
	public class ChatEmojiGroupButton : ChatProxy.ChatProxy_BaseBehaviour
	{
		public ChatEmojiGroupButton.EmojiGroupData GroupData { get; private set; }

		public bool IsChoose
		{
			get
			{
				return this.mIsChoose;
			}
		}

		public string ButtonName
		{
			get
			{
				return ((this.GroupData != null) ? this.GroupData.GroupKey : (-1)).ToString();
			}
		}

		public int GroupKey
		{
			get
			{
				if (this.GroupData == null)
				{
					return -1;
				}
				return this.GroupData.GroupKey;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.Button.m_onClick = new Action(this.InternalClick);
			this.ObjSelect.SetActive(false);
			this.ObjNormal.SetActive(true);
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public void SetData(ChatEmojiGroupButton.EmojiGroupData data)
		{
			this.GroupData = data;
			CustomImage imageIcon = this.ImageIcon;
			if (imageIcon == null)
			{
				return;
			}
			imageIcon.SetImage(this.GroupData.AtlasID, this.GroupData.Icon);
		}

		public void SetChoose(bool value)
		{
			this.mIsChoose = value;
			if (base.gameObject == null)
			{
				return;
			}
			this.ObjSelect.SetActive(value);
			this.ObjNormal.SetActive(!value);
		}

		private void InternalClick()
		{
			Action<ChatEmojiGroupButton> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(this);
		}

		public CustomButton Button;

		public GameObject ObjSelect;

		public GameObject ObjNormal;

		public CustomImage ImageIcon;

		private bool mIsChoose;

		public Action<ChatEmojiGroupButton> OnClick;

		public class EmojiGroupData
		{
			public int GroupKey;

			public string AtlasID;

			public string Icon;
		}
	}
}
