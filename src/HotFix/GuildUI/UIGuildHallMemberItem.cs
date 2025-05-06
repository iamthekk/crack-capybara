using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildHallMemberItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.aniRt = this.canvasGroup.GetComponent<RectTransform>();
			this.textLastLoginOutline = this.textLastLogin.GetComponent<CustomOutLine>();
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
			this.headIcon.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			this.headIcon.DeInit();
		}

		public void Refresh(GuildUserShareData data)
		{
			this.m_memberData = data;
			if (data == null)
			{
				return;
			}
			this.headIcon.Refresh(data.Avatar, data.AvatarFrame, new Action<object>(this.OnClickUser));
			this.textName.text = data.GetNick();
			this.textDevote.text = DxxTools.FormatNumber((long)data.WeeklyActive);
			this.textPosition.text = data.GetPositionLanguage();
			GuildProxy.Language.GetInfoByID1("400059", data.Level);
			this.Text_Power.text = DxxTools.FormatNumber((long)data.Power);
			if (data.GuildPosition == GuildPositionType.President)
			{
				this.Image_Position.sprite = this.PositionSprite_President;
			}
			else if (data.GuildPosition == GuildPositionType.VicePresident || data.GuildPosition == GuildPositionType.Manager)
			{
				this.Image_Position.sprite = this.PositionSprite_VicePresident;
			}
			else
			{
				this.Image_Position.sprite = this.PositionSprite_Member;
			}
			this.RefreshTime();
		}

		private void RefreshTime()
		{
			if (this.m_memberData == null)
			{
				return;
			}
			long num = GuildProxy.Net.ServerTime() - (long)this.m_memberData.LastOnlineTime;
			long num2 = (long)GuildProxy.UI.OfflineSec();
			if (num < num2 || this.m_memberData.UserID == base.SDK.User.UserID)
			{
				this.textLastLogin.text = GuildProxy.Language.GetInfoByID("guild_memberOnline");
				this.textLastLoginOutline.enabled = true;
				return;
			}
			this.textLastLogin.text = Singleton<LanguageManager>.Instance.GetInfoByID("guild_memberOffLineTime", new object[] { Singleton<LanguageManager>.Instance.GetTime(num) });
			this.textLastLoginOutline.enabled = false;
		}

		private void OnClickUser()
		{
			if (this.m_memberData == null)
			{
				return;
			}
			GuildProxy.UI.OpenUserDetailUI(this.m_memberData.UserID);
		}

		private void OnClickUser(object obj)
		{
			this.OnClickUser();
		}

		public void ClearAni()
		{
			this.canvasGroup.alpha = 1f;
			this.aniRt.anchoredPosition = Vector2.zero;
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
		}

		public void ShowAni(int index, bool isWait)
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = false;
			this.canvasGroup.alpha = 0f;
			if (isWait)
			{
				this.OnShowAni();
				return;
			}
			DelayCall.Instance.CallOnce(index * 50, new DelayCall.CallAction(this.OnShowAni));
		}

		private void OnShowAni()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = true;
			this.animator.Play("UIGuildHallMemberItem_Show");
		}

		[SerializeField]
		private UIGuildHead headIcon;

		[SerializeField]
		private CustomText textName;

		[SerializeField]
		private CustomText textDevote;

		[SerializeField]
		private CustomText textPosition;

		[SerializeField]
		private CustomText textLastLogin;

		private CustomOutLine textLastLoginOutline;

		[SerializeField]
		private CustomButton buttonBorder;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private CustomText Text_Power;

		public Sprite PositionSprite_President;

		public Sprite PositionSprite_VicePresident;

		public Sprite PositionSprite_Member;

		public CustomImage Image_Position;

		private GuildUserShareData m_memberData;

		private RectTransform aniRt;
	}
}
