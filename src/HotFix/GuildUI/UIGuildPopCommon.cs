using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildPopCommon : MonoBehaviour
	{
		public void OnEnable()
		{
			if (this.ButtonMask != null)
			{
				this.ButtonMask.onClick.AddListener(new UnityAction(this.OnInternalClickMask));
			}
			if (this.ButtonClose != null)
			{
				this.ButtonClose.onClick.AddListener(new UnityAction(this.OnInternalClickClose));
			}
			if (this.TapToClose != null)
			{
				this.TapToClose.OnClose = new Action(this.OnInternalClickTapToClose);
			}
		}

		public void OnDisable()
		{
			if (this.ButtonMask != null)
			{
				this.ButtonMask.onClick.RemoveListener(new UnityAction(this.OnInternalClickMask));
			}
			if (this.ButtonClose != null)
			{
				this.ButtonClose.onClick.RemoveListener(new UnityAction(this.OnInternalClickClose));
			}
			if (this.TapToClose != null)
			{
				this.TapToClose.OnClose = null;
			}
		}

		private void OnInternalClickMask()
		{
			Action<int> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(1);
		}

		private void OnInternalClickClose()
		{
			Action<int> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(2);
		}

		private void OnInternalClickTapToClose()
		{
			Action<int> onClick = this.OnClick;
			if (onClick == null)
			{
				return;
			}
			onClick(3);
		}

		public CustomButton ButtonMask;

		public CustomButton ButtonClose;

		public TapToCloseCtrl TapToClose;

		public RectTransform RTFPopUI;

		public CustomLanguageText TextTitle;

		public Action<int> OnClick;
	}
}
