using System;

namespace Dxx.Chat
{
	public abstract class ChatDataCtrlRule
	{
		public abstract ChatDataRuleKind RuleKind { get; }

		public virtual void OnAddChat(ChatData chatdata)
		{
		}

		public virtual void OnDelChat(ChatData chatdata)
		{
		}

		public virtual ChatData GetChatByUserID(ulong userid)
		{
			return null;
		}
	}
}
