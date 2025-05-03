using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIGameEventBoxSkillItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.fxPurple.gameObject.SetActiveSafe(false);
			this.fxGolden.gameObject.SetActiveSafe(false);
			this.skillItem.Init();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
			this.skillItem.DeInit();
		}

		public void Refresh(GameEventSkillBuildData data)
		{
			if (data == null)
			{
				return;
			}
			Sprite sprite = null;
			switch (data.quality)
			{
			case SkillBuildQuality.Gray:
				sprite = this.spriteRegister.GetSprite(UIGameEventBoxSkillItem.BoxLightQuality.Blue.ToString());
				this.currentPs = null;
				break;
			case SkillBuildQuality.Gold:
				sprite = this.spriteRegister.GetSprite(UIGameEventBoxSkillItem.BoxLightQuality.Purple.ToString());
				this.currentPs = this.fxPurple;
				break;
			case SkillBuildQuality.Red:
				sprite = this.spriteRegister.GetSprite(UIGameEventBoxSkillItem.BoxLightQuality.Yellow.ToString());
				this.currentPs = this.fxGolden;
				break;
			}
			if (sprite != null)
			{
				this.imageLight.sprite = sprite;
			}
			this.skillItem.Refresh(data, false, true);
		}

		public Sequence ShowAni(bool isCallBack, Action onFinish)
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.2f;
			float num2 = 0.08f;
			Color color = this.imageLight.color;
			color.a = 0f;
			this.imageLight.color = color;
			this.skillItem.transform.localScale = new Vector3(0f, 1f, 1f);
			this.imageBack.transform.localScale = Vector3.one;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.imageLight, 1f, num));
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.imageBack.transform, new Vector3(0f, 0.7f, 0.7f), num), 1)), delegate
			{
				this.imageBack.transform.localScale = new Vector3(0f, 1f, 1f);
			});
			TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.skillItem.transform, Vector3.one * 1.2f, num2), 11)), delegate
			{
				if (this.currentPs != null)
				{
					this.PlayBGFx(this.currentPs);
				}
			});
			float num3 = 0.1f;
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.skillItem.transform, Vector3.one * 0.95f, num3)), ShortcutExtensions.DOScale(this.skillItem.transform, Vector3.one, 0.03f));
			if (isCallBack)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					Action onFinish2 = onFinish;
					if (onFinish2 == null)
					{
						return;
					}
					onFinish2();
				});
			}
			return sequence;
		}

		public void PlayBGFx(ParticleSystem ps)
		{
			ps.gameObject.SetActiveSafe(true);
			ps.Play();
		}

		public void StopBGFx(ParticleSystem ps)
		{
			ps.gameObject.SetActiveSafe(true);
			ps.Play();
		}

		public UIGameEventSkillItem skillItem;

		public GameObject imageBack;

		public GameObject animNode;

		public SpriteRegister spriteRegister;

		public CustomImage imageLight;

		public ParticleSystem fxPurple;

		public ParticleSystem fxGolden;

		private ParticleSystem currentPs;

		public enum BoxLightQuality
		{
			Blue,
			Purple,
			Yellow
		}
	}
}
