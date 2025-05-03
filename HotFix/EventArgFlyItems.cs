using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgFlyItems : BaseEventArgs
	{
		public List<NodeParamBase> flyItems { get; private set; }

		public void SetData(List<NodeParamBase> list)
		{
			this.flyItems = list;
		}

		public override void Clear()
		{
		}
	}
}
