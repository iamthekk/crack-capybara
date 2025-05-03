using System;
using DG.Tweening;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRaceTabButtonLock
	{
		public void SetButton(CustomButton button)
		{
			this.Button = button;
			this.ObjButton = button.gameObject;
			this.RTFButton = button.transform as RectTransform;
			this.CanvasGroup = button.GetComponent<CanvasGroup>();
		}

		public void PlayToUnLock(SequencePool m_seqPool)
		{
			this.IsLock = false;
			Sequence sequence = m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.RTFButton, 1.5f, 0.3f));
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				if (this.ObjButton != null)
				{
					this.ObjButton.SetActive(false);
				}
			});
			TweenSettingsExtensions.Append(m_seqPool.Get(), ShortcutExtensions46.DOFade(this.CanvasGroup, 0f, 0.3f));
		}

		public void SetLockNow(bool islock)
		{
			this.IsLock = islock;
			this.ObjButton.SetActive(islock);
			this.CanvasGroup.alpha = (float)(islock ? 1 : 0);
			this.RTFButton.localScale = Vector3.one;
		}

		public CustomButton Button;

		public GameObject ObjButton;

		public RectTransform RTFButton;

		public CanvasGroup CanvasGroup;

		public bool IsLock;
	}
}
