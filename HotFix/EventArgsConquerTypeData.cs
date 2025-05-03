using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsConquerTypeData : BaseEventArgs
	{
		public void SetData(long targetUserID)
		{
			this.m_targetUserID = targetUserID;
		}

		public override void Clear()
		{
		}

		public long m_targetUserID;
	}
}
