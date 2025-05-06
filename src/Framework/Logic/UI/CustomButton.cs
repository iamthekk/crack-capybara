using System;
using Framework.Logic.AttributeExpansion;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomButton : Button
	{
		public Action onDown { get; set; }

		public Action onUp { get; set; }

		protected override void Awake()
		{
			base.Awake();
			this.SetDefaultAnim();
			this._animator = base.GetComponent<Animator>();
		}

		private void SetDefaultAnim()
		{
			if (!Application.isPlaying || this.IsPrefab())
			{
				return;
			}
			if (base.transition == 3 && GameApp.View != null)
			{
				Animator animator = base.GetComponent<Animator>();
				if (animator == null)
				{
					animator = base.gameObject.AddComponent<Animator>();
				}
				animator.runtimeAnimatorController = GameApp.View.Pool.m_buttonAnimatorController;
				animator.updateMode = 2;
			}
		}

		private bool IsPrefab()
		{
			return false;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			this.isCanClick = true;
			if (!this.IsActive() || !this.IsInteractable())
			{
				this.isCanClick = false;
			}
			if (!this.isCanClick)
			{
				return;
			}
			if (this.m_lastClickTime != 0f)
			{
				this.m_currentTime = Time.unscaledTime;
				this.isCanClick = this.m_currentTime - this.m_lastClickTime >= this.m_intervals;
				this.m_lastClickTime = this.m_currentTime;
			}
			else
			{
				this.m_currentTime = Time.unscaledTime;
				this.isCanClick = true;
				this.m_lastClickTime = this.m_currentTime;
			}
			if (!this.isCanClick)
			{
				return;
			}
			this.PlayAudio();
			base.OnPointerClick(eventData);
			if (this.m_onClick != null)
			{
				this.m_onClick();
			}
		}

		private void PlayAudio()
		{
			if (string.IsNullOrEmpty(this.m_audioPath))
			{
				return;
			}
			GameApp.Sound.PlaySoundEffect(this.m_audioPath, 1f);
		}

		public override void OnPointerDown(PointerEventData eventData)
		{
			this._isDown = true;
			if (!this.isCanClick)
			{
				return;
			}
			base.OnPointerDown(eventData);
			if (this.onDown != null)
			{
				this.onDown();
			}
		}

		public override void OnPointerUp(PointerEventData eventData)
		{
			this._isDown = false;
			if (!this.isCanClick)
			{
				return;
			}
			base.OnPointerUp(eventData);
			if (this.onUp != null)
			{
				this.onUp();
			}
		}

		public override void OnPointerEnter(PointerEventData eventData)
		{
			if (this._isDown && this._animator != null)
			{
				this._animator.SetTrigger(CustomButton.AnimationTrigger_Pressed);
			}
		}

		public override void OnPointerExit(PointerEventData eventData)
		{
			if (this._isDown && this._animator != null)
			{
				this._animator.SetTrigger(CustomButton.AnimationTrigger_Normal);
			}
		}

		private static int AnimationTrigger_Normal = Animator.StringToHash("Normal");

		private static int AnimationTrigger_Pressed = Animator.StringToHash("Pressed");

		[Header("Intervals Setting")]
		public float m_intervals = 0.2f;

		[Label]
		[SerializeField]
		private float m_lastClickTime;

		[SerializeField]
		[Label]
		private float m_currentTime;

		[Header("Audio Setting")]
		public string m_audioPath = "Assets/_Resources/Sound/UI/Button_Click_Common.wav";

		public Action m_onClick;

		private bool isCanClick = true;

		private Animator _animator;

		private bool _isDown;
	}
}
