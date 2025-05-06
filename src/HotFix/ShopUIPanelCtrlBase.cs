using System;
using Framework.Logic.Component;

namespace HotFix
{
	public class ShopUIPanelCtrlBase : CustomBehaviour
	{
		public ShopType ThisShopType { get; set; }

		protected override void OnInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public virtual void PlayRefreshShopAnim()
		{
		}

		public void SetView(bool isShow)
		{
			base.SetActive(isShow);
			if (isShow)
			{
				this.OnShow();
				return;
			}
			this.OnHide();
		}

		protected virtual void OnShow()
		{
		}

		protected virtual void OnHide()
		{
		}
	}
}
