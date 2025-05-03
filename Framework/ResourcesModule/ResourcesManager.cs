using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Framework.ResourcesModule
{
	public class ResourcesManager : MonoBehaviour
	{
		public IInstanceProvider InstanceProvider
		{
			get
			{
				return Addressables.InstanceProvider;
			}
		}

		public string ResolveInternalId(string id)
		{
			return Addressables.ResolveInternalId(id);
		}

		public Func<IResourceLocation, string> InternalIdTransformFunc
		{
			get
			{
				return Addressables.InternalIdTransformFunc;
			}
			set
			{
				Addressables.InternalIdTransformFunc = value;
			}
		}

		public string StreamingAssetsSubFolder
		{
			get
			{
				return Addressables.StreamingAssetsSubFolder;
			}
		}

		public string BuildPath
		{
			get
			{
				return Addressables.BuildPath;
			}
		}

		public string PlayerBuildDataPath
		{
			get
			{
				return Addressables.PlayerBuildDataPath;
			}
		}

		public string RuntimePath
		{
			get
			{
				return Addressables.RuntimePath;
			}
		}

		public AsyncOperationHandle<IResourceLocator> InitializeAsync()
		{
			return Addressables.InitializeAsync();
		}

		public AsyncOperationHandle<IResourceLocator> LoadContentCatalogAsync(string catalogPath, string providerSuffix = null)
		{
			return Addressables.LoadContentCatalogAsync(catalogPath, false, providerSuffix);
		}

		public AsyncOperationHandle<IResourceLocator> LoadContentCatalogAsync(string catalogPath, bool autoReleaseHandle, string providerSuffix = null)
		{
			return Addressables.LoadContentCatalogAsync(catalogPath, autoReleaseHandle, providerSuffix);
		}

		public AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(IResourceLocation location)
		{
			return Addressables.LoadAssetAsync<TObject>(location);
		}

		public AsyncOperationHandle<TObject> LoadAssetAsync<TObject>(object key)
		{
			return Addressables.LoadAssetAsync<TObject>(key);
		}

		[Obsolete]
		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(IList<object> keys, Addressables.MergeMode mode, Type type = null)
		{
			return Addressables.LoadResourceLocationsAsync(keys, mode, type);
		}

		public AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocationsAsync(object key, Type type = null)
		{
			return Addressables.LoadResourceLocationsAsync(key, type);
		}

		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<IResourceLocation> locations, Action<TObject> callback)
		{
			return Addressables.LoadAssetsAsync<TObject>(locations, callback, true);
		}

		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<IResourceLocation> locations, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return Addressables.LoadAssetsAsync<TObject>(locations, callback, releaseDependenciesOnFailure);
		}

		[Obsolete]
		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<object> keys, Action<TObject> callback, Addressables.MergeMode mode)
		{
			return Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, true);
		}

		[Obsolete]
		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IList<object> keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			return Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, releaseDependenciesOnFailure);
		}

		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(IEnumerable keys, Action<TObject> callback, Addressables.MergeMode mode, bool releaseDependenciesOnFailure)
		{
			return Addressables.LoadAssetsAsync<TObject>(keys, callback, mode, releaseDependenciesOnFailure);
		}

		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(object key, Action<TObject> callback)
		{
			return Addressables.LoadAssetsAsync<TObject>(key, callback, true);
		}

		public AsyncOperationHandle<IList<TObject>> LoadAssetsAsync<TObject>(object key, Action<TObject> callback, bool releaseDependenciesOnFailure)
		{
			return Addressables.LoadAssetsAsync<TObject>(key, callback, releaseDependenciesOnFailure);
		}

		public void Release<TObject>(TObject obj)
		{
			Addressables.Release<TObject>(obj);
		}

		public void Release<TObject>(AsyncOperationHandle<TObject> handle)
		{
			Addressables.Release<TObject>(handle);
		}

		public void Release(AsyncOperationHandle handle)
		{
			Addressables.Release(handle);
		}

		public bool ReleaseInstance(GameObject instance)
		{
			return Addressables.ReleaseInstance(instance);
		}

		public bool ReleaseInstance(AsyncOperationHandle handle)
		{
			return Addressables.ReleaseInstance(handle);
		}

		public bool ReleaseInstance(AsyncOperationHandle<GameObject> handle)
		{
			return Addressables.ReleaseInstance(handle);
		}

		public AsyncOperationHandle<long> GetDownloadSizeAsync(object key)
		{
			return Addressables.GetDownloadSizeAsync(key);
		}

		[Obsolete]
		public AsyncOperationHandle<long> GetDownloadSizeAsync(IList<object> keys)
		{
			return Addressables.GetDownloadSizeAsync(keys);
		}

		public AsyncOperationHandle DownloadDependenciesAsync(object key, bool autoReleaseHandle = false)
		{
			return Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
		}

		public AsyncOperationHandle DownloadDependenciesAsync(IList<IResourceLocation> locations, bool autoReleaseHandle = false)
		{
			return Addressables.DownloadDependenciesAsync(locations, autoReleaseHandle);
		}

		[Obsolete]
		public AsyncOperationHandle DownloadDependenciesAsync(IList<object> keys, Addressables.MergeMode mode, bool autoReleaseHandle = false)
		{
			return Addressables.DownloadDependenciesAsync(keys, mode, autoReleaseHandle);
		}

		public void ClearDependencyCacheAsync(object key)
		{
			Addressables.ClearDependencyCacheAsync(key);
		}

		public void ClearDependencyCacheAsync(IList<IResourceLocation> locations)
		{
			Addressables.ClearDependencyCacheAsync(locations);
		}

		[Obsolete]
		public void ClearDependencyCacheAsync(IList<object> keys)
		{
			Addressables.ClearDependencyCacheAsync(keys);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, new InstantiationParameters(parent, instantiateInWorldSpace), trackHandle);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, position, rotation, parent, trackHandle);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, Transform parent = null, bool instantiateInWorldSpace = false, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, parent, instantiateInWorldSpace, trackHandle);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, Vector3 position, Quaternion rotation, Transform parent = null, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, position, rotation, parent, trackHandle);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(object key, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(key, instantiateParameters, trackHandle);
		}

		public AsyncOperationHandle<GameObject> InstantiateAsync(IResourceLocation location, InstantiationParameters instantiateParameters, bool trackHandle = true)
		{
			return Addressables.InstantiateAsync(location, instantiateParameters, trackHandle);
		}

		public AsyncOperationHandle<List<string>> CheckForCatalogUpdates(bool autoReleaseHandle = true)
		{
			return Addressables.CheckForCatalogUpdates(autoReleaseHandle);
		}

		public AsyncOperationHandle<List<IResourceLocator>> UpdateCatalogs(IEnumerable<string> catalogs = null, bool autoReleaseHandle = true)
		{
			return Addressables.UpdateCatalogs(catalogs, autoReleaseHandle);
		}

		public void AddResourceLocator(IResourceLocator locator, string localCatalogHash = null, IResourceLocation remoteCatalogLocation = null)
		{
			Addressables.AddResourceLocator(locator, localCatalogHash, remoteCatalogLocation);
		}

		public void RemoveResourceLocator(IResourceLocator locator)
		{
			Addressables.RemoveResourceLocator(locator);
		}

		public void ClearResourceLocators()
		{
			Addressables.ClearResourceLocators();
		}

		public bool AgreeNotReachableDownload { get; set; }

		public async void CheckUpdate(Action<bool, long> update, Action<long> downPrompt, Action<float, long, long> progressSize, Action<bool, string> updataFinished)
		{
			this.m_progressSize = progressSize;
			this.m_downPrompt = downPrompt;
			this.m_needUpdate = update;
			this.m_finished = updataFinished;
			this.m_isUpdate = false;
			this.OnCheckUpdate();
			await Task.CompletedTask;
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (!this.m_isUpdate)
			{
				return;
			}
			if (this.m_progressSize == null)
			{
				return;
			}
			if (this.m_downHandle.IsDone)
			{
				return;
			}
			this.m_downloadStatus = this.m_downHandle.GetDownloadStatus();
			this.m_totalDownloadSize = this.m_downloadStatus.TotalBytes;
			this.m_currentDownloadSize = this.m_downloadStatus.DownloadedBytes;
			if (this.m_currentDownloadSize >= this.m_totalDownloadSize)
			{
				return;
			}
			this.m_progressSize(this.m_downloadStatus.Percent, this.m_currentDownloadSize, this.m_totalDownloadSize);
		}

		private void OnCheckUpdate()
		{
			ResourcesManager.<OnCheckUpdate>d__76 <OnCheckUpdate>d__;
			<OnCheckUpdate>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<OnCheckUpdate>d__.<>4__this = this;
			<OnCheckUpdate>d__.<>1__state = -1;
			<OnCheckUpdate>d__.<>t__builder.Start<ResourcesManager.<OnCheckUpdate>d__76>(ref <OnCheckUpdate>d__);
		}

		public async Task OnDownAssets()
		{
			this.m_isUpdate = true;
			for (;;)
			{
				if (this.m_downHandle.IsValid())
				{
					Addressables.Release(this.m_downHandle);
				}
				this.m_downHandle = Addressables.DownloadDependenciesAsync(this.m_downLoads, 1, false);
				await this.m_downHandle.Task;
				if (this.m_downHandle.Status == 1)
				{
					goto IL_01E1;
				}
				if (Application.internetReachability == 2)
				{
					await Task.Delay(500);
				}
				else
				{
					if (!this.AgreeNotReachableDownload)
					{
						break;
					}
					await Task.Delay(500);
				}
			}
			if (this.m_downHandle.IsValid())
			{
				Addressables.Release(this.m_downHandle);
			}
			this.m_isUpdate = false;
			Action<bool, string> finished = this.m_finished;
			if (finished != null)
			{
				finished(false, this.Error_DownloadDependenciesAsync.ToString());
			}
			return;
			IL_01E1:
			GameApp.SDK.Analyze.TrackHotUpdate("success", 0L, "", "");
			this.Release(this.m_downHandle);
			this.m_downLoads.Clear();
			this.m_isUpdate = false;
			Action<float, long, long> progressSize = this.m_progressSize;
			if (progressSize != null)
			{
				progressSize(1f, this.m_totalDownloadSize, this.m_totalDownloadSize);
			}
			Action<bool, string> finished2 = this.m_finished;
			if (finished2 != null)
			{
				finished2(true, string.Empty);
			}
		}

		public void SwitchPreHotFix()
		{
			if (PlayerPrefs.GetInt("SwitchPreHotFix", 0) > 0)
			{
				this.SwitchToPreReleaseCdn();
			}
		}

		private void SwitchToPreReleaseCdn()
		{
			Addressables.InternalIdTransformFunc = new Func<IResourceLocation, string>(ResourcesManager.LoadFunc);
		}

		private static string LoadFunc(IResourceLocation location)
		{
			if (location.ResourceType == typeof(IAssetBundleResource) && location.InternalId.StartsWith("http"))
			{
				return ResourcesManager.ReplaceUrl(location.InternalId);
			}
			if (location.ResourceType == typeof(ContentCatalogData) && location.InternalId.StartsWith("http"))
			{
				return ResourcesManager.ReplaceUrl(location.InternalId);
			}
			if (location.PrimaryKey == "AddressablesMainContentCatalogRemoteHash")
			{
				return ResourcesManager.ReplaceUrl(location.InternalId);
			}
			return location.InternalId;
		}

		private static string ReplaceUrl(string internalId)
		{
			string resourcesUrl = Singleton<PathManager>.Instance.GetResourcesUrl(true);
			string resourcesUrl2 = Singleton<PathManager>.Instance.GetResourcesUrl(false);
			return internalId.Replace(resourcesUrl, resourcesUrl2);
		}

		private bool m_isUpdate;

		private AsyncOperationHandle m_downHandle;

		private Action<bool, long> m_needUpdate;

		private Action<float, long, long> m_progressSize;

		private Action<long> m_downPrompt;

		private Action<bool, string> m_finished;

		private long m_totalDownloadSize;

		private long m_currentDownloadSize;

		private DownloadStatus m_downloadStatus;

		protected List<string> m_downLoads = new List<string>();

		private readonly int Error_InitializeAsync = 501;

		private readonly int Error_CheckForCatalogUpdates = 502;

		private readonly int Error_NotReachable = 503;

		private readonly int Error_UpdateCatalogs = 504;

		private readonly int Error_LoadResourceLocationsAsync = 505;

		private readonly int Error_GetDownloadSizeAsync = 506;

		private readonly int Error_DownloadDependenciesAsync = 507;
	}
}
