using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class BaseMainEquipPanel : CustomBehaviour
	{
		public abstract void OnShow();

		public abstract void OnHide();

		public abstract void PlayAnimation();
	}
}
