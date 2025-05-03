using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_UserSetPop : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.PositionPresident.Init();
			this.PositionPresident.OnClick = new Action<GuildInfoPopUI_UserSetPowerItem>(this.OnPositionChange);
			this.PositionVicePresident.Init();
			this.PositionVicePresident.OnClick = new Action<GuildInfoPopUI_UserSetPowerItem>(this.OnPositionChange);
			this.PositionMember.Init();
			this.PositionMember.OnClick = new Action<GuildInfoPopUI_UserSetPowerItem>(this.OnPositionChange);
			this.ButtonFullClick.onClick.AddListener(new UnityAction(this.OnClickFullClick));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.PositionPresident.DeInit();
			this.PositionVicePresident.DeInit();
			this.PositionMember.DeInit();
			this.ButtonFullClick.onClick.RemoveListener(new UnityAction(this.OnTickUser));
		}

		private void OnPositionChange(GuildInfoPopUI_UserSetPowerItem powerset)
		{
			if (powerset == null)
			{
				return;
			}
			GuildPositionType guildPositionType = ((powerset == this.PositionPresident) ? GuildPositionType.President : ((powerset == this.PositionVicePresident) ? GuildPositionType.VicePresident : GuildPositionType.Member));
			Action<GuildUserShareData, GuildPositionType> onChangePosition = this.OnChangePosition;
			if (onChangePosition == null)
			{
				return;
			}
			onChangePosition(this.mUserData, guildPositionType);
		}

		private void OnTickUser()
		{
			Action<GuildUserShareData> onKickUser = this.OnKickUser;
			if (onKickUser == null)
			{
				return;
			}
			onKickUser(this.mUserData);
		}

		private void OnClickFullClick()
		{
			this.PlayHide();
		}

		public void SetData(GuildUserShareData userdata)
		{
			this.mUserData = userdata;
		}

		public void RefreshUI()
		{
			if (this.mUserData == null)
			{
				return;
			}
			this.PositionPresident.Sel(this.mUserData.GuildPosition == GuildPositionType.President);
			this.PositionVicePresident.Sel(this.mUserData.GuildPosition == GuildPositionType.VicePresident);
			this.PositionMember.Sel(this.mUserData.GuildPosition == GuildPositionType.Member);
			bool flag = this.mUserData.UserID == base.SDK.User.MyUserData.UserID;
			GuildPermissionDataModule permission = base.SDK.Permission;
			this.PositionPresident.SetGray(permission.MyGuildPosition != GuildPositionType.President || this.mUserData.GuildPosition == GuildPositionType.President || flag);
			int positionMaxCount = base.SDK.GuildInfo.GuildData.GetPositionMaxCount(GuildPositionType.VicePresident);
			bool flag2 = base.SDK.GuildInfo.GetMemberCountByPosition(GuildPositionType.VicePresident) >= positionMaxCount;
			this.PositionVicePresident.SetGray(!permission.HasPermission(GuildPermissionKind.ChangeVicePresident, this.mUserData) || this.mUserData.GuildPosition == GuildPositionType.VicePresident || flag || flag2);
			this.PositionMember.SetGray(!permission.HasPermission(GuildPermissionKind.ChangePosition, this.mUserData) || this.mUserData.GuildPosition == GuildPositionType.Member || flag);
		}

		public void PlayShow()
		{
			base.SetActive(true);
		}

		public void PlayHide()
		{
			this.m_seqPool.Clear(false);
			base.SetActive(false);
		}

		[Header("职位设置")]
		public GuildInfoPopUI_UserSetPowerItem PositionPresident;

		public GuildInfoPopUI_UserSetPowerItem PositionVicePresident;

		public GuildInfoPopUI_UserSetPowerItem PositionMember;

		[Header("其他")]
		public RectTransform RTFFollowRoot;

		public CustomButton ButtonFullClick;

		public Action<GuildUserShareData, GuildPositionType> OnChangePosition;

		public Action<GuildUserShareData> OnKickUser;

		private GuildUserShareData mUserData;

		private SequencePool m_seqPool = new SequencePool();
	}
}
