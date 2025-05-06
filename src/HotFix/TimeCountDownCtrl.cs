using System;
using System.Collections;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class TimeCountDownCtrl : CustomBehaviour
	{
		public bool IfShowTimeCountDown
		{
			get
			{
				return this.TimeText.text != "";
			}
		}

		protected override void OnInit()
		{
			this.SetText("0");
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.StopTimeCountDown();
		}

		private string GetTimeString(TimeSpan deltaTime)
		{
			int days = deltaTime.Days;
			int hours = deltaTime.Hours;
			int minutes = deltaTime.Minutes;
			int seconds = deltaTime.Seconds;
			return "";
		}

		public string GetDetailedTimeCountDown(TimeSpan countDownTime)
		{
			string text = countDownTime.Hours.ToString("00");
			string text2 = countDownTime.Minutes.ToString("00");
			string text3 = countDownTime.Seconds.ToString("00");
			return string.Format("{0}:{1}:{2}", text, text2, text3);
		}

		public string GetTimeCountDown(TimeSpan countDownTime)
		{
			string text;
			string text2;
			string text3;
			if (countDownTime.Days > 0)
			{
				text = "187";
				text2 = countDownTime.Days.ToString("00");
				text3 = countDownTime.Hours.ToString("00");
			}
			else if (countDownTime.Hours > 0)
			{
				text = "188";
				text2 = countDownTime.Hours.ToString("00");
				text3 = countDownTime.Minutes.ToString("00");
			}
			else
			{
				text = "189";
				text2 = countDownTime.Minutes.ToString("00");
				text3 = countDownTime.Seconds.ToString("00");
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID(text, new object[] { text2, text3 });
		}

		public void StopTimeCountDown()
		{
			if (this._countDownCoroutine != null)
			{
				base.StopCoroutine(this._countDownCoroutine);
			}
		}

		public void SetFormatString(string formatString)
		{
			this.formatString = formatString;
		}

		public void ShowTimeCountDown(long endTime, float delayTime, Action onFinish)
		{
			if (this._countDownCoroutine != null)
			{
				base.StopCoroutine(this._countDownCoroutine);
			}
			if (endTime == 0L)
			{
				this.SetText("0");
				return;
			}
			if (!base.gameObject.activeSelf)
			{
				return;
			}
			this._endTime = endTime;
			this._delayTime = delayTime;
			this._onFinish = onFinish;
			this._countDownCoroutine = null;
			this._countDownCoroutine = base.StartCoroutine(this.PlayCountDown(this._endTime, this._delayTime, this._onFinish));
		}

		private IEnumerator PlayCountDown(long endTime, float deltaTime, Action onFinish)
		{
			string text;
			if (!this.DetailedCount)
			{
				text = this.GetTimeCountDown(new TimeSpan((endTime - DxxTools.Time.ServerTimestamp) * 10000000L));
			}
			else
			{
				text = this.GetDetailedTimeCountDown(new TimeSpan((endTime - DxxTools.Time.ServerTimestamp) * 10000000L));
			}
			if (this.TimeCountDownLanguageID > 0)
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(this.TimeCountDownLanguageID, new object[] { text });
			}
			if (!string.IsNullOrEmpty(this.TimeCountDownLanguageID_str))
			{
				text = Singleton<LanguageManager>.Instance.GetInfoByID(this.TimeCountDownLanguageID_str, new object[] { text });
			}
			this.SetText(text);
			float timeCounter = 0f;
			for (;;)
			{
				timeCounter += Time.unscaledDeltaTime;
				if (timeCounter >= this.DeltaCheckTime)
				{
					timeCounter = 0f;
					long serverTimestamp = DxxTools.Time.ServerTimestamp;
					long num = endTime - serverTimestamp;
					if (num < 0L)
					{
						break;
					}
					if (!this.DetailedCount)
					{
						text = this.GetTimeCountDown(new TimeSpan(num * 10000000L));
					}
					else
					{
						text = this.GetDetailedTimeCountDown(new TimeSpan(num * 10000000L));
					}
					if (this.TimeCountDownLanguageID > 0)
					{
						text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(this.TimeCountDownLanguageID, new object[] { text });
					}
					if (!string.IsNullOrEmpty(this.TimeCountDownLanguageID_str))
					{
						text = Singleton<LanguageManager>.Instance.GetInfoByID(this.TimeCountDownLanguageID_str, new object[] { text });
					}
					this.SetText(text);
				}
				yield return null;
			}
			this.SetText("0");
			yield return deltaTime;
			if (onFinish != null)
			{
				onFinish();
			}
			yield break;
		}

		private void SetText(string str)
		{
			if (string.IsNullOrEmpty(this.formatString))
			{
				this.TimeText.text = str;
				return;
			}
			this.TimeText.text = string.Format(this.formatString, str);
		}

		[SerializeField]
		private Text TimeText;

		[SerializeField]
		private float DeltaCheckTime = 0.9f;

		[SerializeField]
		private TimeCountDownCtrl.TimeCountDownType CountDownType;

		[SerializeField]
		private int TimeCountDownLanguageID;

		[SerializeField]
		private string TimeCountDownLanguageID_str = "";

		[SerializeField]
		private bool DetailedCount;

		private Coroutine _countDownCoroutine;

		private long _endTime;

		private float _delayTime;

		private Action _onFinish;

		private string formatString;

		public enum TimeCountDownType
		{
			eIcon,
			ePanel
		}
	}
}
