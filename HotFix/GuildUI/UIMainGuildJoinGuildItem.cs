using System;
using Dxx.Guild;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIMainGuildJoinGuildItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.buttonCheck.onClick.AddListener(new UnityAction(this.CheckGuild));
			this.buttonApply.onClick.AddListener(new UnityAction(this.ApplyJoin));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonCheck != null)
			{
				this.buttonCheck.onClick.AddListener(new UnityAction(this.CheckGuild));
			}
			if (this.buttonApply != null)
			{
				this.buttonApply.onClick.AddListener(new UnityAction(this.ApplyJoin));
			}
		}

		public void Refresh(GuildShareData data)
		{
			if (data == null)
			{
				return;
			}
			this.guildShareData = data;
			this.iconCtrl.SetIcon(data.GuildIcon);
			this.textName.text = data.GuildShowName;
			this.textMembers.text = GuildProxy.Language.GetInfoByID2("400029", data.GuildMemberCount, data.GuildMemberMaxCount);
			this.textLevel.SetText(400150, data.GuildLevel.ToString());
			this.RefreshState();
		}

		private void RefreshState()
		{
			if (this.guildShareData.IsApply)
			{
				this.textState.text = GuildProxy.Language.GetInfoByID("400165");
				this.buttonApply.gameObject.SetActiveSafe(false);
				return;
			}
			if (this.guildShareData.GuildMemberCount >= this.guildShareData.GuildMemberMaxCount)
			{
				this.textState.text = GuildProxy.Language.GetInfoByID("400163");
				this.buttonApply.gameObject.SetActiveSafe(false);
				return;
			}
			if ((long)GuildProxy.GameUser.MyLevel() < this.guildShareData.LevelNeed)
			{
				this.textState.text = GuildProxy.Language.GetInfoByID("400224");
				this.buttonApply.gameObject.SetActiveSafe(false);
				return;
			}
			this.textState.text = "";
			this.buttonApply.gameObject.SetActiveSafe(true);
		}

		public void SwitchToAlreadyApply()
		{
			if (this.textState != null)
			{
				this.textState.text = GuildProxy.Language.GetInfoByID("400165");
			}
			if (this.buttonApply != null)
			{
				this.buttonApply.gameObject.SetActiveSafe(false);
			}
		}

		private void CheckGuild()
		{
			if (this.guildShareData == null)
			{
				return;
			}
			GuildProxy.UI.OpenUIGuildCheckPop(this.guildShareData.GuildID, 0UL);
		}

		private void ApplyJoin()
		{
			if (!GuildSDKManager.Instance.CheckQuitGuildTime())
			{
				return;
			}
			GuildNetUtil.Guild.DoRequest_RequireJoinGuild(this.guildShareData.GuildID, GuildSDKManager.Instance.User.UserLanguage, delegate(bool result, int joinResult, GuildApplyJoinResponse response)
			{
				if (result)
				{
					string infoByID = GuildProxy.Language.GetInfoByID("400119");
					switch (joinResult)
					{
					case 0:
					{
						string infoByID2 = GuildProxy.Language.GetInfoByID("400030");
						GuildProxy.UI.OpenUIPopCommonSimple(infoByID, infoByID2);
						return;
					}
					case 1:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400108"));
						GuildProxy.UI.CloseMainGuildInfo();
						GuildProxy.UI.OpenMainGuild();
						return;
					case 2:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400031"));
						this.guildShareData.IsApply = true;
						this.SwitchToAlreadyApply();
						return;
					case 3:
						GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID("400032"));
						break;
					default:
						return;
					}
				}
			});
		}

		private string GetConditionType(GuildJoinKind kind)
		{
			if (kind == GuildJoinKind.Conditional)
			{
				return GuildProxy.Language.GetInfoByID("400027");
			}
			return GuildProxy.Language.GetInfoByID("400028");
		}

		[SerializeField]
		private UIGuildIcon iconCtrl;

		[SerializeField]
		private CustomText textName;

		[SerializeField]
		private CustomText textMembers;

		[SerializeField]
		private CustomButton buttonCheck;

		[SerializeField]
		private CanvasGroup canvasGroup;

		[SerializeField]
		private CustomText textState;

		[SerializeField]
		private CustomButton buttonApply;

		[SerializeField]
		private CustomText textLevel;

		private GuildShareData guildShareData;
	}
}
