using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UITreasureProgressItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int state, bool isCurrent)
		{
			this.successObj.SetActiveSafe(state == 1);
			this.failObj.SetActiveSafe(state == 0);
			this.unClickObj.SetActiveSafe(state < 0);
			this.unClickObj.transform.localScale = Vector3.one;
			if (state < 0 && isCurrent)
			{
				this.unClickObj.transform.localScale = Vector3.one * 1.2f;
			}
		}

		public GameObject successObj;

		public GameObject failObj;

		public GameObject unClickObj;
	}
}
