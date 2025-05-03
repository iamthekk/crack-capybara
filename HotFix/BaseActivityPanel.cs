using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class BaseActivityPanel : CustomBehaviour
	{
		public abstract void OnShow();

		public abstract void OnHide();
	}
}
