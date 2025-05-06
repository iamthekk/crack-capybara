using System;
using System.Collections;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class GiftShopPanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.GiftShop;
			}
		}

		protected override void OnPreInit()
		{
			this.shopGiftPackGroupDay.Init();
			this.shopGiftPackGroupWeek.Init();
			this.shopGiftPackGroupMonth.Init();
		}

		protected override void OnPreDeInit()
		{
			this.shopGiftPackGroupDay.DeInit();
			this.shopGiftPackGroupWeek.DeInit();
			this.shopGiftPackGroupMonth.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopGiftPackGroupDay.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopGiftPackGroupWeek.OnUpdate(deltaTime, unscaledDeltaTime);
			this.shopGiftPackGroupMonth.OnUpdate(deltaTime, unscaledDeltaTime);
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
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			float time = Time.time;
			this.shopGiftPackGroupDay.PlayAnimation(time, 0);
			int num = dataModule.TimePackData.GetPurchaseData(this.shopGiftPackGroupDay.packType).Count + 1;
			this.shopGiftPackGroupWeek.PlayAnimation(time, num);
			num += dataModule.TimePackData.GetPurchaseData(this.shopGiftPackGroupWeek.packType).Count + 1;
			this.shopGiftPackGroupMonth.PlayAnimation(time, num);
			yield break;
		}

		public override void UpdateContent()
		{
			this.shopGiftPackGroupDay.UpdateContent();
			this.shopGiftPackGroupWeek.UpdateContent();
			this.shopGiftPackGroupMonth.UpdateContent();
		}

		public RectTransform scrollContentRect;

		public MainShopGiftPackGroup shopGiftPackGroupDay;

		public MainShopGiftPackGroup shopGiftPackGroupWeek;

		public MainShopGiftPackGroup shopGiftPackGroupMonth;
	}
}
