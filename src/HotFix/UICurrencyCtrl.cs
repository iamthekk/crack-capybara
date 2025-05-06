using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UICurrencyCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mPlayerInfoNode.Init();
			this.m_goldUI.Init();
			this.m_goldUI.CurrencyType = CurrencyType.Gold;
			this.m_goldUI.OnClick = new Action<CurrencyType>(this.OnClickGold);
			this.m_diamondUI.Init();
			this.m_diamondUI.CurrencyType = CurrencyType.Diamond;
			this.m_diamondUI.OnClick = new Action<CurrencyType>(this.OnClickDiamonds);
			this.m_energyUI.Init();
			this.m_energyUI.OnClick = new Action<CurrencyType>(this.OnClickEnergy);
			this.m_ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currecy_SetGold, new HandlerEvent(this.Event_SetGold));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnTicketUpdate));
			RedPointController.Instance.RegRecordChange("Currency.Energy", new Action<RedNodeListenData>(this.OnCurrencyChanged_Energy));
			this.OnRefreshUI();
			LayoutRebuilder.ForceRebuildLayoutImmediate(this.m_topRightTrans);
			LayoutRebuilder.MarkLayoutForRebuild(this.m_topRightTrans);
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currecy_SetGold, new HandlerEvent(this.Event_SetGold));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currecy_SetDiamond, new HandlerEvent(this.Event_SetDiamond));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.Event_Update));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_AddAttributeData_RefreshDatas, new HandlerEvent(this.OnTicketUpdate));
			RedPointController.Instance.UnRegRecordChange("Currency.Energy", new Action<RedNodeListenData>(this.OnCurrencyChanged_Energy));
			if (this.mPlayerInfoNode != null)
			{
				this.mPlayerInfoNode.DeInit();
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
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.m_energyUI.OnUpdate(deltaTime, unscaledDeltaTime);
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

		private void OnEventChangeLanguage(object sender, int type, BaseEventArgs eventObject)
		{
			this.OnRefreshUI();
			if (this.mPlayerInfoNode != null)
			{
				this.mPlayerInfoNode.OnRefreshUI();
			}
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

		public void OnRefreshUI()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.setGold(dataModule.userCurrency.Coins);
			this.setDiamond((long)dataModule.userCurrency.Diamonds);
			UserTicket ticket = this.m_ticketDataModule.GetTicket(UserTicketKind.UserLife);
			this.SetEnergy(ticket.NewNum, ticket.RevertLimit);
		}

		public void OnRefreshAll()
		{
			this.OnRefreshUI();
			if (this.mPlayerInfoNode != null)
			{
				this.mPlayerInfoNode.OnRefreshUI();
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

		public UIPlayerInfoNode mPlayerInfoNode;

		public RectTransform m_topRightTrans;

		public CurrencyUICtrl m_goldUI;

		public CurrencyUICtrl m_diamondUI;

		public CurrencyUICtrl m_energyUI;

		public RedNodeOneCtrl m_energyRedNode;

		private TicketDataModule m_ticketDataModule;
	}
}
