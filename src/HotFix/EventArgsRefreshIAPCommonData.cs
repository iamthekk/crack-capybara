using System;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsRefreshIAPCommonData : BaseEventArgs
	{
		public MapField<uint, uint> BuyId { get; private set; }

		public bool IsClear { get; private set; }

		public void SetData(MapField<uint, uint> buyId, bool isClear)
		{
			this.BuyId = buyId;
			this.IsClear = isClear;
		}

		public override void Clear()
		{
			this.BuyId = null;
			this.IsClear = false;
		}
	}
}
