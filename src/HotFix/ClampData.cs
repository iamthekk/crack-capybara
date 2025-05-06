using System;

namespace HotFix
{
	public class ClampData
	{
		public ClampData(long current, long max, long min)
		{
			this.min = min;
			this.max = max;
			this.current = current;
		}

		public long min;

		public long max;

		public long current;
	}
}
