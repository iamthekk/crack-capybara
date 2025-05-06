using System;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class MainGuildViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			MainGuildViewModuleLoader mainGuildViewModuleLoader = base.Loader as MainGuildViewModuleLoader;
			this.m_joined.SetPreload(mainGuildViewModuleLoader.mPreload);
			this.m_joined.Init();
			this.m_loading.SetActive(false);
			this.m_buttonClose.onClick.AddListener(new UnityAction(this.OnClickCloseButton));
		}

		protected override void OnViewOpen(object data)
		{
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_ShowHall, new HandlerEvent(this.OnEventShowHall));
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnEventLeave));
			base.SDK.Event.RegisterEvent(8, new GuildHandlerEvent(this.OnRefreshByGuild));
			if (!GuildSDKManager.Instance.IsDataInitOver)
			{
				this.SendGuildLoginMsg();
			}
			else
			{
				this.RefreshGuildShow(true);
				this.SendGuildLoginMsg();
			}
			ulong customRefreshTime = base.SDK.GetCustomRefreshTime();
			DxxTools.UI.AddServerTimeCallback("MainGuildViewModule", new Action(this.OnRefreshGuildInfoByDayPass), (long)customRefreshTime, 0);
		}

		public void RefreshGuildShow(bool isopen)
		{
			this.m_loading.SetActive(false);
			this.m_isJoined = base.SDK.GuildInfo.HasGuild;
			if (this.m_isJoined)
			{
				this.SwitchJoinStateView(true);
			}
			else
			{
				GuildProxy.UI.CloseMainGuild();
			}
			if (isopen)
			{
				this.CheckShowGuildLevelUp();
			}
			this.CheckShowBeKickOut();
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
			if (this.m_joined != null)
			{
				this.m_joined.OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		protected override void OnViewClose()
		{
			DxxTools.UI.RemoveServerTimeClockCallback("MainGuildViewModule");
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_ShowHall, new HandlerEvent(this.OnEventShowHall));
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnEventLeave));
			base.SDK.Event.UnRegisterEvent(8, new GuildHandlerEvent(this.OnRefreshByGuild));
			this.m_joined.Close();
		}

		protected override void OnViewDelete()
		{
			if (this.m_buttonClose != null)
			{
				this.m_buttonClose.onClick.RemoveListener(new UnityAction(this.OnClickCloseButton));
			}
			this.m_joined.DeInit();
		}

		private void OnEventShowHall(object sender, int type, BaseEventArgs eventArgs)
		{
			this.RefreshGuildShow(false);
		}

		private void OnEventLeave(object sender, int type, BaseEventArgs eventArgs)
		{
			this.SwitchJoinStateView(false);
			this.CheckShowBeKickOut();
		}

		private void OnRefreshByGuild(int type, GuildBaseEvent eventArgs)
		{
			this.RefreshGuildShow(false);
		}

		private void SendGuildLoginMsg()
		{
			if (!GuildSDKManager.Instance.IsDataInitOver)
			{
				this.m_joined.Close();
			}
			GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
			{
				if (!base.CheckIsViewOpen())
				{
					return;
				}
				if (result)
				{
					this.RefreshGuildShow(true);
					return;
				}
				this.m_loading.SetActive(false);
			});
		}

		private void CheckShowGuildLevelUp()
		{
			if (base.SDK.GuildInfo.IsShowLevelUp)
			{
				this.m_isJoined = base.SDK.GuildInfo.HasGuild;
				if (this.m_isJoined && GuildProxy.UI.IsCanShowLevelUp())
				{
					GuildProxy.UI.OpenUIGuildLevelUp(null);
				}
			}
		}

		private void CheckShowBeKickOut()
		{
			if (base.SDK.GuildInfo.BeKickOut != null)
			{
				GuildBeKickOutInfo beKickOut = base.SDK.GuildInfo.BeKickOut;
				GuildEvent_BeKickOutSetData guildEvent_BeKickOutSetData = new GuildEvent_BeKickOutSetData();
				guildEvent_BeKickOutSetData.Info = null;
				base.SDK.Event.DispatchNow(22, guildEvent_BeKickOutSetData);
				this.m_isJoined = base.SDK.GuildInfo.HasGuild;
				if (!this.m_isJoined)
				{
					string infoByID = GuildProxy.Language.GetInfoByID("400119");
					string positionLanguageByPos = GuildUserShareDataEx.GetPositionLanguageByPos((int)beKickOut.KickUserPos);
					string text = (string.IsNullOrEmpty(beKickOut.KickUserNick) ? GuildProxy.GameUser.GetPlayerDefaultNick(beKickOut.KickUserID) : beKickOut.KickUserNick);
					string infoByID2 = GuildProxy.Language.GetInfoByID2("400208", positionLanguageByPos, text);
					GuildProxy.UI.OpenUIPopCommonOnlySure(infoByID, infoByID2, delegate
					{
						GuildProxy.UI.CloseUIGuildBoss();
					});
				}
			}
		}

		private void OnRefreshGuildInfoByDayPass()
		{
			GuildNetUtil.Guild.DoRequest_GuildGetFeaturesInfoRequest(delegate(bool result, GuildGetFeaturesInfoResponse resp)
			{
				if (base.gameObject == null || !base.gameObject.activeSelf)
				{
					return;
				}
				if (result)
				{
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshHall);
					GuildProxy.GameEvent.PushEvent(LocalMessageName.CC_Guild_RefreshShop);
				}
			});
		}

		private void OnClickCloseButton()
		{
			if (this.m_hookClose != null)
			{
				if (this.m_hookClose())
				{
					GameApp.View.CloseView(ViewName.MainGuildViewModule, null);
				}
				return;
			}
			GameApp.View.CloseView(ViewName.MainGuildViewModule, null);
		}

		private void SwitchJoinStateView(bool isJoined)
		{
			if (isJoined)
			{
				this.m_joined.Show();
				this.m_hookClose = new Func<bool>(this.m_joined.OnClickCloseHook);
				return;
			}
			GuildProxy.UI.CloseMainGuild();
		}

		[SerializeField]
		private MainGuild_JoinedCtrl m_joined;

		[SerializeField]
		private GameObject m_loading;

		[SerializeField]
		private CustomButton m_buttonClose;

		private bool m_isJoined;

		private Func<bool> m_hookClose;
	}
}
