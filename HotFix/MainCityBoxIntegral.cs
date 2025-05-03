using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class MainCityBoxIntegral : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_mainCityDataModule = GameApp.Data.GetDataModule(DataName.MainCityDataModule);
			this.m_integralBtn.onClick.AddListener(new UnityAction(this.OnClickInegralButton));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPlayingLerpIntegral)
			{
				this.m_lerpIntegralTime += deltaTime;
				if (this.m_lerpIntegralTime >= this.m_lerpIntegralDuration)
				{
					this.m_isPlayingLerpIntegral = false;
					this.m_lerpIntegralTime = this.m_lerpIntegralDuration;
					if (this.m_mainCityDataModule.IsCanReceiveBoxIntegral())
					{
						this.m_integralBtnTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6508);
						this.m_integralBtnAnimator.SetTrigger("FirstRun");
						if (this.m_intgralBtnGrays != null)
						{
							this.m_intgralBtnGrays.Recovery();
						}
					}
				}
				this.m_integralSlider.maxValue = (float)this.m_mainCityDataModule.GetBoxMaxIntegralCount();
				this.m_currentIntegral = Mathf.Lerp(this.m_fromIntegral, this.m_toIntegral, this.m_lerpIntegralTime / this.m_lerpIntegralDuration);
				this.m_integralSlider.value = this.m_currentIntegral;
				this.m_integralSliderTxt.text = string.Format("{0}/{1}", (int)this.m_currentIntegral, this.m_mainCityDataModule.GetBoxMaxIntegralCount());
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_integralBtn != null)
			{
				this.m_integralBtn.onClick.AddListener(new UnityAction(this.OnClickInegralButton));
			}
		}

		public void OnViewOpen()
		{
			this.m_isPlayingLerpIntegral = false;
			this.m_lerpIntegralTime = 0f;
			EventArgFlyItemViewModuleSetEnd eventArgFlyItemViewModuleSetEnd = new EventArgFlyItemViewModuleSetEnd();
			eventArgFlyItemViewModuleSetEnd.SetData(FlyItemModel.Default, CurrencyType.ChestScore, new List<Vector3> { this.m_integralIcon.position });
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_FlyItemViewModule_SetEnd, eventArgFlyItemViewModuleSetEnd);
		}

		public void RefreshUI(bool isInit = true)
		{
			if (isInit || (float)this.m_mainCityDataModule.m_chestIntegral <= this.m_integralSlider.value)
			{
				this.m_integralSlider.maxValue = (float)this.m_mainCityDataModule.GetBoxMaxIntegralCount();
				this.m_integralSlider.value = (float)this.m_mainCityDataModule.m_chestIntegral;
				this.m_integralSliderTxt.text = string.Format("{0}/{1}", this.m_mainCityDataModule.m_chestIntegral, this.m_mainCityDataModule.GetBoxMaxIntegralCount());
			}
			else
			{
				this.m_fromIntegral = this.m_integralSlider.value;
				this.m_toIntegral = (float)this.m_mainCityDataModule.m_chestIntegral;
				this.m_lerpIntegralTime = 0f;
			}
			bool flag = this.m_mainCityDataModule.IsCanReceiveBoxIntegral();
			if (this.m_integralBtnRedPoint != null)
			{
				this.m_integralBtnRedPoint.SetActive(flag);
			}
			if (flag)
			{
				if (isInit)
				{
					this.m_integralBtnTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6508);
					this.m_integralBtnAnimator.SetTrigger("Run");
					if (this.m_intgralBtnGrays != null)
					{
						this.m_intgralBtnGrays.Recovery();
						return;
					}
				}
			}
			else
			{
				this.m_integralBtnTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6507);
				this.m_integralBtnAnimator.SetTrigger("Idle");
				if (this.m_intgralBtnGrays != null)
				{
					this.m_intgralBtnGrays.SetUIGray();
				}
			}
		}

		public void OnEventRefreshIntergral()
		{
			if (this.m_integralIconAniamtor != null)
			{
				this.m_integralIconAniamtor.SetTrigger("Run");
			}
			if (this.m_integralIconEffect != null)
			{
				this.m_integralIconEffect.Play();
			}
			this.m_isPlayingLerpIntegral = true;
		}

		private void OnClickInegralButton()
		{
			Action onClickInegralBtn = this.m_onClickInegralBtn;
			if (onClickInegralBtn == null)
			{
				return;
			}
			onClickInegralBtn();
		}

		public RectTransform m_integralIcon;

		public Animator m_integralIconAniamtor;

		public ParticleSystem m_integralIconEffect;

		public Slider m_integralSlider;

		public CustomText m_integralSliderTxt;

		public CustomButton m_integralBtn;

		public GameObject m_integralBtnRedPoint;

		public Animator m_integralBtnAnimator;

		public CustomText m_integralBtnTxt;

		public UIGrays m_intgralBtnGrays;

		[Header("控制参数")]
		public bool m_isPlayingLerpIntegral;

		public float m_fromIntegral;

		public float m_toIntegral;

		public float m_lerpIntegralDuration = 0.5f;

		public float m_lerpIntegralTime;

		public MainCityDataModule m_mainCityDataModule;

		private float m_currentIntegral;

		public Action m_onClickInegralBtn;
	}
}
