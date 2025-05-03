using System;
using Framework.Logic;

namespace HotFix
{
	public class AdData
	{
		public int GetRemainTimes()
		{
			return Utility.Math.Max(this.watchCountMax - this.watchCount, 0);
		}

		public int adId;

		public long lastWatchTime;

		public int watchCount;

		public int watchCountMax;

		public int adCountDown;
	}
}
