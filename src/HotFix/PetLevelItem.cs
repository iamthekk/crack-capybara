using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class PetLevelItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int petLv, int itemLevel, bool isEndIndex)
		{
			if (petLv >= itemLevel)
			{
				this.uiGrays.Recovery();
			}
			else
			{
				this.uiGrays.SetUIGray();
			}
			this.uiGrays.transform.localScale = (isEndIndex ? (Vector3.one * 1.25f) : Vector3.one);
			this.txtLevel.text = itemLevel.ToString();
		}

		public UIGrays uiGrays;

		public CustomText txtLevel;
	}
}
