using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildHall_Notice : UIGuildHall_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.buttonEdit.onClick.AddListener(new UnityAction(this.OnOpenChangeView));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonEdit != null)
			{
				this.buttonEdit.onClick.RemoveListener(new UnityAction(this.OnOpenChangeView));
			}
			base.GuildUI_OnUnInit();
		}

		public override void OnRefreshUI()
		{
			GuildShareData guildData = GuildSDKManager.Instance.GuildInfo.GuildData;
			if (guildData == null)
			{
				return;
			}
			this.textNotic.text = guildData.GuildNotice;
			this.textNoticTip.text = (string.IsNullOrEmpty(guildData.GuildNotice) ? GuildProxy.Language.GetInfoByID("400217") : "");
			this.buttonEdit.gameObject.SetActive(base.SDK.Permission.HasPermission(GuildPermissionKind.ChangeGuildInfo, null));
		}

		private void OnOpenChangeView()
		{
			GuildProxy.UI.OpenUIGuildAnnouncementModify(null);
		}

		private CustomText textNotic;

		private CustomButton buttonEdit;

		private CustomText textNoticTip;
	}
}
