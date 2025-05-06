using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using Habby.Mail;
using Habby.Mail.Data;
using Habby.Tool.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace HotFix
{
	public class NoticeManager : Singleton<NoticeManager>
	{
		public void Init(string userId)
		{
			this.DoRequestNotice(userId, null);
		}

		public void DoRequestNotice(string userId, Action onComplete)
		{
			var <>f__AnonymousType = new
			{
				userId = userId,
				clientData = this.GetClientData()
			};
			this.m_noticeList.Clear();
			HttpManager<MailHttpManager>.Instance.StartPost<MailResponse<NoticeResponse>>(this.Url, <>f__AnonymousType, delegate(MailResponse<NoticeResponse> response, string errormsg, int errorcode)
			{
				if (response != null && response.data != null)
				{
					for (int i = 0; i < response.data.announcements.Count; i++)
					{
						NoticeInfoData noticeInfoData = new NoticeInfoData();
						noticeInfoData.announcementId = response.data.announcements[i].announcementId;
						noticeInfoData.title = response.data.announcements[i].title;
						noticeInfoData.content = response.data.announcements[i].content;
						noticeInfoData.effectiveTimestamp = response.data.announcements[i].effectiveTimestamp;
						noticeInfoData.expireTimestamp = response.data.announcements[i].expireTimestamp;
						noticeInfoData.isNew = response.data.announcements[i].isNew;
						noticeInfoData.readed = response.data.announcements[i].readed;
						if (i < response.data.banners.Count)
						{
							noticeInfoData.BannerInfoData = new NoticeBannerInfoData
							{
								description = response.data.banners[i].description,
								url = response.data.banners[i].url
							};
						}
						this.m_noticeList.Add(noticeInfoData);
					}
					RedPointController.Instance.ReCalc("Main.SelfInfo.Notice", true);
					Action onComplete2 = onComplete;
					if (onComplete2 == null)
					{
						return;
					}
					onComplete2();
				}
			}, 60, false);
		}

		public void DoRequestNoticeRead(string userId, string noticeId, Action onComplete)
		{
			var <>f__AnonymousType = new
			{
				userId = userId,
				clientData = this.GetClientData(),
				announcementId = noticeId
			};
			HttpManager<MailHttpManager>.Instance.StartPost<MailResponse<NoticeResponse>>(this.ReadUrl, <>f__AnonymousType, delegate(MailResponse<NoticeResponse> response, string errormsg, int errorcode)
			{
				if (response != null && response.code == 0)
				{
					this.SetNoticeInfoDataRead(noticeId);
				}
				RedPointController.Instance.ReCalc("Main.SelfInfo.Notice", true);
				Action onComplete2 = onComplete;
				if (onComplete2 == null)
				{
					return;
				}
				onComplete2();
			}, 60, false);
		}

		public void SetNoticeInfoDataRead(string noticeId)
		{
			for (int i = 0; i < this.m_noticeList.Count; i++)
			{
				if (this.m_noticeList[i].announcementId == noticeId)
				{
					this.m_noticeList[i].readed = true;
					this.m_noticeList[i].isNew = false;
				}
			}
		}

		public List<NoticeInfoData> GetNoticeInfoDataList()
		{
			return this.m_noticeList;
		}

		public NoticeInfoData GetNoticeInfoData(string noticeId)
		{
			for (int i = 0; i < this.m_noticeList.Count; i++)
			{
				if (this.m_noticeList[i].announcementId == noticeId)
				{
					return this.m_noticeList[i];
				}
			}
			return null;
		}

		public IEnumerator GetImage(string url, CustomImage targetImage)
		{
			Sprite sprite;
			if (this.m_cacheSprites.TryGetValue(url, out sprite))
			{
				if (targetImage != null)
				{
					targetImage.sprite = sprite;
					targetImage.SetAlpha(1f);
				}
				yield break;
			}
			using (UnityWebRequest web = UnityWebRequestTexture.GetTexture(url))
			{
				yield return web.SendWebRequest();
				if (web.result == 3)
				{
					HLog.LogError("load", url, "--error--", web.error);
				}
				else if (web.isDone)
				{
					Texture2D content = DownloadHandlerTexture.GetContent(web);
					sprite = Sprite.Create(content, new Rect(0f, 0f, (float)content.width, (float)content.height), new Vector2(0.5f, 0.5f));
					if (targetImage != null)
					{
						targetImage.sprite = sprite;
						targetImage.SetAlpha(1f);
					}
					if (this.m_cacheSprites.ContainsKey(url))
					{
						this.m_cacheSprites[url] = sprite;
					}
					else
					{
						this.m_cacheSprites.Add(url, sprite);
					}
				}
			}
			UnityWebRequest web = null;
			yield break;
			yield break;
		}

		private string Url
		{
			get
			{
				if (!GameApp.Config.GetBool("IsReleaseServer"))
				{
					return "https://test-mail.advrpg.com/api/v1/announcements/list";
				}
				return "https://mail.advrpg.com/api/v1/announcements/list";
			}
		}

		private string ReadUrl
		{
			get
			{
				if (!GameApp.Config.GetBool("IsReleaseServer"))
				{
					return "https://test-mail.advrpg.com/api/v1/announcements/read";
				}
				return "https://mail.advrpg.com/api/v1/announcements/read";
			}
		}

		public Dictionary<string, object> GetClientData()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["deviceId"] = SystemInfo.deviceUniqueIdentifier;
			dictionary["appVersion"] = Application.version;
			dictionary["osVersion"] = SystemInfo.operatingSystem;
			dictionary["appLanguage"] = Singleton<LanguageManager>.Instance.GetCurrentLanguageShortening();
			dictionary["systemLanguage"] = Application.systemLanguage.ToString();
			dictionary["appBundle"] = Application.identifier;
			dictionary["deviceModel"] = SystemInfo.deviceModel;
			int num = 2;
			dictionary["os"] = num;
			dictionary["channelId"] = num;
			return dictionary;
		}

		public int OnRefreshRedCount()
		{
			for (int i = 0; i < this.m_noticeList.Count; i++)
			{
				if (this.m_noticeList[i].isValid() && this.m_noticeList[i].isNew)
				{
					return 1;
				}
			}
			return 0;
		}

		private List<NoticeInfoData> m_noticeList = new List<NoticeInfoData>();

		private Dictionary<string, Sprite> m_cacheSprites = new Dictionary<string, Sprite>();
	}
}
