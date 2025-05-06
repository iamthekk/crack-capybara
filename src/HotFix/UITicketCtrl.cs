using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Common;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UITicketCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.mTicketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			this.ButtonTicket.onClick.AddListener(new UnityAction(this.InternalClickTicket));
			this.RefreshUIByInit();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Ticket_Update, new HandlerEvent(this.OnTicketUpdate));
			if (this.ButtonTicket != null)
			{
				this.ButtonTicket.onClick.AddListener(new UnityAction(this.InternalClickTicket));
			}
		}

		private void RefreshUIByInit()
		{
			this.RefreshUI();
		}

		public void RefreshUI()
		{
			UserTicket ticket = this.mTicketDataModule.GetTicket(this.TicketKind);
			if (ticket != null)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)this.TicketKind);
				if (elementById != null)
				{
					this.ImageTicket.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
				}
				else
				{
					HLog.LogError(string.Format("UITicketCtrl 未找到物品：{0} : {1}", this.TicketKind, (int)this.TicketKind));
				}
				this.mCurShowTicketCount = (int)ticket.NewNum;
				this.TextTicket.text = this.mCurShowTicketCount.ToString();
			}
		}

		private void InternalClickTicket()
		{
			if (this.OnClick != null)
			{
				this.OnClick(this);
				return;
			}
			if (this.TicketKind == UserTicketKind.UserLife)
			{
				CommonTicketDailyExchangeTipModule.OpenData openData = default(CommonTicketDailyExchangeTipModule.OpenData);
				openData.SetData(UserTicketKind.UserLife);
				GameApp.View.OpenView(ViewName.CommonTicketDailyExchangeTipModule, openData, 1, null, null);
				return;
			}
			CommonTicketBuyTipModule.OpenData openData2 = default(CommonTicketBuyTipModule.OpenData);
			openData2.SetData(this.TicketKind);
			GameApp.View.OpenView(ViewName.CommonTicketBuyTipModule, openData2, 1, null, null);
		}

		private void OnTicketUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsTicketUpdate eventArgsTicketUpdate = eventArgs as EventArgsTicketUpdate;
			if (eventArgsTicketUpdate != null && eventArgsTicketUpdate.TicketKind == this.TicketKind)
			{
				int num = this.mCurShowTicketCount;
				this.RefreshUI();
				if (this.AutoPlaySubAnimation && num > this.mCurShowTicketCount)
				{
					EventArgsAddItemTipData eventArgsAddItemTipData = new EventArgsAddItemTipData();
					int num2 = -(num - this.mCurShowTicketCount);
					eventArgsAddItemTipData.SetDataCount((int)this.TicketKind, num2, this.TextTicket.transform.position);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_TipViewModule_AddItemTipNode, eventArgsAddItemTipData);
				}
			}
		}

		public UserTicketKind TicketKind;

		public CustomImage ImageTicket;

		public CustomText TextTicket;

		private int mCurShowTicketCount;

		public CustomButton ButtonTicket;

		[Tooltip("true:如果门票更新后减少了，则会自动播放门票减少动画")]
		public bool AutoPlaySubAnimation;

		public Action<UITicketCtrl> OnClick;

		public TicketDataModule mTicketDataModule;
	}
}
