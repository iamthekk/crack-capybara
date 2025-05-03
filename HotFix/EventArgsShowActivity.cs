using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsShowActivity : BaseEventArgs
	{
		public List<ChapterActivityKind> flyActList { get; private set; }

		public bool IsShow { get; private set; }

		public void SetData(List<ChapterActivityKind> list, bool isShow)
		{
			this.flyActList = list;
			this.IsShow = isShow;
		}

		public override void Clear()
		{
		}
	}
}
