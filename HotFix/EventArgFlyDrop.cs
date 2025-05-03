using System;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgFlyDrop : BaseEventArgs
	{
		public Vector3 startPos { get; private set; }

		public void SetData(Vector3 pos)
		{
			this.startPos = pos;
		}

		public override void Clear()
		{
		}
	}
}
