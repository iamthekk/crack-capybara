using System;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UINetLoading : MonoBehaviour
	{
		private void OnEnable()
		{
			this.m_time = 0f;
			this.OnLanguageChange();
			this.CheckDelayShow();
		}

		private void Update()
		{
			this.OnUpdateText();
			this.OnUpdateAlpha();
		}

		public void OnLanguageChange()
		{
			this.m_loadTxtInfo = Singleton<LanguageManager>.Instance.GetInfoByID(this.LanguageID.ToString());
			this.m_text.text = this.m_loadTxtInfo;
			this.m_pointCount = 1;
			this.m_time = 0f;
			this.OnUpdateText();
		}

		private void OnUpdateText()
		{
			this.m_time += Time.unscaledDeltaTime;
			if (this.m_time >= 0.5f)
			{
				this.m_pointCount++;
				if (this.m_pointCount > 3)
				{
					this.m_pointCount = 1;
				}
				string text = "";
				for (int i = 0; i < this.m_pointCount; i++)
				{
					text += ".";
				}
				this.m_text.text = string.Format(this.m_loadTxtInfo, text);
				this.m_time = 0f;
			}
		}

		private void CheckDelayShow()
		{
			if (this.m_delayShowTime > 0.0 && this.m_canvasGroup != null)
			{
				this.m_delayShowTimeLeft = this.m_delayShowTime;
				this.m_canvasGroup.alpha = 0f;
				return;
			}
			if (this.m_canvasGroup != null)
			{
				this.m_canvasGroup.alpha = 1f;
			}
			this.m_delayShowTimeLeft = 0.0;
		}

		private void OnUpdateAlpha()
		{
			if (this.m_delayShowTimeLeft > 0.0)
			{
				this.m_delayShowTimeLeft -= (double)Time.unscaledDeltaTime;
				if (this.m_delayShowTimeLeft <= 0.0)
				{
					this.m_delayShowTimeLeft = 0.0;
					if (this.m_canvasGroup != null)
					{
						this.m_canvasGroup.alpha = 1f;
					}
				}
			}
		}

		public int LanguageID = 15200;

		public CustomText m_text;

		[Header("Setting")]
		public double m_delayShowTime = 0.3;

		private double m_delayShowTimeLeft;

		public CanvasGroup m_canvasGroup;

		[Label]
		public string m_loadTxtInfo = string.Empty;

		[Label]
		public float m_time;

		[Label]
		public int m_pointCount = 1;
	}
}
