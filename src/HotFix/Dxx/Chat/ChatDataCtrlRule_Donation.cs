using System;
using System.Collections.Generic;

namespace Dxx.Chat
{
	public class ChatDataCtrlRule_Donation : ChatDataCtrlRule
	{
		public override ChatDataRuleKind RuleKind
		{
			get
			{
				return ChatDataRuleKind.GuildDonation;
			}
		}

		public ChatDataCtrlRule_Donation(ChatGroupData chatGroup)
		{
			this.mChatGroup = chatGroup;
		}

		public override void OnAddChat(ChatData chatdata)
		{
			if (chatdata == null)
			{
				return;
			}
			if (chatdata.ShowKind != ChatShowKind.GuildDonation)
			{
				return;
			}
			ulong userId = chatdata.userId;
			ChatData chatData;
			if (this.mDonationChatByUserID.TryGetValue(userId, out chatData) && chatData != null && chatData == chatdata)
			{
				return;
			}
			this.mDonationChatByUserID[userId] = chatdata;
			if (chatData != null)
			{
				ChatGroupData chatGroupData = this.mChatGroup;
				if (chatGroupData == null)
				{
					return;
				}
				chatGroupData.DeleteMessage(chatData);
			}
		}

		public override void OnDelChat(ChatData chatdata)
		{
			if (chatdata == null)
			{
				return;
			}
			if (chatdata.ShowKind != ChatShowKind.GuildDonation)
			{
				return;
			}
			this.mDonationChatByUserID.Remove(chatdata.userId);
		}

		public override ChatData GetChatByUserID(ulong userid)
		{
			ChatData chatData;
			if (this.mDonationChatByUserID.TryGetValue(userid, out chatData))
			{
				return chatData;
			}
			return null;
		}

		private Dictionary<ulong, ChatData> mDonationChatByUserID = new Dictionary<ulong, ChatData>();

		private ChatGroupData mChatGroup;
	}
}
