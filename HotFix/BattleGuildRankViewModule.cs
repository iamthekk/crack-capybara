using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class BattleGuildRankViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
		}

		public override void OnOpen(object data)
		{
			this.m_battleGuildRankDataModule = GameApp.Data.GetDataModule(DataName.BattleGuildRankDataModule);
			if (this.m_leftAvatar != null)
			{
				this.m_leftAvatar.Init();
				this.m_leftAvatar.SetEnableButton(false);
				this.m_leftAvatar.RefreshData(this.m_battleGuildRankDataModule.Record.OwnerUser.Avatar, this.m_battleGuildRankDataModule.Record.OwnerUser.AvatarFrame);
			}
			if (this.m_leftNickName != null)
			{
				this.m_leftNickName.text = (string.IsNullOrEmpty(this.m_battleGuildRankDataModule.Record.OwnerUser.NickName) ? DxxTools.GetDefaultNick(this.m_battleGuildRankDataModule.Record.OwnerUser.UserId) : this.m_battleGuildRankDataModule.Record.OwnerUser.NickName);
			}
			if (this.m_leftGuildName != null)
			{
				this.m_leftGuildName.text = this.m_battleGuildRankDataModule.GetGuildName(this.m_battleGuildRankDataModule.Record.OwnerUser);
			}
			if (this.m_rightAvatar != null)
			{
				this.m_rightAvatar.Init();
				this.m_rightAvatar.SetEnableButton(false);
				this.m_rightAvatar.RefreshData(this.m_battleGuildRankDataModule.Record.OtherUser.Avatar, this.m_battleGuildRankDataModule.Record.OtherUser.AvatarFrame);
			}
			if (this.m_rightNickName != null)
			{
				this.m_rightNickName.text = (string.IsNullOrEmpty(this.m_battleGuildRankDataModule.Record.OtherUser.NickName) ? DxxTools.GetDefaultNick(this.m_battleGuildRankDataModule.Record.OtherUser.UserId) : this.m_battleGuildRankDataModule.Record.OtherUser.NickName);
			}
			if (this.m_rightGuildName != null)
			{
				this.m_rightGuildName.text = this.m_battleGuildRankDataModule.GetGuildName(this.m_battleGuildRankDataModule.Record.OtherUser);
			}
			this.m_BtnJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
			this.m_BtnSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvE);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			if (this.m_leftAvatar != null)
			{
				this.m_leftAvatar.DeInit();
			}
			if (this.m_rightAvatar != null)
			{
				this.m_rightAvatar.DeInit();
			}
			this.m_BtnJump.onClick.RemoveListener(new UnityAction(this.OnClickBtnJump));
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		[SerializeField]
		private CustomButton m_BtnJump;

		[SerializeField]
		private UISpeedButtonCtrl m_BtnSpeedUp;

		public RectTransform m_leftAvatarGroup;

		public UIAvatarCtrl m_leftAvatar;

		public CustomText m_leftNickName;

		public CustomText m_leftGuildName;

		public RectTransform m_rightAvatarGroup;

		public UIAvatarCtrl m_rightAvatar;

		public CustomText m_rightNickName;

		public CustomText m_rightGuildName;

		private BattleGuildRankDataModule m_battleGuildRankDataModule;
	}
}
