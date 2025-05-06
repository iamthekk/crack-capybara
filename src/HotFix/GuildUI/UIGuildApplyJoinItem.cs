using System;
using DG.Tweening;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildApplyJoinItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		public long UserID
		{
			get
			{
				if (this.guildUserShareData == null)
				{
					return 0L;
				}
				return this.guildUserShareData.UserID;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.buttonAgree.onClick.AddListener(new UnityAction(this.OnClickAgreeButton));
			this.userIcon.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonAgree != null)
			{
				this.buttonAgree.onClick.RemoveListener(new UnityAction(this.OnClickAgreeButton));
			}
			this.userIcon.DeInit();
			this.state = UIGuildApplyJoinItem.ApplyJoinState.NotApply;
			this.guildUserShareData = null;
		}

		public void Refresh(GuildUserShareData data)
		{
			if (data == null)
			{
				return;
			}
			if (base.gameObject != null)
			{
				RectTransform rectTransform = base.gameObject.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(0f, rectTransform.anchoredPosition.y);
			}
			if (this.guildUserShareData != null && this.guildUserShareData.UserID != data.UserID)
			{
				this.state = UIGuildApplyJoinItem.ApplyJoinState.NotApply;
			}
			this.guildUserShareData = data;
			this.userIcon.Refresh(data.Avatar, data.AvatarFrame, new Action<object>(this.OnClickUser));
			this.textName.text = data.GetNick();
			this.textCombat.text = DxxTools.FormatNumber((long)data.Power);
			this.RefreshTime();
			this.RefreshResult();
		}

		public void RefreshResult()
		{
		}

		private void RefreshTime()
		{
			GuildUserShareData guildUserShareData = this.guildUserShareData;
		}

		public void PlayItemOut(SequencePool m_seqPool, float delay, bool hide, Action callback)
		{
			if (base.gameObject == null)
			{
				return;
			}
			RectTransform rectTransform = base.gameObject.transform as RectTransform;
			Sequence sequence = m_seqPool.Get();
			if (delay > 0f)
			{
				TweenSettingsExtensions.AppendInterval(sequence, delay);
			}
			TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOAnchorPosX(rectTransform, 2000f, 0.4f, false));
			if (hide)
			{
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					if (this.gameObject != null)
					{
						this.gameObject.SetActive(false);
					}
				});
			}
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Action callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2();
			});
		}

		private void OnClickUser(object obj)
		{
			if (this.guildUserShareData == null)
			{
				return;
			}
			GuildProxy.UI.OpenUserDetailUI(this.guildUserShareData.UserID);
		}

		private void OnClickAgreeButton()
		{
			Action<UIGuildApplyJoinItem> onClickAgree = this.OnClickAgree;
			if (onClickAgree == null)
			{
				return;
			}
			onClickAgree(this);
		}

		private void OnClickDisagreeButton()
		{
			Action<UIGuildApplyJoinItem> onClickDisagree = this.OnClickDisagree;
			if (onClickDisagree == null)
			{
				return;
			}
			onClickDisagree(this);
		}

		public const float PlayOutTime = 0.4f;

		[SerializeField]
		private UIGuildHead userIcon;

		[SerializeField]
		private CustomText textName;

		[SerializeField]
		private CustomButton buttonAgree;

		[SerializeField]
		private CustomText textCombat;

		private UIGuildApplyJoinItem.ApplyJoinState state;

		private GuildUserShareData guildUserShareData;

		public Action<UIGuildApplyJoinItem> OnClickAgree;

		public Action<UIGuildApplyJoinItem> OnClickDisagree;

		public enum ApplyJoinState
		{
			NotApply,
			Agree,
			Disagree
		}
	}
}
