using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class CurrencyUICtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_ticketDataModule = GameApp.Data.GetDataModule(DataName.TicketDataModule);
			this.Button.m_onClick = new Action(this.OnClickCurrency);
			this.TimeObj.SetActiveSafe(false);
			this.ifShowPay = GameApp.SDK.GetCloudDataValue<bool>("IfShowPay", true);
			bool flag = this.ifShowPay && !this.notVisibleList.Contains(this.CurrencyType);
			this.PlusObj.SetActiveSafe(flag);
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.TimeObj.activeSelf)
			{
				long num = this.m_ticketDataModule.GetRecoverTimestamp((UserTicketKind)this.CurrencyType) - DxxTools.Time.ServerTimestamp;
				if (num > 0L)
				{
					this.Text_Time.text = DxxTools.FormatTime(num);
					return;
				}
				this.Text_Time.text = "";
				this.TimeObj.SetActiveSafe(false);
			}
		}

		private void OnClickCurrency()
		{
			if (this.ifShowPay)
			{
				Action<CurrencyType> onClick = this.OnClick;
				if (onClick == null)
				{
					return;
				}
				onClick(this.CurrencyType);
			}
		}

		public void SetText(string value)
		{
			this.Text.text = value;
			if (this.CurrencyType == CurrencyType.AP || this.CurrencyType == CurrencyType.ChallengeTower)
			{
				this.TimeObj.SetActiveSafe(!this.IsMax());
				return;
			}
			this.TimeObj.SetActiveSafe(false);
		}

		private bool IsMax()
		{
			UserTicket ticket = this.m_ticketDataModule.GetTicket((UserTicketKind)this.CurrencyType);
			return ticket != null && ticket.NewNum >= ticket.RevertLimit;
		}

		public CustomImage Image;

		public CustomButton Button;

		public CustomText Text;

		public GameObject TimeObj;

		public CustomText Text_Time;

		public GameObject PlusObj;

		public CurrencyType CurrencyType;

		public Action<CurrencyType> OnClick;

		private TicketDataModule m_ticketDataModule;

		private bool ifShowPay;

		private List<CurrencyType> notVisibleList = new List<CurrencyType>
		{
			CurrencyType.GuildCoin,
			CurrencyType.ManaCrystal,
			CurrencyType.MiningBonus,
			CurrencyType.MeteoriteIron
		};
	}
}
