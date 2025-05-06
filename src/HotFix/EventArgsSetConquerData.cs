using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsSetConquerData : BaseEventArgs
	{
		public void SetData(long targetUserID, string targetNick)
		{
			this.m_targetUserID = targetUserID;
			this.m_targetNick = targetNick;
		}

		public override void Clear()
		{
		}

		public long m_targetUserID;

		public string m_targetNick;
	}
}
