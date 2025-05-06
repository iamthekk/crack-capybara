using System;
using Dxx.Chat;
using Framework.Logic.AttributeExpansion;
using SuperScrollView;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public abstract class ChatItemBase : ChatProxy.ChatProxy_BaseBehaviour
	{
		public RectTransform RTFRoot
		{
			get
			{
				return base.rectTransform;
			}
		}

		public long MsgID
		{
			get
			{
				if (this.Data == null)
				{
					return 0L;
				}
				return this.Data.msgId;
			}
		}

		protected override void ChatUI_OnInit()
		{
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public virtual void SetData(ChatData data)
		{
			this.Data = data;
		}

		public virtual void RefreshUI()
		{
		}

		public virtual void RefreshSize()
		{
		}

		public LoopListViewItem2 GetLoopItem()
		{
			return this.LoopItem;
		}

		public virtual void SetSizeRate(float show)
		{
		}

		[HideInInspector]
		public LoopListViewItem2 LoopItem;

		[HideInInspector]
		public int RootMaxWidth;

		[HideInInspector]
		public int ExtureHeight;

		[HideInInspector]
		public int RootSizeHeight;

		public ChatData Data;

		[Label]
		public string ChatGroupID;

		[Label]
		public SocketGroupType ChatGroupType;

		public Action<ChatItemBase> OnSizeChange;
	}
}
