using System;
using DG.Tweening;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class FlyEndNodeDefault : BaseFlyEndNode
	{
		protected override void OnInit()
		{
			if (!this.m_isShow)
			{
				this.m_seqPool.Clear(false);
				if (this.m_scaleObj != null)
				{
					this.m_scaleObj.transform.localScale = Vector3.one;
				}
			}
			base.OnInit();
			if (this.m_progressText != null)
			{
				this.m_progressText.text = DxxTools.FormatNumber(this.m_from);
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			if (!this.m_isShow)
			{
				this.m_seqPool.Clear(false);
			}
		}

		public override void OnItemFinished(int current, int maxCount)
		{
			if (this.m_progressText != null)
			{
				this.m_progressText.text = DxxTools.FormatNumber((long)((float)this.m_from + (float)this.m_count * 1f * (float)current / (float)maxCount));
			}
			if (this.m_effect != null)
			{
				this.m_effect.Play();
			}
			if (this.m_scaleObj != null)
			{
				this.m_seqPool.Clear(false);
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_scaleObj.transform.transform, Vector3.one * 1.3f, 0.05f), 1));
				TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.m_scaleObj.transform.transform, Vector3.one, 0.1f), 1));
			}
		}

		public override void OnFinished()
		{
		}

		public GameObject m_scaleObj;

		public CustomText m_progressText;

		public ParticleSystem m_effect;

		private SequencePool m_seqPool = new SequencePool();
	}
}
