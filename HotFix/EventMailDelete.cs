using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventMailDelete : BaseEventArgs
	{
		public void SetData(List<string> idList)
		{
			this.idList = idList;
		}

		public override void Clear()
		{
			this.idList = null;
		}

		public List<string> idList;
	}
}
