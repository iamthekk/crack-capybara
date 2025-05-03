using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class UISingleSlotImageItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(Sprite sprite)
		{
			this.image.sprite = sprite;
		}

		public void SetGray(bool isGray)
		{
			if (isGray)
			{
				this.uiGray.SetUIGray();
				return;
			}
			this.uiGray.Recovery();
		}

		public void ShowEffect()
		{
			if (this.effect)
			{
				this.effect.Play();
			}
		}

		public void PlayAni()
		{
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOScale(this.image.transform, 1.2f, 0.15f)), ShortcutExtensions.DOScale(this.image.transform, 1f, 0.05f));
		}

		public Image image;

		public UIGray uiGray;

		public ParticleSystem effect;
	}
}
