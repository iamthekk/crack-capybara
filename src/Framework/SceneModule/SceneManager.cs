﻿using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Framework.SceneModule
{
	public class SceneManager : MonoBehaviour
	{
		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(object key, LoadSceneMode loadMode = 0, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.LoadSceneAsync(key, loadMode, activateOnLoad, priority);
		}

		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(IResourceLocation location, LoadSceneMode loadMode = 0, bool activateOnLoad = true, int priority = 100)
		{
			return Addressables.LoadSceneAsync(location, loadMode, activateOnLoad, priority);
		}

		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(SceneInstance scene, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(scene, autoReleaseHandle);
		}

		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle handle, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(handle, autoReleaseHandle);
		}

		public AsyncOperationHandle<SceneInstance> UnloadSceneAsync(AsyncOperationHandle<SceneInstance> handle, bool autoReleaseHandle = true)
		{
			return Addressables.UnloadSceneAsync(handle, autoReleaseHandle);
		}
	}
}
