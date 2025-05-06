using System;
using System.Collections;
using Framework;
using Proto.IntegralShop;
using UnityEngine;

namespace HotFix
{
	public class GuildShopPanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.GuildShop;
			}
		}

		protected override void OnPreInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.dayShopGuildShopPackGroup.Init();
			this.weekShopGuildShopPackGroup.Init();
			this.monthShopGuildShopPackGroup.Init();
		}

		protected override void OnPreDeInit()
		{
			this.dayShopGuildShopPackGroup.DeInit();
			this.weekShopGuildShopPackGroup.DeInit();
			this.monthShopGuildShopPackGroup.DeInit();
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
			this.dayShopGuildShopPackGroup.PlayAnimation(time, 0);
			this.weekShopGuildShopPackGroup.PlayAnimation(time, 0);
			this.monthShopGuildShopPackGroup.PlayAnimation(time, 0);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.dayShopGuildShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.weekShopGuildShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
			this.monthShopGuildShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			int num;
			int num2;
			this.shopDataModule.GetRefreshCount(ShopType.Guild, out num, out num2);
			int num3;
			this.shopDataModule.GetShopRefreshInfo(ShopType.Guild, out num3);
			bool flag = num3 <= 0;
			this.dayShopGuildShopPackGroup.UpdateContent();
			this.weekShopGuildShopPackGroup.UpdateContent();
			this.monthShopGuildShopPackGroup.UpdateContent();
		}

		private void RefreshShop()
		{
			int num;
			this.shopDataModule.GetShopRefreshInfo(ShopType.Guild, out num);
			NetworkUtils.Shop.IntegralShopRefreshRequest(ShopType.Guild, 2, num, delegate(bool isOk, IntegralShopRefreshResponse response)
			{
				if (isOk && response != null)
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				}
			});
		}

		public RectTransform scrollContentRect;

		public MainShopGuildShopPackGroup dayShopGuildShopPackGroup;

		public MainShopGuildShopPackGroup weekShopGuildShopPackGroup;

		public MainShopGuildShopPackGroup monthShopGuildShopPackGroup;

		private ShopDataModule shopDataModule;
	}
}
