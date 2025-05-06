using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgDropInfo : BaseEventArgs
	{
		public void SetData(List<NodeParamBase> list)
		{
			if (list != null)
			{
				this.paramList = list;
				return;
			}
			this.paramList = new List<NodeParamBase>();
		}

		public override void Clear()
		{
		}

		public List<NodeParamBase> paramList;
	}
}
