using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildManageMemberItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.headIcon.Init();
			this.positionList.Clear();
			this.president.Position = GuildPositionType.President;
			this.positionList.Add(this.president);
			this.vicePresident.Position = GuildPositionType.VicePresident;
			this.positionList.Add(this.vicePresident);
			this.member.Position = GuildPositionType.Member;
			this.positionList.Add(this.member);
			for (int i = 0; i < this.positionList.Count; i++)
			{
				this.positionList[i].Init();
				this.positionList[i].OnChange = new Action<GuildManageMemberPositionChangeButton, bool>(this.OnSwitchPosition);
			}
			this.kickButton.onClick.AddListener(new UnityAction(this.KickUser));
		}

		protected override void GuildUI_OnUnInit()
		{
			this.headIcon.DeInit();
			for (int i = 0; i < this.positionList.Count; i++)
			{
				this.positionList[i].DeInit();
			}
			this.positionList.Clear();
			this.kickButton.onClick.RemoveListener(new UnityAction(this.KickUser));
		}

		private bool IsSelf()
		{
			return this.memberData.UserID == GuildProxy.GameUser.UserID();
		}

		public void RefreshMember(GuildUserShareData data)
		{
			this.memberData = data;
			this.headIcon.Refresh(data.Avatar, data.AvatarFrame);
			this.headIcon.SetDefaultClick(data.UserID);
			this.textName.text = data.GetNick();
			string infoByID = GuildProxy.Language.GetInfoByID1("400058", data.WeeklyActive);
			this.textDevote.text = string.Format(infoByID, data.WeeklyActive);
			string infoByID2 = GuildProxy.Language.GetInfoByID1("400059", data.Level);
			this.textLevel.text = infoByID2;
			this.memberPosition = this.memberData.GuildPosition;
			this.RefreshMemberPosition();
			bool flag = this.IsSelf();
			this.bgSelf.SetActive(flag);
			this.bgOther.SetActive(!flag);
			this.kickButton.gameObject.SetActive(this.IsHavePermission(GuildPermissionKind.KickMember));
		}

		private void KickUser()
		{
			string infoByID = GuildProxy.Language.GetInfoByID1("400121", this.memberData.GetNick());
			GuildProxy.UI.OpenUIPopCommon(GuildProxy.Language.GetInfoByID("400119"), infoByID, false, true, delegate
			{
				GuildNetUtil.Guild.DoRequest_GuildKickUser(this.memberData.UserID, delegate(bool result, GuildKickOutResponse resp)
				{
					if (result)
					{
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400114"));
					}
				});
			}, null, null);
		}

		private void RefreshMemberPosition()
		{
			for (int i = 0; i < this.positionList.Count; i++)
			{
				GuildManageMemberPositionChangeButton guildManageMemberPositionChangeButton = this.positionList[i];
				guildManageMemberPositionChangeButton.SetSelected(guildManageMemberPositionChangeButton.Position == this.memberPosition);
				guildManageMemberPositionChangeButton.SetIsCanChange(this.CanChangePos(guildManageMemberPositionChangeButton.Position));
			}
		}

		private void OnSwitchPosition(GuildManageMemberPositionChangeButton button, bool sel)
		{
			if (button == null)
			{
				return;
			}
			if (button.Position == this.memberPosition)
			{
				return;
			}
			if (this.TryChangePosition(button.Position))
			{
				this.memberPosition = button.Position;
				this.RefreshMemberPosition();
				return;
			}
			this.OnRevertPositionShow();
		}

		private void OnRevertPositionShow()
		{
			this.memberPosition = this.memberData.GuildPosition;
			this.RefreshMemberPosition();
		}

		private bool TryChangePosition(GuildPositionType positionType)
		{
			if (this.memberData == null)
			{
				return false;
			}
			if (this.memberData.UserID == GuildProxy.GameUser.UserID())
			{
				return false;
			}
			if (positionType == this.memberData.GuildPosition)
			{
				return false;
			}
			switch (positionType)
			{
			case GuildPositionType.President:
			{
				string infoByID = GuildProxy.Language.GetInfoByID1("400120", this.memberData.GetNick());
				GuildProxy.UI.OpenUIPopCommon(GuildProxy.Language.GetInfoByID("400119"), infoByID, false, true, delegate
				{
					GuildNetUtil.Guild.DoRequest_GuildTransfer(this.memberData.UserID, delegate(bool result, GuildTransferPresidentResponse resp)
					{
						if (result)
						{
							GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400109"));
							GuildProxy.UI.CloseUIGuildManageMember();
							return;
						}
						this.RefreshMember(this.memberData);
					});
				}, new Action(this.OnRevertPositionShow), new Action(this.OnRevertPositionShow));
				break;
			}
			case GuildPositionType.VicePresident:
				GuildNetUtil.Guild.DoRequest_ChangePosition(this.memberData.UserID, 2, delegate(bool result, GuildUpPositionResponse resp)
				{
					if (result)
					{
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400110"));
						return;
					}
					this.RefreshMember(this.memberData);
				});
				break;
			case GuildPositionType.Manager:
				GuildNetUtil.Guild.DoRequest_ChangePosition(this.memberData.UserID, 3, delegate(bool result, GuildUpPositionResponse resp)
				{
					if (result)
					{
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400111"));
						return;
					}
					this.RefreshMember(this.memberData);
				});
				break;
			case GuildPositionType.Member:
				GuildNetUtil.Guild.DoRequest_ChangePosition(this.memberData.UserID, 4, delegate(bool result, GuildUpPositionResponse resp)
				{
					if (!result)
					{
						this.RefreshMember(this.memberData);
					}
				});
				break;
			}
			return true;
		}

		private bool CanChangePos(GuildPositionType position)
		{
			if (this.memberData == null)
			{
				return false;
			}
			if (this.memberData.UserID == GuildProxy.GameUser.UserID())
			{
				return false;
			}
			GuildPositionType myGuildPosition = base.SDK.Permission.MyGuildPosition;
			if (myGuildPosition == GuildPositionType.President)
			{
				return true;
			}
			if (this.memberData.GuildPosition < myGuildPosition)
			{
				return false;
			}
			GuildPermissionKind guildPermissionKind = GuildPermissionKind.ChangePosition;
			if (this.memberData.GuildPosition < position)
			{
				GuildPositionType guildPosition = this.memberData.GuildPosition;
				if (guildPosition != GuildPositionType.VicePresident)
				{
					if (guildPosition == GuildPositionType.Manager)
					{
						guildPermissionKind = GuildPermissionKind.ChangeManager;
					}
				}
				else
				{
					guildPermissionKind = GuildPermissionKind.ChangeVicePresident;
				}
			}
			else if (position != GuildPositionType.VicePresident)
			{
				if (position == GuildPositionType.Manager)
				{
					guildPermissionKind = GuildPermissionKind.ChangeManager;
				}
			}
			else
			{
				guildPermissionKind = GuildPermissionKind.ChangeVicePresident;
			}
			return this.IsHavePermission(guildPermissionKind);
		}

		private bool IsHavePermission(GuildPermissionKind kind)
		{
			GuildPermissionDataModule permission = GuildSDKManager.Instance.Permission;
			return permission != null && permission.HasPermission(kind, this.memberData);
		}

		public UIGuildHead headIcon;

		public CustomText textName;

		public CustomText textDevote;

		public CustomText textLevel;

		public GuildManageMemberPositionChangeButton president;

		public GuildManageMemberPositionChangeButton vicePresident;

		public GuildManageMemberPositionChangeButton member;

		public CustomButton kickButton;

		public GameObject bgSelf;

		public GameObject bgOther;

		private List<GuildManageMemberPositionChangeButton> positionList = new List<GuildManageMemberPositionChangeButton>();

		private GuildUserShareData memberData;

		private GuildPositionType memberPosition;
	}
}
