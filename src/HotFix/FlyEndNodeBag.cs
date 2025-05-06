using System;
using DG.Tweening;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class FlyEndNodeBag : BaseFlyEndNode
	{
		protected override void OnInit()
		{
			if (!this.m_isShow)
			{
				this.m_seqPool.Clear(false);
				if (this.m_nameTxt != null)
				{
					this.m_nameTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("1301");
				}
				base.transform.localScale = Vector3.one;
				if (this.m_redNode != null && this.m_animator != null)
				{
					string text = ((this.m_redNode.count > 0) ? "Shake" : "Idle");
					this.m_animator.ResetTrigger("Shake");
					this.m_animator.ResetTrigger("Idle");
					this.m_animator.SetTrigger(text);
				}
			}
			base.OnInit();
			this.m_redNode.SetType(240);
			this.m_redNode.Value = 0;
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			if (!this.m_isShow)
			{
				if (this.m_animator != null)
				{
					this.m_animator.StopPlayback();
				}
				this.m_seqPool.Clear(false);
			}
		}

		public override void OnItemFinished(int current, int maxCount)
		{
			if (this.m_effect != null)
			{
				this.m_effect.Play();
			}
			this.m_seqPool.Clear(false);
			Sequence sequence = this.m_seqPool.Get();
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform.transform, Vector3.one * 1.3f, 0.05f), 1));
			TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(base.transform.transform, Vector3.one, 0.1f), 1));
		}

		public override void OnFinished()
		{
		}

		public CustomText m_nameTxt;

		public RedNodeOneCtrl m_redNode;

		public Animator m_animator;

		public ParticleSystem m_effect;

		private SequencePool m_seqPool = new SequencePool();
	}
}
