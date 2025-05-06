using System;
using Dxx.Guild;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_Info : GuildInfoPopUI_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.GuildIcon.Init();
			this.Button_Settings.onClick.AddListener(new UnityAction(this.OnOpenSettingView));
			this.announceEmptyObj.SetActive(false);
			this.defaultAnnounceTextFontSize = this.announceText.fontSize;
			this.Button_AnnounceSettings.onClick.AddListener(new UnityAction(this.OnOpenAnnouncementModifyView));
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_RefreshHall, new HandlerEvent(this.OnRefreshEvent));
			base.SDK.Event.RegisterEvent(15, new GuildHandlerEvent(this.OnRefreshEvent));
			this.Button_SetIcon.onClick.AddListener(new UnityAction(this.OnClickSetIcon));
		}

		private void OnRefreshEvent(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshUI();
		}

		private void OnRefreshEvent(int type, GuildBaseEvent eventargs)
		{
			this.RefreshUI();
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			if (this.Button_Settings != null)
			{
				this.Button_Settings.onClick.RemoveListener(new UnityAction(this.OnOpenSettingView));
			}
			if (this.Button_AnnounceSettings != null)
			{
				this.Button_AnnounceSettings.onClick.RemoveListener(new UnityAction(this.OnOpenAnnouncementModifyView));
			}
			this.GuildIcon.DeInit();
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_RefreshHall, new HandlerEvent(this.OnRefreshEvent));
			base.SDK.Event.UnRegisterEvent(15, new GuildHandlerEvent(this.OnRefreshEvent));
			this.Button_SetIcon.onClick.RemoveListener(new UnityAction(this.OnClickSetIcon));
		}

		public override void RefreshUI()
		{
			base.RefreshUI();
			if (!base.SDK.GuildInfo.HasGuild)
			{
				return;
			}
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			this.GuildIcon.SetIcon(guildData.GuildIcon);
			this.Text_Name.text = guildData.GuildShowName;
			guildData.GetPresidentNick();
			this.Text_PresidentName.text = guildData.GuildID;
			bool flag = base.SDK.Permission.HasPermission(GuildPermissionKind.ChangeGuildInfo, null);
			this.Button_Settings.gameObject.SetActiveSafe(flag);
			this.Button_SetIcon.gameObject.SetActive(flag);
			this.Text_MemberCount.text = string.Format("{0}/{1}", guildData.GuildMemberCount, guildData.GuildMemberMaxCount);
			this.announceText.text = guildData.GuildNotice;
			DxxTools.UI.AutoChangeTextSize(this.announceText, this.defaultAnnounceTextFontSize);
			this.announceEmptyObj.SetActive(string.IsNullOrEmpty(guildData.GuildNotice));
			this.Button_AnnounceSettings.gameObject.SetActive(flag);
			this.Text_Power.text = DxxTools.FormatNumber(guildData.GuildPower);
			if (GuildSDKManager.Instance.GuildInfo.DayContributeTimes >= GuildSDKManager.Instance.GuildInfo.GuildContributeConfigs.Count)
			{
				this.DonationMask.gameObject.SetActiveSafe(true);
			}
			else
			{
				this.DonationMask.gameObject.SetActiveSafe(false);
			}
			this.Text_Level.text = "Lv." + guildData.GuildLevel.ToString();
		}

		private void OnOpenSettingView()
		{
			this.RenameUI.gameObject.SetActiveSafe(true);
			this.RenameUI.Init();
		}

		private void OnOpenAnnouncementModifyView()
		{
			GuildProxy.UI.OpenUIGuildAnnouncementModify(null);
		}

		private void OnClickSetIcon()
		{
			GuildProxy.UI.OpenUIGuildIconSet(new GuildIconSetData
			{
				defaultIconId = GuildSDKManager.Instance.GuildInfo.GuildData.GuildIcon
			});
		}

		[SerializeField]
		private UIGuildIcon GuildIcon;

		[SerializeField]
		private CustomText Text_Name;

		[SerializeField]
		private CustomText Text_PresidentName;

		[SerializeField]
		private CustomButton Button_Settings;

		[SerializeField]
		private CustomText Text_MemberCount;

		[SerializeField]
		private CustomButton Button_AnnounceSettings;

		[SerializeField]
		private CustomText announceText;

		[SerializeField]
		private GameObject announceEmptyObj;

		[SerializeField]
		private CustomText Text_Power;

		[SerializeField]
		private GuildInfoPopUI_Rename RenameUI;

		[SerializeField]
		private CustomButton Button_SetIcon;

		[SerializeField]
		private GameObject DonationMask;

		[SerializeField]
		private CustomText Text_Level;

		private int defaultAnnounceTextFontSize = 1;
	}
}
