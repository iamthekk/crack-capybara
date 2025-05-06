using System;
using Framework;
using Framework.Logic;
using UnityEngine.UI;

namespace HotFix
{
	public class AttributeExpAnim
	{
		public void Init(Slider der)
		{
			this.sliderExp = der;
			this.m_currentHp = 0;
			this.m_maxHp = 0;
			this.CurLevel = 1;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			float num = Utility.Math.Clamp01(this.m_time / 0.4f);
			int num2 = this.m_newCurrentHp;
			if (this.m_currentHp != this.m_newCurrentHp)
			{
				num2 = this.m_currentHp + (int)((float)(this.m_newCurrentHp - this.m_currentHp) * num);
			}
			int num3 = this.m_newMaxHp;
			if (this.m_maxHp != this.m_newMaxHp)
			{
				num3 = this.m_maxHp + (int)((float)(this.m_newMaxHp - this.m_maxHp) * num);
			}
			this.SetSlider(num2, num3);
			if (Utility.Math.Abs(num - 1f) < 0.0001f)
			{
				if (this.isLevel)
				{
					this.SetSlider(0, this.tempMaxValue);
					GameApp.Sound.PlayClip(57, 1f);
					if (this.CurLevel < this.tempLevel)
					{
						this.CurLevel++;
					}
					else
					{
						Action onAniFinish = this.OnAniFinish;
						if (onAniFinish != null)
						{
							onAniFinish();
						}
						this.isLevel = false;
						this.m_newCurrentHp = this.tempCurValue;
						this.m_newMaxHp = this.tempMaxValue;
					}
					this.m_currentHp = 0;
					this.m_isPlaying = true;
					this.m_time = 0f;
					return;
				}
				this.m_isPlaying = false;
				this.m_currentHp = this.m_newCurrentHp;
				this.m_maxHp = this.m_newMaxHp;
				this.SetSlider(this.m_newCurrentHp, this.m_newMaxHp);
				Action onAniFinish2 = this.OnAniFinish;
				if (onAniFinish2 == null)
				{
					return;
				}
				onAniFinish2();
			}
		}

		public void SetValue(int current, int max, int curLevel, Action onFinish = null)
		{
			if (curLevel == this.CurLevel)
			{
				if (current == this.m_currentHp && max == this.m_maxHp)
				{
					Action onAniFinish = this.OnAniFinish;
					if (onAniFinish == null)
					{
						return;
					}
					onAniFinish();
					return;
				}
				else if (current == this.m_newCurrentHp && max == this.m_newMaxHp)
				{
					Action onAniFinish2 = this.OnAniFinish;
					if (onAniFinish2 == null)
					{
						return;
					}
					onAniFinish2();
					return;
				}
			}
			this.OnAniFinish = onFinish;
			if (this.CurLevel < curLevel)
			{
				this.isLevel = true;
				this.CurLevel++;
				this.tempLevel = curLevel;
				this.tempCurValue = current;
				this.tempMaxValue = max;
			}
			if (this.m_isPlaying)
			{
				this.m_isPlaying = false;
				this.m_currentHp = this.m_newCurrentHp;
				this.m_maxHp = this.m_newMaxHp;
				this.SetSlider(this.m_newCurrentHp, this.m_newMaxHp);
			}
			this.m_newCurrentHp = (this.isLevel ? this.m_maxHp : current);
			this.m_newMaxHp = (this.isLevel ? this.m_maxHp : max);
			if (this.m_maxHp > 0)
			{
				if (this.m_currentHp != this.m_newCurrentHp || this.m_maxHp != this.m_newMaxHp)
				{
					this.SetSlider(this.m_currentHp, this.m_maxHp);
					this.m_time = 0f;
					this.m_isPlaying = true;
					return;
				}
			}
			else
			{
				this.m_currentHp = this.m_newCurrentHp;
				this.m_maxHp = this.m_newMaxHp;
				this.SetSlider(this.m_currentHp, this.m_maxHp);
			}
		}

		private void SetSlider(int current, int max)
		{
			if (max == 0)
			{
				this.sliderExp.value = 0f;
				return;
			}
			this.sliderExp.value = (float)current / (float)max;
		}

		public const float PlayDuration = 0.4f;

		private Slider sliderExp;

		private bool m_isPlaying;

		private int m_currentHp;

		private int m_maxHp;

		private int m_newCurrentHp;

		private int m_newMaxHp;

		private Action OnAniFinish;

		private float m_time;

		private bool isLevel;

		private int CurLevel = 1;

		private int tempCurValue;

		private int tempMaxValue;

		private int tempLevel = 1;
	}
}
