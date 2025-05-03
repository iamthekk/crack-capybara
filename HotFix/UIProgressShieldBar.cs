using System;
using UnityEngine;

namespace HotFix
{
	public class UIProgressShieldBar : UIProgressBar
	{
		public new void OnInit()
		{
			base.OnInit();
			this.shiledObject.SetActive(false);
			this.m_shield = this.shiledObject.GetComponent<RectTransform>();
			this.m_shieldAllWidth = this.m_bg.sizeDelta.x - this.m_shield.anchoredPosition.x * 2f;
		}

		private void SetShieldPercent(float value)
		{
			this.m_shield.sizeDelta = new Vector2(this.m_shieldAllWidth * value, this.m_shield.sizeDelta.y);
		}

		public new void OnDeInit()
		{
		}

		public new void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this.m_isPlayingFg && this.m_fg != null && this.m_isPlayingFg)
			{
				this.m_timefg += deltaTime * this.m_fgSpeed;
				if (this.m_timefg > this.m_durationfg)
				{
					this.m_timefg = this.m_durationfg;
					this.m_isPlayingFg = false;
				}
				this.m_currentFg = Mathf.Lerp(this.m_lastCurrentFg, (float)this.m_current, this.m_timefg / this.m_durationfg);
				float num = this.m_currentFg * 1f / (float)this.m_max;
				base.SetFGPercent(num);
			}
		}

		public new void SetProgress(long hp, long maxHp)
		{
			if (this.m_bg == null)
			{
				return;
			}
			this.SetProgress(hp, maxHp, (long)this.m_Shiled);
		}

		public void SetShield(long shieldValue)
		{
			if (this.m_bg == null)
			{
				return;
			}
			if (this.m_shield != null)
			{
				this.m_Shiled = (float)shieldValue;
				this.SetProgress(this.m_current, this.m_max, (long)this.m_Shiled);
			}
		}

		private void SetProgress(long hp, long maxHp, long shieldValue)
		{
			if (shieldValue > 0L)
			{
				if (this.shiledObject != null)
				{
					this.shiledObject.SetActiveSafe(true);
				}
				if (hp + shieldValue > maxHp)
				{
					float num = (float)(hp + shieldValue);
					if (this.m_value != null)
					{
						float num2 = (float)hp / num * 1f;
						base.SetValuePercent(num2);
					}
					this.m_lastCurrentFg = (float)this.m_current;
					if (this.m_fg != null)
					{
						float num3 = (float)this.m_current / num * 1f;
						base.SetFGPercent(num3);
						this.m_isPlayingFg = true;
					}
					if (this.m_shield != null)
					{
						this.SetShieldPercent(1f);
					}
				}
				else if (maxHp == hp + shieldValue)
				{
					this.SetHp(hp, maxHp);
					if (this.m_shield != null)
					{
						this.SetShieldPercent(1f);
					}
				}
				else
				{
					this.SetHp(hp, maxHp);
					if (this.m_shield != null)
					{
						float num4 = (float)(hp + shieldValue) * 1f / (float)maxHp;
						this.SetShieldPercent(num4);
					}
				}
			}
			else
			{
				if (this.m_shield != null)
				{
					this.shiledObject.SetActiveSafe(false);
				}
				this.SetHp(hp, maxHp);
			}
			this.m_timefg = 0f;
			this.m_current = hp;
			this.m_max = maxHp;
			string text = DxxTools.FormatNumber(this.m_current);
			this.SetValueTxt(text);
		}

		private new void SetValueTxt(string info)
		{
			if (this.m_valueTxt == null)
			{
				return;
			}
			this.m_valueTxt.text = info;
		}

		private void SetHp(long value, long max)
		{
			if (this.m_value != null)
			{
				float num = (float)value * 1f / (float)max;
				base.SetValuePercent(num);
			}
			this.m_lastCurrentFg = (float)this.m_current;
			if (this.m_fg != null)
			{
				float num2 = (float)this.m_current * 1f / (float)max;
				base.SetFGPercent(num2);
				this.m_isPlayingFg = true;
			}
		}

		public new void SetValueTxtFormat(string format)
		{
			this.m_valueTxtFormat = format;
		}

		public new void SetFgSpeed(float speed = 1f)
		{
			this.m_fgSpeed = speed;
		}

		public new void SetFgDuration(float duration = 0.5f)
		{
			this.m_durationfg = duration;
		}

		public RectTransform m_shield;

		public GameObject shiledObject;

		private float m_shieldAllWidth;

		private float m_Shiled;
	}
}
