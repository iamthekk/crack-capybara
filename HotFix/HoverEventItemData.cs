using System;
using UnityEngine;

namespace HotFix
{
	public class HoverEventItemData
	{
		public GameEventItemData item { get; private set; }

		public Vector3 endPos { get; private set; }

		public void SetData(GameEventItemData data, Vector3 pos)
		{
			this.item = data;
			this.endPos = pos;
		}
	}
}
