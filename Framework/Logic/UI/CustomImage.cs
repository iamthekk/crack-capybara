using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomImage : Image
	{
		public void SetSprite(Sprite spriteValue)
		{
			base.sprite = spriteValue;
			if (this.KeepNativeSize && base.sprite != null)
			{
				this.SetNativeSize();
			}
		}

		public void SetImage(string atlasPath, string spriteName)
		{
			this.SetImageTask(atlasPath, spriteName);
		}

		public async Task SetImageTask(string atlasPath, string spriteName)
		{
			this.m_spriteName = spriteName;
			if (string.Equals(this.m_atlasPath, atlasPath) && this.m_atlas != null)
			{
				if (this.m_atlas != null)
				{
					this.SetSprite(this.m_atlas.GetSprite(spriteName));
				}
				this.onFinished.Invoke(atlasPath, spriteName);
			}
			else
			{
				this.UnLoad();
				this.m_atlasPath = atlasPath;
				this.m_spriteName = spriteName;
				this.m_handle = GameApp.Resources.LoadAssetAsync<SpriteAtlas>(atlasPath);
				await this.m_handle.Task;
				if (this.m_handle.Status != 1)
				{
					HLog.LogError(string.Concat(new string[] { "CustomImage.Load(atlasPath:", atlasPath, ",spriteName:", spriteName, ") " }));
				}
				else if (string.Equals(this.m_atlasPath, atlasPath) && string.Equals(this.m_spriteName, spriteName))
				{
					this.m_atlas = this.m_handle.Result;
					if (this.m_atlas != null)
					{
						this.SetSprite(this.m_atlas.GetSprite(this.m_spriteName));
					}
					this.onFinished.Invoke(atlasPath, this.m_spriteName);
				}
			}
		}

		public void SetImageSingle(string imgpath)
		{
			this.SetImageSingleTask(imgpath);
		}

		public async Task SetImageSingleTask(string imgpath)
		{
			if (string.Equals(imgpath, this.m_pathSingle) && this.m_textureSingle != null)
			{
				if (this.KeepNativeSize && base.sprite != null)
				{
					this.SetNativeSize();
				}
				this.onFinished.Invoke(imgpath, string.Empty);
			}
			else
			{
				this.UnLoad();
				this.m_pathSingle = imgpath;
				this.m_handleSingle = GameApp.Resources.LoadAssetAsync<Texture2D>(imgpath);
				await this.m_handleSingle.Task;
				if (this.m_handleSingle.Status == 1)
				{
					if (string.Equals(this.m_pathSingle, imgpath))
					{
						this.m_textureSingle = this.m_handleSingle.Result;
						Sprite sprite = Sprite.Create(this.m_textureSingle, new Rect(0f, 0f, (float)this.m_textureSingle.width, (float)this.m_textureSingle.height), Vector2.zero);
						this.SetSprite(sprite);
						this.onFinished.Invoke(imgpath, string.Empty);
					}
				}
			}
		}

		public void SetImageUrl(string url)
		{
			if (string.Equals(this.m_pathUrl, url))
			{
				this.onFinished.Invoke(url, string.Empty);
				return;
			}
			this.UnLoad();
			this.m_pathUrl = url;
			this.m_handleUrl = this.OnGetTexture(this.m_pathUrl);
			base.StartCoroutine(this.m_handleUrl);
		}

		private IEnumerator OnGetTexture(string url)
		{
			UnityWebRequest uwr = new UnityWebRequest(url);
			DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
			uwr.downloadHandler = downloadTexture;
			yield return uwr.SendWebRequest();
			if (uwr.result != 1)
			{
				yield break;
			}
			Texture2D texture = downloadTexture.texture;
			if (texture == null)
			{
				yield break;
			}
			Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), Vector2.zero);
			this.SetSprite(sprite);
			this.onFinished.Invoke(url, string.Empty);
			yield break;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			this.UnLoad();
		}

		private void UnLoad()
		{
			if (this.m_atlas != null)
			{
				this.SetSprite(null);
				this.m_atlas = null;
			}
			if (this.m_handle.IsValid())
			{
				GameApp.Resources.Release<SpriteAtlas>(this.m_handle);
			}
			if (this.m_textureSingle != null)
			{
				this.SetSprite(null);
				this.m_textureSingle = null;
			}
			if (this.m_handleSingle.IsValid())
			{
				GameApp.Resources.Release<Texture2D>(this.m_handleSingle);
			}
			if (this.m_textureUrl != null)
			{
				this.SetSprite(null);
				this.m_textureUrl = null;
			}
			if (this.m_handleUrl != null)
			{
				base.StopCoroutine(this.m_handleUrl);
			}
			this.m_handleUrl = null;
			this.m_atlasPath = string.Empty;
			this.m_spriteName = string.Empty;
			this.m_pathSingle = string.Empty;
			this.m_pathUrl = string.Empty;
		}

		public void SetAlpha(float alpha)
		{
			Color color = this.color;
			color.a = alpha;
			this.color = color;
		}

		private SpriteAtlas m_atlas;

		private string m_atlasPath;

		private string m_spriteName;

		private AsyncOperationHandle<SpriteAtlas> m_handle;

		private Texture2D m_textureSingle;

		private string m_pathSingle = string.Empty;

		private AsyncOperationHandle<Texture2D> m_handleSingle;

		private Texture2D m_textureUrl;

		private string m_pathUrl = string.Empty;

		private IEnumerator m_handleUrl;

		[Header("自适应图片尺寸")]
		public bool KeepNativeSize;

		public CustomImage.FinishedEvent onFinished = new CustomImage.FinishedEvent();

		[Serializable]
		public class FinishedEvent : UnityEvent<string, string>
		{
		}
	}
}
