using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Platform;
using Framework.MailManager;
using Habby.Mail;
using Habby.Mail.Data;
using Habby.Tool.Http;
using Proto.Common;

namespace HotFix
{
	public class HabbyMailManager : IMailManager
	{
		public HabbyMailManager.ControlModule Control { get; private set; }

		public MailMessageListen Listen { get; private set; }

		public void SetData(string version, string url, MailServerType serverType)
		{
			this.m_version = version;
			this.m_url = url;
			this.m_serverType = serverType;
		}

		public void OnInit()
		{
			this.m_isLogin = false;
			this.Listen = new MailMessageListen();
			this.Control = new HabbyMailManager.ControlModule(this.Listen);
			MailMessageListen listen = this.Listen;
			listen.m_codeListen = (Action<string, int>)Delegate.Combine(listen.m_codeListen, new Action<string, int>(this.OnCodeListen));
			MailMessageListen listen2 = this.Listen;
			listen2.m_rewardsListen = (Action<CommonData>)Delegate.Combine(listen2.m_rewardsListen, new Action<CommonData>(this.OnRewardsListen));
			HttpManager<MailHttpManager>.Instance.OnHttpStartEvent += this.OnHttpStartEvent;
			HttpManager<MailHttpManager>.Instance.OnHttpFinishedEvent += this.OnHttpFinishedEvent;
			MailManager.UpdateSettingDataEvent += this.OnUpdateSettingDataEvent;
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventRefreshLanaguage));
		}

		public void OnDeInit()
		{
			this.m_isLogin = false;
			if (this.Listen.m_codeListen != null)
			{
				MailMessageListen listen = this.Listen;
				listen.m_codeListen = (Action<string, int>)Delegate.Remove(listen.m_codeListen, new Action<string, int>(this.OnCodeListen));
			}
			if (this.Listen.m_rewardsListen != null)
			{
				MailMessageListen listen2 = this.Listen;
				listen2.m_rewardsListen = (Action<CommonData>)Delegate.Remove(listen2.m_rewardsListen, new Action<CommonData>(this.OnRewardsListen));
			}
			HttpManager<MailHttpManager>.Instance.OnHttpStartEvent -= this.OnHttpStartEvent;
			HttpManager<MailHttpManager>.Instance.OnHttpFinishedEvent -= this.OnHttpFinishedEvent;
			MailManager.UpdateSettingDataEvent -= this.OnUpdateSettingDataEvent;
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventRefreshLanaguage));
		}

		public void Login(string userID)
		{
			if (this.m_isLogin)
			{
				return;
			}
			this.m_setting = new MailSetting
			{
				serverUrl = this.m_url,
				userId = userID,
				appLanguage = Singleton<LanguageManager>.Instance.GetCurrentLanguageShortening(),
				netVersion = this.m_version,
				os = Singleton<PlatformHelper>.Instance.GetPlatformIndex(),
				channelId = Singleton<PlatformHelper>.Instance.GetChannelIndex()
			};
			MailManager.Init(this.m_setting);
		}

		private void OnEventRefreshLanaguage(object sender, int type, BaseEventArgs eventargs)
		{
			MailManager.Setting.appLanguage = Singleton<LanguageManager>.Instance.GetCurrentLanguageShortening();
		}

		private MailSetting OnUpdateSettingDataEvent()
		{
			return this.m_setting;
		}

		private void OnHttpStartEvent(HttpObject obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj.isMask)
			{
				this.ShowMask();
			}
		}

		private void OnHttpFinishedEvent(HttpObject obj)
		{
			if (obj == null)
			{
				return;
			}
			if (obj.isMask)
			{
				this.CloseMask();
			}
		}

		private void OnCodeListen(string message, int code)
		{
			HLog.LogError(string.Format("[MailManager] code = {0}", code));
			HabbyMailCodeName habbyMailCodeName = (HabbyMailCodeName)code;
			int num;
			if (!int.TryParse(habbyMailCodeName.ToString(), out num))
			{
				return;
			}
		}

		private void OnRewardsListen(CommonData commonData)
		{
			if (commonData == null)
			{
				return;
			}
			NetworkUtils.HandleResponse_CommonDataInternal(commonData);
		}

		private void ShowMask()
		{
			if (!this.m_maskShow)
			{
				this.m_maskShow = true;
				GameApp.View.ShowNetLoading(true);
			}
		}

		private void CloseMask()
		{
			if (this.m_maskShow)
			{
				this.m_maskShow = false;
				GameApp.View.ShowNetLoading(false);
			}
		}

		public void OnRefreshMailListData(Action onComplete = null)
		{
			GameApp.Mail.GetManager().Control.SendGetMailList(delegate(List<MailData> list)
			{
				EventMailList instance = Singleton<EventMailList>.Instance;
				instance.SetData(list);
				GameApp.Event.DispatchNow(this, 225, instance);
				RedPointController.Instance.ReCalc("Main.Mail", true);
				if (onComplete != null)
				{
					onComplete();
				}
			}, false);
		}

		private string m_version;

		private string m_url;

		private MailServerType m_serverType;

		private bool m_maskShow;

		private bool m_isLogin;

		private MailSetting m_setting;

		public class ControlModule
		{
			public ControlModule(MailMessageListen listen)
			{
				this.m_listen = listen;
			}

			private void InvokeCode(string message, int codeID)
			{
				if (this.m_listen == null || this.m_listen.m_codeListen == null)
				{
					return;
				}
				this.m_listen.m_codeListen(message, codeID);
			}

			private void InvokeReward(CommonData reward)
			{
				if (this.m_listen == null || this.m_listen.m_rewardsListen == null)
				{
					return;
				}
				this.m_listen.m_rewardsListen(reward);
			}

			public void SendGetMailList(Action<List<MailData>> onComplete, bool isMask = true)
			{
				HttpObject mailList = MailManager.GetMailList(delegate(List<MailData> reslist, string msg, int errorcode)
				{
					if (errorcode != 0)
					{
						HLog.LogError(string.Format("[MailManager] GetMailList fail msg = {0},code = {1}", msg, errorcode));
						this.InvokeCode(msg, errorcode);
						return;
					}
					if (onComplete != null)
					{
						onComplete(reslist);
					}
				});
				if (mailList != null)
				{
					mailList.isMask = isMask;
				}
			}

			public void SendDeleteMails(string[] pMails, Action<MailsDeleteResponse> onComplete, bool isMask = true)
			{
				HttpObject httpObject = MailManager.DeleteMails(pMails, delegate(MailsDeleteResponse reslist, string msg, int errorcode)
				{
					if (errorcode != 0)
					{
						HLog.LogError(string.Format("[MailManager] SendDeleteMails fail msg = {0},code = {1}", msg, errorcode));
						this.InvokeCode(msg, errorcode);
						return;
					}
					if (onComplete != null)
					{
						onComplete(reslist);
					}
				});
				if (httpObject != null)
				{
					httpObject.isMask = isMask;
				}
			}

			public void SendMarkMailAsRead(string mailId, int mailScope, Action<MailData> onComplete, bool isMask = true)
			{
				HttpObject httpObject = MailManager.MarkMailAsRead(mailId, mailScope, delegate(MailData reslist, string msg, int errorcode)
				{
					if (errorcode != 0)
					{
						HLog.LogError(string.Format("[MailManager] MarkMailAsRead fail msg = {0},code = {1}", msg, errorcode));
						this.InvokeCode(msg, errorcode);
						return;
					}
					if (onComplete != null)
					{
						onComplete(reslist);
					}
				});
				if (httpObject != null)
				{
					httpObject.isMask = isMask;
				}
			}

			public void SendGetMailReward(string[] mailIDs, Action<List<MailRewardObject>, CommonData> onComplete, bool isMask = true)
			{
				if (mailIDs == null)
				{
					return;
				}
				CommonData data = Singleton<GuildDataConverter>.Instance.GetDefaultCommonData();
				List<MailRewardObject> mailDatas = new List<MailRewardObject>();
				HttpObject mailReward = MailManager.GetMailReward(mailIDs, delegate(List<MailRewardObject> reslist, string msg, int errorcode)
				{
					if (errorcode != 0)
					{
						HLog.LogError(string.Format("[MailManager] GetMailReward fail msg = {0},code = {1}", msg, errorcode));
						this.InvokeCode(msg, errorcode);
						return;
					}
					for (int i = 0; i < reslist.Count; i++)
					{
						MailRewardObject mailRewardObject = reslist[i];
						if (mailRewardObject != null && mailRewardObject.code == 0 && !string.IsNullOrEmpty(mailRewardObject.rewards))
						{
							CommonData commonDataForJson = Singleton<GuildDataConverter>.Instance.GetCommonDataForJson(mailRewardObject.rewards);
							if (commonDataForJson != null)
							{
								data.MergeCommonData(commonDataForJson);
								mailDatas.Add(mailRewardObject);
							}
						}
					}
					if (data.Reward != null)
					{
						data.Reward.ToOverlay();
					}
					this.InvokeReward(data);
					if (onComplete != null)
					{
						onComplete(mailDatas, data);
					}
					RedPointController.Instance.ReCalcAsync("Main.Mail");
				});
				if (mailReward != null)
				{
					mailReward.isMask = isMask;
				}
			}

			private MailMessageListen m_listen;
		}
	}
}
