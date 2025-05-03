using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dxx.Chat;
using Framework;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix.ChatUI
{
	public class ChatEmojiPool
	{
		public static ChatEmojiPool Instance
		{
			get
			{
				if (ChatEmojiPool.pool == null)
				{
					ChatEmojiPool.pool = new ChatEmojiPool();
					ChatEmojiPool.pool.InitInstance();
				}
				return ChatEmojiPool.pool;
			}
		}

		private void InitInstance()
		{
		}

		private string InternalGetEmojiPath(int emojiid)
		{
			string text;
			if (this.mLoadPathDic.TryGetValue(emojiid, out text))
			{
				return text;
			}
			Emoji_Emoji emojiTab = ChatProxy.Table.GetEmojiTab(emojiid);
			text = "Assets/_Resources/Prefab/UI/Emoji/" + emojiTab.path;
			this.mLoadPathDic[emojiid] = text;
			return text;
		}

		public async void CreateEmoji(int emojiid, Action<GameObject> callback)
		{
			string objkey = string.Format("emoji_{0}", emojiid);
			List<GameObject> list;
			if (this.mObjListDic.TryGetValue(objkey, out list) && list.Count > 0)
			{
				GameObject gameObject = list[0];
				list.RemoveAt(0);
			}
			string text = this.InternalGetEmojiPath(emojiid);
			await this.InternalGetEmoji(text, delegate(GameObject obj)
			{
				if (obj != null)
				{
					obj.name = objkey;
				}
				Action<GameObject> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(obj);
			}, this.mPoolParent);
		}

		private void CheckCreateEmojiPool()
		{
			if (this.mPoolParent == null)
			{
				GameObject gameObject = GameObject.Find("EmojiPool");
				if (gameObject == null)
				{
					gameObject = new GameObject("EmojiPool");
				}
				this.mPoolParent = gameObject.transform;
				this.mPoolParent.gameObject.SetActive(false);
				Object.DontDestroyOnLoad(this.mPoolParent.gameObject);
			}
		}

		public void ReleaseEmoji(GameObject obj)
		{
			if (obj == null)
			{
				return;
			}
			this.CheckCreateEmojiPool();
			string name = obj.name;
			List<GameObject> list;
			if (!this.mObjListDic.TryGetValue(name, out list))
			{
				list = new List<GameObject>();
				this.mObjListDic[name] = list;
			}
			list.Add(obj);
			obj.transform.SetParent(this.mPoolParent, false);
		}

		public void ClearAll()
		{
			for (int i = 0; i < this.mAllCreatedObjList.Count; i++)
			{
				if (this.mAllCreatedObjList[i] != null)
				{
					this.InternalReleaseEmoji(this.mAllCreatedObjList[i]);
				}
			}
			this.mObjListDic.Clear();
			this.mAllCreatedObjList.Clear();
			if (this.mPoolParent != null)
			{
				this.mPoolParent.DestroyChildren();
			}
		}

		private async Task InternalGetEmoji(string path, Action<GameObject> callback, Transform parent = null)
		{
			GameObject prefab;
			if (!this.mPrefabDic.TryGetValue(path, out prefab))
			{
				AsyncOperationHandle<GameObject> loadtask = GameApp.Resources.LoadAssetAsync<GameObject>(path);
				await loadtask.Task;
				if (loadtask.Result != null)
				{
					this.mPrefabDic[path] = loadtask.Result;
					prefab = loadtask.Result;
				}
				loadtask = default(AsyncOperationHandle<GameObject>);
			}
			if (prefab == null)
			{
				HLog.LogError("加载表情失败：" + path);
			}
			else
			{
				GameObject gameObject = Object.Instantiate<GameObject>(prefab, parent);
				if (callback != null)
				{
					callback(gameObject);
				}
			}
		}

		private void InternalReleaseEmoji(GameObject obj)
		{
			Object.Destroy(obj);
		}

		private static ChatEmojiPool pool = new ChatEmojiPool();

		private Dictionary<int, string> mLoadPathDic = new Dictionary<int, string>();

		private Dictionary<string, GameObject> mPrefabDic = new Dictionary<string, GameObject>();

		private Transform mPoolParent;

		private Dictionary<string, List<GameObject>> mObjListDic = new Dictionary<string, List<GameObject>>();

		private List<GameObject> mAllCreatedObjList = new List<GameObject>();
	}
}
