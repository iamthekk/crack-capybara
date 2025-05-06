using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class UIPopCommon : MonoBehaviour
	{
		public Action<UIPopCommon.UIPopCommonClickType> OnClick { get; set; }

		private void Awake()
		{
			this.buttonMask.onClick.AddListener(new UnityAction(this.OnInternalClickMask));
			this.buttonClose.onClick.AddListener(new UnityAction(this.OnInternalClickClose));
			this.commonPanelHeight = this.rtfPopUI.sizeDelta.y;
		}

		public void SetActiveForClose(bool active)
		{
			if (this.buttonMask != null)
			{
				this.buttonMask.enabled = active;
			}
			if (this.buttonClose != null)
			{
				this.buttonClose.gameObject.SetActive(active);
			}
		}

		private void OnInternalClickMask()
		{
			Action<UIPopCommon.UIPopCommonClickType> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(UIPopCommon.UIPopCommonClickType.ButtonMask);
		}

		private void OnInternalClickClose()
		{
			Action<UIPopCommon.UIPopCommonClickType> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(UIPopCommon.UIPopCommonClickType.ButtonClose);
		}

		public void SetRtfHeightByOffset(float offsetHeight)
		{
			this.rtfPopUI.sizeDelta = new Vector2(this.rtfPopUI.sizeDelta.x, this.commonPanelHeight + offsetHeight);
		}

		[SerializeField]
		private CustomButton buttonMask;

		[SerializeField]
		private CustomButton buttonClose;

		[SerializeField]
		private RectTransform rtfPopUI;

		[SerializeField]
		private CustomLanguageText textTitle;

		private float commonPanelHeight;

		public enum UIPopCommonClickType
		{
			ButtonMask,
			ButtonClose
		}
	}
}
