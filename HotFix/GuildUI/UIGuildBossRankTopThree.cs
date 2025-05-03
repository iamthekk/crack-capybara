using System;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildBossRankTopThree : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetData(GuildBossGuildRankData data)
		{
			this.mRankData = data;
		}

		public void RefreshUI()
		{
			if (this.mRankData == null)
			{
				this.RefreshAsNull();
				return;
			}
			this.ObjEmptyGuildIcon.SetActive(false);
			this.GuildIcon.SetActive(true);
			this.GuildIcon.SetIcon(this.mRankData.GuildData.GuildIcon);
			this.TextName.text = this.mRankData.GuildData.GuildName;
			if (this.mRankData.GuildData.GuildDamage == 0L)
			{
				this.TextDamage.gameObject.SetActiveSafe(false);
				this.DamageIcon.gameObject.SetActiveSafe(false);
				return;
			}
			this.TextDamage.text = GuildProxy.Language.FormatNumber(this.mRankData.GuildData.GuildDamage);
			this.TextDamage.gameObject.SetActiveSafe(true);
			this.DamageIcon.gameObject.SetActiveSafe(true);
		}

		private void RefreshAsNull()
		{
			this.ObjEmptyGuildIcon.SetActive(true);
			this.GuildIcon.SetActive(false);
			this.TextName.text = GuildProxy.Language.GetInfoByID_LogError(400283);
			this.TextDamage.gameObject.SetActiveSafe(false);
			this.DamageIcon.gameObject.SetActiveSafe(false);
		}

		public void RefreshAsPreLoad()
		{
			this.TextName.rectTransform.localScale = new Vector2(0f, 1f);
			this.RTFIconNode.localScale = Vector3.zero;
			this.RTFFlagNode.localScale = new Vector3(1f, 0f, 1f);
		}

		public void PlayShowAni(SequencePool m_seqPool, float delay = 0f)
		{
			Sequence sequence = m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, delay);
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.TextName.rectTransform, 1f, 0.2f));
			Sequence sequence2 = m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence2, delay + 0.1f);
			TweenSettingsExtensions.Append(sequence2, ShortcutExtensions.DOScale(this.RTFIconNode, 1f, 0.2f));
			Sequence sequence3 = m_seqPool.Get();
			TweenSettingsExtensions.AppendInterval(sequence3, delay + 0.2f);
			TweenSettingsExtensions.Append(sequence3, ShortcutExtensions.DOScale(this.RTFFlagNode, new Vector3(1f, 1.2f, 1f), 0.15f));
			TweenSettingsExtensions.Append(sequence3, ShortcutExtensions.DOScale(this.RTFFlagNode, new Vector3(1f, 0.95f, 1f), 0.05f));
			TweenSettingsExtensions.Append(sequence3, ShortcutExtensions.DOScale(this.RTFFlagNode, new Vector3(1f, 1f, 1f), 0.05f));
		}

		public UIGuildIcon GuildIcon;

		public GameObject ObjEmptyGuildIcon;

		public CustomText TextName;

		public CustomText TextDamage;

		public Image DamageIcon;

		[Header("动画组件")]
		public RectTransform RTFNameNode;

		public RectTransform RTFIconNode;

		public RectTransform RTFFlagNode;

		private GuildBossGuildRankData mRankData;
	}
}
