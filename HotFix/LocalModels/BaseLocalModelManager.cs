using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Framework;
using Framework.Platfrom;
using Framework.TableModule;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace LocalModels
{
	public abstract class BaseLocalModelManager : ITableManager
	{
		public virtual void InitialiseLocalModels()
		{
			this.DeInitialiseLocalModels();
			this.m_handlesSingle.Clear();
			this.m_handlesGroup.Clear();
		}

		public virtual void DeInitialiseLocalModels()
		{
			foreach (KeyValuePair<string, BaseLocalModel> keyValuePair in this.m_localModels)
			{
				if (keyValuePair.Value != null)
				{
					byte[] data = keyValuePair.Value.Data;
					keyValuePair.Value.DeInitialise();
				}
			}
			this.m_localModels.Clear();
			this.m_ids.Clear();
			this.UnLoadALl();
		}

		public virtual void Load(string fileName, Action callBack)
		{
			string text = Path.Combine("Assets/_Resources/LocalModel", fileName);
			text = Path.ChangeExtension(text, "bytes");
			AsyncOperationHandle<TextAsset> asyncOperationHandle;
			if (this.m_handlesSingle.TryGetValue(fileName, out asyncOperationHandle) && asyncOperationHandle.IsValid())
			{
				return;
			}
			AsyncOperationHandle<TextAsset> _handler = GameApp.Resources.LoadAssetAsync<TextAsset>(text);
			this.m_handlesSingle[fileName] = _handler;
			_handler.Completed += delegate(AsyncOperationHandle<TextAsset> x)
			{
				if (_handler.Status != 1)
				{
					HLog.LogError("<color=red>[LocalModel]</color>Load LocalModel " + fileName + " error !!!");
					return;
				}
				BaseLocalModel baseLocalModel = null;
				this.m_localModels.TryGetValue(fileName, out baseLocalModel);
				if (baseLocalModel == null)
				{
					HLog.LogError("<color=red>[LocalModel]</color>Load LocalModel " + fileName + " is null !!!");
					return;
				}
				baseLocalModel.Initialise(fileName, _handler.Result.bytes);
				if (callBack != null)
				{
					callBack();
				}
			};
		}

		public void UnLoad(string fileName)
		{
			BaseLocalModel baseLocalModel;
			this.m_localModels.TryGetValue(fileName, out baseLocalModel);
			if (baseLocalModel == null)
			{
				return;
			}
			baseLocalModel.DeInitialise();
			AsyncOperationHandle<TextAsset> asyncOperationHandle;
			if (this.m_handlesSingle.TryGetValue(fileName, out asyncOperationHandle))
			{
				GameApp.Resources.Release<TextAsset>(asyncOperationHandle);
			}
		}

		public void UnLoadALl()
		{
			foreach (KeyValuePair<string, AsyncOperationHandle<TextAsset>> keyValuePair in this.m_handlesSingle)
			{
				if (keyValuePair.Value.IsValid())
				{
					GameApp.Resources.Release<TextAsset>(keyValuePair.Value);
				}
			}
			for (int i = 0; i < this.m_handlesGroup.Count; i++)
			{
				AsyncOperationHandle<IList<TextAsset>> asyncOperationHandle = this.m_handlesGroup[i];
				if (asyncOperationHandle.IsValid())
				{
					GameApp.Resources.Release<IList<TextAsset>>(asyncOperationHandle);
				}
			}
			this.m_handlesSingle.Clear();
			this.m_handlesGroup.Clear();
		}

		public async void InitialiseLocalModelsAsync()
		{
			this.initModelsTask = TaskExpand.Run(new Action(this.InitialiseLocalModels));
			await this.initModelsTask;
		}

		public virtual async void LoadAll(Action callBack)
		{
			if (this.initModelsTask != null && !this.initModelsTask.IsCompleted)
			{
				await this.initModelsTask;
			}
			this.initModelsTask = null;
			List<string> list = this.m_localModels.Keys.ToList<string>();
			this.Loads(list, callBack);
		}

		public virtual void Loads(List<string> fileNames, Action callBack)
		{
			if (fileNames == null)
			{
				HLog.LogError("<color=red>[LocalModel]</color>Load  fileNames == null");
				return;
			}
			List<object> _filePaths = new List<object>(fileNames.Count);
			for (int i = 0; i < fileNames.Count; i++)
			{
				string filePath = this.GetFilePath(fileNames[i]);
				_filePaths.Add(filePath);
			}
			AsyncOperationHandle<IList<TextAsset>> _handler = GameApp.Resources.LoadAssetsAsync<TextAsset>(_filePaths, delegate(TextAsset x)
			{
			}, 1, true);
			this.m_handlesGroup.Add(_handler);
			_handler.Completed += delegate(AsyncOperationHandle<IList<TextAsset>> x)
			{
				if (_handler.Status != 1)
				{
					HLog.LogError("<color=red>[LocalModel]</color>Load  LocalModel " + _filePaths.ToString() + " error !!!");
					return;
				}
				this.OnLoadAllComplete(_handler.Result, callBack);
			};
		}

		private async void OnLoadAllComplete(IList<TextAsset> pList, Action callBack)
		{
			if (this.initModelsTask != null && !this.initModelsTask.IsCompleted)
			{
				await this.initModelsTask;
			}
			this.initModelsTask = null;
			List<BaseLocalModelManager.LocalModelData> list = new List<BaseLocalModelManager.LocalModelData>();
			for (int i = 0; i < pList.Count; i++)
			{
				TextAsset textAsset = pList[i];
				BaseLocalModel baseLocalModel = null;
				this.m_localModels.TryGetValue(textAsset.name, out baseLocalModel);
				if (baseLocalModel == null)
				{
					HLog.LogError("<color=red>[LocalModel]</color>Load LocalModel " + textAsset.name + " is null !!!");
					return;
				}
				list.Add(new BaseLocalModelManager.LocalModelData
				{
					fileName = textAsset.name,
					localModel = baseLocalModel,
					data = textAsset.bytes
				});
			}
			this.LoadAllLocalModels(list, callBack);
		}

		private async void LoadAllLocalModels(List<BaseLocalModelManager.LocalModelData> pDatas, Action callBack)
		{
			await TaskExpand.Run(delegate
			{
				int i = 0;
				int count = pDatas.Count;
				while (i < count)
				{
					BaseLocalModelManager.LocalModelData localModelData = pDatas[i];
					localModelData.localModel.Initialise(localModelData.fileName, localModelData.data);
					i++;
				}
			});
			if (callBack != null)
			{
				callBack();
			}
		}

		private string GetFilePath(string fileName)
		{
			return "Assets/_Resources/LocalModel/" + fileName + ".bytes";
		}

		public T GetBaseLocalModel<T>(int localModelID) where T : BaseLocalModel
		{
			BaseLocalModel baseLocalModel;
			if (this.m_ids.TryGetValue(localModelID, out baseLocalModel) && baseLocalModel != null)
			{
				return baseLocalModel as T;
			}
			return default(T);
		}

		protected Dictionary<string, BaseLocalModel> m_localModels = new Dictionary<string, BaseLocalModel>();

		protected Dictionary<int, BaseLocalModel> m_ids = new Dictionary<int, BaseLocalModel>();

		protected Dictionary<string, AsyncOperationHandle<TextAsset>> m_handlesSingle = new Dictionary<string, AsyncOperationHandle<TextAsset>>();

		protected List<AsyncOperationHandle<IList<TextAsset>>> m_handlesGroup = new List<AsyncOperationHandle<IList<TextAsset>>>();

		private Task initModelsTask;

		public class LocalModelData
		{
			public string fileName;

			public BaseLocalModel localModel;

			public byte[] data;
		}
	}
}
