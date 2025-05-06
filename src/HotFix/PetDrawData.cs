using System;
using System.Collections.Generic;
using Proto.Common;

namespace HotFix
{
	public class PetDrawData
	{
		public void SetData(PetInfo petInfo)
		{
			this.freeTimes = (int)petInfo.FreeTimes;
		}

		public int freeTimes;

		public Dictionary<uint, uint> dayDrawTimes = new Dictionary<uint, uint>();

		public long dayResetTimestamp;

		public long freeTimeRemind;

		public List<int> fetters = new List<int>();
	}
}
