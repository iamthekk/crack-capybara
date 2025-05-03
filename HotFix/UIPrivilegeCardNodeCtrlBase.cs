using System;
using DG.Tweening;
using Framework.Logic.Component;
using UnityEngine;

namespace HotFix
{
	public class UIPrivilegeCardNodeCtrlBase : CustomBehaviour
	{
		public float SizeY
		{
			get
			{
				return base.rectTransform.sizeDelta.y;
			}
		}

		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public virtual void Refresh()
		{
		}

		public void SetAnchorPosY(float y)
		{
			this.mPosY = y;
			base.rectTransform.anchoredPosition = new Vector2(base.rectTransform.anchoredPosition.x, this.mPosY);
		}

		public void PlayShowAni(SequencePool m_seqPool, int index)
		{
			base.SetActive(true);
			Sequence sequence = m_seqPool.Get();
			CanvasGroup component = base.gameObject.GetComponent<CanvasGroup>();
			component.alpha = 0f;
			TweenSettingsExtensions.AppendInterval(sequence, (float)index * 0.03f);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(component, 1f, 0.2f));
			base.rectTransform.anchoredPosition = new Vector2(base.rectTransform.anchoredPosition.x, this.mPosY - 300f);
			Sequence sequence2 = m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence2, (float)index * 0.03f);
			TweenSettingsExtensions.Append(sequence2, ShortcutExtensions46.DOAnchorPosY(base.rectTransform, this.mPosY, 0.2f, false));
		}

		private float mPosY;
	}
}
