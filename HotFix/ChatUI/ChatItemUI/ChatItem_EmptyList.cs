using System;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_EmptyList : ChatItemBase
	{
		public override void RefreshSize()
		{
			Vector2 vector;
			vector..ctor(base.RTFRoot.sizeDelta.x, 0f);
			if (this.ExtureHeight > 0)
			{
				vector.y += (float)this.ExtureHeight;
			}
			base.RTFRoot.sizeDelta = vector;
			this.RootSizeHeight = (int)vector.y;
			Action<ChatItemBase> onSizeChange = this.OnSizeChange;
			if (onSizeChange == null)
			{
				return;
			}
			onSizeChange(this);
		}
	}
}
