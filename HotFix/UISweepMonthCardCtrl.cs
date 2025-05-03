using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UISweepMonthCardCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonMonthCard.onClick.AddListener(new UnityAction(this.OnClickMonthCard));
		}

		protected override void OnDeInit()
		{
			this.buttonMonthCard.onClick.RemoveListener(new UnityAction(this.OnClickMonthCard));
		}

		public void Refresh()
		{
			bool flag = GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsActivation(IAPMonthCardData.CardType.Month);
			if (flag)
			{
				this.imageBg.sprite = this.bgSR.GetSprite("light");
				this.textActive.text = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_month_card_act");
			}
			else
			{
				this.imageBg.sprite = this.bgSR.GetSprite("dark");
				this.textActive.text = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_month_card_inact");
			}
			this.effectObj.SetActiveSafe(flag);
		}

		private void OnClickMonthCard()
		{
			ChapterSweepViewModule viewModule = GameApp.View.GetViewModule(ViewName.ChapterSweepViewModule);
			if (viewModule != null && viewModule.IsStartSweep)
			{
				string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("sweep_button_tip");
				GameApp.View.ShowStringTip(infoByID);
				return;
			}
			GameApp.View.OpenView(ViewName.BuyMonthCardViewModule, null, 2, null, null);
		}

		public CustomButton buttonMonthCard;

		public Image imageBg;

		public SpriteRegister bgSR;

		public GameObject effectObj;

		public CustomText textActive;
	}
}
