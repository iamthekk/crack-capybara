using System;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public abstract class MainShopPackGroupBase : CustomBehaviour
	{
		public abstract void GetPriority(out int priority, out int subPriority);

		public abstract void UpdateContent();

		public virtual void ShowTitle(bool isShow)
		{
			if (this.goTitleNode != null)
			{
				this.goTitleNode.SetActive(isShow);
			}
		}

		public abstract int PlayAnimation(float startTime, int index);

		public RectTransform titleFg;

		public GameObject goTitleNode;
	}
}
