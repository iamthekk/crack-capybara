using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshHeroLevelUpData : BaseEventArgs
	{
		public void SetData(int level)
		{
			this.m_level = level;
		}

		public override void Clear()
		{
		}

		public int m_level;
	}
}
