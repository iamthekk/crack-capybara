using System;
using Framework.EventSystem;
using Proto.Common;

namespace HotFix
{
	public class EventArgsCurrencyChanged : BaseEventArgs
	{
		public override void Clear()
		{
			this.OldCurrency = null;
			this.NewCurrency = null;
		}

		public UserCurrency OldCurrency;

		public UserCurrency NewCurrency;
	}
}
