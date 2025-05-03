using System;
using Framework.EventSystem;
using Server;

namespace HotFix
{
	public class EventArgsChangeMemberCount : BaseEventArgs
	{
		public void SetData(MemberCamp camp, int count = 1)
		{
			this.m_camp = camp;
			this.m_count = count;
		}

		public override void Clear()
		{
		}

		public MemberCamp m_camp = MemberCamp.Friendly;

		public int m_count;
	}
}
