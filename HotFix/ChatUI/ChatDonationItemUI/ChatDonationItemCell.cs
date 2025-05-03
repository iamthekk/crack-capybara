using System;
using Dxx.Chat;
using Framework.Logic.UI;

namespace HotFix.ChatUI.ChatDonationItemUI
{
	public class ChatDonationItemCell : ChatProxy.ChatProxy_BaseBehaviour
	{
		public int ItemID
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.ItemID;
			}
		}

		public int ItemCount
		{
			get
			{
				if (this.mData == null)
				{
					return 0;
				}
				return this.mData.Count;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.RewardItem.Init();
			this.Button_Item.m_onClick = new Action(this.OnClickItem);
		}

		protected override void ChatUI_OnUnInit()
		{
			UIItem rewardItem = this.RewardItem;
			if (rewardItem == null)
			{
				return;
			}
			rewardItem.DeInit();
		}

		public void SetData(ChatDonationItemData data)
		{
			this.mData = data;
			this.mItemData = new PropData();
			if (this.mData != null)
			{
				this.mItemData.id = (uint)this.mData.ItemID;
				this.mItemData.count = (ulong)this.mData.Count;
				return;
			}
			this.mItemData.id = 1U;
			this.mItemData.count = 1UL;
		}

		public void RefreshUI()
		{
			this.RewardItem.SetData(this.mItemData);
			this.RewardItem.OnRefresh();
		}

		private void OnClickItem()
		{
			if (this.mData == null)
			{
				return;
			}
			ChatProxy.Net.ReqGuildDonationReqItem(this.mData.ItemID, null);
			ChatProxy.UI.CloseChatDonationItemView();
		}

		public UIItem RewardItem;

		public CustomButton Button_Item;

		private ChatDonationItemData mData;

		private PropData mItemData;
	}
}
