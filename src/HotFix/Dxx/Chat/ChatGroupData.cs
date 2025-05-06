using System;
using System.Collections.Generic;
using HotFix;

namespace Dxx.Chat
{
	public class ChatGroupData
	{
		public string ChatGroupID { get; private set; }

		public SocketGroupType ChatGroupType { get; private set; }

		public IList<ChatData> ChatMessageList
		{
			get
			{
				return this.mChatMessageList;
			}
		}

		public ChatGroupData(SocketGroupType groupType, string groupid)
		{
			this.ChatGroupID = groupid;
			this.ChatGroupType = groupType;
			this.AddChatDataCtrlRule(new ChatDataCtrlRule_Donation(this));
		}

		private bool InternalAddNewChat(ChatData chatdata, bool notice)
		{
			if (chatdata == null)
			{
				return false;
			}
			chatdata.CalcData();
			if (this.mChatMessageList.Count == 0)
			{
				this.mChatMessageList.Add(chatdata);
				this.Rule_OnAddChat(chatdata);
				if (notice)
				{
					this._InternalNoticeAddChat(chatdata);
				}
				return true;
			}
			ChatData chatData = this.mChatMessageList[0];
			if (chatdata.msgId > chatData.msgId)
			{
				this.mChatMessageList.Insert(0, chatdata);
				this.Rule_OnAddChat(chatdata);
				if (notice)
				{
					this._InternalNoticeAddChat(chatdata);
				}
				return true;
			}
			ChatData chatData2 = this.mChatMessageList[this.mChatMessageList.Count - 1];
			if (chatdata.msgId < chatData2.msgId)
			{
				this.mChatMessageList.Add(chatdata);
				this.Rule_OnAddChat(chatdata);
				if (notice)
				{
					this._InternalNoticeAddChat(chatdata);
				}
				return true;
			}
			bool flag = false;
			for (int i = 1; i < this.mChatMessageList.Count; i++)
			{
				ChatData chatData3 = this.mChatMessageList[i - 1];
				ChatData chatData4 = this.mChatMessageList[i];
				if (chatdata.msgId == chatData3.msgId)
				{
					flag = false;
					break;
				}
				if (chatdata.msgId == chatData4.msgId)
				{
					flag = false;
					break;
				}
				if (chatdata.msgId < chatData3.msgId && chatdata.msgId >= chatData4.msgId)
				{
					this.mChatMessageList.Insert(i, chatdata);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				this.Rule_OnAddChat(chatdata);
				if (notice)
				{
					this._InternalNoticeAddChat(chatdata);
				}
			}
			return flag;
		}

		private bool InternalDelChat(ChatData chatdata, bool notice)
		{
			bool flag = false;
			for (int i = 0; i < this.mChatMessageList.Count; i++)
			{
				if (chatdata == this.mChatMessageList[i])
				{
					this.mChatMessageList.RemoveAt(i);
					i--;
					flag = true;
					this.Rule_OnDelChat(chatdata);
					if (notice)
					{
						this._InternalNoticeDelChat(chatdata);
					}
				}
			}
			return flag;
		}

		private void Rule_OnAddChat(ChatData chatdata)
		{
			foreach (KeyValuePair<ChatDataRuleKind, ChatDataCtrlRule> keyValuePair in this.mChatCtrlRule)
			{
				keyValuePair.Value.OnAddChat(chatdata);
			}
		}

		private void Rule_OnDelChat(ChatData chatdata)
		{
			foreach (KeyValuePair<ChatDataRuleKind, ChatDataCtrlRule> keyValuePair in this.mChatCtrlRule)
			{
				keyValuePair.Value.OnDelChat(chatdata);
			}
		}

		public void AddDataListener(IChatGroupDataChange listener)
		{
			for (int i = 0; i < this.mChangeListener.Count; i++)
			{
				if (this.mChangeListener[i] == listener)
				{
					return;
				}
			}
			this.mChangeListener.Add(listener);
		}

		public void RemoveDataListener(IChatGroupDataChange listener)
		{
			for (int i = 0; i < this.mChangeListener.Count; i++)
			{
				if (this.mChangeListener[i] == listener)
				{
					this.mChangeListener.RemoveAt(i);
					return;
				}
			}
		}

		private void _InternalNoticeAddChat(ChatData chatdata)
		{
			this._InternalNoticeAddChatList(new List<ChatData> { chatdata });
		}

		private void _InternalNoticeAddChatList(List<ChatData> chatdatalist)
		{
			if (chatdatalist == null || chatdatalist.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < this.mChangeListener.Count; i++)
			{
				IChatGroupDataChange chatGroupDataChange = this.mChangeListener[i];
				if (chatGroupDataChange != null)
				{
					chatGroupDataChange.OnAddChatData(chatdatalist);
				}
			}
		}

		private void _InternalNoticeChgChat(ChatData chatdata)
		{
			this._InternalNoticeChgChatList(new List<ChatData> { chatdata });
		}

		private void _InternalNoticeChgChatList(List<ChatData> chatdatalist)
		{
			if (chatdatalist == null || chatdatalist.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < this.mChangeListener.Count; i++)
			{
				IChatGroupDataChange chatGroupDataChange = this.mChangeListener[i];
				if (chatGroupDataChange != null)
				{
					chatGroupDataChange.OnChangeChatData(chatdatalist);
				}
			}
		}

		private void _InternalNoticeDelChat(ChatData chatdata)
		{
			this._InternalNoticeDelChatList(new List<ChatData> { chatdata });
		}

		private void _InternalNoticeDelChatList(List<ChatData> chatdatalist)
		{
			if (chatdatalist == null || chatdatalist.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < this.mChangeListener.Count; i++)
			{
				IChatGroupDataChange chatGroupDataChange = this.mChangeListener[i];
				if (chatGroupDataChange != null)
				{
					chatGroupDataChange.OnDelChatData(chatdatalist);
				}
			}
		}

		public void AddNewChat(ChatData chatdata)
		{
			this.InternalAddNewChat(chatdata, true);
		}

		public void AddNewChatList(IList<ChatData> chatlist)
		{
			if (chatlist == null)
			{
				return;
			}
			List<ChatData> list = new List<ChatData>();
			for (int i = 0; i < chatlist.Count; i++)
			{
				if (this.InternalAddNewChat(chatlist[i], false))
				{
					list.Add(chatlist[i]);
				}
			}
			this._InternalNoticeAddChatList(list);
		}

		public void DeleteMessage(ChatData chatdata)
		{
			if (chatdata == null)
			{
				return;
			}
			this.InternalDelChat(chatdata, true);
		}

		public ChatData SearchChatData(long msgid)
		{
			return this.mChatMessageList.Find((ChatData chat) => chat.msgId == msgid);
		}

		public void MarkChatDataChange(ChatData chatData)
		{
			this._InternalNoticeChgChat(chatData);
		}

		public void SetChatTranslate(ChatData chat, int translatelankind, string translatecontent)
		{
			if (chat == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(translatecontent))
			{
				return;
			}
			for (int i = 0; i < this.mChatMessageList.Count; i++)
			{
				ChatData chatData = this.mChatMessageList[i];
				if (chatData.msgId == chat.msgId && chatData.chatContent == chat.chatContent)
				{
					chatData.TranslateContent = translatecontent;
					chatData.TranslateLanguageKind = translatelankind;
				}
			}
		}

		public void AddPage()
		{
			this.HistoryPage++;
		}

		public void AddChatDataCtrlRule(ChatDataCtrlRule rule)
		{
			if (rule == null)
			{
				return;
			}
			this.mChatCtrlRule[rule.RuleKind] = rule;
		}

		public ChatDataCtrlRule GetChatDataCtrlRule(ChatDataRuleKind rulekind)
		{
			ChatDataCtrlRule chatDataCtrlRule;
			if (this.mChatCtrlRule.TryGetValue(rulekind, out chatDataCtrlRule))
			{
				return chatDataCtrlRule;
			}
			return null;
		}

		public int HistoryPage = 1;

		public long OldestMessageID;

		private List<ChatData> mChatMessageList = new List<ChatData>();

		private List<ChatData> mEmptyList = new List<ChatData>();

		private List<IChatGroupDataChange> mChangeListener = new List<IChatGroupDataChange>();

		private Dictionary<ChatDataRuleKind, ChatDataCtrlRule> mChatCtrlRule = new Dictionary<ChatDataRuleKind, ChatDataCtrlRule>();
	}
}
