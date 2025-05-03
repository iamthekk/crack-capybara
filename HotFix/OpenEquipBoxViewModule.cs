using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.ViewModule;

namespace HotFix
{
	public class OpenEquipBoxViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.drawAnimationPage.Init();
			this.showRewardsPage.Init();
			this.drawAnimationPage.gameObject.SetActive(false);
			this.showRewardsPage.gameObject.SetActive(false);
			this.m_commonDataModule = GameApp.Data.GetDataModule(DataName.CommonDataModule);
		}

		public override async void OnOpen(object data)
		{
			this.OnRefreshView(data);
		}

		private void OnRefreshView(object data)
		{
			this.openData = data as OpenEquipBoxViewModule.OpenData;
			if (this.openData == null)
			{
				HLog.LogError("OpenEquipBoxViewModule data is null");
				GameApp.View.CloseView(ViewName.OpenEquipBoxViewModule, null);
				return;
			}
			bool rememberTipState = this.m_commonDataModule.GetRememberTipState(RememberTipType.MainShopTabSkip);
			this.drawAnimationPage.gameObject.SetActive(true);
			this.showRewardsPage.gameObject.SetActive(false);
			if (rememberTipState)
			{
				this.drawAnimationPage.SetData(this.openData.boxId, this.openData.itemDatas, this.openData.iapMainActivityType, new Action(this.OnBtnCloseClick));
				this.OnShowRewardsPage();
				return;
			}
			this.drawAnimationPage.SetData(this.openData.boxId, this.openData.itemDatas, this.openData.iapMainActivityType, new Action(this.OnShowRewardsPage));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.openData != null && this.openData.onCloseCallback != null)
			{
				this.openData.onCloseCallback();
			}
		}

		public override void OnDelete()
		{
			this.drawAnimationPage.DeInit();
			this.showRewardsPage.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_AgainBuy, new HandlerEvent(this.OnEventShopBuyAgain));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_AgainBuy, new HandlerEvent(this.OnEventShopBuyAgain));
		}

		private void OnShowRewardsPage()
		{
			if (this.openData.itemDatas.Count <= 1)
			{
				this.OnBtnCloseClick();
				return;
			}
			GameApp.Sound.PlayClip(664, 1f);
			this.drawAnimationPage.gameObject.SetActive(false);
			this.showRewardsPage.gameObject.SetActive(true);
			this.showRewardsPage.PlayRewardAnimation(this.openData.itemDatas, new Action(this.OnBtnCloseClick), this.openData);
		}

		private void OnBtnCloseClick()
		{
			if (this.openData != null && this.openData.onCloseCallback != null)
			{
				this.openData.onCloseCallback();
			}
			GameApp.View.CloseView(ViewName.OpenEquipBoxViewModule, null);
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainTab_Equip, false) && Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen())
			{
				GameApp.View.OpenView(ViewName.FunctionOpenViewModule, null, 2, null, null);
			}
		}

		private void OnEventShopBuyAgain(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsShopAgainBuyData eventArgsShopAgainBuyData = eventargs as EventArgsShopAgainBuyData;
			if (eventArgsShopAgainBuyData == null)
			{
				return;
			}
			this.OnRefreshView(eventArgsShopAgainBuyData.OpenData);
		}

		[GameTestMethod("界面", "打开装备开箱子界面", "", 0)]
		private static void OnTest()
		{
			OpenEquipBoxViewModule.OpenData openData = new OpenEquipBoxViewModule.OpenData();
			openData.boxId = 2;
			openData.itemDatas = new List<ItemData>();
			openData.itemDatas.Add(new ItemData(110101, 1L));
			openData.itemDatas.Add(new ItemData(110201, 1L));
			openData.itemDatas.Add(new ItemData(110301, 1L));
			openData.itemDatas.Add(new ItemData(110401, 1L));
			openData.itemDatas.Add(new ItemData(210101, 1L));
			openData.itemDatas.Add(new ItemData(210201, 1L));
			openData.itemDatas.Add(new ItemData(120101, 1L));
			openData.itemDatas.Add(new ItemData(120201, 1L));
			openData.itemDatas.Add(new ItemData(120301, 1L));
			openData.itemDatas.Add(new ItemData(120401, 1L));
			GameApp.View.OpenView(ViewName.OpenEquipBoxViewModule, openData, 0, null, null);
		}

		public OpenEquipBox_DrawAnimation drawAnimationPage;

		public OpenEquipBox_ShowRewards showRewardsPage;

		private CommonDataModule m_commonDataModule;

		private OpenEquipBoxViewModule.OpenData openData;

		public class OpenData
		{
			public int boxId;

			public List<ItemData> itemDatas;

			public Action onCloseCallback;

			public int CostType = 1;

			public eEquipChestType ChestType = eEquipChestType.BronzeChest;

			public int shopSummonId;

			public int iapMainActivityType;
		}
	}
}
