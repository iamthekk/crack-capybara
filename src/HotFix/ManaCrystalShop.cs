using System;
using System.Collections;
using Framework;
using Proto.IntegralShop;
using UnityEngine;

namespace HotFix
{
	public class ManaCrystalShop : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.ManaCrystalShop;
			}
		}

		protected override void OnPreInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.dayShopPackGroup.Init();
			this.weekShopPackGroup.Init();
			this.monthShopPackGroup.Init();
		}

		protected override void OnPreDeInit()
		{
			this.dayShopPackGroup.DeInit();
			this.weekShopPackGroup.DeInit();
			this.monthShopPackGroup.DeInit();
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
			this.dayShopPackGroup.PlayAnimation(time, 0);
			this.weekShopPackGroup.PlayAnimation(time, 0);
			this.monthShopPackGroup.PlayAnimation(time, 0);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.dayShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.weekShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.monthShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			int num;
			int num2;
			this.shopDataModule.GetRefreshCount(ShopType.ManaCrystal, out num, out num2);
			int num3;
			this.shopDataModule.GetShopRefreshInfo(ShopType.ManaCrystal, out num3);
			bool flag = num3 <= 0;
			this.dayShopPackGroup.UpdateContent();
			this.weekShopPackGroup.UpdateContent();
			this.monthShopPackGroup.UpdateContent();
		}

		private void RefreshShop()
		{
			int num;
			this.shopDataModule.GetShopRefreshInfo(ShopType.ManaCrystal, out num);
			NetworkUtils.Shop.IntegralShopRefreshRequest(ShopType.ManaCrystal, 2, num, delegate(bool isOk, IntegralShopRefreshResponse response)
			{
				if (isOk && response != null)
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				}
			});
		}

		public RectTransform scrollContentRect;

		public MainShopManaCrystalShopPackGroup dayShopPackGroup;

		public MainShopManaCrystalShopPackGroup weekShopPackGroup;

		public MainShopManaCrystalShopPackGroup monthShopPackGroup;

		private ShopDataModule shopDataModule;
	}
}
