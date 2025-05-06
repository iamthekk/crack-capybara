using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshMainCityGoldData : BaseEventArgs
	{
		public void SetData(int level, long timeSpan)
		{
			this.m_level = level;
			this.m_timeSpan = timeSpan;
		}

		public override void Clear()
		{
		}

		public int m_level;

		public long m_timeSpan;
	}
}
