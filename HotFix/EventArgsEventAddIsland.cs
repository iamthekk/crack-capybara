using System;
using System.Collections.Generic;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventAddIsland : BaseEventArgs
	{
		public int islandId { get; private set; }

		public Dictionary<int, int> npcDic { get; private set; }

		public Action onComplete { get; private set; }

		public void SetData(int islandId, Dictionary<int, int> dic, Action onFinish)
		{
			this.islandId = islandId;
			this.npcDic = dic;
			this.onComplete = onFinish;
		}

		public override void Clear()
		{
		}
	}
}
