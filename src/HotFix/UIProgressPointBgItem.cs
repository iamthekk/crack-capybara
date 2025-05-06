using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIProgressPointBgItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int index)
		{
			if (index == 0)
			{
				this.imageObj.transform.localScale = Vector3.one;
				return;
			}
			this.imageObj.transform.localScale = Vector3.one * 0.8f;
		}

		public GameObject imageObj;
	}
}
