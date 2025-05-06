using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgLevelupHp : BaseEventArgs
	{
		public void SetLevel(int lv)
		{
			this.level = lv;
		}

		public void SetAttData(List<NodeAttParam> list)
		{
			this.paramList = list;
		}

		public override void Clear()
		{
		}

		public int level;

		public List<NodeAttParam> paramList;
	}
}
