using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsLong : BaseEventArgs
	{
		public long Value
		{
			get
			{
				return this.m_count;
			}
		}

		public void SetData(long count)
		{
			this.m_count = count;
		}

		public override void Clear()
		{
			this.m_count = 0L;
		}

		public long m_count;
	}
}
