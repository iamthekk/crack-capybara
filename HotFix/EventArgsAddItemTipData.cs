using System;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class EventArgsAddItemTipData : BaseEventArgs
	{
		public int ItemId { get; private set; }

		public string Tip { get; private set; }

		public bool IsSetPosition { get; private set; }

		public Vector3 Position { get; private set; }

		public void SetData(int itemId, string tip)
		{
			this.ItemId = itemId;
			this.Tip = tip;
		}

		public void SetData(int itemId, string tip, Vector3 position)
		{
			this.SetData(itemId, tip);
			this.IsSetPosition = true;
			this.Position = position;
		}

		public void SetDataCount(int itemId, int count, Vector3 position)
		{
			if (count > 0)
			{
				this.SetData(itemId, string.Format(" +{0}", count));
			}
			else
			{
				this.SetData(itemId, string.Format(" {0}", count));
			}
			this.IsSetPosition = true;
			this.Position = position;
		}

		public void SetDataCountRed(int itemId, int count, Vector3 position)
		{
			if (count > 0)
			{
				this.SetData(itemId, string.Format("<color=red> +{0}</color>", count));
			}
			else
			{
				this.SetData(itemId, string.Format("<color=red> {0}</color>", count));
			}
			this.IsSetPosition = true;
			this.Position = position;
		}

		public override void Clear()
		{
			this.ItemId = 0;
			this.Tip = string.Empty;
			this.IsSetPosition = false;
			this.Position = Vector3.zero;
		}
	}
}
