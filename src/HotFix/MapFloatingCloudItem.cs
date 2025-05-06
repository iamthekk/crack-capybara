using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class MapFloatingCloudItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.spriteDaytime)
			{
				this.SetAlpha(this.spriteDaytime, 1f);
			}
			if (this.spriteDaytime)
			{
				this.SetAlpha(this.spriteDusk, 0f);
			}
			if (this.spriteDaytime)
			{
				this.SetAlpha(this.spriteNight, 0f);
			}
			this.currentTime = 0;
		}

		protected override void OnDeInit()
		{
		}

		private void SetAlpha(SpriteRenderer sr, float alpha)
		{
			if (sr == null)
			{
				return;
			}
			Color color = sr.color;
			color.a = alpha;
			sr.color = color;
		}

		public void SetTime(int time, float duration)
		{
			if (this.isAni)
			{
				return;
			}
			if (this.currentTime != time)
			{
				SpriteRenderer spriteRender = this.GetSpriteRender(this.currentTime);
				SpriteRenderer spriteRender2 = this.GetSpriteRender(time);
				if (spriteRender != null && spriteRender2 != null)
				{
					this.isAni = true;
					Sequence sequence = DOTween.Sequence();
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions43.DOFade(spriteRender, 0f, duration));
					TweenSettingsExtensions.Join(sequence, ShortcutExtensions43.DOFade(spriteRender2, 1f, duration));
					TweenSettingsExtensions.AppendCallback(sequence, delegate
					{
						this.currentTime = time;
						this.isAni = false;
					});
				}
			}
		}

		private SpriteRenderer GetSpriteRender(int time)
		{
			if (time == 0)
			{
				return this.spriteDaytime;
			}
			if (time == 1)
			{
				return this.spriteDusk;
			}
			if (time == 2)
			{
				return this.spriteNight;
			}
			return null;
		}

		public SpriteRenderer spriteDaytime;

		public SpriteRenderer spriteDusk;

		public SpriteRenderer spriteNight;

		private int currentTime;

		private bool isAni;
	}
}
