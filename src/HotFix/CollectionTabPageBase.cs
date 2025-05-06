using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class CollectionTabPageBase : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public abstract void OnShow();

		public abstract void OnHide();

		public abstract void UpdateView(bool playAnimation);

		public abstract void SetTweenFloatingPos(float localPosY);
	}
}
