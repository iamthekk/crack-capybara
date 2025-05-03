using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Google.Protobuf.Collections;
using Shop.Arena;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HotFix
{
	public class TVRewardDataModule : IDataModule
	{
		public bool IsLoadingAllTextures { get; private set; }

		private float LoadAllTexturesStartTime { get; set; }

		public int AllLoadCount { get; private set; }

		public int AllLoadedCount { get; private set; }

		public int AllLoadedSuccessCount { get; private set; }

		public bool IsLoadedAll
		{
			get
			{
				return this.AllLoadedSuccessCount >= this.AllLoadCount;
			}
		}

		public bool WatchTVEndByReturnUnity
		{
			get
			{
				bool isEditor = Application.isEditor;
				return false;
			}
		}

		public int GetName()
		{
			return 130;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
			this.ClearData();
			this.ClearCacheTextures();
		}

		private void ClearData()
		{
			this.tvInfos.Clear();
			this.watchingTV = null;
		}

		public void ClearCacheTextures()
		{
			foreach (Texture2D texture2D in this.urlTextureDic.Values)
			{
				Object.Destroy(texture2D);
			}
			this.urlTextureDic.Clear();
		}

		public bool TryGetTVInfo(string videoId, out GmVideoDto tvInfo)
		{
			tvInfo = null;
			for (int i = 0; i < this.tvInfos.Count; i++)
			{
				if (this.tvInfos[i].VideoId == videoId)
				{
					tvInfo = this.tvInfos[i];
					return true;
				}
			}
			return false;
		}

		public Texture GetTVTexture(string url)
		{
			Texture2D texture2D;
			if (this.urlTextureDic.TryGetValue(url, out texture2D))
			{
				return texture2D;
			}
			return null;
		}

		public string GetTVTextureUrl(GmVideoDto tvInfo, LanguageType language = -1)
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			if (language == -1)
			{
				language = dataModule.GetCurrentLanguageType;
			}
			string languageAbbr = dataModule.GetLanguageAbbr(language);
			string text = "";
			for (int i = 0; i < tvInfo.ImgUrl.Count; i++)
			{
				if (tvInfo.ImgUrl[i].Lang == languageAbbr && !string.IsNullOrEmpty(tvInfo.ImgUrl[i].Url))
				{
					text = tvInfo.ImgUrl[i].Url;
					break;
				}
				if (tvInfo.ImgUrl[i].Lang == this.urlTextureDefaultLang)
				{
					text = tvInfo.ImgUrl[i].Url;
				}
			}
			return text;
		}

		public void RequestTVInfos(bool isLoginRequest = false)
		{
			NetworkUtils.TVReward.GetTVInfoListRequest(isLoginRequest, false, true);
		}

		private void UpdateTVInfos(RepeatedField<GmVideoDto> gmVideoDtos)
		{
			this.ClearData();
			for (int i = 0; i < gmVideoDtos.Count; i++)
			{
				if (!string.IsNullOrEmpty(gmVideoDtos[i].VideoUrl))
				{
					this.tvInfos.Add(gmVideoDtos[i]);
				}
			}
		}

		private void LoadAllUrlTextures()
		{
			this.AllLoadCount = this.tvInfos.Count;
			this.AllLoadedCount = 0;
			this.AllLoadedSuccessCount = 0;
			if (this.AllLoadCount > 0)
			{
				this.IsLoadingAllTextures = true;
				this.LoadAllTexturesStartTime = Time.time;
				GlobalUpdater.Instance.RegisterUpdater(new Action(this.LoadOutTimeChecker));
				for (int i = 0; i < this.tvInfos.Count; i++)
				{
					string url = this.GetTVTextureUrl(this.tvInfos[i], -1);
					if (string.IsNullOrEmpty(url))
					{
						int allLoadedSuccessCount = this.AllLoadedSuccessCount;
						this.AllLoadedSuccessCount = allLoadedSuccessCount + 1;
						this.OnLoadedOneUrlTexture();
					}
					else
					{
						this.GetUrlTexture(url, delegate(Texture2D texture)
						{
							if (texture != null)
							{
								this.urlTextureDic[url] = texture;
								int allLoadedSuccessCount2 = this.AllLoadedSuccessCount;
								this.AllLoadedSuccessCount = allLoadedSuccessCount2 + 1;
							}
							this.OnLoadedOneUrlTexture();
						});
					}
				}
				return;
			}
			this.GetTVInfoListEnd();
		}

		private void OnLoadedOneUrlTexture()
		{
			int allLoadedCount = this.AllLoadedCount;
			this.AllLoadedCount = allLoadedCount + 1;
			if (this.IsLoadingAllTextures && this.AllLoadedCount >= this.AllLoadCount)
			{
				this.GetTVInfoListEnd();
			}
		}

		private void LoadOutTimeChecker()
		{
			if (this.IsLoadingAllTextures && Time.time - this.LoadAllTexturesStartTime > (float)Singleton<GameConfig>.Instance.GetUrlImageOutTime)
			{
				this.GetTVInfoListEnd();
			}
		}

		public void SetUrlImage(string url, RawImage image)
		{
			if (string.IsNullOrEmpty(url))
			{
				return;
			}
			Texture tvtexture = this.GetTVTexture(url);
			if (tvtexture != null)
			{
				if (image != null)
				{
					image.texture = tvtexture;
					return;
				}
			}
			else
			{
				this.GetUrlTexture(url, delegate(Texture2D texture)
				{
					if (texture != null)
					{
						this.urlTextureDic[url] = texture;
						if (image != null)
						{
							image.texture = texture;
						}
					}
				});
			}
		}

		private void GetUrlTexture(string url, Action<Texture2D> actionResult)
		{
			GameApp.Http.StartCoroutine(this.OnGetTexture(url, actionResult));
		}

		private IEnumerator OnGetTexture(string url, Action<Texture2D> actionResult)
		{
			UnityWebRequest uwr = null;
			try
			{
				Uri uri = new Uri(url);
				uwr = new UnityWebRequest(uri);
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			Texture2D t = null;
			if (uwr != null)
			{
				uwr.timeout = Singleton<GameConfig>.Instance.GetUrlImageOutTime;
				DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
				uwr.downloadHandler = downloadTexture;
				yield return uwr.SendWebRequest();
				if (uwr.result == 1)
				{
					t = downloadTexture.texture;
				}
				downloadTexture = null;
			}
			t == null;
			if (actionResult != null)
			{
				actionResult(t);
			}
			yield break;
		}

		public void RequestWatchTVStart(string videoId)
		{
			GmVideoDto gmVideoDto;
			if (this.TryGetTVInfo(videoId, out gmVideoDto))
			{
				this.watchingTV = videoId;
				Application.OpenURL(gmVideoDto.VideoUrl);
				if (!this.WatchTVEndByReturnUnity)
				{
					this.RequestWatchTVEnd();
				}
			}
		}

		public void RequestWatchTVEnd()
		{
			GmVideoDto gmVideoDto;
			if (string.IsNullOrEmpty(this.watchingTV) || !this.TryGetTVInfo(this.watchingTV, out gmVideoDto))
			{
				return;
			}
			NetworkUtils.WatchTVRequest(this.watchingTV, true, null);
		}

		public void OnNetResponse_TVInfoList(RepeatedField<GmVideoDto> gmVideoDtos, bool isLoginRequest = false)
		{
			this.UpdateTVInfos(gmVideoDtos);
			RedPointController.Instance.ReCalc("Main.TVReward", true);
			if (!isLoginRequest)
			{
				if (this.loadAllTexturesBeforeShow)
				{
					this.LoadAllUrlTextures();
					return;
				}
				this.GetTVInfoListEnd();
			}
		}

		private void GetTVInfoListEnd()
		{
			this.IsLoadingAllTextures = false;
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.LoadOutTimeChecker));
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TVReward_GetInfos, null);
		}

		public void OnNetResponse_WatchTV(RepeatedField<GmVideoDto> gmVideoDtos)
		{
			this.UpdateTVInfos(gmVideoDtos);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_TVReward_GetInfos, null);
			RedPointController.Instance.ReCalc("Main.TVReward", true);
		}

		public bool ShowAnyRed()
		{
			for (int i = 0; i < this.tvInfos.Count; i++)
			{
				if (!this.tvInfos[i].IsReward)
				{
					return true;
				}
			}
			return false;
		}

		public bool ShowRed(string videoId)
		{
			GmVideoDto gmVideoDto;
			return this.TryGetTVInfo(videoId, out gmVideoDto) && !gmVideoDto.IsReward;
		}

		private bool loadAllTexturesBeforeShow = true;

		public RepeatedField<GmVideoDto> tvInfos = new RepeatedField<GmVideoDto>();

		private string watchingTV;

		private Dictionary<string, Texture2D> urlTextureDic = new Dictionary<string, Texture2D>();

		private string urlTextureDefaultLang = "en";
	}
}
