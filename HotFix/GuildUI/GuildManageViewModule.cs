using System;
using Dxx.Guild;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildManageViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.Button_Mask.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.ClickCloseThis));
			this.headIcon.Init();
			this.Button_President.OnSetPosition = new Action<GuildPositionType>(this.ChangeUserPosition);
			this.Button_President.Init();
			this.Button_VicePresident.OnSetPosition = new Action<GuildPositionType>(this.ChangeUserPosition);
			this.Button_VicePresident.Init();
			this.Button_Manager.OnSetPosition = new Action<GuildPositionType>(this.ChangeUserPosition);
			this.Button_Manager.Init();
			this.Button_QuitGuild.onClick.AddListener(new UnityAction(this.KickUser));
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.m_userData = (GuildUserShareData)data;
			this.headIcon.Refresh(this.m_userData.Avatar, this.m_userData.AvatarFrame);
			this.Text_Name.text = this.m_userData.GetNick();
			this.Text_Position.text = this.m_userData.GetPositionLanguage();
			this.RefreshManageButtons();
			this.CalcPanelSize();
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshHallMember);
		}

		protected override void OnViewDelete()
		{
			if (this.Button_Mask != null)
			{
				this.Button_Mask.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			}
			if (this.m_closeBt != null)
			{
				this.m_closeBt.onClick.RemoveListener(new UnityAction(this.ClickCloseThis));
			}
			if (this.Button_QuitGuild != null)
			{
				this.Button_QuitGuild.onClick.RemoveListener(new UnityAction(this.KickUser));
			}
			base.OnViewDelete();
			this.headIcon.DeInit();
			this.Button_President.DeInit();
			this.Button_VicePresident.DeInit();
			this.Button_Manager.DeInit();
		}

		private void KickUser()
		{
			string infoByID = GuildProxy.Language.GetInfoByID1("400121", this.m_userData.GetNick());
			GuildProxy.UI.OpenUIPopCommon(GuildProxy.Language.GetInfoByID("400119"), infoByID, false, true, delegate
			{
				GuildNetUtil.Guild.DoRequest_GuildKickUser(this.m_userData.UserID, delegate(bool result, GuildKickOutResponse resp)
				{
					if (result)
					{
						this.ClickCloseThis();
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400114"));
					}
				});
			}, null, null);
		}

		private bool isSelf()
		{
			return this.m_userData.UserID == GuildProxy.GameUser.UserID();
		}

		private void SetManageButtonsShow(bool active)
		{
			this.Obj_PositionButtons.SetActive(active);
		}

		private bool isHavePermission(GuildPermissionKind kind)
		{
			GuildPermissionDataModule permission = GuildSDKManager.Instance.Permission;
			return permission != null && permission.HasPermission(kind, this.m_userData);
		}

		private void ChangeUserPosition(GuildPositionType position)
		{
			switch (position)
			{
			case GuildPositionType.President:
			{
				string infoByID = GuildProxy.Language.GetInfoByID1("400120", this.m_userData.GetNick());
				GuildProxy.UI.OpenUIPopCommon(GuildProxy.Language.GetInfoByID("400119"), infoByID, false, true, delegate
				{
					GuildNetUtil.Guild.DoRequest_GuildTransfer(this.m_userData.UserID, delegate(bool result, GuildTransferPresidentResponse resp)
					{
						if (result)
						{
							this.ClickCloseThis();
							GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400109"));
						}
					});
				}, null, null);
				return;
			}
			case GuildPositionType.VicePresident:
				if (this.m_userData.GuildPosition == GuildPositionType.VicePresident)
				{
					GuildNetUtil.Guild.DoRequest_ChangePosition(this.m_userData.UserID, 4, delegate(bool result, GuildUpPositionResponse resp)
					{
						if (result)
						{
							this.ClickCloseThis();
							GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400112"));
						}
					});
					return;
				}
				GuildNetUtil.Guild.DoRequest_ChangePosition(this.m_userData.UserID, 2, delegate(bool result, GuildUpPositionResponse resp)
				{
					if (result)
					{
						this.ClickCloseThis();
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400110"));
					}
				});
				return;
			case GuildPositionType.Manager:
				if (this.m_userData.GuildPosition == GuildPositionType.Manager)
				{
					GuildNetUtil.Guild.DoRequest_ChangePosition(this.m_userData.UserID, 4, delegate(bool result, GuildUpPositionResponse resp)
					{
						if (result)
						{
							this.ClickCloseThis();
							GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400113"));
						}
					});
					return;
				}
				GuildNetUtil.Guild.DoRequest_ChangePosition(this.m_userData.UserID, 3, delegate(bool result, GuildUpPositionResponse resp)
				{
					if (result)
					{
						this.ClickCloseThis();
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400111"));
					}
				});
				break;
			case GuildPositionType.Member:
				break;
			default:
				return;
			}
		}

		private void RefreshManageButtons()
		{
			if (this.isSelf())
			{
				this.SetManageButtonsShow(false);
				return;
			}
			this.ButtonCount = 0;
			if (base.SDK.Permission.MyGuildPosition == GuildPositionType.President)
			{
				this.Button_President.SetPosition(this.m_userData.GuildPosition, GuildPositionType.President);
				this.Button_President.SetActive(true);
				this.ButtonCount++;
			}
			else
			{
				this.Button_President.SetActive(false);
			}
			if (this.isHavePermission(GuildPermissionKind.ChangeVicePresident))
			{
				this.Button_VicePresident.SetPosition(this.m_userData.GuildPosition, GuildPositionType.VicePresident);
				this.Button_VicePresident.SetActive(true);
				this.ButtonCount++;
			}
			else
			{
				this.Button_VicePresident.SetActive(false);
			}
			if (this.isHavePermission(GuildPermissionKind.ChangeManager))
			{
				this.Button_Manager.SetPosition(this.m_userData.GuildPosition, GuildPositionType.Manager);
				this.Button_Manager.SetActive(true);
				this.ButtonCount++;
			}
			else
			{
				this.Button_Manager.SetActive(false);
			}
			if (this.isHavePermission(GuildPermissionKind.KickMember))
			{
				this.Button_QuitGuild.gameObject.SetActive(true);
				this.ButtonCount++;
			}
			else
			{
				this.Button_QuitGuild.gameObject.SetActive(false);
			}
			this.SetManageButtonsShow(this.ButtonCount > 0);
		}

		private void CalcPanelSize()
		{
			float num = 0f;
			num += 200f;
			num += 200f;
			if (this.Obj_PositionButtons.activeSelf)
			{
				num += (float)(((this.ButtonCount - 1) / 2 + 1) * 180);
			}
			Vector2 sizeDelta = this.RTFPanel.sizeDelta;
			sizeDelta.y = num;
			this.RTFPanel.sizeDelta = sizeDelta;
		}

		private void ClickCloseThis()
		{
			GuildProxy.UI.CloseUIGuildManage();
		}

		public UIGuildHead headIcon;

		public CustomText Text_Name;

		public CustomText Text_Position;

		public RectTransform RTFPanel;

		public CustomButton Button_Mask;

		public CustomButton m_closeBt;

		public GameObject Obj_PositionButtons;

		[Label]
		public int ButtonCount;

		public GuildPositionChangeButton Button_President;

		public GuildPositionChangeButton Button_VicePresident;

		public GuildPositionChangeButton Button_Manager;

		public CustomButton Button_QuitGuild;

		private GuildUserShareData m_userData;
	}
}
