using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class MainShopViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.MainShopViewModule;
		}

		public override void OnCreate(object data)
		{
			this.closeButton.onClick.AddListener(new UnityAction(this.CloseSelf));
			this.panelCtrlCtrl.ThisShopType = ShopType.BlackMarket;
			this.panelCtrlCtrl.Init();
			this.guildShopCtrl.ThisShopType = ShopType.Guild;
			this.guildShopCtrl.Init();
			this.integralShopCtrl.ThisShopType = ShopType.Integral;
			this.integralShopCtrl.Init();
			this.shopPanelList.Add(this.panelCtrlCtrl);
			this.shopPanelList.Add(this.guildShopCtrl);
			this.shopPanelList.Add(this.integralShopCtrl);
			this.blackMarketShopTab.SetData(false, ShopType.BlackMarket, new Action<ShopType>(this.SwitchPanel));
			this.blackMarketShopTab.Init();
			this.guildShopTab.SetData(false, ShopType.Guild, new Action<ShopType>(this.SwitchPanel));
			this.guildShopTab.Init();
			this.integralShopTab.SetData(false, ShopType.Integral, new Action<ShopType>(this.SwitchPanel));
			this.integralShopTab.Init();
			this.shopTabList.Add(this.blackMarketShopTab);
			this.shopTabList.Add(this.guildShopTab);
			this.shopTabList.Add(this.integralShopTab);
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(this.GetName(), null);
		}

		public override void OnOpen(object data)
		{
			ShopType shopType = ShopType.BlackMarket;
			if (data is MainShopViewModuleData)
			{
				MainShopViewModuleData mainShopViewModuleData = (MainShopViewModuleData)data;
				if (mainShopViewModuleData.ShopTag != ShopType.Null)
				{
					shopType = mainShopViewModuleData.ShopTag;
				}
			}
			this.curShopTag = ShopType.Null;
			this.SwitchPanel(shopType);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (ShopUIPanelCtrlBase shopUIPanelCtrlBase in this.shopPanelList)
			{
				shopUIPanelCtrlBase.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void OnClose()
		{
			this.closeButton.onClick.RemoveListener(new UnityAction(this.OnClose));
		}

		public override void OnDelete()
		{
			foreach (ShopUIPanelCtrlBase shopUIPanelCtrlBase in this.shopPanelList)
			{
				shopUIPanelCtrlBase.DeInit();
			}
			this.shopPanelList.Clear();
			this.shopTabList.Clear();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void SwitchPanel(ShopType shopTag)
		{
			if (this.curShopTag == shopTag)
			{
				return;
			}
			ShopType lastShopTag = this.curShopTag;
			this.curShopTag = shopTag;
			foreach (ShopTabNode shopTabNode in this.shopTabList)
			{
				shopTabNode.SetSelect(shopTabNode.TabType == this.curShopTag);
			}
			ShopTabNode shopTabNode2 = this.shopTabList.Find((ShopTabNode item) => item.TabType == this.curShopTag);
			ShopTabNode shopTabNode3 = this.shopTabList.Find((ShopTabNode item) => item.TabType == lastShopTag);
			float num = 0f;
			float num2 = 0f;
			if (shopTabNode2 != null && shopTabNode3 != null)
			{
				num = this.fullViewRect.rect.width;
				if (shopTabNode2.rectTransform.anchoredPosition.x < shopTabNode3.rectTransform.anchoredPosition.x)
				{
					num = -num;
				}
				num2 = 0.4f;
			}
			this.sequencePool.Clear(true);
			Sequence sequence = this.sequencePool.Get();
			ShopUIPanelCtrlBase lastShopPanel = this.shopPanelList.Find((ShopUIPanelCtrlBase item) => item.ThisShopType == lastShopTag);
			ShopUIPanelCtrlBase shopUIPanelCtrlBase = this.shopPanelList.Find((ShopUIPanelCtrlBase item) => item.ThisShopType == this.curShopTag);
			if (shopUIPanelCtrlBase != null)
			{
				shopUIPanelCtrlBase.SetView(true);
				Vector2 vector;
				vector..ctor(0f, 0f);
				if (num2 > 0f)
				{
					shopUIPanelCtrlBase.rectTransform.anchoredPosition = new Vector2(num, 0f);
					Tweener tweener = ShortcutExtensions46.DOAnchorPos(shopUIPanelCtrlBase.rectTransform, vector, num2, false);
					TweenSettingsExtensions.Insert(sequence, 0f, tweener);
				}
				else
				{
					shopUIPanelCtrlBase.rectTransform.anchoredPosition = vector;
					shopUIPanelCtrlBase.PlayRefreshShopAnim();
				}
			}
			if (lastShopPanel != null)
			{
				if (num2 > 0f)
				{
					Tweener tweener2 = TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions46.DOAnchorPos(lastShopPanel.rectTransform, new Vector2(-num, 0f), num2, false), delegate
					{
						lastShopPanel.SetView(false);
					});
					TweenSettingsExtensions.Insert(sequence, 0f, tweener2);
				}
				else
				{
					lastShopPanel.SetView(false);
				}
			}
			foreach (ShopUIPanelCtrlBase shopUIPanelCtrlBase2 in this.shopPanelList)
			{
				if (!(shopUIPanelCtrlBase2 == shopUIPanelCtrlBase) && !(shopUIPanelCtrlBase2 == lastShopPanel))
				{
					shopUIPanelCtrlBase2.SetView(false);
				}
			}
		}

		[GameTestMethod("商城", "黑市", "", 0)]
		private static void TestBlackMarket()
		{
			GameApp.View.OpenView(ViewName.MainShopViewModule, new MainShopViewModuleData(ShopType.BlackMarket), 1, null, null);
		}

		[GameTestMethod("商城", "公会", "", 0)]
		private static void TestGuild()
		{
			GameApp.View.OpenView(ViewName.MainShopViewModule, new MainShopViewModuleData(ShopType.Guild), 1, null, null);
		}

		[GameTestMethod("商城", "积分", "", 0)]
		private static void TestIntegral()
		{
			GameApp.View.OpenView(ViewName.MainShopViewModule, new MainShopViewModuleData(ShopType.Integral), 1, null, null);
		}

		[SerializeField]
		private CustomButton closeButton;

		[SerializeField]
		private RectTransform fullViewRect;

		[SerializeField]
		private CommonShopUIPanelCtrl panelCtrlCtrl;

		[SerializeField]
		private CommonShopUIPanelCtrl guildShopCtrl;

		[SerializeField]
		private CommonShopUIPanelCtrl integralShopCtrl;

		[SerializeField]
		private ShopTabNode guildShopTab;

		[SerializeField]
		private ShopTabNode blackMarketShopTab;

		[SerializeField]
		private ShopTabNode integralShopTab;

		private readonly List<ShopTabNode> shopTabList = new List<ShopTabNode>();

		private readonly List<ShopUIPanelCtrlBase> shopPanelList = new List<ShopUIPanelCtrlBase>();

		private readonly SequencePool sequencePool = new SequencePool();

		private ShopType curShopTag;
	}
}
