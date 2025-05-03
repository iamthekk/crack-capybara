using System;
using System.Collections;
using Framework;
using Framework.Logic.UI;
using Proto.IntegralShop;
using UnityEngine;

namespace HotFix
{
	public class BlackMarketPanel : MainShopPanelBase
	{
		public override MainShopType ShopPanelType
		{
			get
			{
				return MainShopType.BlackMarket;
			}
		}

		protected override void OnPreInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.mainShopBlackMarketShopPackGroup.Init();
			this.shopCoinPackGroup.Init();
			this.uiItemInfoButton.Init();
			this.uiItemInfoButton.SetOnClick(new Action(this.RefreshShop));
			this.uiItemInfoButton.SetItemIcon(2);
			this.uiItemInfoButton.SetInfoTextByLanguageId(2405);
		}

		protected override void OnPreDeInit()
		{
			this.mainShopBlackMarketShopPackGroup.DeInit();
			this.shopCoinPackGroup.DeInit();
			this.uiItemInfoButton.DeInit();
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
			this.mainShopBlackMarketShopPackGroup.PlayAnimation(time, 0);
			int count = this.shopDataModule.GetShopItemsConfig(ShopType.BlackMarket, GoodsRefreshType.None).Count;
			this.shopCoinPackGroup.PlayAnimation(time, count);
			yield break;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.mainShopBlackMarketShopPackGroup.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void UpdateContent()
		{
			int num;
			int num2;
			this.shopDataModule.GetRefreshCount(ShopType.BlackMarket, out num, out num2);
			int num3;
			bool shopRefreshInfo = this.shopDataModule.GetShopRefreshInfo(ShopType.BlackMarket, out num3);
			if (num3 <= 0)
			{
				this.uiItemInfoButton.SetCountTextByLanguageId(101, true);
			}
			else
			{
				this.uiItemInfoButton.SetCountText(DxxTools.FormatNumber((long)num3), false);
			}
			this.textRefreshTimes.text = string.Format("({0}/{1})", num, num2);
			this.uiItemInfoButton.SetActive(shopRefreshInfo);
			this.goUnrefreshNode.gameObject.SetActive(!shopRefreshInfo);
			this.goRefreshNode.gameObject.SetActive(shopRefreshInfo);
			this.mainShopBlackMarketShopPackGroup.UpdateContent();
			this.shopCoinPackGroup.UpdateContent();
		}

		private void RefreshShop()
		{
			int num;
			this.shopDataModule.GetShopRefreshInfo(ShopType.BlackMarket, out num);
			NetworkUtils.Shop.IntegralShopRefreshRequest(ShopType.BlackMarket, 2, num, delegate(bool isOk, IntegralShopRefreshResponse response)
			{
				if (isOk && response != null)
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				}
			});
		}

		public RectTransform scrollContentRect;

		public MainShopBlackMarketShopPackGroup mainShopBlackMarketShopPackGroup;

		public MainShopCoinPackGroup shopCoinPackGroup;

		public UIItemInfoButton uiItemInfoButton;

		public GameObject goUnrefreshNode;

		public GameObject goRefreshNode;

		public CustomText textRefreshTimes;

		private ShopDataModule shopDataModule;
	}
}
