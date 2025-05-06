using System;
using System.Collections.Generic;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIRewardFencesItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(List<MiningTreasureGridData> treasureGrid, int treasurePos)
		{
			if (treasureGrid == null)
			{
				this.child.gameObject.SetActiveSafe(false);
				return;
			}
			MiningTreasureGridData data = this.GetData(treasureGrid, treasurePos);
			if (data == null)
			{
				this.child.gameObject.SetActiveSafe(false);
				return;
			}
			this.child.gameObject.SetActiveSafe(true);
			this.top.SetActiveSafe(true);
			this.leftTop.SetActiveSafe(true);
			this.rightTop.SetActiveSafe(true);
			this.left.SetActiveSafe(true);
			this.right.SetActiveSafe(true);
			this.bottom.SetActiveSafe(true);
			this.leftBottom.SetActiveSafe(true);
			this.rightBottom.SetActiveSafe(true);
			if (data.Up > 0)
			{
				MiningTreasureGridData data2 = this.GetData(treasureGrid, data.Up);
				this.top.SetActiveSafe(data2.serverPos <= 0);
			}
			if (data.Down > 0)
			{
				MiningTreasureGridData data3 = this.GetData(treasureGrid, data.Down);
				this.bottom.SetActiveSafe(data3.serverPos <= 0);
			}
			if (data.Left > 0)
			{
				MiningTreasureGridData data4 = this.GetData(treasureGrid, data.Left);
				this.left.SetActiveSafe(data4.serverPos <= 0);
			}
			if (data.Right > 0)
			{
				MiningTreasureGridData data5 = this.GetData(treasureGrid, data.Right);
				this.right.SetActiveSafe(data5.serverPos <= 0);
			}
			this.leftTop.SetActiveSafe(this.left.activeSelf && this.top.activeSelf);
			this.rightTop.SetActiveSafe(this.right.activeSelf && this.top.activeSelf);
			this.leftBottom.SetActiveSafe(this.left.activeSelf && this.bottom.activeSelf);
			this.rightBottom.SetActiveSafe(this.right.activeSelf && this.bottom.activeSelf);
			if (this.leftTop.activeSelf)
			{
				this.left.SetActiveSafe(false);
				this.top.SetActiveSafe(false);
				return;
			}
			if (this.rightTop.activeSelf)
			{
				this.right.SetActiveSafe(false);
				this.top.SetActiveSafe(false);
				return;
			}
			if (this.leftBottom.activeSelf)
			{
				this.left.SetActiveSafe(false);
				this.bottom.SetActiveSafe(false);
				return;
			}
			if (this.rightBottom.activeSelf)
			{
				this.right.SetActiveSafe(false);
				this.bottom.SetActiveSafe(false);
			}
		}

		private MiningTreasureGridData GetData(List<MiningTreasureGridData> treasureGrids, int pos)
		{
			for (int i = 0; i < treasureGrids.Count; i++)
			{
				if (treasureGrids[i].treasurePos == pos)
				{
					return treasureGrids[i];
				}
			}
			return null;
		}

		public GameObject child;

		public GameObject top;

		public GameObject leftTop;

		public GameObject rightTop;

		public GameObject left;

		public GameObject right;

		public GameObject bottom;

		public GameObject leftBottom;

		public GameObject rightBottom;
	}
}
