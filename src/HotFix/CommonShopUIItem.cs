using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.IntegralShop;
using UnityEngine;

namespace HotFix
{
	public class CommonShopUIItem : CustomBehaviour
	{
		public Action OnClaimReward { get; set; }

		protected override void OnInit()
		{
			this.isInit = true;
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.uiItemCtrl.onClick = new Action<UIItem, PropData, object>(this.OnItemClick);
			this.uiItemCtrl.SetCountShowType(UIItem.CountShowType.ShowAll);
			this.uiItemCtrl.Init();
			this.buyButton.SetData(new Action(this.BuyClick));
			this.buyButton.Init();
			this.offBuyButton.SetData(new Action(this.BuyClick));
			this.offBuyButton.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.isInit = false;
			this.uiItemCtrl.DeInit();
			this.buyButton.DeInit();
			this.offBuyButton.DeInit();
		}

		private void UpdateView()
		{
			this.RefreshData(this.model);
		}

		public void RefreshData(IntegralShop_goods itemModel)
		{
			this.model = itemModel;
			if (itemModel == null || this.shopDataModule.GetIsSellOut(itemModel))
			{
				this.SetCanPurchase(false);
				return;
			}
			this.SetCanPurchase(true);
			int num;
			bool offInfo = this.shopDataModule.GetOffInfo(itemModel, out num);
			this.discountObj.SetActive(offInfo);
			this.discountText.text = Singleton<LanguageManager>.Instance.GetInfoByID("400187", new object[] { num });
			IntegralShop_data integralShop_data;
			this.shopDataModule.GetShopConfig((ShopType)this.model.TypeId, out integralShop_data);
			int num2;
			int num3;
			this.shopDataModule.GetPrice(itemModel, out num2, out num3);
			string text = num2.ToString();
			string text2 = num3.ToString();
			if (!this.shopDataModule.IsEnoughCurrency(itemModel))
			{
				text2 = "<color=#F63D39>" + text2 + "</color>";
			}
			this.buyButton.SetActive(!offInfo);
			this.offBuyButton.SetActive(offInfo);
			(offInfo ? this.offBuyButton : this.buyButton).RefreshView(text2, text, integralShop_data.currencyID);
			int num4;
			int num5;
			this.shopDataModule.GetShopItemData(this.model, out num4, out num5);
			this.uiItemCtrl.SetData(new PropData
			{
				id = (uint)num4,
				count = (ulong)num5
			});
			this.uiItemCtrl.OnRefresh();
		}

		private void SetCanPurchase(bool isCan)
		{
			this.canPurchase.SetActive(isCan);
			this.notCanPurchase.SetActive(!isCan);
			if (isCan)
			{
				this.bgGray.Recovery();
				return;
			}
			this.bgGray.SetUIGray();
		}

		private void OnItemClick(UIItem item, PropData data, object arg)
		{
			DxxTools.UI.ShowItemInfo(new ItemInfoOpenData
			{
				m_propData = data,
				m_openDataType = ItemInfoOpenDataType.eShow,
				m_onItemInfoMathVolume = new OnItemInfoMathVolume(DxxTools.UI.OnItemInfoMathVolume)
			}, default(Vector3), 0f);
		}

		private void BuyClick()
		{
			IntegralShop_data integralShop_data;
			this.shopDataModule.GetShopConfig((ShopType)this.model.TypeId, out integralShop_data);
			int num;
			int num2;
			this.shopDataModule.GetPrice(this.model, out num, out num2);
			NetworkUtils.Shop.IntegralShopBuyRequest((ShopType)this.model.TypeId, this.model.ID, integralShop_data.currencyID, num2, delegate(bool res, IntegralShopBuyResponse response)
			{
				if (!this.isInit)
				{
					return;
				}
				if (!res)
				{
					return;
				}
				DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, this.OnClaimReward, true);
				this.UpdateView();
			});
		}

		[SerializeField]
		private CommonShopBuyButton buyButton;

		[SerializeField]
		private CommonShopBuyButton offBuyButton;

		[SerializeField]
		private GameObject discountObj;

		[SerializeField]
		private CustomText discountText;

		[SerializeField]
		private UIItem uiItemCtrl;

		[SerializeField]
		private GameObject canPurchase;

		[SerializeField]
		private GameObject notCanPurchase;

		[SerializeField]
		private UIGray bgGray;

		private IntegralShop_goods model;

		private ShopDataModule shopDataModule;

		private bool isInit;
	}
}
