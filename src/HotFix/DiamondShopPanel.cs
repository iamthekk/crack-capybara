using System;
using System.Collections;
using UnityEngine;

namespace HotFix
{
	public class DiamondShopPanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.DiamondShop;
			}
		}

		protected override void OnPreInit()
		{
			this.shopDiamondPackGroup.Init();
		}

		protected override void OnPreDeInit()
		{
			this.shopDiamondPackGroup.DeInit();
		}

		protected override void OnSelect(MainShopJumpTabData jumpTabData)
		{
			base.OnSelect(jumpTabData);
			Vector2 anchoredPosition = this.scrollContentRect.anchoredPosition;
			anchoredPosition.y = 0f;
			this.scrollContentRect.anchoredPosition = anchoredPosition;
			this.UpdateContent();
			if (this != null && base.isActiveAndEnabled)
			{
				base.StartCoroutine(this.PlayAnimation());
			}
		}

		private IEnumerator PlayAnimation()
		{
			float time = Time.time;
			this.shopDiamondPackGroup.PlayAnimation(time, 0);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopDiamondPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			this.shopDiamondPackGroup.UpdateContent();
		}

		public RectTransform scrollContentRect;

		public MainShopDiamondPackGroup shopDiamondPackGroup;
	}
}
