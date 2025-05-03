using System;

namespace HotFix
{
	public class FlyItemData
	{
		public FlyItemData(FlyItemOtherType type, long from, long to, long count, object param)
		{
			this.m_type = type;
			this.m_from = from;
			this.m_to = to;
			this.m_count = count;
			this.m_param = param;
		}

		public FlyItemOtherType m_type;

		public long m_from;

		public long m_to;

		public long m_count;

		public object m_param;
	}
}
