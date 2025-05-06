using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;

namespace HotFix
{
	public class UIMainShop : UIBaseMainPageNode
	{
		protected override void OnInit()
		{
			this.currencyCtrl.Init();
			this.currencyCtrl.SetStyle(EModuleId.IAPShop, new List<int> { 1, 2, 9 });
			this.redNodeDic.Clear();
			foreach (UIMainShopTab uimainShopTab in this.tabList)
			{
				RedNodeOneCtrl componentInChildren = uimainShopTab.GetComponentInChildren<RedNodeOneCtrl>(true);
				if (componentInChildren != null)
				{
					componentInChildren.Value = 0;
					this.redNodeDic[uimainShopTab.TabType] = componentInChildren;
				}
			}
		}

		protected override void OnShow(UIBaseMainPageNode.OpenData openData)
		{
			this.CommonShow();
			this.SwitchShop(this.tabList[0].TabType, null);
		}

		protected override void OnShow(object param)
		{
			base.OnShow(param);
			this.CommonShow();
			if (param != null && param is MainShopJumpTabData)
			{
				MainShopJumpTabData mainShopJumpTabData = (MainShopJumpTabData)param;
				for (int i = 0; i < this.tabList.Count; i++)
				{
					if (this.tabList[i].TabType == mainShopJumpTabData.shopType)
					{
						this.SwitchShop(this.tabList[i].TabType, mainShopJumpTabData);
						return;
					}
				}
				this.SwitchShop(this.tabList[0].TabType, null);
				return;
			}
			this.SwitchShop(this.tabList[0].TabType, null);
		}

