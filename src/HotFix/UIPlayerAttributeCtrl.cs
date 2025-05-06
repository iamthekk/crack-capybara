using System;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class UIPlayerAttributeCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.Button_HP.onClick.AddListener(new UnityAction(this.OnClickHP));
			this.Button_Attack.onClick.AddListener(new UnityAction(this.OnClickAttack));
			this.Button_Defence.onClick.AddListener(new UnityAction(this.OnClickDefence));
			this.HpAnima = new AttributeHpAnim();
			this.HpAnima.Init(this.tranHpInfo, this.Text_HP);
			this.HpPercentAnima = new AttributeAnim();
			this.HpPercentAnima.Init(this.tranHpInfo, this.Text_HpPercent, true);
			this.AttackAnim = new AttributeAnim();
			this.AttackAnim.Init(this.tranAttackInfo, this.Text_Attack, false);
			this.DefenseAnim = new AttributeAnim();
			this.DefenseAnim.Init(this.tranDefenseInfo, this.Text_Defence, false);
		}

		protected override void OnDeInit()
		{
			this.Button_HP.onClick.RemoveListener(new UnityAction(this.OnClickHP));
			this.Button_Attack.onClick.RemoveListener(new UnityAction(this.OnClickAttack));
			this.Button_Defence.onClick.RemoveListener(new UnityAction(this.OnClickDefence));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			AttributeHpAnim hpAnima = this.HpAnima;
			if (hpAnima != null)
			{
				hpAnima.OnUpdate(deltaTime);
			}
			AttributeAnim hpPercentAnima = this.HpPercentAnima;
			if (hpPercentAnima != null)
			{
				hpPercentAnima.OnUpdate(deltaTime);
			}
			AttributeAnim attackAnim = this.AttackAnim;
			if (attackAnim != null)
			{
				attackAnim.OnUpdate(deltaTime);
			}
			AttributeAnim defenseAnim = this.DefenseAnim;
			if (defenseAnim == null)
			{
				return;
			}
			defenseAnim.OnUpdate(deltaTime);
		}

		public void SetHP(long current, long max, bool useAnimation)
		{
			double num = (double)current / (double)max * 100.0;
			if (max > 0L)
			{
				this.m_sliderHP.value = (float)current / ((float)max + 0f);
			}
			this.HpAnima.SetHp(current, max, useAnimation);
			if (num <= 30.0)
			{
				this.HpAnima.SetTextColor(this.GetColorByType(UIAttributeController.AttributeColorType.Red));
				return;
			}
			this.HpAnima.SetTextColor(Color.white);
		}

		public void SetHpPercent(long current)
		{
			this.HpPercentAnima.SetValue(current);
			if (current <= 30L)
			{
				this.hpPercentAni.Play("TweenAlpha");
				return;
			}
			this.hpPercentAni.Play("Idle");
		}

		public void SetAttack(long value)
		{
			this.AttackAnim.SetValue(value);
		}

		public void SetAttackColor(UIAttributeController.AttributeColorType colorType)
		{
			Color colorByType = this.GetColorByType(colorType);
			this.AttackAnim.SetTextColor(colorByType);
		}

		public void SetDefence(long value)
		{
			this.DefenseAnim.SetValue(value);
		}

		public void SetDefenceColor(UIAttributeController.AttributeColorType colorType)
		{
			Color colorByType = this.GetColorByType(colorType);
			this.DefenseAnim.SetTextColor(colorByType);
		}

		private Color GetColorByType(UIAttributeController.AttributeColorType colorType)
		{
			Color white = Color.white;
			if (colorType != UIAttributeController.AttributeColorType.Green)
			{
				if (colorType == UIAttributeController.AttributeColorType.Red)
				{
					ColorUtility.TryParseHtmlString("#FF604D", ref white);
				}
			}
			else
			{
				ColorUtility.TryParseHtmlString("#D3F24E", ref white);
			}
			return white;
		}

		private void OnClickHP()
		{
			this.ShowTip("UIGameEvent_17", "UIGameEvent_120", this.Button_HP.transform.position);
		}

		private void OnClickAttack()
		{
			this.ShowTip("UIGameEvent_121", "UIGameEvent_122", this.Button_Attack.transform.position);
		}

		private void OnClickDefence()
		{
			this.ShowTip("UIGameEvent_123", "UIGameEvent_124", this.Button_Defence.transform.position);
		}

		private void ShowTip(string nameLanguageId, string infoLanguageId, Vector3 position)
		{
			new InfoTipViewModule.InfoTipData
			{
				m_name = Singleton<LanguageManager>.Instance.GetInfoByID(nameLanguageId),
				m_info = Singleton<LanguageManager>.Instance.GetInfoByID(infoLanguageId),
				m_position = position,
				m_offsetY = 226f
			}.Open();
		}

		public CustomButton Button_HP;

		public CustomButton Button_Attack;

		public CustomButton Button_Defence;

		public Slider m_sliderHP;

		public CustomText Text_HP;

		public CustomText Text_Attack;

		public CustomText Text_Defence;

		public CustomText Text_HpPercent;

		public Transform tranHpInfo;

		public Transform tranAttackInfo;

		public Transform tranDefenseInfo;

		public Animator hpPercentAni;

		public const float TipOffset = 226f;

		private AttributeHpAnim HpAnima;

		private AttributeAnim HpPercentAnima;

		private AttributeAnim AttackAnim;

		private AttributeAnim DefenseAnim;
	}
}
