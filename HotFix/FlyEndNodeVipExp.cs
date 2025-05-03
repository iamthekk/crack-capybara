using System;
using DG.Tweening;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class FlyEndNodeVipExp : BaseFlyEndNode
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
			VIPDataModule dataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			if (this.m_levelTxt != null)
			{
				this.m_levelTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("8804", new object[] { dataModule.VipLevel.ToString() });
			}
			EventArgsVIPExpRollBack eventArgsVIPExpRollBack = new EventArgsVIPExpRollBack();
			eventArgsVIPExpRollBack.RollBackVIPExp = (int)this.m_from;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_CurrecyVIPEXP_RollBack, eventArgsVIPExpRollBack);
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
			EventArgsInt eventArgsInt = new EventArgsInt();
			eventArgsInt.SetData((int)((float)this.m_from + (float)this.m_count * 1f * (float)current / (float)maxCount));
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_CurrecyVIPExp_Update, eventArgsInt);
		}

		public override void OnFinished()
		{
			VIPDataModule dataModule = GameApp.Data.GetDataModule(DataName.VIPDataModule);
			if (dataModule.LastVipLevel != dataModule.VipLevel)
			{
				if (GameApp.View.IsOpened(ViewName.VIPLevelUpViewModule))
				{
					GameApp.View.CloseView(ViewName.VIPLevelUpViewModule, null);
				}
				VIPLevelUpViewModule.OpenData openData = new VIPLevelUpViewModule.OpenData();
				openData.VIPLevelOld = dataModule.LastVipLevel;
				openData.VIPLevelNew = dataModule.VipLevel;
				GameApp.View.OpenView(ViewName.VIPLevelUpViewModule, openData, 1, null, null);
			}
		}

		public GameObject m_scaleObj;

		public CustomText m_levelTxt;

		public ParticleSystem m_effect;

		private SequencePool m_seqPool = new SequencePool();
	}
}
