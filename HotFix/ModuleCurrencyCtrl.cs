using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ModuleCurrencyCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_uiModuleInfoGroup.Init();
			this.m_goldUI.Init();
			this.m_goldUI.CurrencyType = CurrencyType.Gold;
			this.m_goldUI.OnClick = new Action<CurrencyType>(this.OnClickGold);
			this.m_diamondUI.Init();
			this.m_diamondUI.CurrencyType = CurrencyType.Diamond;
			this.m_diamondUI.OnClick = new Action<CurrencyType>(this.OnClickDiamonds);
			this.m_energyUI.Init();
			this.m_energyUI.CurrencyType = CurrencyType.AP;
			this.m_energyUI.OnClick = new Action<CurrencyType>(this.OnClickEnergy);
			this.m_towerTicketUI.Init();
			this.m_towerTicketUI.CurrencyType = CurrencyType.ChallengeTower;
			this.m_towerTicketUI.OnClick = new Action<CurrencyType>(this.OnClickTowerTicket);
			this.m_arenaTicketUI.Init();
			this.m_arenaTicketUI.CurrencyType = CurrencyType.ArenaTicket;
			this.m_arenaTicketUI.OnClick = new Action<CurrencyType>(this.OnClickArenaTicket);
			this.m_petEggUI.Init();
			this.m_petEggUI.CurrencyType = CurrencyType.PetEgg;
			this.m_petEggUI.OnClick = new Action<CurrencyType>(this.OnClickPetEgg);
			this.m_guildUI.Init();
			this.m_guildUI.CurrencyType = CurrencyType.GuildCoin;
			this.m_manaCrystalUI.Init();
			this.m_manaCrystalUI.CurrencyType = CurrencyType.ManaCrystal;
			this.m_rogueDungeonUI.Init();
			this.m_rogueDungeonUI.CurrencyType = CurrencyType.RogueDungeon;
			this.m_rogueDungeonUI.OnClick = new Action<CurrencyType>(this.OnClickRogueDungeonTicket);
			this.m_miningBonusUI.Init();
			this.m_miningBonusUI.CurrencyType = CurrencyType.MiningBonus;
			this.m_talentLegacySpeedUI.Init();
			this.m_talentLegacySpeedUI.CurrencyType = CurrencyType.TalentLegacySpeed;
			this.m_talentLegacySpeedUI.OnClick = new Action<CurrencyType>(this.OnClickTalentLegacy);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currecy_SetGold, new HandlerEvent(this.Event_SetGold));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.OnItemUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnTicketUpdate));
			RedPointController.Instance.RegRecordChange("Currency.Energy", new Action<RedNodeListenData>(this.OnCurrencyChanged_Energy));
			this.OnRefreshUI();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_topRightTrans);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_topRightTrans);
			this.SetFlyPosition();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currecy_SetGold, new HandlerEvent(this.Event_SetGold));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.OnItemUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnTicketUpdate));
			RedPointController.Instance.UnRegRecordChange("Currency.Energy", new Action<RedNodeListenData>(this.OnCurrencyChanged_Energy));
			if (this.m_uiModuleInfoGroup != null)
			{
				this.m_uiModuleInfoGroup.DeInit();
			}
			if (this.m_goldUI != null)
			{
				this.m_goldUI.DeInit();
			}
			if (this.m_diamondUI != null)
			{
				this.m_diamondUI.DeInit();
			}
			if (this.m_energyUI != null)
			{
				this.m_energyUI.DeInit();
			}
			if (this.m_towerTicketUI != null)
			{
				this.m_towerTicketUI.DeInit();
			}
			if (this.m_arenaTicketUI != null)
			{
				this.m_arenaTicketUI.DeInit();
			}
			if (this.m_petEggUI != null)
			{
				this.m_petEggUI.DeInit();
			}
			if (this.m_guildUI != null)
			{
				this.m_guildUI.DeInit();
			}
			if (this.m_manaCrystalUI != null)
			{
				this.m_manaCrystalUI.DeInit();
			}
			if (this.m_rogueDungeonUI != null)
			{
				this.m_rogueDungeonUI.DeInit();
			}
			if (this.m_miningBonusUI != null)
			{
				this.m_miningBonusUI.DeInit();
			}
			if (this.m_meteoriteIronUI != null)
			{
				this.m_meteoriteIronUI.DeInit();
			}
			if (this.m_talentLegacySpeedUI != null)
			{
				this.m_talentLegacySpeedUI.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_energyUI != null)
			{
				this.m_energyUI.OnUpdate(deltaTime, unscaledDeltaTime);
			}
			if (this.m_towerTicketUI != null)
			{
				this.m_towerTicketUI.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		private void OnClickGold(CurrencyType type)
		{
			if (!Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.BlackMarket, null, true))
			{
				return;
			}
			Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.BlackMarket, null);
		}

		private void OnClickDiamonds(CurrencyType type)
		{
			if (!Singleton<ViewJumpCtrl>.Instance.IsCanJumpTo(ViewJumpType.MainShop_DiamonShop, null, true))
			{
				return;
			}
			Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop_DiamonShop, null);
		}

		private void OnClickEnergy(CurrencyType type)
		{
			CommonTicketDailyExchangeTipModule.OpenData openData = default(CommonTicketDailyExchangeTipModule.OpenData);
			openData.SetData(UserTicketKind.UserLife);
			GameApp.View.OpenView(ViewName.CommonTicketDailyExchangeTipModule, openData, 1, null, null);
		}

		private void OnClickTowerTicket(CurrencyType type)
		{
			CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
			openData.SetData(UserTicketKind.Tower);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
		}

		private void OnClickArenaTicket(CurrencyType type)
		{
			CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
			openData.SetData(UserTicketKind.CrossArena);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
		}

		private void OnClickPetEgg(CurrencyType type)
		{
			Singleton<ViewJumpCtrl>.Instance.JumpTo(ViewJumpType.MainShop_GiftShop, null);
		}

		private void OnClickRogueDungeonTicket(CurrencyType type)
		{
			CommonTicketBuyTipModule.OpenData openData = default(CommonTicketBuyTipModule.OpenData);
			openData.SetData(UserTicketKind.RogueDungeon);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData, 1, null, null);
		}

		private void OnClickTalentLegacy(CurrencyType type)
		{
			FrameworkExpand.DispatchItemNotEnoughEvent(47, true);
		}

		private void Event_SetGold(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsLong eventArgsLong = eventArgs as EventArgsLong;
			this.setGold(eventArgsLong.Value);
		}

		private void Event_SetDiamond(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsLong eventArgsLong = eventArgs as EventArgsLong;
			this.setDiamond(eventArgsLong.Value);
		}

		private void Event_Update(object sender, int eventid, BaseEventArgs eventArgs)
		{
			this.OnRefreshUI();
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			this.OnRefreshUI();
		}

		private void OnItemUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			if (eventArgs is EventArgsItemUpdate)
			{
				this.OnRefreshUI();
			}
		}

		public void SetStyle(EModuleId moduleId, List<int> currencyList)
		{
			this.moduleId = moduleId;
			this.m_uiModuleInfoGroup.Refresh((int)moduleId);
			this.m_goldUI.gameObject.SetActive(currencyList != null && currencyList.Contains(1));
			this.m_diamondUI.gameObject.SetActive(currencyList != null && currencyList.Contains(2));
			this.m_energyUI.gameObject.SetActive(currencyList != null && currencyList.Contains(9));
			this.m_towerTicketUI.gameObject.SetActive(currencyList != null && currencyList.Contains(19));
			this.m_arenaTicketUI.gameObject.SetActive(currencyList != null && currencyList.Contains(10));
			this.m_petEggUI.gameObject.SetActive(currencyList != null && currencyList.Contains(11));
			this.m_guildUI.gameObject.SetActive(currencyList != null && currencyList.Contains(7000001));
			this.m_manaCrystalUI.gameObject.SetActive(currencyList != null && currencyList.Contains(12));
			this.m_rogueDungeonUI.gameObject.SetActive(currencyList != null && currencyList.Contains(34));
			this.m_miningBonusUI.gameObject.SetActive(currencyList != null && currencyList.Contains(33));
			this.m_meteoriteIronUI.gameObject.SetActive(currencyList != null && currencyList.Contains(27));
			this.m_talentLegacySpeedUI.gameObject.SetActive(currencyList != null && currencyList.Contains(47));
		}

		private void SetEnergy(uint value, uint max)
		{
			this.m_energyUI.SetText(DxxTools.FormatNumber((long)((ulong)value)) + "/" + DxxTools.FormatNumber((long)((ulong)max)));
		}

		private void setGold(long value)
		{
			this.m_goldUI.SetText(DxxTools.FormatNumber(value));
		}

		private void setDiamond(long value)
		{
			this.m_diamondUI.SetText(DxxTools.FormatNumber(value));
		}

		private void setTowerTicket(uint value)
		{
			this.m_towerTicketUI.SetText(DxxTools.FormatNumber((long)((ulong)value)));
		}

		private void SetArenaTicket(uint value)
		{
			this.m_arenaTicketUI.SetText(DxxTools.FormatNumber((long)((ulong)value)));
		}

		private void SetPetEgg(long value)
		{
			this.m_petEggUI.SetText(DxxTools.FormatNumber(value));
		}

		private void SetGuildCoin(long value)
		{
			this.m_guildUI.SetText(DxxTools.FormatNumber((long)((ulong)((uint)value))));
		}

		private void SetManaCrystal(long value)
		{
			this.m_manaCrystalUI.SetText(DxxTools.FormatNumber((long)((ulong)((uint)value))));
		}

		private void SetRogueDungeonTicket(uint value)
		{
			this.m_rogueDungeonUI.SetText(DxxTools.FormatNumber((long)((ulong)value)));
		}

		private void SetMiningBonus(long value)
		{
			this.m_miningBonusUI.SetText(DxxTools.FormatNumber(value));
		}

		private void SetMeteoriteIron(long value)
		{
			this.m_meteoriteIronUI.SetText(DxxTools.FormatNumber(value));
		}

		private void SetTalentLegacySpeed(long value)
		{
			this.m_talentLegacySpeedUI.SetText(DxxTools.FormatNumber(value));
		}

		public void SetFlyPosition()
		{
			GameApp.CoroutineSystem.AddTask(1, this.UpdateCurrencyPosition());
		}

		public void OnRefreshUI()
		{
			if (this.isLockRefresh)
			{
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.setGold(dataModule.userCurrency.Coins);
			this.setDiamond((long)dataModule.userCurrency.Diamonds);
			TicketDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			UserTicket ticket = dataModule2.GetTicket(UserTicketKind.UserLife);
			this.SetEnergy(ticket.NewNum, ticket.RevertLimit);
			this.setTowerTicket(dataModule2.GetTicket(UserTicketKind.Tower).NewNum);
			this.SetArenaTicket(dataModule2.GetTicket(UserTicketKind.CrossArena).NewNum);
			UserTicket ticket2 = dataModule2.GetTicket(UserTicketKind.RogueDungeon);
			if (ticket2 != null)
			{
				this.SetRogueDungeonTicket(ticket2.NewNum);
			}
			else
			{
				this.SetRogueDungeonTicket(0U);
			}
			PropDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.SetPetEgg(dataModule3.GetItemDataCountByid(11UL));
			this.SetGuildCoin(dataModule3.GetItemDataCountByid(7000001UL));
			this.SetManaCrystal(dataModule3.GetItemDataCountByid(12UL));
			this.SetMiningBonus(dataModule3.GetItemDataCountByid(33UL));
			this.SetMeteoriteIron(dataModule3.GetItemDataCountByid(27UL));
			this.SetTalentLegacySpeed(dataModule3.GetItemDataCountByid(47UL));
		}

		public void LockRefresh(bool isLock)
		{
			this.isLockRefresh = isLock;
		}

		private IEnumerator UpdateCurrencyPosition()
		{
			yield return 1;
			if (this.m_goldUI != null && this.m_goldUI.isActiveAndEnabled)
			{
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Default, CurrencyType.Gold, new List<Transform> { this.m_goldUI.Image.transform });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
			}
			if (this.m_diamondUI != null && this.m_diamondUI.isActiveAndEnabled)
			{
				EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd2 = new EventArgFlyItemViewModuleSetEnd();
				eventArgFlyItemViewModuleSetEnd2.SetData(FlyItemModel.Default, CurrencyType.Diamond, new List<Transform> { this.m_diamondUI.Image.transform });
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd2);
			}
			GameApp.CoroutineSystem.RemoveTask(1, this.UpdateCurrencyPosition());
			yield break;
		}

		private void OnEventChangeLanguage(object sender, int type, BaseEventArgs eventObject)
		{
			if (this.m_uiModuleInfoGroup != null && this.moduleId != EModuleId.Default)
			{
				this.m_uiModuleInfoGroup.Refresh((int)this.moduleId);
			}
		}

		private void OnCurrencyChanged_Energy(RedNodeListenData redData)
		{
			if (this.m_energyRedNode == null)
			{
				return;
			}
			this.m_energyRedNode.Value = redData.m_count;
		}

		public UIModuleInfoGroup m_uiModuleInfoGroup;

		public RectTransform m_topRightTrans;

		public CurrencyUICtrl m_goldUI;

		public CurrencyUICtrl m_diamondUI;

		public CurrencyUICtrl m_energyUI;

		public RedNodeOneCtrl m_energyRedNode;

		public CurrencyUICtrl m_towerTicketUI;

		public CurrencyUICtrl m_arenaTicketUI;

		public CurrencyUICtrl m_petEggUI;

		public CurrencyUICtrl m_guildUI;

		public CurrencyUICtrl m_manaCrystalUI;

		public CurrencyUICtrl m_rogueDungeonUI;

		public CurrencyUICtrl m_miningBonusUI;

		public CurrencyUICtrl m_meteoriteIronUI;

		public CurrencyUICtrl m_talentLegacySpeedUI;

		private bool isLockRefresh;

		private EModuleId moduleId;
	}
}
