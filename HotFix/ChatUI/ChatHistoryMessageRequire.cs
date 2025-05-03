using System;
using Dxx.Chat;

namespace HotFix.ChatUI
{
	public class ChatHistoryMessageRequire
	{
		public virtual void RequireHistory(ChatData chat, Action<bool> callback)
		{
			if (callback != null)
			{
				callback(true);
			}
		}
	}
}
