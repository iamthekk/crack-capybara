using System;
using Proto.Common;

namespace HotFix
{
	public class MailMessageListen
	{
		public Action<string, int> m_codeListen;

		public Action<CommonData> m_rewardsListen;
	}
}
