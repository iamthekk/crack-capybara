﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class LocalUnityObjctPool : MonoBehaviour
	{
		public static LocalUnityObjctPool Create(GameObject parent)
		{
			GameObject gameObject = new GameObject("LocalPool");
			gameObject.transform.SetParent(parent.transform, false);
			LocalUnityObjctPool localUnityObjctPool = gameObject.AddComponent<LocalUnityObjctPool>();
			gameObject.SetActive(false);
			return localUnityObjctPool;
		}

		public void CreateCache<T>(GameObject copyItem) where T : Component
		{
			this.CreateCache<T>(base.gameObject, copyItem);
		}

		public void CreateCache<T>(GameObject parent, GameObject copyItem) where T : Component
		{
			this.CreateCache(typeof(T).Name, parent.transform, copyItem);
		}

		public T DeQueue<T>() where T : Component
		{
			return this.DeQueue<T>(typeof(T).Name);
		}

		public T DeQueueWithName<T>(string name) where T : Component
		{
			return this.DeQueueWithName<T>(typeof(T).Name, name);
		}

		public void EnQueue<T>(GameObject item) where T : Component
		{
			this.EnQueue(typeof(T).Name, item);
		}

		public void Collect<T>() where T : Component
		{
			this.Collect(typeof(T).Name);
		}

		public void CollectAll()
		{
			foreach (KeyValuePair<string, LocalUnityObjctPool.Cache> keyValuePair in this.m_Cache)
			{
				keyValuePair.Value.Collect();
			}
		}

		public void ClearCache<T>() where T : Component
		{
			this.ClearCache(typeof(T).Name);
		}

		public bool IsHavePool<T>() where T : Component
		{
			string name = typeof(T).Name;
			return this.IsHavePool(name);
		}

		public bool IsHavePool(string name)
		{
			return this.m_Cache.ContainsKey(name);
		}

		public void CreateCache(string cacheName, GameObject copyItem)
		{
			this.CreateCache(cacheName, base.transform, copyItem);
		}

		public void CreateCache(string cacheName, Transform parent, GameObject copyItem)
		{
			this.m_Cache.Add(cacheName, new LocalUnityObjctPool.Cache(parent, copyItem));
		}

		public T DeQueue<T>(string cacheName) where T : Component
		{
			return this.m_Cache[cacheName].Dequeue().GetComponent<T>();
		}

		public GameObject DeQueue(string cacheName)
		{
			return this.m_Cache[cacheName].Dequeue();
		}

		public T DeQueueWithName<T>(string cacheName, string name) where T : Component
		{
			return this.m_Cache[cacheName].Dequeue(name).GetComponent<T>();
		}

		public void EnQueue(string cacheName, GameObject item)
		{
			if (this.m_Cache.ContainsKey(cacheName))
			{
				this.m_Cache[cacheName].EnQueue(item);
			}
		}

		public void Collect(string cacheName)
		{
			if (this.m_Cache.ContainsKey(cacheName))
			{
				this.m_Cache[cacheName].Collect();
			}
		}

		public void ClearCache(string cacheName)
		{
			if (this.m_Cache.ContainsKey(cacheName))
			{
				this.m_Cache[cacheName].Destroy();
			}
		}

		public void ClearAllCache()
		{
			foreach (LocalUnityObjctPool.Cache cache in this.m_Cache.Values)
			{
				cache.Destroy();
			}
			this.m_Cache.Clear();
		}

		protected Dictionary<string, LocalUnityObjctPool.Cache> m_Cache = new Dictionary<string, LocalUnityObjctPool.Cache>();

		protected class Cache
		{
			public Cache(Transform rootParent, GameObject copyItem)
			{
				this.rootParent = rootParent;
				this.copyItem = copyItem;
			}

			public void EnQueue(GameObject item)
			{
				item.gameObject.SetActive(false);
				item.transform.SetParent(this.rootParent.transform);
				this.cache.Enqueue(item);
				if (this.collection.Contains(item))
				{
					this.collection.Remove(item);
				}
			}

			public GameObject Dequeue()
			{
				return this.Dequeue("");
			}

			public GameObject Dequeue(string name)
			{
				GameObject gameObject;
				if (this.cache.Count > 0)
				{
					gameObject = this.cache.Dequeue();
				}
				else
				{
					gameObject = Object.Instantiate<GameObject>(this.copyItem);
				}
				this.collection.Add(gameObject);
				if (!string.IsNullOrEmpty(name))
				{
					gameObject.name = name;
				}
				gameObject.SetActive(true);
				return gameObject;
			}

			public void Collect()
			{
				for (int i = this.collection.Count - 1; i >= 0; i--)
				{
					this.EnQueue(this.collection[i]);
				}
				this.collection.Clear();
			}

			public void Destroy()
			{
				this.Collect();
				while (this.cache.Count > 0)
				{
					Object.DestroyImmediate(this.cache.Dequeue());
				}
			}

			public GameObject copyItem;

			private List<GameObject> collection = new List<GameObject>();

			private Queue<GameObject> cache = new Queue<GameObject>();

			private Transform rootParent;
		}
	}
}
