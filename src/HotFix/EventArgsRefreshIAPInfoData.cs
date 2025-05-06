using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsRefreshIAPInfoData : BaseEventArgs
	{
		public IAPDto IapInfo { get; private set; }

		public void SetData(IAPDto data)
		{
			this.IapInfo = data;
		}

		public override void Clear()
		{
		}
	}
}
