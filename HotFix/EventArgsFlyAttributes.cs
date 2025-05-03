using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgsFlyAttributes : BaseEventArgs
	{
		public Dictionary<GameEventAttType, GameObject> flyItems { get; private set; }

		public void SetData(Dictionary<GameEventAttType, GameObject> dic)
		{
			this.flyItems = dic;
		}

		public override void Clear()
		{
		}
	}
}
