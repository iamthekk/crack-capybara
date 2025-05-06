using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Framework;
using Framework.UnityGlobalManager;
using UnityEngine;

namespace HotFix
{
	public class UserUnityGlobalManager : IUnityGlobalManager
	{
		public async void Load(Action finished)
		{
			await Task.WhenAll(new List<Task>
			{
				this.LoadCurve(),
				this.LoadGlobalGameObject()
			});
			if (finished != null)
			{
				finished();
			}
		}

		public void UnLoad(Action finished)
		{
			this.UnLoadCurve();
			this.UnLoadGlobalGameObject();
		}

		public CurveScriptable GetCurve()
		{
			return this.m_curve;
		}

		public Task LoadCurve()
		{
			UserUnityGlobalManager.<LoadCurve>d__4 <LoadCurve>d__;
			<LoadCurve>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadCurve>d__.<>4__this = this;
			<LoadCurve>d__.<>1__state = -1;
			<LoadCurve>d__.<>t__builder.Start<UserUnityGlobalManager.<LoadCurve>d__4>(ref <LoadCurve>d__);
			return <LoadCurve>d__.<>t__builder.Task;
		}

		public void UnLoadCurve()
		{
			if (this.m_curve != null)
			{
				GameApp.Resources.Release<CurveScriptable>(this.m_curve);
			}
			this.m_curve = null;
		}

		public async Task LoadGlobalGameObject()
		{
			if (this.ItemGlobalLoader != null)
			{
				this.ItemGlobalLoader.UnLoad();
				this.ItemGlobalLoader = null;
			}
			this.ItemGlobalLoader = new UserUnityGlobalManager.GlobalGameObjectLoader();
			await this.ItemGlobalLoader.Load();
		}

		public void UnLoadGlobalGameObject()
		{
			if (this.ItemGlobalLoader != null)
			{
				this.ItemGlobalLoader.UnLoad();
			}
			this.ItemGlobalLoader = null;
		}

		public GameObject GetGlobalGameObject(string path)
		{
			if (this.ItemGlobalLoader == null)
			{
				return null;
			}
			return this.ItemGlobalLoader.GetGlobalGameObject(path);
		}

		private CurveScriptable m_curve;

		public UserUnityGlobalManager.GlobalGameObjectLoader ItemGlobalLoader;

		public class GlobalGameObjectLoader
		{
			public GameObject GetGlobalGameObject(string path)
			{
				if (string.IsNullOrEmpty(path))
				{
					return null;
				}
				GameObject gameObject;
				if (this.mPrefabDic.TryGetValue(path, out gameObject))
				{
					return gameObject;
				}
				return null;
			}

			public async Task Load()
			{
				List<Task> list = new List<Task>();
				for (int i = 0; i < this._prefabs.Count; i++)
				{
					list.Add(this.LoadGameObject(this._prefabs[i]));
				}
				await Task.WhenAll(list);
			}

			private Task LoadGameObject(string path)
			{
				UserUnityGlobalManager.GlobalGameObjectLoader.<LoadGameObject>d__5 <LoadGameObject>d__;
				<LoadGameObject>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
				<LoadGameObject>d__.<>4__this = this;
				<LoadGameObject>d__.path = path;
				<LoadGameObject>d__.<>1__state = -1;
				<LoadGameObject>d__.<>t__builder.Start<UserUnityGlobalManager.GlobalGameObjectLoader.<LoadGameObject>d__5>(ref <LoadGameObject>d__);
				return <LoadGameObject>d__.<>t__builder.Task;
			}

			public void UnLoad()
			{
				this.mReleased = true;
				foreach (KeyValuePair<string, GameObject> keyValuePair in this.mPrefabDic)
				{
					GameApp.Resources.Release<GameObject>(keyValuePair.Value);
				}
				this.mPrefabDic.Clear();
			}

			private List<string> _prefabs = new List<string> { "Assets/_Resources/Prefab/UI/Common/Item/Item_Star.prefab", "Assets/_Resources/Prefab/UI/Common/Item/Item_Header.prefab", "Assets/_Resources/Prefab/UI/Common/Item/Item_Slider.prefab" };

			private Dictionary<string, GameObject> mPrefabDic = new Dictionary<string, GameObject>();

			private bool mReleased;
		}
	}
}
