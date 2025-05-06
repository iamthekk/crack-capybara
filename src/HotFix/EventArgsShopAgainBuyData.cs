using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsShopAgainBuyData : BaseEventArgs
	{
		public override void Clear()
		{
			this.OpenData = null;
		}

		public OpenEquipBoxViewModule.OpenData OpenData;
	}
}
