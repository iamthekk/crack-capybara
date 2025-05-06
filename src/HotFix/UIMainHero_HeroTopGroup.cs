using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainHero_HeroTopGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
			this.m_seqPool.Clear(false);
		}

		public void SetTitle(string info)
		{
			if (this.m_titleTxt == null)
			{
				return;
			}
			this.m_titleTxt.text = info;
		}

		public void PlayTitle()
		{
			if (this.m_titleTxt == null)
			{
				return;
			}
			this.m_seqPool.Clear(false);
			Sequence sequence = this.m_seqPool.Get();
			Vector3 vector = Vector3.one * 1.2f;
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_titleTxt.rectTransform, vector, 0.25f), 28));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_titleTxt.rectTransform, Vector3.one, 0.2f), 6));
		}

		[SerializeField]
		private CustomText m_titleTxt;

		private SequencePool m_seqPool = new SequencePool();
	}
}
