using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UICoinNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.coinAnim = new AttributeAnim();
			this.coinAnim.Init(this.flyNode.transform, this.textCoin, false);
			Vector2 anchoredPosition = this.aniTrans.anchoredPosition;
			anchoredPosition.x = this.HideX;
			this.aniTrans.anchoredPosition = anchoredPosition;
			this.currentCoin = 0L;
		}

		protected override void OnDeInit()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			AttributeAnim attributeAnim = this.coinAnim;
			if (attributeAnim == null)
			{
				return;
			}
			attributeAnim.OnUpdate(deltaTime);
		}

		public void SetCoin(long coin, float duration)
		{
			if (coin == this.currentCoin)
			{
				return;
			}
			this.currentCoin = coin;
			this.coinAnim.SetDuration(duration);
			if (this.isShow)
			{
				this.coinAnim.SetValue(coin, new Action(this.Hide));
				return;
			}
			this.Show(delegate
			{
				this.coinAnim.SetValue(coin, new Action(this.Hide));
			});
		}

		public void Show(Action callback = null)
		{
			base.gameObject.SetActiveSafe(true);
			this.isShow = true;
			if (this.hideSeq != null)
			{
				TweenExtensions.Kill(this.hideSeq, false);
			}
			Vector2 anchoredPosition = this.aniTrans.anchoredPosition;
			anchoredPosition.x = this.HideX;
			this.aniTrans.anchoredPosition = anchoredPosition;
			this.showSeq = DOTween.Sequence();
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(this.showSeq, ShortcutExtensions46.DOAnchorPosX(this.aniTrans, -20f, 0.25f, false)), ShortcutExtensions46.DOAnchorPosX(this.aniTrans, this.ShowX, 0.15f, false));
			TweenSettingsExtensions.AppendInterval(this.showSeq, 0.5f);
			TweenSettingsExtensions.AppendCallback(this.showSeq, delegate
			{
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
		}

		public void Hide()
		{
			this.isShow = false;
			if (this.showSeq != null)
			{
				TweenExtensions.Kill(this.showSeq, false);
			}
			Vector2 anchoredPosition = this.aniTrans.anchoredPosition;
			anchoredPosition.x = this.ShowX;
			this.aniTrans.anchoredPosition = anchoredPosition;
			this.hideSeq = DOTween.Sequence();
			TweenSettingsExtensions.AppendInterval(this.hideSeq, 1f);
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(this.hideSeq, ShortcutExtensions46.DOAnchorPosX(this.aniTrans, -20f, 0.15f, false)), ShortcutExtensions46.DOAnchorPosX(this.aniTrans, this.HideX, 0.25f, false));
			TweenSettingsExtensions.AppendCallback(this.hideSeq, delegate
			{
				base.gameObject.SetActiveSafe(false);
			});
		}

		public CurrencyType currencyType;

		public GameObject flyNode;

		public RectTransform aniTrans;

		public CustomText textCoin;

		private bool isShow;

		private AttributeAnim coinAnim;

		private Sequence showSeq;

		private Sequence hideSeq;

		private long currentCoin;

		private float HideX = 300f;

		private float ShowX;
	}
}
