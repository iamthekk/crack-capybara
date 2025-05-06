using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.User;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class PlayerInformationPlayerGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_uiAvatarCtrl.Init();
			this.m_btnUIDCopy.m_onClick = new Action(this.OnBtnUIDCopyClick);
		}

		protected override void OnDeInit()
		{
			this.m_btnUIDCopy.m_onClick = null;
			this.m_uiAvatarCtrl.DeInit();
			this.m_playerInfoData = null;
		}

		public void RefreshUI(PlayerInfoDto playerInfo)
		{
			this.m_playerInfoData = playerInfo;
			if (playerInfo == null)
			{
				return;
			}
			if (this.m_uiAvatarCtrl != null)
			{
				this.m_uiAvatarCtrl.RefreshData((int)playerInfo.Avatar, (int)playerInfo.AvatarFrame);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = (string.IsNullOrEmpty(playerInfo.NickName) ? DxxTools.GetDefaultNick(playerInfo.UserId) : playerInfo.NickName);
			}
			if (this.m_uidTxt != null)
			{
				this.m_uidTxt.text = playerInfo.UserId.ToString();
			}
			bool flag = !string.IsNullOrEmpty(playerInfo.GuildName);
			if (this.m_guildTxt != null)
			{
				string text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("9304", new object[] { playerInfo.GuildName }) : Singleton<LanguageManager>.Instance.GetInfoByID("9304", new object[] { Singleton<LanguageManager>.Instance.GetInfoByID("9307") }));
				this.m_guildTxt.text = text;
			}
			if (this.m_maxChapterTxt != null)
			{
				this.m_maxChapterTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9303", new object[] { DxxTools.GetChapterLevel(playerInfo.ChapterId, playerInfo.WaveIndex - 1) });
			}
			if (this.m_ServerTxt != null)
			{
				uint serverZoneIdByServerId = GameApp.Data.GetDataModule(DataName.SelectServerDataModule).GetServerZoneIdByServerId(playerInfo.ServerId);
				ServerList_serverList elementById = GameApp.Table.GetManager().GetServerList_serverListModelInstance().GetElementById((int)serverZoneIdByServerId);
				if (elementById != null)
				{
					uint num = playerInfo.ServerId - (uint)elementById.range[0] + 1U;
					this.m_ServerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("playerinformation_server", new object[] { Singleton<LanguageManager>.Instance.GetInfoByID(elementById.serverPrefix, new object[] { num }) });
				}
				else
				{
					this.m_ServerTxt.text = "";
				}
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = DxxTools.FormatNumber(playerInfo.Power);
			}
			if (this.m_talentTxt != null)
			{
				TalentNew_talentEvolution elementById2 = GameApp.Table.GetManager().GetTalentNew_talentEvolutionModelInstance().GetElementById(playerInfo.TalentId);
				if (elementById2 != null)
				{
					this.m_talentTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById2.stepLanguageId);
				}
				else
				{
					this.m_talentTxt.text = "";
				}
			}
			LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
			LayoutRebuilder.ForceRebuildLayoutImmediate(base.rectTransform);
		}

		private void OnBtnUIDCopyClick()
		{
			if (this.m_playerInfoData == null)
			{
				return;
			}
			GUIUtility.systemCopyBuffer = this.m_playerInfoData.UserId.ToString();
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("30017"));
		}

		private void OnClickGuildInfoBt()
		{
			if (this.m_playerInfoData == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.m_playerInfoData.GuildName))
			{
				return;
			}
			if (GameApp.View.IsOpened(ViewName.GuildCheckPopViewModule))
			{
				GameApp.View.CloseView(ViewName.GuildCheckPopViewModule, null);
			}
			GuildProxy.UI.OpenUIGuildCheckPop(this.m_playerInfoData.GuildId, 0UL);
		}

		public UIAvatarCtrl m_uiAvatarCtrl;

		public CustomText m_nameTxt;

		public CustomText m_uidTxt;

		public CustomText m_guildTxt;

		public CustomText m_maxChapterTxt;

		public CustomButton m_btnUIDCopy;

		public CustomText m_ServerTxt;

		public CustomText m_powerTxt;

		public CustomText m_talentTxt;

		private PlayerInfoDto m_playerInfoData;
	}
}
