using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class PetBoxGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.petBoxFree.Init();
			this.petBoxAdvance1.Init();
			this.petBoxAdvance10.Init();
			this.m_btnProbabilityTip.m_onClick = new Action(this.OnBtnProbabilityTipClick);
			this.petBoxFree.SetCallback(new Action<PetBoxFree>(this.OnPetBoxFreeClick));
			this.petBoxAdvance1.SetCallback(new Action<PetBoxAdvance>(this.OnPetBoxAdvance15Click));
			this.petBoxAdvance10.SetCallback(new Action<PetBoxAdvance>(this.OnPetBoxAdvance35Click));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_RefreshAllPetData, new HandlerEvent(this.OnEventRefreshAllPetData));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, new HandlerEvent(this.OnEventRefreshPetDrawInfo));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.OnEventCurrencyUpdate));
			this.ScreenAdaptation();
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_RefreshAllPetData, new HandlerEvent(this.OnEventRefreshAllPetData));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_PetDataModule_RefreshPetDrawInfo, new HandlerEvent(this.OnEventRefreshPetDrawInfo));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Currency_Update, new HandlerEvent(this.OnEventCurrencyUpdate));
			this.m_btnProbabilityTip.m_onClick = null;
			this.petBoxFree.DeInit();
			this.petBoxAdvance1.DeInit();
			this.petBoxAdvance10.DeInit();
		}

		public void OnShow()
		{
			base.SetActive(true);
		}

		public void OnHide()
		{
			base.SetActive(false);
		}

		public void Refresh(PetDataModule petDataModule)
		{
			this.petDataModule = petDataModule;
			this.petBoxFree.Refresh(petDataModule, EPetBoxType.AdDraw);
			this.petBoxAdvance1.Refresh(petDataModule, EPetBoxType.Draw15);
			this.petBoxAdvance10.Refresh(petDataModule, EPetBoxType.Draw35);
			this.UpdateExp();
		}

		private void UpdateExp()
		{
			this.m_txtLevel.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_draw_level", new object[] { this.petDataModule.m_petDrawExpData.DrawLevel });
			if (this.petDataModule.m_petDrawExpData.IsMaxLevel)
			{
				this.m_txtExp.text = Singleton<LanguageManager>.Instance.GetInfoByID("pet_draw_exp_max") ?? "";
				this.m_sliderExp.value = 1f;
				return;
			}
			this.m_txtExp.text = string.Format("{0}/{1}", this.petDataModule.m_petDrawExpData.LevelCurExp, this.petDataModule.m_petDrawExpData.LevelMaxExp);
			this.m_sliderExp.value = (float)this.petDataModule.m_petDrawExpData.LevelCurExp / (float)this.petDataModule.m_petDrawExpData.LevelMaxExp;
		}

		private void OnBtnProbabilityTipClick()
		{
			GameApp.View.OpenView(ViewName.PetProbabilityTipViewModule, null, 1, null, null);
		}

		private void OnPetBoxFreeClick(PetBoxFree petBox)
		{
			PetOpenEggHelper.OnPetBoxFreeClick(petBox.petBoxType);
		}

		private void OnPetBoxAdvance15Click(PetBoxAdvance petBox)
		{
			PetOpenEggHelper.OnPetBoxAdvanceClickImpl(petBox.petBoxType);
		}

		private void OnPetBoxAdvance35Click(PetBoxAdvance petBox)
		{
			PetOpenEggHelper.OnPetBoxAdvanceClickImpl(petBox.petBoxType);
		}

		private void OnEventRefreshAllPetData(object sender, int type, BaseEventArgs eventArgs)
		{
			if (base.gameObject != null && base.gameObject.activeSelf)
			{
				PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
				this.Refresh(dataModule);
			}
		}

		private void OnEventRefreshPetDrawInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			if (base.gameObject != null && base.gameObject.activeSelf)
			{
				PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
				this.Refresh(dataModule);
			}
		}

		private void OnEventCurrencyUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			if (base.gameObject != null && base.gameObject.activeSelf)
			{
				PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
				this.Refresh(dataModule);
			}
		}

		private void ScreenAdaptation()
		{
			RectTransform component = this.m_backImage.GetComponent<RectTransform>();
			float width = this.m_backImage.sprite.rect.width;
			float x = component.rect.size.x;
			this.petBoxFree.rectTransform.anchoredPosition = this.OffestX(this.petBoxFree.rectTransform, x, width);
			this.petBoxAdvance1.rectTransform.anchoredPosition = this.OffestX(this.petBoxAdvance1.rectTransform, x, width);
			this.petBoxAdvance10.rectTransform.anchoredPosition = this.OffestX(this.petBoxAdvance10.rectTransform, x, width);
		}

		private Vector2 OffestX(RectTransform target, float screen, float originalWidth)
		{
			Vector2 anchoredPosition = target.anchoredPosition;
			return new Vector2(screen * anchoredPosition.x / originalWidth, anchoredPosition.y);
		}

		public PetViewModule petViewModule;

		public PetBoxFree petBoxFree;

		public PetBoxAdvance petBoxAdvance1;

		public PetBoxAdvance petBoxAdvance10;

		public CustomButton m_btnProbabilityTip;

		public CustomText m_txtLevel;

		public CustomText m_txtExp;

		public Slider m_sliderExp;

		public Image m_backImage;

		public PetDataModule petDataModule;
	}
}
