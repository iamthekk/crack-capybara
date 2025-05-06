using System;
using DG.Tweening;
using Framework;
using Framework.EventSystem;
using UnityEngine;

namespace HotFix
{
	public class UIMainPageTabNodeDefalut : UIBaseMainPageTabNode
	{
		protected override void OnInit()
		{
			base.OnInit();
			this.reClickAnimationTarget = this.m_iconParent;
			this.OnLanguageChange();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UITalent_UpGradeBack, new HandlerEvent(this.OnTalentUpGradeBack));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnTalentUpGradeBack));
		}

		private void OnTalentUpGradeBack(object sender, int type, BaseEventArgs eventargs)
		{
			this.SetTitleText();
		}

		protected override void PlayUpdate(bool isFinished, float progress)
		{
			base.PlayUpdate(isFinished, progress);
			this.m_iconParent.anchoredPosition = Vector2.Lerp(this.m_fromIconPos, this.m_toIconPos, progress);
			this.m_iconParent.localScale = Vector2.Lerp(this.m_fromIconScale, this.m_toIconScale, progress);
		}

		public override void OnLanguageChange()
		{
			if (this.m_pageNameTxt == null)
			{
				return;
			}
			this.SetTitleText();
		}

		private void SetTitleText()
		{
			if (base.m_pageName == 4)
			{
				string text = "UIMain_Talent";
				if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.TalentLegacy, false))
				{
					text = "legacy_main_title";
				}
				this.m_pageNameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(text);
				return;
			}
			this.m_pageNameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(base.m_languageID);
		}

		public override void OnSelect(bool isSelect, bool isLerp = false)
		{
			if (!this.isSelected || !isSelect)
			{
				if (this.reClickAnimationTarget != null)
				{
					ShortcutExtensions.DOKill(this.reClickAnimationTarget, false);
				}
				if (this.m_bgUnselect)
				{
					this.m_bgUnselect.gameObject.SetActive(!isSelect);
				}
				if (this.m_bgselect)
				{
					this.m_bgselect.gameObject.SetActive(isSelect);
				}
				if (this.m_pageNameTxt != null)
				{
					this.m_pageNameTxt.gameObject.SetActive(isSelect);
				}
				if (isSelect)
				{
					if (!base.m_redPointName.Equals("MainShop") && !base.m_redPointName.Equals("Talent") && !base.m_redPointName.Equals("Equip") && !base.m_redPointName.Equals("Chest") && !string.IsNullOrEmpty(base.m_redPointName))
					{
						RedPointController.Instance.ClickRecord(base.m_redPointName);
					}
					if (!isLerp)
					{
						this.m_iconParent.anchoredPosition = this.IconPosMax;
						this.m_iconParent.localScale = this.IconScaleMax;
					}
					else
					{
						this.m_fromIconScale = this.m_iconParent.localScale;
						this.m_toIconScale = this.IconScaleMax;
						this.m_fromIconPos = this.m_iconParent.anchoredPosition;
						this.m_toIconPos = this.IconPosMax;
					}
				}
				else if (!isLerp)
				{
					this.m_iconParent.anchoredPosition = this.IconPosMin;
					this.m_iconParent.localScale = this.IconScaleMin;
				}
				else
				{
					this.m_fromIconScale = this.m_iconParent.localScale;
					this.m_toIconScale = this.IconScaleMin;
					this.m_fromIconPos = this.m_iconParent.anchoredPosition;
					this.m_toIconPos = this.IconPosMin;
				}
				this.SetTitleText();
				base.OnSelect(isSelect, isLerp);
				return;
			}
			if (base.m_isPlaying)
			{
				return;
			}
			this.PlayReClickAnimation(this.IconScaleMax);
		}

		public override void OnRefreshFunctionOpenState()
		{
			bool flag = base.IsLock();
			base.SetLock(flag);
		}

		public override void OnSetTabIcon(string iconName)
		{
			base.OnSetTabIcon(iconName);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(100, iconName);
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UITalent_UpGradeBack, new HandlerEvent(this.OnTalentUpGradeBack));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnTalentUpGradeBack));
		}

		private Vector2 m_fromIconPos;

		private Vector2 m_toIconPos;

		private Vector2 IconPosMin = new Vector2(0f, 0f);

		private Vector2 IconPosMax = new Vector2(0f, 50f);

		private Vector3 m_fromIconScale;

		private Vector3 m_toIconScale;

		private Vector3 IconScaleMin = Vector3.one;

		private Vector3 IconScaleMax = Vector3.one * 1.25f;
	}
}
