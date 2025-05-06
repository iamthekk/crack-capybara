using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class IAPFundEmptyItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(bool showMask, bool showFg)
		{
			if (this.mask)
			{
				this.mask.SetActiveSafe(showMask);
			}
			if (this.fg)
			{
				this.fg.SetActiveSafe(showFg);
			}
		}

		public GameObject mask;

		public GameObject fg;
	}
}
