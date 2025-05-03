using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsUserVIPLevelUpdata : BaseEventArgs
	{
		public void SetData(UserVipLevel oldData, UserVipLevel newData)
		{
			this.m_oldLevel = oldData;
			this.m_newLevel = newData;
		}

		public override void Clear()
		{
			this.m_oldLevel = null;
			this.m_newLevel = null;
		}

		public UserVipLevel m_oldLevel;

		public UserVipLevel m_newLevel;
	}
}
