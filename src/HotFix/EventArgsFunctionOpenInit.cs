using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsFunctionOpenInit : BaseEventArgs
	{
		public override void Clear()
		{
		}

		internal void SetData(IList<uint> list)
		{
			this.OpenList.Clear();
			this.OpenList.AddRange(list);
		}

		public List<uint> OpenList = new List<uint>();
	}
}
