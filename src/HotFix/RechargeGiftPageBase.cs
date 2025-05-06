using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class RechargeGiftPageBase : CustomBehaviour
	{
		public abstract IAPRechargeGiftViewModule.EPageType PageType { get; }

		private protected bool _isOpen { protected get; private set; }

		protected sealed override void OnInit()
		{
			this._isOpen = true;
			this.OnPageInit();
		}

		protected sealed override void OnDeInit()
		{
			this._isOpen = false;
			this.OnPageDeInit();
		}

		public virtual void OnShow()
		{
		}

		public void Refresh()
		{
			if (!this._isOpen)
			{
				return;
			}
			this.OnRefresh();
		}

		protected abstract void OnRefresh();

		protected abstract void OnPageInit();

		protected abstract void OnPageDeInit();
	}
}
