using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIEnergyGiftItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.item.Init();
		}

		protected override void OnDeInit()
		{
			this.item.DeInit();
		}

		public void SetData(PropData data)
		{
			if (data == null)
			{
				return;
			}
			this.bgObj.SetActiveSafe(data.id == 9U);
			this.item.SetData(data);
			this.item.OnRefresh();
		}

		public GameObject bgObj;

		public UIItem item;
	}
}