		private void CommonShow()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopInfoData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_Shop_Refresh, new HandlerEvent(this.OnEventShopDataChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoDataChange));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopDataChange));
			RedPointController.Instance.RegRecordChange("MainShop.TabEquip", new Action<RedNodeListenData>(this.OnRedPointChange_EquipShop));
			if (!this.isGuildShop)
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCurrency, Singleton<EventArgsBool>.Instance.SetData(false));
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_Main_ShowCorner, Singleton<EventArgsBool>.Instance.SetData(false));
			}
			foreach (UIMainShopTab uimainShopTab in this.tabList)
			{
				uimainShopTab.Init();
				uimainShopTab.SetData(false, delegate(MainShopType shopType)
				{
					this.SwitchShop(shopType, null);
				});
				uimainShopTab.SetSelect(false);
			}
			foreach (MainShopPanelBase mainShopPanelBase in this.shopPanelList)
			{
				if (mainShopPanelBase.IsSelect)
				{
					mainShopPanelBase.SetUnSelect();
				}
			}
			this.currencyCtrl.SetFlyPosition();
		}

		protected override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopInfoData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_Shop_Refresh, new HandlerEvent(this.OnEventShopDataChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventIAPInfoDataChange));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventShopDataChange));
			RedPointController.Instance.UnRegRecordChange("MainShop.TabEquip", new Action<RedNodeListenData>(this.OnRedPointChange_EquipShop));
			GameTGAExtend.OnViewClose("Shop" + this.currentShopType.ToString());
			this.currentShopType = MainShopType.Null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			foreach (MainShopPanelBase mainShopPanelBase in this.shopPanelList)
			{
				if (mainShopPanelBase != null && mainShopPanelBase.IsSelect)
				{
					mainShopPanelBase.OnUpdate(deltaTime, unscaledDeltaTime);
				}
			}
			this.currencyCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnDeInit()
		{
			this.currencyCtrl.DeInit();
			foreach (UIMainShopTab uimainShopTab in this.tabList)
			{
				uimainShopTab.DeInit();
			}
		}

		public override void OnLanguageChange()
		{
			for (int i = 0; i < this.shopPanelList.Count; i++)
			{
				if (this.shopPanelList[i] != null && this.shopPanelList[i].IsSelect)
				{
					this.shopPanelList[i].UpdateContent();
				}
			}
		}

		private bool GetShopPanel(MainShopType shopType, out MainShopPanelBase shopPanel)
		{
			int num = this.shopPanelList.FindIndex((MainShopPanelBase item) => item.ShopPanelType == shopType);
			if (num >= 0)
			{
				shopPanel = this.shopPanelList[num];
				return true;
			}
			string text = MainShopConst.MainShopPanelPathDic[shopType];
			GameObject gameObject = Object.Instantiate<GameObject>(this.m_loadPages.Get(text), this.shopPanelParent);
			shopPanel = gameObject.GetComponent<MainShopPanelBase>();
			shopPanel.transform.localPosition = Vector3.zero;
			this.shopPanelList.Add(shopPanel);
			shopPanel.Init();
			return true;
		}

		public void SwitchShop(MainShopType shopType, MainShopJumpTabData jumpTabData = null)
		{
			if (shopType == MainShopType.GuildShop)
			{
				if (!GuildSDKManager.Instance.IsDataInitOver)
				{
					GameApp.View.ShowNetLoading(true);
					GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse res)
					{
						GameApp.View.ShowNetLoading(false);
						if (result)
						{
							this.SwitchShop(MainShopType.GuildShop, jumpTabData);
							return;
						}
					});
				}
				else if (!GuildSDKManager.Instance.GuildInfo.HasGuild)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("guild_not_join");
					GameApp.View.ShowStringTip(infoByID);
					return;
				}
			}
			this.UpdateCurrency(shopType);
			foreach (UIMainShopTab uimainShopTab in this.tabList)
			{
				uimainShopTab.SetSelect(uimainShopTab.TabType == shopType);
			}
			foreach (MainShopPanelBase mainShopPanelBase in this.shopPanelList)
			{
				if (mainShopPanelBase.ShopPanelType != shopType)
				{
					mainShopPanelBase.SetUnSelect();
				}
			}
			MainShopPanelBase mainShopPanelBase2;
			if (this.GetShopPanel(shopType, out mainShopPanelBase2))
			{
				if (mainShopPanelBase2.IsSelect)
				{
					mainShopPanelBase2.UpdateContent();
				}
				else
				{
					mainShopPanelBase2.SetSelect(jumpTabData);
				}
			}
			if (shopType != this.currentShopType)
			{
				GameTGAExtend.OnViewClose("Shop" + this.currentShopType.ToString());
				this.currentShopType = shopType;
				GameTGAExtend.OnViewOpen("Shop" + this.currentShopType.ToString());
			}
		}

		private void UpdateShopView()
		{
			for (int i = 0; i < this.shopPanelList.Count; i++)
			{
				if (this.shopPanelList[i] != null && this.shopPanelList[i].IsSelect)
				{
					this.shopPanelList[i].UpdateContent();
				}
			}
		}

		private void UpdateCurrency(MainShopType shopType)
		{
			if (shopType == MainShopType.GuildShop)
			{
				this.currencyCtrl.SetStyle(EModuleId.IAPShop, new List<int> { 1, 2, 7000001 });
				return;
			}
			if (shopType == MainShopType.ManaCrystalShop)
			{
				this.currencyCtrl.SetStyle(EModuleId.IAPShop, new List<int> { 1, 2, 12 });
				return;
			}
			this.currencyCtrl.SetStyle(EModuleId.IAPShop, new List<int> { 1, 2, 9 });
		}

		private void OnEventIAPInfoDataChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.UpdateShopView();
		}

		private void OnEventShopDataChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.UpdateShopView();
		}

		private void OnEventShopInfoData(object sender, int type, BaseEventArgs eventargs)
		{
			this.UpdateShopView();
		}

		private void OnRedPointChange_EquipShop(RedNodeListenData redData)
		{
			RedNodeOneCtrl redNodeOneCtrl;
			if (this.redNodeDic.TryGetValue(MainShopType.EquipShop, out redNodeOneCtrl))
			{
				redNodeOneCtrl.gameObject.SetActive(redData.m_count > 0);
			}
		}

		public ModuleCurrencyCtrl currencyCtrl;

		public List<UIMainShopTab> tabList;

		private Dictionary<MainShopType, RedNodeOneCtrl> redNodeDic = new Dictionary<MainShopType, RedNodeOneCtrl>();

		public Transform shopPanelParent;

		[NonSerialized]
		public bool isGuildShop;

		private readonly List<MainShopPanelBase> shopPanelList = new List<MainShopPanelBase>();

		private MainShopType currentShopType;
	}
}
