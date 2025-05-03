using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_System : ChatItemBase
	{
		public override void RefreshUI()
		{
			this.TextContent.text = ((this.Data != null) ? this.Data.chatContent : "");
		}

		public override void RefreshSize()
		{
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			sizeDelta.x = this.TextContent.preferredWidth + 100f;
			if (sizeDelta.x > (float)this.RootMaxWidth)
			{
				sizeDelta.x = (float)this.RootMaxWidth;
			}
			base.RTFRoot.sizeDelta = sizeDelta;
			this.RootSizeHeight = (int)sizeDelta.y;
		}

		public CustomText TextContent;
	}
}
