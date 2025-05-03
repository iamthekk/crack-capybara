using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Framework.Http
{
	public class HttpManager : MonoBehaviour
	{
		public void Get(string url, Action<UnityWebRequest> actionResult)
		{
			base.StartCoroutine(this.OnGet(url, actionResult));
		}

		public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
		{
			base.StartCoroutine(this.OnDownloadFile(url, downloadFilePathAndName, actionResult));
		}

		public void GetTexture(string url, Action<Texture2D> actionResult)
		{
			base.StartCoroutine(this.OnGetTexture(url, actionResult));
		}

		public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
		{
			base.StartCoroutine(this.OnGetAssetBundle(url, actionResult));
		}

		public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = 20)
		{
			base.StartCoroutine(this.OnGetAudioClip(url, actionResult, audioType));
		}

		public void Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
		{
			base.StartCoroutine(this.OnPost(serverURL, lstformData, actionResult));
		}

		public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
		{
			base.StartCoroutine(this.OnUploadByPut(url, contentBytes, actionResult, ""));
		}

		private IEnumerator OnGet(string url, Action<UnityWebRequest> actionResult)
		{
			using (UnityWebRequest uwr = UnityWebRequest.Get(url))
			{
				yield return uwr.SendWebRequest();
				if (actionResult != null)
				{
					actionResult(uwr);
				}
			}
			UnityWebRequest uwr = null;
			yield break;
			yield break;
		}

		private IEnumerator OnDownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
		{
			UnityWebRequest uwr = new UnityWebRequest(url, "GET");
			uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
			yield return uwr.SendWebRequest();
			if (actionResult != null)
			{
				actionResult(uwr);
			}
			yield break;
		}

		private IEnumerator OnGetTexture(string url, Action<Texture2D> actionResult)
		{
			UnityWebRequest uwr = new UnityWebRequest(url);
			DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
			uwr.downloadHandler = downloadTexture;
			yield return uwr.SendWebRequest();
			Texture2D texture2D = null;
			if (uwr.result == 1)
			{
				texture2D = downloadTexture.texture;
			}
			if (actionResult != null)
			{
				actionResult(texture2D);
			}
			yield break;
		}

		private IEnumerator OnGetAssetBundle(string url, Action<AssetBundle> actionResult)
		{
			UnityWebRequest www = new UnityWebRequest(url);
			DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
			www.downloadHandler = handler;
			yield return www.SendWebRequest();
			AssetBundle assetBundle = null;
			if (www.result == 1)
			{
				assetBundle = handler.assetBundle;
			}
			if (actionResult != null)
			{
				actionResult(assetBundle);
			}
			yield break;
		}

		private IEnumerator OnGetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = 20)
		{
			using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
			{
				yield return uwr.SendWebRequest();
				if (uwr.result == 1 && actionResult != null)
				{
					actionResult(DownloadHandlerAudioClip.GetContent(uwr));
				}
			}
			UnityWebRequest uwr = null;
			yield break;
			yield break;
		}

		private IEnumerator OnPost(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
		{
			UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
			yield return uwr.SendWebRequest();
			if (actionResult != null)
			{
				actionResult(uwr);
			}
			yield break;
		}

		private IEnumerator OnUploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
		{
			UnityWebRequest uwr = new UnityWebRequest();
			uwr.uploadHandler = new UploadHandlerRaw(contentBytes)
			{
				contentType = contentType
			};
			yield return uwr.SendWebRequest();
			bool flag = true;
			if (uwr.result != 1)
			{
				flag = false;
			}
			if (actionResult != null)
			{
				actionResult(flag);
			}
			yield break;
		}
	}
}
