using System;
using Framework.Logic.Component;

namespace HotFix
{
	public abstract class ShopPanelBase<T> : CustomBehaviour where T : Enum
	{
		public abstract T ShopPanelType { get; }

		public bool IsSelect { get; private set; }

		protected sealed override void OnInit()
		{
			this.OnPreInit();
			this.SetUnSelect();
		}

		protected sealed override void OnDeInit()
		{
			this.OnPreDeInit();
		}

		public abstract void UpdateContent();

		public void SetSelect(MainShopJumpTabData jumpTabData)
		{
			if (this.IsSelect)
			{
				return;
			}
			this.IsSelect = true;
			this.OnSelect(jumpTabData);
		}

		public void SetUnSelect()
		{
			if (!this.IsSelect)
			{
				return;
			}
			this.IsSelect = false;
			this.OnUnSelect();
		}

		protected abstract void OnPreInit();

		protected abstract void OnPreDeInit();

		protected virtual void OnSelect(MainShopJumpTabData jumpTabData)
		{
			base.gameObject.SetActive(true);
		}

		protected virtual void OnUnSelect()
		{
			base.gameObject.SetActive(false);
		}
	}
}
