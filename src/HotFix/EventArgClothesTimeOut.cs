using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgClothesTimeOut : BaseEventArgs
	{
		public Dictionary<int, List<int>> PartTimeOuts { get; private set; }

		public EventArgClothesTimeOut(Dictionary<int, List<int>> partTimeOuts)
		{
			this.PartTimeOuts = partTimeOuts;
		}

		public override void Clear()
		{
			this.PartTimeOuts.Clear();
		}
	}
}
