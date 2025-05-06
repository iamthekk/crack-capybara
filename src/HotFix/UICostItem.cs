using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UICostItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.uiItem.Init();
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
		}

		public void SetData(int itemId, long have, long need)
		{
			PropData propData = new PropData();
			propData.id = (uint)itemId;
			propData.count = 0UL;
			this.uiItem.SetData(propData);
			this.uiItem.OnRefresh();
			this.uiItem.SetCountText("");
			this.textCost.text = string.Format("{0}/{1}", have, need);
			if (have >= need)
			{
				this.textCost.color = Color.white;
				return;
			}
			this.textCost.color = Color.red;
		}

		public UIItem uiItem;

		public CustomText textCost;
	}
}
