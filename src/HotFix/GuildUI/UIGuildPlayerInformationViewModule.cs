using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildPlayerInformationViewModule : PlayerInformationViewModule
	{
		public override void OnCreate(object data)
		{
			this._sdk = GuildSDKManager.Instance;
			this._dataModule = this._sdk.GuildInfo;
			base.OnCreate(data);
			this.Btn_ChangePosition.onClick.AddListener(new UnityAction(this.OnClickBtnChangePosition));
			this.Btn_KickOut.onClick.AddListener(new UnityAction(this.OnClickKickUser));
		}

		private void OnClickBtnChangePosition()
		{
			this.memberPositionSet.RefreshUI();
			this.memberPositionSet.PlayShow();
		}

		private void OnChangeUserPosition(GuildUserShareData user, GuildPositionType pos)
		{
			this.memberPositionSet.PlayHide();
			if (pos == GuildPositionType.President)
			{
				Action<bool, GuildTransferPresidentResponse> <>9__1;
				DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("guild_transferPresident"), delegate(int yes)
				{
					if (yes == 1)
					{
						long userID = user.UserID;
						Action<bool, GuildTransferPresidentResponse> action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate(bool result, GuildTransferPresidentResponse resp)
							{
								this._sdk.Event.DispatchNow(8, null);
								this.memberPositionSet.RefreshUI();
								this.OnRefresh();
							});
						}
						GuildNetUtil.Guild.DoRequest_GuildTransfer(userID, action);
					}
				});
				return;
			}
			Action<bool, GuildUpPositionResponse> <>9__3;
			DxxTools.UI.OpenPopCommon((pos == GuildPositionType.Member) ? Singleton<LanguageManager>.Instance.GetInfoByID("guild_setPosition_member") : Singleton<LanguageManager>.Instance.GetInfoByID("guild_setPosition_vicePresident"), delegate(int yes)
			{
				if (yes == 1)
				{
					long userID2 = user.UserID;
					int pos2 = (int)pos;
					Action<bool, GuildUpPositionResponse> action2;
					if ((action2 = <>9__3) == null)
					{
						action2 = (<>9__3 = delegate(bool result, GuildUpPositionResponse resp)
						{
							if (resp.Code == 0)
							{
								this._sdk.Event.DispatchNow(8, null);
								this.memberPositionSet.RefreshUI();
								this.OnRefresh();
							}
						});
					}
					GuildNetUtil.Guild.DoRequest_ChangePosition(userID2, pos2, action2);
				}
			});
		}

		private void OnClickKickUser()
		{
			this.memberPositionSet.PlayHide();
			DxxTools.UI.OpenPopCommon(Singleton<LanguageManager>.Instance.GetInfoByID("guild_kickout"), delegate(int result)
			{
				if (result == 1)
				{
					GuildNetUtil.Guild.DoRequest_GuildKickUser(this._memberData.UserID, delegate(bool result, GuildKickOutResponse resp)
					{
						if (result)
						{
							this.OnClickCloseBt();
						}
					});
				}
			});
		}

		protected override void OnClickCloseBt()
		{
			GameApp.View.CloseView(ViewName.GuildPlayerInformationViewModule, null);
		}

		protected override bool IsSelfOpened()
		{
			return GameApp.View.IsOpened(ViewName.GuildPlayerInformationViewModule);
		}

		public override void OnOpen(object data)
		{
			base.OnOpen(data);
			this.m_buttons.gameObject.SetActive(false);
			this._isMember = true;
			this._memberData = this._dataModule.GetMemberShareData(this.m_openData.userId);
			this._guildShareDetailData = this._dataModule.GuildDetailData;
			if (this._memberData == null)
			{
				this._isMember = false;
				this._memberData = GuildSDKManager.Instance.GuildInfo.GetPlayer2JoinData(this.m_openData.userId);
				this._guildShareDetailData = null;
				this.PositionObj.gameObject.SetActiveSafe(false);
			}
			if (this._memberData == null)
			{
				this._isMember = false;
				this._memberData = GuildSDKManager.Instance.GuildList.GetGuildMemberData(this.m_openData.userId, out this._guildShareDetailData);
			}
			this.memberPositionSet.Init();
			this.memberPositionSet.PlayHide();
			this.memberPositionSet.OnChangePosition = new Action<GuildUserShareData, GuildPositionType>(this.OnChangeUserPosition);
			this.memberPositionSet.SetData(this._memberData);
			if (this._isMember)
			{
				bool flag = false;
				if (this._sdk.Permission.MyGuildPosition != GuildPositionType.Member && this._memberData.UserID != this._sdk.User.MyUserData.UserID)
				{
					flag = true;
					this.Btn_ChangePosition.gameObject.SetActiveSafe(true);
				}
				else
				{
					this.Btn_ChangePosition.gameObject.SetActiveSafe(false);
				}
				if (this._sdk.Permission.HasPermission(GuildPermissionKind.KickMember, this._memberData))
				{
					flag = true;
					this.Btn_KickOut.gameObject.SetActiveSafe(true);
				}
				else
				{
					this.Btn_KickOut.gameObject.SetActiveSafe(false);
				}
				if (flag)
				{
					this.m_buttons.gameObject.SetActiveSafe(true);
				}
			}
			if (!this._isMember)
			{
				this.m_buttons.gameObject.SetActiveSafe(false);
			}
			this.Text_Position.text = this._memberData.GetPositionLanguage();
			if (this._memberData != null)
			{
				if (this._memberData.GuildPosition == GuildPositionType.President)
				{
					this.Image_Position.sprite = this.PositionSprite_President;
					return;
				}
				if (this._memberData.GuildPosition == GuildPositionType.VicePresident || this._memberData.GuildPosition == GuildPositionType.Manager)
				{
					this.Image_Position.sprite = this.PositionSprite_VicePresident;
					return;
				}
				this.Image_Position.sprite = this.PositionSprite_Member;
			}
		}

		private void OnRefresh()
		{
			this._memberData = this._dataModule.GetMemberShareData(this.m_openData.userId);
			this.Text_Position.text = this._memberData.GetPositionLanguage();
			if (this._memberData != null)
			{
				if (this._memberData.GuildPosition == GuildPositionType.President)
				{
					this.Image_Position.sprite = this.PositionSprite_President;
					return;
				}
				if (this._memberData.GuildPosition == GuildPositionType.VicePresident || this._memberData.GuildPosition == GuildPositionType.Manager)
				{
					this.Image_Position.sprite = this.PositionSprite_VicePresident;
					return;
				}
				this.Image_Position.sprite = this.PositionSprite_Member;
			}
		}

		public override void OnDelete()
		{
			base.OnDelete();
			this.Btn_ChangePosition.onClick.RemoveListener(new UnityAction(this.OnClickBtnChangePosition));
			this.Btn_KickOut.onClick.RemoveListener(new UnityAction(this.OnClickKickUser));
		}

		public override string GetCameraKey()
		{
			return "UIGuildPlayerInformationViewModule";
		}

		[SerializeField]
		private CustomButton Btn_ChangePosition;

		[SerializeField]
		private GuildInfoPopUI_UserSetPop memberPositionSet;

		[SerializeField]
		private CustomButton Btn_KickOut;

		[SerializeField]
		private CustomText Text_Position;

		[SerializeField]
		private GameObject PositionObj;

		public Sprite PositionSprite_President;

		public Sprite PositionSprite_VicePresident;

		public Sprite PositionSprite_Member;

		public CustomImage Image_Position;

		private GuildInfoDataModule _dataModule;

		private GuildUserShareData _memberData;

		private GuildShareDetailData _guildShareDetailData;

		private GuildSDKManager _sdk;

		private bool _isMember;
	}
}
