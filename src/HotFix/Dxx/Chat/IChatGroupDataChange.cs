using System;
using System.Collections.Generic;

namespace Dxx.Chat
{
	public interface IChatGroupDataChange
	{
		void OnAddChatData(List<ChatData> chatdata);

		void OnChangeChatData(List<ChatData> chatdata);

		void OnDelChatData(List<ChatData> chatdata);
	}
}
