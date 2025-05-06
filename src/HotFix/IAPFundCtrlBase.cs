using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class IAPFundCtrlBase : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public virtual void OnShow()
		{
			base.gameObject.SetActiveSafe(true);
		}

		public virtual void OnHide()
		{
			base.gameObject.SetActiveSafe(false);
		}

		public virtual void OnRefresh()
		{
		}
	}
}
