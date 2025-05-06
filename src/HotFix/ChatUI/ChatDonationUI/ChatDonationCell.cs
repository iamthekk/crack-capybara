using System;
using Dxx.Chat;

namespace HotFix.ChatUI.ChatDonationUI
{
	public class ChatDonationCell : ChatProxy.ChatProxy_BaseBehaviour
	{
		public int RequireItemID
		{
			get
			{
				if (this.mDonation == null)
				{
					return 0;
				}
				return this.mDonation.itemId;
			}
		}

		protected override void ChatUI_OnInit()
		{
			this.RewardItemUI.Init();
		}

		protected override void ChatUI_OnUnInit()
		{
			UIItem rewardItemUI = this.RewardItemUI;
			if (rewardItemUI == null)
			{
				return;
			}
			rewardItemUI.DeInit();
		}

		public virtual void SetData(ChatData data)
		{
			this.mData = data;
			ChatData chatData = this.mData;
			this.mDonation = ((chatData != null) ? chatData.donation : null);
		}

		public virtual void RefreshUI()
		{
			if (this.mDonation != null)
			{
				this.ProgressUI.SetData(this.mDonation.count, this.mDonation.receiveCnt, this.mDonation.maxCount);
				PropData propData = new PropData();
				propData.rowId = 0UL;
				propData.count = (ulong)this.mDonation.count;
				propData.id = (uint)this.mDonation.itemId;
				this.RewardItemUI.SetData(propData);
				this.RewardItemUI.OnRefresh();
			}
		}

		public ChatDonationCell_Progress ProgressUI;

		public UIItem RewardItemUI;

		protected ChatData mData;

		protected ChatDonation mDonation;
	}
}
