using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class PoolManager
	{
		public static PoolManager Instance
		{
			get
			{
				if (PoolManager.instance == null)
				{
					PoolManager.instance = new PoolManager();
					PoolManager.trParentRoot = new GameObject("PoolManager").transform;
					PoolManager.trParentRoot.position = new Vector3(2000f, 2000f, 2000f);
					Object.DontDestroyOnLoad(PoolManager.trParentRoot);
					PoolManager.trParentRoot.gameObject.SetActive(false);
				}
				return PoolManager.instance;
			}
		}

		private PoolManager()
		{
		}

		public async Task CheckPrefab(string path)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle;
			if (!this.m_asynsObjects.TryGetValue(path, out asyncOperationHandle))
			{
				AsyncOperationHandle<GameObject> asyHandler = GameApp.Resources.LoadAssetAsync<GameObject>(path);
				await asyHandler.Task;
				if (asyHandler.Status != 1)
				{
					HLog.LogError("CheckPrefab Load Prefab is null! path = " + path + " ");
				}
				else
				{
					AsyncOperationHandle<GameObject> asyncOperationHandle2 = asyHandler;
					this.m_asynsObjects[path] = asyncOperationHandle2;
					asyHandler = default(AsyncOperationHandle<GameObject>);
				}
			}
		}

		public GameObject GetAsset(string path)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle;
			if (this.m_asynsObjects.TryGetValue(path, out asyncOperationHandle))
			{
				return asyncOperationHandle.Result;
			}
			return null;
		}

		public GameObject Out(string path, Vector3 position, Quaternion rotation, Transform parent = null)
		{
			AsyncOperationHandle<GameObject> asyncOperationHandle;
			if (!this.m_asynsObjects.TryGetValue(path, out asyncOperationHandle))
			{
				HLog.LogError("Out Load Prefab is null! path = " + path + " ");
				return null;
			}
			if (asyncOperationHandle.Result == null)
			{
				return null;
			}
			List<int> list;
			if (!this.m_inObjects.TryGetValue(path, out list))
			{
				list = new List<int>();
				this.m_inObjects[path] = list;
			}
			if (list.Count < 1)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(asyncOperationHandle.Result, Vector3.zero, Quaternion.identity, PoolManager.trParentRoot);
				int instanceID = gameObject.GetInstanceID();
				this.m_instanceIDs[instanceID] = gameObject;
				list.Add(instanceID);
				this.m_inObjects[path] = list;
				this.m_paths[instanceID] = path;
			}
			int num = list[list.Count - 1];
			GameObject gameObject2 = this.m_instanceIDs[num];
			list.RemoveAt(list.Count - 1);
			this.m_inObjects[path] = list;
			List<int> list2;
			if (!this.m_outObjects.TryGetValue(path, out list2))
			{
				list2 = new List<int>();
				this.m_outObjects[path] = list2;
			}
			list2.Add(num);
			this.m_outObjects[path] = list2;
			gameObject2.transform.SetPositionAndRotation(position, rotation);
			gameObject2.transform.SetParent(parent);
			gameObject2.transform.localScale = Vector3.one;
			return gameObject2;
		}

		public bool Put(GameObject target)
		{
			if (target == null)
			{
				return false;
			}
			int instanceID = target.GetInstanceID();
			string text;
			if (!this.m_paths.TryGetValue(instanceID, out text))
			{
				return false;
			}
			List<int> list;
			this.m_outObjects.TryGetValue(text, out list);
			if (list == null)
			{
				return false;
			}
			list.Remove(instanceID);
			this.m_outObjects[text] = list;
			List<int> list2;
			this.m_inObjects.TryGetValue(text, out list2);
			if (list2 == null)
			{
				return false;
			}
			if (!list2.Contains(instanceID))
			{
				list2.Add(instanceID);
			}
			this.m_inObjects[text] = list2;
			if (PoolManager.trParentRoot == null)
			{
				return false;
			}
			target.transform.SetParent(PoolManager.trParentRoot);
			target.transform.SetPositionAndRotation(PoolManager.trParentRoot.position, PoolManager.trParentRoot.rotation);
			target.transform.localScale = Vector3.one;
			return true;
		}

		public async Task Cache(string path)
		{
			await this.CheckPrefab(path);
			AsyncOperationHandle<GameObject> asyncOperationHandle;
			if (!this.m_asynsObjects.TryGetValue(path, out asyncOperationHandle))
			{
				HLog.LogError(HLog.ToColor("Cache Load Prefab is null! path = " + path + " ", 3));
			}
			else if (!(asyncOperationHandle.Result == null))
			{
				List<int> list;
				if (!this.m_inObjects.TryGetValue(path, out list))
				{
					list = new List<int>();
					this.m_inObjects[path] = list;
				}
				if (list.Count < 1)
				{
					GameObject gameObject = Object.Instantiate<GameObject>(asyncOperationHandle.Result, PoolManager.trParentRoot.position, PoolManager.trParentRoot.rotation, PoolManager.trParentRoot);
					int instanceID = gameObject.GetInstanceID();
					this.m_instanceIDs[instanceID] = gameObject;
					list.Add(instanceID);
					this.m_inObjects[path] = list;
					this.m_paths[instanceID] = path;
					List<int> list2;
					this.m_inObjects.TryGetValue(path, out list2);
					if (!list2.Contains(instanceID))
					{
						list2.Add(instanceID);
					}
					this.m_inObjects[path] = list2;
				}
			}
		}

		private Task DestroyPool()
		{
			List<GameObject> list = this.m_instanceIDs.Values.ToList<GameObject>();
			for (int i = 0; i < list.Count; i++)
			{
				if (!(list[i] == null))
				{
					Object.Destroy(list[i]);
					list[i] = null;
				}
			}
			this.m_inObjects.Clear();
			this.m_outObjects.Clear();
			this.m_instanceIDs.Clear();
			this.m_paths.Clear();
			return Task.CompletedTask;
		}

		private void ReleasePool()
		{
			List<AsyncOperationHandle<GameObject>> list = this.m_asynsObjects.Values.ToList<AsyncOperationHandle<GameObject>>();
			for (int i = 0; i < list.Count; i++)
			{
				GameApp.Resources.Release<GameObject>(list[i]);
			}
			this.m_asynsObjects.Clear();
		}

		public async Task OnClear()
		{
			await this.DestroyPool();
			this.ReleasePool();
		}

		private static Transform trParentRoot;

		private Dictionary<string, AsyncOperationHandle<GameObject>> m_asynsObjects = new Dictionary<string, AsyncOperationHandle<GameObject>>();

		private Dictionary<string, List<int>> m_inObjects = new Dictionary<string, List<int>>();

		private Dictionary<string, List<int>> m_outObjects = new Dictionary<string, List<int>>();

		private Dictionary<int, GameObject> m_instanceIDs = new Dictionary<int, GameObject>();

		private Dictionary<int, string> m_paths = new Dictionary<int, string>();

		private static PoolManager instance;
	}
}
