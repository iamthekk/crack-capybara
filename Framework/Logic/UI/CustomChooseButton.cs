using System;
using Framework.Logic.AttributeExpansion;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomChooseButton : Button
	{
		public bool IsSelected
		{
			get
			{
				return this.m_isSelect;
			}
		}

		protected override void Awake()
		{
			base.Awake();
			if (this.m_isCoercionSelect)
			{
				this.SetSelect(false);
			}
			this.EnableOnClickButton();
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			bool flag = true;
			if (!this.IsActive() || !this.IsInteractable())
			{
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			if (this.m_lastClickTime != 0f)
			{
				this.m_currentTime = Time.unscaledTime;
				flag = this.m_currentTime - this.m_lastClickTime >= this.m_intervals;
				this.m_lastClickTime = this.m_currentTime;
			}
			else
			{
				this.m_currentTime = Time.unscaledTime;
				flag = true;
				this.m_lastClickTime = this.m_currentTime;
			}
			if (!flag)
			{
				return;
			}
			base.OnPointerClick(eventData);
			if (this.m_isCoercionSelect)
			{
				this.m_isSelect = !this.m_isSelect;
				this.SetSelect(this.m_isSelect);
			}
			this.PlayAudio();
		}

		private void PlayAudio()
		{
			if (string.IsNullOrEmpty(this.m_audioPath))
			{
				return;
			}
			GameApp.Sound.PlaySoundEffect(this.m_audioPath, 1f);
		}

		public void EnableOnClickButton()
		{
			base.onClick.RemoveListener(new UnityAction(this.InternalClickButton));
			base.onClick.AddListener(new UnityAction(this.InternalClickButton));
		}

		private void InternalClickButton()
		{
			Action<CustomChooseButton> onClickButton = this.OnClickButton;
			if (onClickButton == null)
			{
				return;
			}
			onClickButton(this);
		}

		public void SetSelect(bool isSelect)
		{
			this.m_isSelect = isSelect;
			this.onSelectChanged.Invoke(isSelect);
			if (this.m_imageTarget != null)
			{
				this.m_imageTarget.sprite = (isSelect ? this.m_selectSprite : this.m_unSelectSprite);
			}
			if (this.m_unSelectText != null)
			{
				this.m_unSelectText.gameObject.SetActive(!isSelect);
			}
			if (this.m_selectText != null)
			{
				this.m_selectText.gameObject.SetActive(isSelect);
			}
			if (this.m_arrowObj != null)
			{
				this.m_arrowObj.SetActive(isSelect);
			}
		}

		public void SetUnSelectText(string text)
		{
			if (this.m_unSelectText != null)
			{
				this.m_unSelectText.text = text;
			}
		}

		public void SetSelectText(string text)
		{
			if (this.m_selectText != null)
			{
				this.m_selectText.text = text;
			}
		}

		public void SetText(string text)
		{
			this.SetUnSelectText(text);
			this.SetSelectText(text);
		}

		[Header("Intervals Setting")]
		public float m_intervals = 0.1f;

		[Label]
		[SerializeField]
		private float m_lastClickTime;

		[Label]
		[SerializeField]
		private float m_currentTime;

		[Header("Image Setting")]
		public Image m_imageTarget;

		public Sprite m_unSelectSprite;

		public Sprite m_selectSprite;

		public Text m_unSelectText;

		public Text m_selectText;

		public GameObject m_arrowObj;

		public bool m_isCoercionSelect = true;

		[Label]
		[SerializeField]
		private bool m_isSelect;

		[Header("Audio Setting")]
		public string m_audioPath = "Assets/_Resources/Sound/UI/Button_Click_Common.wav";

		public Action<CustomChooseButton> OnClickButton;

		public CustomChooseButton.SelectChangedEvent onSelectChanged = new CustomChooseButton.SelectChangedEvent();

		[Serializable]
		public class SelectChangedEvent : UnityEvent<bool>
		{
		}
	}
}
