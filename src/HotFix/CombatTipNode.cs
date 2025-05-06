using System;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CombatTipNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_listen != null)
			{
				this.m_listen.onListen.AddListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			if (this.m_time >= this.m_duration)
			{
				this.m_time = this.m_duration;
				this.m_isPlaying = false;
			}
			this.m_currentFrom = Utility.Math.Lerp(this.m_from, this.m_to, this.m_time / this.m_duration);
			if (this.m_fromTxt != null)
			{
				this.m_fromTxt.text = DxxTools.FormatNumber(this.m_currentFrom);
			}
			this.m_currentTo = (this.m_isUp ? (this.m_to - this.m_currentFrom) : (this.m_currentFrom - this.m_to));
			if (this.m_customText != null)
			{
				this.m_customText.text = ((this.m_currentTo != 0L) ? DxxTools.FormatNumber(this.m_currentTo) : string.Empty);
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_listen != null)
			{
				this.m_listen.onListen.RemoveListener(new UnityAction<GameObject, string>(this.OnAnimatorListen));
			}
		}

		public void SetData(long from, long to)
		{
			this.m_from = from;
			this.m_to = to;
			this.m_isUp = to > from;
		}

		public void Play()
		{
			if (this.m_fromTxt != null)
			{
				this.m_fromTxt.text = DxxTools.FormatNumber(this.m_from);
			}
			if (this.m_isUp)
			{
				this.m_currentIcon = this.m_upIcon;
				this.m_customText = this.m_upTxt;
				if (this.m_downIcon != null)
				{
					this.m_downIcon.SetActive(false);
				}
				if (this.m_downTxt != null)
				{
					this.m_downTxt.gameObject.SetActive(false);
				}
			}
			else
			{
				this.m_currentIcon = this.m_downIcon;
				this.m_customText = this.m_downTxt;
				if (this.m_upIcon != null)
				{
					this.m_upIcon.SetActive(false);
				}
				if (this.m_upTxt != null)
				{
					this.m_upTxt.gameObject.SetActive(false);
				}
			}
			if (this.m_currentIcon != null)
			{
				this.m_currentIcon.gameObject.SetActive(true);
			}
			if (this.m_customText != null)
			{
				this.m_customText.gameObject.SetActive(true);
				this.m_customText.text = DxxTools.FormatNumber(this.m_isUp ? (this.m_to - this.m_from) : (this.m_from - this.m_to));
			}
			this.m_time = 0f;
		}

		private void OnAnimatorListen(GameObject gameObject, string eventParameter)
		{
			if (string.Equals(eventParameter, "Run"))
			{
				this.m_isPlaying = true;
			}
			if (string.Equals(eventParameter, "End"))
			{
				this.m_isPlaying = false;
				Action<CombatTipNode> onFinished = this.m_onFinished;
				if (onFinished == null)
				{
					return;
				}
				onFinished(this);
			}
		}

		public AnimatorListen m_listen;

		public CustomText m_fromTxt;

		public GameObject m_upIcon;

		public CustomText m_upTxt;

		public GameObject m_downIcon;

		public CustomText m_downTxt;

		public Action<CombatTipNode> m_onFinished;

		private long m_from;

		private long m_to;

		public float m_duration = 0.2f;

		public float m_time;

		public bool m_isPlaying;

		private bool m_isUp;

		private GameObject m_currentIcon;

		private CustomText m_customText;

		private long m_currentFrom;

		private long m_currentTo;
	}
}
