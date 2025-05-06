using System;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UIProgressBar : CustomBehaviour
	{
		public int instanceID
		{
			get
			{
				return base.GetInstanceID();
			}
		}

		protected override void OnInit()
		{
			if (this.m_bg == null)
			{
				string text = "[UIProgressBar] ComponentRegister bg dontHave Image Component";
				GameObject gameObject = base.gameObject;
				HLog.LogError(text, (gameObject != null) ? gameObject.ToString() : null);
				return;
			}
			this.m_fgAllWidth = this.m_bg.sizeDelta.x - this.m_fg.anchoredPosition.x * 2f;
			this.m_valueAllWidth = this.m_bg.sizeDelta.x - this.m_value.anchoredPosition.x * 2f;
		}

		protected override void OnDeInit()
		{
		}

		public void SetFGPercent(float value)
		{
			if (this.m_fg == null)
			{
				return;
			}
			this.m_fg.sizeDelta = new Vector2(this.m_fgAllWidth * value, this.m_fg.sizeDelta.y);
		}

		public void SetValuePercent(float value)
		{
			if (this.m_value == null)
			{
				return;
			}
			this.m_value.sizeDelta = new Vector2(this.m_valueAllWidth * value, this.m_value.sizeDelta.y);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
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
				this.SetFGPercent(num);
			}
		}

		public void SetProgress(long value, long max)
		{
			if (this.m_bg == null)
			{
				return;
			}
			if (this.m_value != null)
			{
				float num = (float)value * 1f / (float)max;
				this.SetValuePercent(num);
			}
			this.m_lastCurrentFg = (float)this.m_current;
			if (this.m_fg != null)
			{
				float num2 = (float)this.m_current * 1f / (float)max;
				Vector2 sizeDelta = this.m_bg.sizeDelta;
				this.SetFGPercent(num2);
				this.m_isPlayingFg = true;
			}
			this.m_timefg = 0f;
			this.m_current = value;
			this.m_max = max;
			this.SetValueTxt(string.Format(this.m_valueTxtFormat, this.m_current, this.m_max));
		}

		public void SetValueTxt(string info)
		{
			if (this.m_valueTxt == null)
			{
				return;
			}
			this.m_valueTxt.text = info;
		}

		public void SetValueTxtFormat(string format)
		{
			this.m_valueTxtFormat = format;
		}

		public void SetFgSpeed(float speed = 1f)
		{
			this.m_fgSpeed = speed;
		}

		public void SetFgDuration(float duration = 0.5f)
		{
			this.m_durationfg = duration;
		}

		public RectTransform m_bg;

		public RectTransform m_fg;

		public RectTransform m_value;

		public Text m_valueTxt;

		private float m_fgAllWidth;

		private float m_valueAllWidth;

		protected long m_current;

		protected long m_max;

		protected string m_valueTxtFormat = "{0}/{1}";

		protected float m_durationfg = 0.5f;

		protected float m_timefg;

		protected float m_lastCurrentFg;

		protected float m_currentFg;

		protected float m_fgSpeed = 1f;

		protected bool m_isPlayingFg;
	}
}
