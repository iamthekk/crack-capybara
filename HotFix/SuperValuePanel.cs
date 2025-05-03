using System;
using System.Collections;
using UnityEngine;

namespace HotFix
{
	public class SuperValuePanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.SuperValue;
			}
		}

		protected override void OnPreInit()
		{
			this.mainShopSuperValueShopPackGroup.Init();
		}

		protected override void OnPreDeInit()
		{
			this.mainShopSuperValueShopPackGroup.DeInit();
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
			this.mainShopSuperValueShopPackGroup.PlayAnimation(time, 0);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.mainShopSuperValueShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			this.mainShopSuperValueShopPackGroup.UpdateContent();
		}

		public RectTransform scrollContentRect;

		public MainShopSuperValueShopPackGroup mainShopSuperValueShopPackGroup;
	}
}
