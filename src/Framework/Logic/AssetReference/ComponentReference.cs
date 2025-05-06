using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.Logic.AssetReference
{
	public class ComponentReference<TComponent> : AssetReference
	{
		public ComponentReference(string guid)
			: base(guid)
		{
		}

		public AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(position, Quaternion.identity, parent), new Func<AsyncOperationHandle<GameObject>, AsyncOperationHandle<TComponent>>(this.GameObjectReady));
		}

		public AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
		{
			return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(parent, instantiateInWorldSpace), new Func<AsyncOperationHandle<GameObject>, AsyncOperationHandle<TComponent>>(this.GameObjectReady));
		}

		public AsyncOperationHandle<TComponent> LoadAssetAsync()
		{
			return Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.LoadAssetAsync<GameObject>(), new Func<AsyncOperationHandle<GameObject>, AsyncOperationHandle<TComponent>>(this.GameObjectReady));
		}

		private AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
		{
			TComponent component = arg.Result.GetComponent<TComponent>();
			return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(component, string.Empty);
		}

		public override bool ValidateAsset(Object obj)
		{
			GameObject gameObject = obj as GameObject;
			return gameObject != null && gameObject.GetComponent<TComponent>() != null;
		}

		public override bool ValidateAsset(string path)
		{
			return false;
		}

		public void ReleaseInstance(AsyncOperationHandle<TComponent> op)
		{
			Component component = op.Result as Component;
			if (component != null)
			{
				Addressables.ReleaseInstance(component.gameObject);
			}
			Addressables.Release<TComponent>(op);
		}
	}
}
