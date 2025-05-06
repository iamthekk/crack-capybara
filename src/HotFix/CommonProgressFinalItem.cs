using System;
using Coffee.UIExtensions;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class CommonProgressFinalItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.buttonReward.onClick.AddListener(new UnityAction(this.OnClickReward));
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		protected override void OnDeInit()
		{
			this.buttonReward.onClick.RemoveListener(new UnityAction(this.OnClickReward));
		}

		public void SetData(int finalGetCount, int finalLimit)
		{
			this.imageRewardNum.text = string.Format("({0}/{1})", finalGetCount, finalLimit);
			this.HideEffect();
		}

		public void HideEffect()
		{
			this.rewardEffect.gameObject.SetActiveSafe(false);
		}

		public Sequence ShowEffect()
		{
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardEffect.gameObject.SetActiveSafe(true);
				this.rewardEffect.Stop();
				this.rewardEffect.Play();
			});
			TweenSettingsExtensions.AppendInterval(sequence, 1f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.rewardEffect.gameObject.SetActiveSafe(false);
			});
			return sequence;
		}

		public Sequence DoCanvasFade()
		{
			this.rewardCanvas.alpha = 1f;
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 0f, 0.2f));
			return sequence;
		}

		public Sequence DoCanvasFadeScale()
		{
			this.rewardCanvas.alpha = 0f;
			this.rewardCanvas.transform.localScale = Vector3.one;
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.rewardCanvas, 1f, 0.2f)), ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1.05f, 0.2f));
			return sequence;
		}

		public Sequence DoCanvasScale()
		{
			this.rewardCanvas.alpha = 0f;
			Sequence sequence = DOTween.Sequence();
			this.rewardCanvas.transform.localScale = Vector3.zero;
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.rewardCanvas.transform, 1f, 0.1f));
			return sequence;
		}

		private void OnClickReward()
		{
		}

		public CanvasGroup rewardCanvas;

		public CustomText imageRewardNum;

		public CustomButton buttonReward;

		public UIParticle rewardEffect;
	}
}
