using System;
using System.Collections.Generic;
using DG.Tweening;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildBuilding : GuildProxy.GuildProxy_BaseBehaviour
	{
		private GuildBossInfo guildBoss
		{
			get
			{
				return base.SDK.GuildActivity.GuildBoss;
			}
		}

		protected override void GuildUI_OnInit()
		{
			this.BossSpine.Init();
			this.guildIconCtrl.Init();
			this.buttonGuild.onClick.AddListener(new UnityAction(this.OnClickGuildHall));
			this.buttonShop.onClick.AddListener(new UnityAction(this.OnClickGuildShop));
			this.buttonActivity.onClick.AddListener(new UnityAction(this.OnClickGuildActivity));
			this.buttonBoss.onClick.AddListener(new UnityAction(this.OnClickBoss));
			this._playedAnim = false;
			base.SDK.Event.RegisterEvent(15, new GuildHandlerEvent(this.OnUpdateGuildInfo));
		}

		protected override void GuildUI_OnShow()
		{
			this.Refresh();
			this.PlayButtonScaleOnShow();
			this.RefreshBossModel();
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void GuildUI_OnClose()
		{
			this.m_seqPool.Clear(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			this.BossSpine.DeInit();
			this.m_seqPool.Clear(false);
			if (this.buttonGuild != null)
			{
				this.buttonGuild.onClick.RemoveListener(new UnityAction(this.OnClickGuildHall));
			}
			if (this.buttonShop != null)
			{
				this.buttonShop.onClick.RemoveListener(new UnityAction(this.OnClickGuildShop));
			}
			if (this.buttonActivity != null)
			{
				this.buttonActivity.onClick.RemoveListener(new UnityAction(this.OnClickGuildActivity));
			}
			if (this.buttonBoss != null)
			{
				this.buttonBoss.onClick.RemoveListener(new UnityAction(this.OnClickBoss));
			}
			base.SDK.Event.UnRegisterEvent(15, new GuildHandlerEvent(this.OnUpdateGuildInfo));
			this._playedAnim = false;
			this.m_guildBossMemberId = -1;
		}

		private void Refresh()
		{
			GuildShareData guildData = GuildSDKManager.Instance.GuildInfo.GuildData;
			this.textGuildName.text = guildData.GuildShowName;
			this.textGuildLv.text = GuildProxy.Language.GetInfoByID1("400150", guildData.GuildLevel);
			this.textGuildActive.text = GuildProxy.Language.GetInfoByID1("400271", guildData.GuildActive);
			this.guildIconCtrl.SetIcon(guildData.GuildIcon);
		}

		private void OnUpdateGuildInfo(int type, GuildBaseEvent eventargs)
		{
			this.Refresh();
		}

		private void OnClickOpenChatView()
		{
			GuildProxy.UI.OpenUIGuildChat(null);
		}

		public void CheckLoadHistoryMessage()
		{
			GuildProxy.Chat.CheckGetChatHistoryWhenEmpty();
		}

		private void OnClickGuildHall()
		{
			GuildProxy.UI.OpenUIGuildInfoPop(null);
		}

		private void OnClickGuildShop()
		{
			GuildProxy.UI.OpenGuildShop();
		}

		private void OnClickGuildActivity()
		{
			GuildProxy.UI.OpenGuildActivity(null);
		}

		private void OnClickBoss()
		{
			GuildProxy.UI.OpenUIGuildBoss(null, null, null);
		}

		private void PlayButtonScaleOnShow()
		{
			if (this._playedAnim)
			{
				return;
			}
			this._playedAnim = true;
			List<Transform> list = new List<Transform>
			{
				this.buttonGuild.transform,
				this.buttonShop.transform,
				this.buttonActivity.transform,
				this.buttonBoss.transform
			};
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				transform.localScale = Vector3.zero;
				Sequence sequence = this.m_seqPool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, (float)i * 0.05f);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(transform, 1f, 0.3f));
			}
		}

		private async void RefreshBossModel()
		{
			if (this.guildBoss == null)
			{
				HLog.LogError("UIGuildBossMainCtrl.RefreshBossModel() but guildBoss is null!");
			}
			else
			{
				GuildBOSS_guildBoss currentBossShowModel = this.guildBoss.GetCurrentBossShowModel();
				if (currentBossShowModel == null)
				{
					HLog.LogError("UIGuildBossMainCtrl.RefreshBossModel() but path is null!");
				}
				else if (this.m_guildBossMemberId != currentBossShowModel.BossId)
				{
					this.BossSpine.gameObject.SetActiveSafe(false);
					if (GameApp.Table.GetManager().GetGameMember_member(currentBossShowModel.BossId) != null)
					{
						this.m_guildBossMemberId = currentBossShowModel.BossId;
						this.BossSpine.gameObject.SetActiveSafe(true);
						this.BossSpine.ShowMemberModel(currentBossShowModel.BossId, "Idle", true);
						this.BossSpine.SetScale(currentBossShowModel.uiScale);
					}
				}
			}
		}

		[SerializeField]
		private CustomButton buttonGuild;

		[SerializeField]
		private CustomButton buttonShop;

		[SerializeField]
		private CustomButton buttonActivity;

		[SerializeField]
		private CustomButton buttonBoss;

		[SerializeField]
		private CustomText textGuildName;

		[SerializeField]
		private CustomText textGuildLv;

		[SerializeField]
		private CustomText textGuildActive;

		[SerializeField]
		private UIGuildIcon guildIconCtrl;

		[Header("公会Boss")]
		public UISpineModelItem BossSpine;

		private int m_guildBossMemberId;

		private bool _playedAnim;

		private SequencePool m_seqPool = new SequencePool();
	}
}
