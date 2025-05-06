using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Google.Protobuf.Collections;

namespace HotFix
{
	public class EventArgsChapterActivityRefreshScore : BaseEventArgs
	{
		public MapField<ulong, uint> scoreMap { get; private set; }

		public List<ItemData> rewards { get; private set; }

		public void SetData(MapField<ulong, uint> map, List<ItemData> list)
		{
			this.scoreMap = map;
			this.rewards = list;
		}

		public override void Clear()
		{
		}
	}
}
