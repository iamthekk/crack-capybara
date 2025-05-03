using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class BaseTaskPanel : CustomBehaviour
	{
		public abstract void OnShow();

		public abstract void OnHide();

		public abstract void OnRefresh();
	}
}
