using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(UIGrays))]
	public class SimpleGrayButton : CustomButton
	{
		public UIGrays UiGrays
		{
			get
			{
				if (this.uiGrays == null)
				{
					this.uiGrays = base.GetComponent<UIGrays>();
				}
				return this.uiGrays;
			}
		}

		public void SetGrayState(bool isGray)
		{
			if (isGray)
			{
				this.UiGrays.SetUIGray();
			}
			else
			{
				this.UiGrays.Recovery();
			}
			this.SetButtonEnable(!isGray);
		}

		public void SetButtonEnable(bool isEnable)
		{
			base.enabled = isEnable;
		}

		private UIGrays uiGrays;
	}
}
