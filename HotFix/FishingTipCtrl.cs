using System;
using DG.Tweening;
using Framework.Logic;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace HotFix
{
	public class FishingTipCtrl : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.canvasGroup.alpha = 0f;
		}

		protected override void OnDeInit()
		{
			ShortcutExtensions.DOKill(this.mTransform, false);
		}

		public void ShowThrowResultTips(FishingEval type)
		{
			TweenExtensions.Kill(this._s1, false);
			TweenExtensions.Kill(this._s2, false);
			if (type == FishingEval.Nice)
			{
				this.throwTitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_Good_1");
				this.throwTipsText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_Good_2");
			}
			else
			{
				this.throwTitleText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_Perfect_1");
				this.throwTipsText.text = Singleton<LanguageManager>.Instance.GetInfoByID("UIFishing_Perfect_2");
			}
			this.mTransform.anchoredPosition = new Vector2(0f, Utility.UI.GetWindowSize().y * 0.23f);
			this.throwAnimEndPos.y = this.mTransform.anchoredPosition.y + 40f;
			this.canvasGroup.alpha = 1f;
			Tweener tweener = TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions46.DOAnchorPosY(this.mTransform, this.throwAnimEndPos.y, this.throwAnimTime * 0.2f, false), this.throwAnimEase), true);
			this._s1 = DOTween.Sequence();
			TweenSettingsExtensions.SetUpdate<Sequence>(this._s1, true);
			TweenSettingsExtensions.Append(this._s1, ShortcutExtensions46.DOFade(this.canvasGroup, 1f, this.throwAnimTime * 0.2f));
			TweenSettingsExtensions.AppendInterval(this._s1, this.throwAnimTime * 0.6f);
			TweenSettingsExtensions.Append(this._s1, ShortcutExtensions46.DOFade(this.canvasGroup, 0f, this.throwAnimTime * 0.3f));
			this._s2 = DOTween.Sequence();
			TweenSettingsExtensions.SetUpdate<Sequence>(this._s2, true);
			TweenSettingsExtensions.Append(this._s2, tweener);
			TweenSettingsExtensions.Join(this._s2, this._s1);
			TweenSettingsExtensions.SetTarget<Sequence>(TweenSettingsExtensions.AppendCallback(this._s2, delegate
			{
			}), this);
		}

		public CustomText throwTitleText;

		public CustomText throwTipsText;

		public RectTransform mTransform;

		[FormerlySerializedAs("canvasgroup")]
		public CanvasGroup canvasGroup;

		public Ease throwAnimEase = 15;

		public float throwAnimTime = 3f;

		public Vector3 throwAnimEndPos = new Vector3(0f, 50f, 0f);

		private Sequence _s1;

		private Sequence _s2;
	}
}
