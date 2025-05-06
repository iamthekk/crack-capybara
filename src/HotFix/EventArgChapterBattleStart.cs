using System;
using System.Collections.Generic;
using Framework.EventSystem;
using Server;

namespace HotFix
{
	public class EventArgChapterBattleStart : BaseEventArgs
	{
		public List<List<CardData>> otherWaves { get; private set; }

		public void SetData(List<List<CardData>> list)
		{
			if (list == null)
			{
				this.otherWaves = new List<List<CardData>>();
				return;
			}
			this.otherWaves = list;
		}

		public override void Clear()
		{
		}
	}
}
