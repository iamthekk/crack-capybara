using System;

namespace HotFix
{
	public class FishingResultData
	{
		public bool isSuccess { get; private set; }

		public FishData fishData { get; private set; }

		public int failType { get; private set; }

		public FishingResultData(bool isSuccess, FishData data, int failType = 0)
		{
			this.isSuccess = isSuccess;
			this.fishData = data;
			this.failType = failType;
		}
	}
}
