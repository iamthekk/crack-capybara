using System;
using Dxx.Chat;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.ChatUI.ChatDonationUI
{
	public class ChatDonationCellOther : ChatDonationCell
	{
		protected override void ChatUI_OnInit()
		{
			base.ChatUI_OnInit();
			this.Reward1UI.Init();
			this.Reward2UI.Init();
			this.Button_Send.m_onClick = new Action(this.OnSendItem);
		}

		protected override void ChatUI_OnUnInit()
		{
			this.Reward1UI.DeInit();
			this.Reward2UI.DeInit();
			base.ChatUI_OnUnInit();
		}

		public override void SetData(ChatData data)
		{
			base.SetData(data);
		}

		public override void RefreshUI()
		{
			base.RefreshUI();
			this.Text_Nick.text = this.mData.GetShowNick();
			int bagItemCount = ChatProxy.User.GetBagItemCount(base.RequireItemID);
			this.Text_HaveCount.text = bagItemCount.ToString();
			Guild_guildDonation donationTable = ChatProxy.Table.GetDonationTable(base.RequireItemID);
			if (donationTable != null)
			{
				this.Reward1UI.SetActive(donationTable.Reward.Length != 0);
				if (donationTable.Reward.Length != 0)
				{
					string text = donationTable.Reward[0];
					if (text.Length >= 2)
					{
						this.Reward1UI.SetData((int)text[0], (int)text[1]);
					}
				}
				this.Reward2UI.SetActive(donationTable.Reward.Length > 1);
				if (donationTable.Reward.Length > 1)
				{
					string text2 = donationTable.Reward[1];
					if (text2.Length >= 2)
					{
						this.Reward2UI.SetData((int)text2[0], (int)text2[1]);
					}
				}
			}
		}

		private void OnSendItem()
		{
			if (this.mData == null)
			{
				return;
			}
			if (this.mData.IsMyChat)
			{
				return;
			}
			if (this.mData.donation == null || this.mData.donation.donated)
			{
				return;
			}
			if (ChatProxy.User.GetBagItemCount(base.RequireItemID) <= 0)
			{
				ChatProxy.Language.TipsItemNotEnough();
				return;
			}
			throw new NotImplementedException();
		}

		public CustomText Text_Nick;

		public CustomText Text_HaveCount;

		public CustomButton Button_Send;

		public ChatDonationCell_Reward Reward1UI;

		public ChatDonationCell_Reward Reward2UI;
	}
}
