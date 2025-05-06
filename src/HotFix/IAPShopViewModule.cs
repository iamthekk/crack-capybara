using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class IAPShopViewModule : BaseViewModule
	{
		[GameTestMethod("商城", "内购商店", "", 0)]
		private static void OnTestBuy()
		{
			IAPShopViewModule.OpenData openData = new IAPShopViewModule.OpenData(IAPShopJumpTabData.CreateMain(IAPMainSubType.Null));
			GameApp.View.OpenView(ViewName.IAPShopViewModule, openData, 1, null, null);
		}

		public override void OnCreate(object data)
		{
			this.closeButton.onClick.AddListener(new UnityAction(this.CloseSelf));
			this.shopLoader = base.Loader as IAPShopViewModuleLoader;
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.IAPShop, new List<int> { 1, 2, 9 });
		}

		public override void OnOpen(object data)
		{
			foreach (IAPShopTab iapshopTab in this.tabNodeList)
			{
				iapshopTab.Init();
				iapshopTab.SetData(false, delegate(IAPShopType shopType)
				{
					this.SwitchShop(shopType, null);
				});
			}
			foreach (IAPShopPanelBase iapshopPanelBase in this.shopPanelList)
			{
				iapshopPanelBase.Init();
			}
			IAPShopViewModule.OpenData openData = (IAPShopViewModule.OpenData)data;
			this.SwitchShop(openData.InitSelectTab.ShopType, openData.InitSelectTab);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (IAPShopPanelBase iapshopPanelBase in this.shopPanelList)
			{
				if (iapshopPanelBase != null)
				{
					iapshopPanelBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
		}

		public override void OnClose()
		{
			foreach (IAPShopTab iapshopTab in this.tabNodeList)
			{
				iapshopTab.DeInit();
			}
			foreach (IAPShopPanelBase iapshopPanelBase in this.shopPanelList)
			{
				iapshopPanelBase.DeInit();
			}
		}

		public override void OnDelete()
		{
			this.currencyCtrl.DeInit();
			this.closeButton.onClick.RemoveListener(new UnityAction(this.CloseSelf));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.IAPShopViewModule, null);
		}

		private bool GetShopPanel(IAPShopType shopType, out IAPShopPanelBase shopPanel)
		{
			int num = this.shopPanelList.FindIndex((IAPShopPanelBase item) => item.PanelType == shopType);
			if (num >= 0)
			{
				shopPanel = this.shopPanelList[num];
				return true;
			}
			IAPShopPanelBase iapshopPanelBase;
			if (this.shopLoader.GetIAPShopPanel(shopType, out iapshopPanelBase))
			{
				shopPanel = Object.Instantiate<IAPShopPanelBase>(iapshopPanelBase, this.shopPanelParent);
				shopPanel.transform.localPosition = Vector3.zero;
				this.shopPanelList.Add(shopPanel);
				shopPanel.SetShopLoader(this.shopLoader);
				shopPanel.Init();
				return true;
			}
			shopPanel = null;
			return false;
		}

		private void SwitchShop(IAPShopType shopType, IAPShopJumpTabData jumpTabData = null)
		{
			foreach (IAPShopTab iapshopTab in this.tabNodeList)
			{
				iapshopTab.SetSelect(iapshopTab.TabType == shopType);
			}
			foreach (IAPShopPanelBase iapshopPanelBase in this.shopPanelList)
			{
				if (iapshopPanelBase.PanelType != shopType)
				{
					iapshopPanelBase.SetUnSelect();
				}
			}
			IAPShopPanelBase iapshopPanelBase2;
			if (this.GetShopPanel(shopType, out iapshopPanelBase2))
			{
				iapshopPanelBase2.SetSelect(jumpTabData);
			}
		}

		[SerializeField]
		private ModuleCurrencyCtrl currencyCtrl;

		[SerializeField]
		private CustomButton closeButton;

		[SerializeField]
		private List<IAPShopTab> tabNodeList;

		[SerializeField]
		private Transform shopPanelParent;

		private readonly List<IAPShopPanelBase> shopPanelList = new List<IAPShopPanelBase>();

		private IAPShopViewModuleLoader shopLoader;

		public class OpenData
		{
			public IAPShopJumpTabData InitSelectTab { get; }

			public OpenData(IAPShopJumpTabData initSelectTab)
			{
				this.InitSelectTab = initSelectTab;
			}
		}
	}
}
