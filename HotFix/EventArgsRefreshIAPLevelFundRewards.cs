using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshIAPLevelFundRewards : BaseEventArgs
	{
		public void SetData(IDictionary<uint, IntegerArray> payArr, IDictionary<uint, IntegerArray> freeArr)
		{
			this.PayRewards = payArr;
			this.FreeRewards = freeArr;
		}

		public override void Clear()
		{
			this.PayRewards = null;
			this.FreeRewards = null;
		}

		public IDictionary<uint, IntegerArray> PayRewards;

		public IDictionary<uint, IntegerArray> FreeRewards;
	}
}
