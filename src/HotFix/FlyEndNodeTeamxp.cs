using System;
using DG.Tweening;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class FlyEndNodeTeamxp : BaseFlyEndNode
	{
		protected override void OnInit()
		{
			if (!this.m_isShow)
			{
				this.m_seqPool.Clear(false);
				this.m_avatar.Init();
				base.transform.localScale = Vector3.one;
			}
			base.OnInit();
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = dataModule.NickName;
			}
			if (this.m_avatar != null)
			{
				this.m_avatar.RefreshData(dataModule.Avatar, dataModule.AvatarFrame);
			}
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			if (!this.m_isShow)
			{
				this.m_seqPool.Clear(false);
				if (this.m_avatar != null)
				{
					this.m_avatar.DeInit();
				}
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

		public UIAvatarCtrl m_avatar;

		public CustomText m_nameTxt;

		public ParticleSystem m_effect;

		private SequencePool m_seqPool = new SequencePool();
	}
}
