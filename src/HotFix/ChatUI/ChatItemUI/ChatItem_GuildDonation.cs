using System;
using Dxx.Chat;
using HotFix.ChatUI.ChatDonationUI;
using UnityEngine;

namespace HotFix.ChatUI.ChatItemUI
{
	public class ChatItem_GuildDonation : ChatItemBase
	{
		protected override void ChatUI_OnInit()
		{
		}

		protected override void ChatUI_OnUnInit()
		{
		}

		public override void SetData(ChatData data)
		{
			this.Data = data;
			if (this.Donation != null)
			{
				this.Donation.SetData(this.Data);
			}
		}

		public override void RefreshUI()
		{
			base.RefreshUI();
			if (this.Donation != null)
			{
				this.Donation.RefreshUI();
			}
		}

		public override void RefreshSize()
		{
			this.RefreshSizeAsDonatio();
		}

		private void RefreshSizeAsDonatio()
		{
			Vector2 sizeDelta = base.RTFRoot.sizeDelta;
			Vector2 vector;
			vector..ctor(base.RTFRoot.sizeDelta.x, 370f);
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

		public ChatDonationCell Donation;
	}
}
