using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildBossKillRewardItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			if (this.animator != null)
			{
				this.animator.enabled = false;
			}
			this.roleInitScale = this.modelCtrlObj.transform.localScale;
			this.iconInitScale = this.imageIcon.transform.localScale;
			this.iconBgInitScale = this.iconBg.transform.localScale;
			this.buttonBox.onClick.AddListener(new UnityAction(this.OnClickBox));
			this.modelCtrl = new UIGuildModel();
			this.modelCtrl.SetGameObject(this.modelCtrlObj);
			this.modelCtrl.OnInitUI();
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonBox != null)
			{
				this.buttonBox.onClick.AddListener(new UnityAction(this.OnClickBox));
			}
			this.modelCtrl.OnUnInitUI();
		}

		public void Refresh(GuildBossKillBox killBox)
		{
			this.killBoxData = killBox;
			this.isCurrent = false;
			if (base.SDK.GuildActivity.GuildBoss.GetCurrentBox().Equals(killBox))
			{
				this.isCurrent = true;
			}
			bool flag = base.SDK.GuildActivity.GuildBoss.IsLastBox(killBox);
			this.pointObj.SetActiveSafe(!flag);
			this.textState.text = this.GetStateString(killBox.boxState);
			this.iconBg.SetActiveSafe(killBox.boxState == GuildBossKillBox.GuildBossKillBoxState.CanGetReward);
			if (this.isCurrent)
			{
				this.modelCtrlObj.transform.localScale = this.roleInitScale;
				this.imageIcon.transform.localScale = this.iconInitScale;
				this.iconBg.transform.localScale = this.iconInitScale;
			}
			else
			{
				this.modelCtrlObj.transform.localScale = this.roleInitScale * 0.6f;
				this.imageIcon.transform.localScale = this.iconInitScale * 0.6f;
				this.iconBg.transform.localScale = this.iconInitScale * 0.6f;
			}
			GuildBOSS_guildBossBox guildBossBoxTable = GuildProxy.Table.GetGuildBossBoxTable(killBox.BoxID);
			if (guildBossBoxTable == null)
			{
				HLog.LogError(string.Format("TableGuildBOSS_guildBossBox not found id={0}", killBox.BoxID));
				return;
			}
			string text = guildBossBoxTable.ShowBoxIconIdle;
			if (killBox.boxState == GuildBossKillBox.GuildBossKillBoxState.AllFinish)
			{
				text = guildBossBoxTable.ShowBoxIconOpen;
			}
			GuildProxy.Resources.SetDxxImage(this.imageIcon, GuildProxy.Resources.GetGuildAtlasID(), text);
			if (GuildProxy.Table.GetGuildBossTable(guildBossBoxTable.BossId) == null)
			{
				HLog.LogError(string.Format("TableGuildBOSS_guildBoss not found id={0}", guildBossBoxTable.BossId));
				return;
			}
			string guildBossNameID = GuildProxy.Table.GetGuildBossNameID(guildBossBoxTable.BossId);
			int guildBossModelID = GuildProxy.Table.GetGuildBossModelID(guildBossBoxTable.BossId);
			this.textBossName.text = (this.isCurrent ? GuildProxy.Language.GetInfoByID(guildBossNameID) : "");
			this.modelCtrl.Refresh(guildBossModelID, GuildProxy.AnimationType.IDLE);
		}

		public void ActivePoints(bool isActive)
		{
			this.pointObj.SetActiveSafe(isActive);
		}

		private string GetStateString(GuildBossKillBox.GuildBossKillBoxState state)
		{
			switch (state)
			{
			case GuildBossKillBox.GuildBossKillBoxState.Undone:
				return GuildProxy.Language.GetInfoByID("400254");
			case GuildBossKillBox.GuildBossKillBoxState.CanGetReward:
				return GuildProxy.Language.GetInfoByID("400253");
			case GuildBossKillBox.GuildBossKillBoxState.AllFinish:
				return GuildProxy.Language.GetInfoByID("400252");
			default:
				return "";
			}
		}

		public void ResetAni()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = false;
		}

		public void ShowAni()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.enabled = true;
		}

		private void OnClickBox()
		{
			if (this.isCurrent && this.killBoxData.boxState == GuildBossKillBox.GuildBossKillBoxState.CanGetReward)
			{
				GuildNetUtil.Guild.DoRequest_GetGuildBossBoxReward(this.killBoxData.BoxID, delegate(bool result, GuildBossBoxRewardResponse resp)
				{
					if (resp != null && resp.CommonData != null)
					{
						GuildProxy.UI.OpenUICommonReward(resp.CommonData.Reward, null);
					}
				});
			}
		}

		public GameObject pointObj;

		public CustomText textState;

		public GameObject modelCtrlObj;

		public CustomImage imageIcon;

		public Animator animator;

		public CustomText textBossName;

		public GameObject iconBg;

		public CustomButton buttonBox;

		private Vector3 roleInitScale;

		private Vector3 iconInitScale;

		private Vector3 iconBgInitScale;

		private UIGuildModel modelCtrl;

		private GuildBossKillBox killBoxData;

		private bool isCurrent;
	}
}
