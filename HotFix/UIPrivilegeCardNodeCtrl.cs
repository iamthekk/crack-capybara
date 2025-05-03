using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class UIPrivilegeCardNodeCtrl : UIPrivilegeCardNodeCtrlBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.copyItem.SetActiveSafe(false);
			this.bottomItem.Init();
			int tableID = this.iapDataModule.MonthCard.GetTableID(this.cardType);
			this.monthCard = GameApp.Table.GetManager().GetIAP_MonthCardModelInstance().GetElementById(tableID);
			if (this.monthCard == null)
			{
				HLog.LogError(string.Format("Table [IAP_MonthCard] not found id={0}", tableID));
				return;
			}
			if (this.monthCard.rewarTips != null)
			{
				for (int i = 0; i < this.monthCard.rewarTips.Length; i++)
				{
					string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(this.monthCard.rewarTips[i]);
					GameObject gameObject = Object.Instantiate<GameObject>(this.copyItem);
					gameObject.SetParentNormal(base.gameObject, false);
					UIPrivilegeCardItem component = gameObject.GetComponent<UIPrivilegeCardItem>();
					if (component)
					{
						component.Init();
						component.SetData(infoByID, i);
						this.cardItems.Add(component);
					}
					gameObject.SetActiveSafe(true);
				}
			}
			this.bottomItem.SetData(this.cardType);
			this.bottomItem.transform.SetAsLastSibling();
			this.textRebate.text = string.Format("{0}%", this.monthCard.rebate);
		}

		protected override void OnDeInit()
		{
			this.bottomItem.DeInit();
			for (int i = 0; i < this.cardItems.Count; i++)
			{
				this.cardItems[i].DeInit();
			}
		}

		public override void Refresh()
		{
			if (this.Text_Days != null)
			{
				bool flag = false;
				if (this.cardType == IAPMonthCardData.CardType.Month || this.cardType == IAPMonthCardData.CardType.AutoMining)
				{
					flag = true;
				}
				this.Text_Days.gameObject.SetActiveSafe(flag);
			}
			bool flag2 = this.iapDataModule.MonthCard.IsActivation(this.cardType);
			this.textActive.text = (flag2 ? Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_active") : Singleton<LanguageManager>.Instance.GetInfoByID("uirechargegift_inactive"));
			this.bottomItem.Refresh();
		}

		public IAPMonthCardData.CardType cardType;

		public GameObject copyItem;

		public UIPrivilegeCardBottomItem bottomItem;

		public CustomText textRebate;

		public CustomText textActive;

		public CustomLanguageText Text_Days;

		private IAP_MonthCard monthCard;

		private IAPDataModule iapDataModule;

		private List<UIPrivilegeCardItem> cardItems = new List<UIPrivilegeCardItem>();
	}
}
