using System;

namespace HotFix
{
	public class EventArgsInt : EventArgsLong
	{
		public new int Value
		{
			get
			{
				return this.m_count;
			}
		}

		public void SetData(int count)
		{
			this.m_count = count;
		}

		public override void Clear()
		{
			this.m_count = 0;
		}

		public new int m_count;
	}
}
