using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.EventSystem;
using Framework.Logic.AttributeExpansion;
using Framework.Logic.Modules;
using Framework.ResourcesModule;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Framework.ViewModule
{
	public sealed class ViewModuleManager : MonoBehaviour, IModuleManager
	{
		public UIPool Pool
		{
			get
			{
				return this.m_uiPool;
			}
		}

		public Camera UICamera
		{
			get
			{
				return this.m_uiCamera;
			}
			private set
			{
				this.m_uiCamera = value;
			}
		}

		private void Awake()
		{
			this.Init();
		}

		public void Init()
		{
			if (this.isInited)
			{
				return;
			}
			this.isInited = true;
			this.InitLayerSortingOrderData();
		}

		public void RegisterViewModule(ViewModuleData viewModuleData)
		{
			if (viewModuleData == null)
			{
				return;
			}
			this.m_viewModuleDatas[viewModuleData.m_id] = viewModuleData;
		}

		public void UnRegisterViewModule(ViewModuleData viewModuleData)
		{
			if (viewModuleData == null)
			{
				return;
			}
			this.m_viewModuleDatas.Remove(viewModuleData.m_id);
		}

		public void UnRegisterAllViewModule(params int[] ignoreIDs)
		{
			List<ViewModuleData> list = new List<ViewModuleData>();
			foreach (KeyValuePair<int, ViewModuleData> keyValuePair in this.m_viewModuleDatas)
			{
				if (keyValuePair.Value != null && (ignoreIDs == null || !ignoreIDs.Contains(keyValuePair.Key)))
				{
					list.Add(keyValuePair.Value);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				ViewModuleData viewModuleData = list[i];
				if (viewModuleData != null)
				{
					this.UnRegisterViewModule(viewModuleData);
				}
			}
		}

		public ViewModuleData GetViewModuleData(int viewName)
		{
			ViewModuleData viewModuleData = null;
			this.m_viewModuleDatas.TryGetValue(viewName, out viewModuleData);
			return viewModuleData;
		}

		public T GetViewModule<T>(int viewName) where T : BaseViewModule
		{
			return this.GetViewModuleData(viewName).m_viewModule as T;
		}

		public async Task OpenView(int viewName, object data = null, UILayers layer = UILayers.First, Action<GameObject> loadedCallBack = null, Action<GameObject> openedCallBack = null)
		{
			await this.OpenViewInternal(viewName, data, layer, loadedCallBack, openedCallBack);
		}

		private async Task OpenViewInternal(int viewName, object data = null, UILayers layer = UILayers.First, Action<GameObject> loadedCallBack = null, Action<GameObject> openedCallBack = null)
		{
			if (!this.IsOpenedOrLoading(viewName))
			{
				ViewModuleData _viewModuleData = null;
				this.m_viewModuleDatas.TryGetValue(viewName, out _viewModuleData);
				if (_viewModuleData == null)
				{
					HLog.LogError(string.Format("[ViewModule]OpenView viewModuleData is null , viewName = {0},UIlayers = {1}", viewName, layer));
				}
				else
				{
					if (_viewModuleData.m_gameObject == null)
					{
						if (_viewModuleData.m_prefab == null)
						{
							this.SetMask(true);
							_viewModuleData.m_viewState = ViewState.Loading;
							AsyncOperationHandle<GameObject> _handler = this.m_resourcesManager.LoadAssetAsync<GameObject>(_viewModuleData.m_assetPath);
							await _handler.Task;
							if (_handler.Status != 1)
							{
								HLog.LogError(string.Format("[ViewModule]OpenView viewModuleData loading Failed , viewName = {0},UIlayers = {1}", viewName, layer));
								this.SetMask(false);
								return;
							}
							if (_viewModuleData.m_viewState != ViewState.Loading)
							{
								HLog.LogError(string.Format("[ViewModule]OpenView viewModuleData not loading , viewName = {0},UIlayers = {1},state = {2}", viewName, layer, _viewModuleData.m_viewState));
								this.SetMask(false);
								return;
							}
							_viewModuleData.m_prefab = _handler.Result;
							if (_viewModuleData.m_loader != null)
							{
								await _viewModuleData.m_loader.OnLoad(data);
							}
							this.SetMask(false);
							if (loadedCallBack != null)
							{
								loadedCallBack(_viewModuleData.m_prefab);
							}
							_viewModuleData.m_gameObject = this.InstantiateByPrefab(_viewModuleData.m_id, _viewModuleData.m_prefab, layer);
							_viewModuleData.m_viewModule = _viewModuleData.m_gameObject.GetComponent<BaseViewModule>();
							if (_viewModuleData.m_viewModule == null)
							{
								HLog.LogError(string.Format("[ViewModule]OpenView viewModuleData not loading , viewName = {0},UIlayers = {1} ,path = {2} viewModule  is null!!!", viewName, layer, _viewModuleData.m_assetPath));
								return;
							}
							_viewModuleData.m_viewModule.SetViewData(_viewModuleData.m_id);
							_viewModuleData.m_viewModule.SetLoader(_viewModuleData.m_loader);
							_viewModuleData.m_viewModule.OnCreate(data);
							if (this.IsDebug && !_viewModuleData.m_gameObject.name.Contains("NetLoading"))
							{
								ViewModuleManager.reuseCount--;
							}
							this.OpenViewByGameObject(_viewModuleData, data, openedCallBack, layer);
							_viewModuleData.m_viewState = ViewState.Opened;
							_handler = default(AsyncOperationHandle<GameObject>);
						}
						else
						{
							_viewModuleData.m_gameObject = this.InstantiateByPrefab(_viewModuleData.m_id, _viewModuleData.m_prefab, layer);
							_viewModuleData.m_viewModule = _viewModuleData.m_gameObject.GetComponent<BaseViewModule>();
							if (_viewModuleData.m_viewModule == null)
							{
								HLog.LogError(string.Format("[ViewModule]viewModuleData not loading , viewName = {0},UIlayers = {1} ,path = {2} viewModule  is null!!!", viewName, layer, _viewModuleData.m_assetPath));
								return;
							}
							this.OpenViewByGameObject(_viewModuleData, data, openedCallBack, layer);
							_viewModuleData.m_viewState = ViewState.Opened;
						}
					}
					else
					{
						_viewModuleData.m_viewModule = _viewModuleData.m_gameObject.GetComponent<BaseViewModule>();
						if (_viewModuleData.m_viewModule == null)
						{
							HLog.LogError(string.Concat(new string[]
							{
								"[ViewModule]OpenView viewModuleData not loading ,",
								string.Format("viewName = {0},", viewName),
								string.Format("UIlayers = {0} ,", layer),
								"path = ",
								_viewModuleData.m_assetPath,
								" ,objname=",
								_viewModuleData.m_gameObject.name,
								"viewModule  is null!!!"
							}));
							return;
						}
						this.OpenViewByGameObject(_viewModuleData, data, openedCallBack, layer);
						_viewModuleData.m_viewState = ViewState.Opened;
					}
					_viewModuleData.lastCloseTime = float.MaxValue;
				}
			}
		}

		private GameObject InstantiateByPrefab(int viewName, GameObject prefab, UILayers layer)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(prefab);
			this.SetLayer(viewName, gameObject, layer);
			return gameObject;
		}

		private void SetLayer(int viewName, GameObject obj, UILayers layer)
		{
			GameObject gameObjectByUILayers = this.GetGameObjectByUILayers(layer);
			if (gameObjectByUILayers == null)
			{
				HLog.LogError(string.Format("[ViewModule]layer gameObject is null ,viewName = {0},UIlayers = {1}", viewName, layer));
				return;
			}
			RectTransform rectTransform = (RectTransform)obj.transform;
			rectTransform.SetParent(gameObjectByUILayers.transform);
			rectTransform.sizeDelta = Vector3.zero;
			rectTransform.localScale = Vector3.one;
			rectTransform.localPosition = Vector3.zero;
			rectTransform.localRotation = Quaternion.identity;
			rectTransform.SetAsLastSibling();
			this.m_layers[viewName] = layer;
		}

		private void OpenViewByGameObject(ViewModuleData viewData, object data, Action<GameObject> openedCallBack, UILayers layer)
		{
			if (this.IsDebug)
			{
				Debug.Log("开启页面：".ToColor(DebugColor.Blue) + viewData.m_gameObject.name);
			}
			try
			{
				string name = viewData.m_gameObject.name;
				this.SetLayer(viewData.m_id, viewData.m_gameObject, layer);
				viewData.m_gameObject.SetActive(true);
				((RectTransform)viewData.m_gameObject.transform).SetAsLastSibling();
				viewData.layerId = (int)layer;
				this.SetViewModuleSortingOrder(viewData.m_viewModule, (int)layer);
				viewData.m_viewModule.RegisterEvents(this.m_eventSystemManager);
				viewData.m_viewModule.OnOpen(data);
				if (this.IsDebug && !viewData.m_gameObject.name.Contains("NetLoading"))
				{
					ViewModuleManager.openCount++;
					ViewModuleManager.reuseCount++;
					float num = ((ViewModuleManager.reuseCount == 0) ? 0f : ((float)ViewModuleManager.reuseCount / (float)ViewModuleManager.openCount));
					Debug.Log("展示UI次数-服用UI次数-服用百分比：".ToColor(DebugColor.Green) + string.Format("{0}-{1}-{2}%", ViewModuleManager.openCount, ViewModuleManager.reuseCount, (num * 100f).ToString("F2")));
				}
				if (openedCallBack != null)
				{
					openedCallBack(viewData.m_gameObject);
				}
			}
			catch (Exception ex)
			{
				HLog.LogError(string.Format("OpenViewByGameObject has exception, id={0},path= {1}", viewData.m_id, viewData.m_assetPath));
				HLog.LogException(ex);
			}
		}

		public void CloseView(BaseViewModule baseViewModule, Action onFinish = null)
		{
			this.CloseView(baseViewModule.m_viewName, onFinish);
		}

		public void CloseView(int viewName, Action onFinish = null)
		{
			ViewModuleData viewModuleData = null;
			this.m_viewModuleDatas.TryGetValue(viewName, out viewModuleData);
			if (viewModuleData == null)
			{
				HLog.LogError(string.Format("[ViewModule]CloseView viewModuleData is null , id = {0}", viewName));
				return;
			}
			if (viewModuleData.m_gameObject == null)
			{
				HLog.LogError(string.Format("[ViewModule]CloseView viewModuleData gameObject is null , id = {0},path= {1}", viewName, viewModuleData.m_assetPath));
				viewModuleData.m_viewState = ViewState.Null;
				return;
			}
			if (viewModuleData.m_viewModule == null)
			{
				HLog.LogError(string.Format("[ViewModule]CloseView viewModuleData gameObject is null , viewName = {0},objName={1}", viewName, viewModuleData.m_gameObject.name));
				viewModuleData.m_viewState = ViewState.Null;
				return;
			}
			viewModuleData.m_viewModule.UnRegisterEvents(this.m_eventSystemManager);
			viewModuleData.m_viewModule.OnClose();
			switch (viewModuleData.m_destoryType)
			{
			case DestoryType.Dont:
				viewModuleData.m_gameObject.SetActive(false);
				break;
			case DestoryType.Immediate:
				this.DestroyUI(viewModuleData);
				break;
			case DestoryType.Auto:
				viewModuleData.m_gameObject.SetActive(false);
				this.SignDestroyUI(viewModuleData);
				break;
			default:
				this.DestroyUI(viewModuleData);
				break;
			}
			viewModuleData.m_viewState = ViewState.Closed;
			this.DynamicCalcLayerCurSortingOrderValue(viewModuleData.layerId);
			if (onFinish != null)
			{
				onFinish();
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Framework_CloseView, null);
		}

		private void SignDestroyUI(ViewModuleData _viewModuleData)
		{
			_viewModuleData.lastCloseTime = Time.time;
		}

		private void DestroyUI(ViewModuleData _viewModuleData)
		{
			if (_viewModuleData.m_viewModule != null)
			{
				_viewModuleData.m_viewModule.OnDelete();
			}
			if (_viewModuleData.m_viewModule != null)
			{
				_viewModuleData.m_viewModule.SetLoader(null);
			}
			Object.Destroy(_viewModuleData.m_gameObject);
			if (_viewModuleData.m_loader != null)
			{
				_viewModuleData.m_loader.OnUnLoad();
			}
			_viewModuleData.m_gameObject = null;
			if (_viewModuleData.m_prefab != null)
			{
				this.m_resourcesManager.Release<GameObject>(_viewModuleData.m_prefab);
			}
			_viewModuleData.m_prefab = null;
			_viewModuleData.m_viewModule = null;
		}

		public void TryDestroyAllCacheUI()
		{
			foreach (KeyValuePair<int, ViewModuleData> keyValuePair in this.m_viewModuleDatas)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.m_destoryType == DestoryType.Auto && keyValuePair.Value.m_viewState != ViewState.Opened)
				{
					this.DestroyUI(keyValuePair.Value);
				}
			}
		}

		private void CheckDestroyUI(ViewModuleData viewModuleData)
		{
			if (Time.time - viewModuleData.lastCloseTime > this.uiMaxLifeTime)
			{
				this.DestroyUI(viewModuleData);
			}
		}

		public GameObject GetGameObjectByUILayers(UILayers uilayer)
		{
			GameObject gameObject = null;
			if (this.Pool != null && uilayer < (UILayers)this.m_layerObjects.Length)
			{
				gameObject = this.m_layerObjects[(int)uilayer];
			}
			return gameObject;
		}

		public void CloseAllView(int[] ignoreIDs = null)
		{
			foreach (int num in this.m_viewModuleDatas.Keys.ToArray<int>())
			{
				if (this.IsOpened(num) && (ignoreIDs == null || !ignoreIDs.Contains(num)))
				{
					this.CloseView(num, null);
				}
			}
		}

		public void CloseAllView(UILayers[] layers, int[] ignoreIDs = null)
		{
			if (layers == null)
			{
				return;
			}
			foreach (int num in this.m_viewModuleDatas.Keys.ToArray<int>())
			{
				if (this.IsOpened(num) && (ignoreIDs == null || !ignoreIDs.Contains(num)))
				{
					UILayers uilayers = UILayers.First;
					if (this.m_layers.TryGetValue(num, out uilayers) && layers.Contains(uilayers))
					{
						this.CloseView(num, null);
					}
				}
			}
		}

		public bool IsOpened(int viewName)
		{
			ViewModuleData viewModuleData = null;
			this.m_viewModuleDatas.TryGetValue(viewName, out viewModuleData);
			if (viewModuleData == null)
			{
				HLog.LogError("[ViewModule]CloseView viewModuleData is null , viewName:", viewName.ToString());
				return false;
			}
			return viewModuleData.m_viewState == ViewState.Opened;
		}

		public bool IsLoading(int viewName)
		{
			ViewModuleData viewModuleData = null;
			this.m_viewModuleDatas.TryGetValue(viewName, out viewModuleData);
			if (viewModuleData == null)
			{
				HLog.LogError("[ViewModule]CloseView viewModuleData is null , viewName:", viewName.ToString());
				return false;
			}
			return viewModuleData.m_viewState == ViewState.Loading;
		}

		public bool IsOpenedOrLoading(int viewName)
		{
			return this.IsLoading(viewName) || this.IsOpened(viewName);
		}

		public void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			foreach (KeyValuePair<int, ViewModuleData> keyValuePair in this.m_viewModuleDatas)
			{
				if (keyValuePair.Value.m_viewModule != null && keyValuePair.Value.m_gameObject != null)
				{
					if (keyValuePair.Value.m_viewState == ViewState.Opened)
					{
						keyValuePair.Value.m_viewModule.OnUpdate(deltaTime, unscaledDeltaTime);
					}
					else if (keyValuePair.Value.m_destoryType == DestoryType.Auto)
					{
						this.CheckDestroyUI(keyValuePair.Value);
					}
				}
			}
		}

		public string GetViewNameString(int viewName)
		{
			if (this.m_funcAnalysisViewName != null)
			{
				return this.m_funcAnalysisViewName(viewName);
			}
			return viewName.ToString();
		}

		public List<ViewModuleData> GetOpenList()
		{
			List<ViewModuleData> list = new List<ViewModuleData>();
			foreach (ViewModuleData viewModuleData in this.m_viewModuleDatas.Values)
			{
				if (viewModuleData != null && !(viewModuleData.m_gameObject == null) && viewModuleData.m_gameObject.activeSelf && viewModuleData.m_viewState == ViewState.Opened)
				{
					list.Add(viewModuleData);
				}
			}
			return list;
		}

		public bool IsLayerTop(int viewId)
		{
			ViewModuleData viewModuleData;
			if (this.m_viewModuleDatas.TryGetValue(viewId, out viewModuleData))
			{
				int layerId = viewModuleData.layerId;
				this.list.Clear();
				foreach (ViewModuleData viewModuleData2 in this.m_viewModuleDatas.Values)
				{
					if (viewModuleData2 != null && !(viewModuleData2.m_gameObject == null) && viewModuleData2.m_gameObject.activeSelf && !(viewModuleData2.m_viewModule == null) && viewModuleData2.m_viewState == ViewState.Opened && viewModuleData2.layerId == layerId)
					{
						this.list.Add(viewModuleData2);
					}
				}
				if (this.list.Count <= 1)
				{
					return true;
				}
				this.list.Sort((ViewModuleData a, ViewModuleData b) => a.m_viewModule.ViewModuleSortingOrderId.CompareTo(b.m_viewModule.ViewModuleSortingOrderId));
				List<ViewModuleData> list = this.list;
				if (list[list.Count - 1].m_id == viewId)
				{
					return true;
				}
			}
			return false;
		}

		private void SetMask(bool active)
		{
			this.m_maskCount += (active ? 1 : (-1));
			if (this.m_maskUI != null)
			{
				this.m_maskUI.SetActive(this.m_maskCount > 0);
			}
		}

		public void ShowNetLoading(bool value)
		{
			this.m_netLoadingCount += (value ? 1 : (-1));
			if (value && this.m_netLoadingCount == 1)
			{
				if (!GameApp.View.IsOpenedOrLoading(ViewName.NetLoadingViewModule))
				{
					GameApp.View.OpenView(ViewName.NetLoadingViewModule, null, UILayers.Third, null, null);
					return;
				}
			}
			else if (!value && this.m_netLoadingCount == 0 && GameApp.View.IsOpened(ViewName.NetLoadingViewModule))
			{
				GameApp.View.CloseView(ViewName.NetLoadingViewModule);
			}
		}

		public void InitLayerSortingOrderData()
		{
			this.layerSortingOrderDataDic.Clear();
			for (int i = 0; i < this.m_layerObjects.Length; i++)
			{
				int num = i;
				Canvas component = this.m_layerObjects[i].GetComponent<Canvas>();
				this.layerSortingOrderDataDic.Add(num, new ViewModuleManager.LayerSortingOrderData
				{
					baseSortingOrder = component.sortingOrder,
					curOffsetSortingOrder = 0
				});
			}
		}

		public void SetViewModuleSortingOrder(BaseViewModule viewModule, int layerId)
		{
			if (!this.layerSortingOrderDataDic.ContainsKey(layerId))
			{
				layerId = 1;
			}
			Canvas canvas = viewModule.GetComponent<Canvas>();
			if (canvas == null)
			{
				canvas = viewModule.gameObject.AddComponent<Canvas>();
			}
			if (viewModule.GetComponent<GraphicRaycaster>() == null)
			{
				viewModule.gameObject.AddComponent<GraphicRaycaster>();
			}
			int num = int.MaxValue;
			if (this.layerSortingOrderDataDic.ContainsKey(layerId + 1))
			{
				num = this.layerSortingOrderDataDic[layerId + 1].baseSortingOrder;
			}
			ViewModuleManager.LayerSortingOrderData layerSortingOrderData = this.layerSortingOrderDataDic[layerId];
			if (layerSortingOrderData.baseSortingOrder + layerSortingOrderData.curOffsetSortingOrder + 10 >= num)
			{
				this.ReSortViewModuleSortingOrder(layerId);
			}
			if (this.layerSortingOrderDataDic.ContainsKey(layerId))
			{
				layerSortingOrderData.curOffsetSortingOrder += 10;
				int num2 = layerSortingOrderData.baseSortingOrder + layerSortingOrderData.curOffsetSortingOrder;
				canvas.overrideSorting = true;
				canvas.sortingOrder = num2;
				viewModule.SetViewModuleSortingOrder(num2);
			}
		}

		private void DynamicCalcLayerCurSortingOrderValue(int layerId)
		{
			if (this.layerSortingOrderDataDic.ContainsKey(layerId))
			{
				bool flag = true;
				foreach (ViewModuleData viewModuleData in this.m_viewModuleDatas.Values)
				{
					if (viewModuleData != null && viewModuleData.layerId == layerId && (viewModuleData.m_viewState == ViewState.Opened || viewModuleData.m_viewState == ViewState.Loading) && viewModuleData.m_gameObject != null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					this.layerSortingOrderDataDic[layerId].curOffsetSortingOrder = 0;
				}
			}
		}

		private void ReSortViewModuleSortingOrder(int layerId)
		{
			if (!this.layerSortingOrderDataDic.ContainsKey(layerId))
			{
				return;
			}
			List<ViewModuleData> list = new List<ViewModuleData>();
			foreach (ViewModuleData viewModuleData in this.m_viewModuleDatas.Values)
			{
				if (viewModuleData != null && viewModuleData.layerId == layerId && viewModuleData.m_viewState == ViewState.Opened && viewModuleData.m_gameObject != null)
				{
					list.Add(viewModuleData);
				}
			}
			list.Sort((ViewModuleData a, ViewModuleData b) => a.m_viewModule.ViewModuleSortingOrderId.CompareTo(b.m_viewModule.ViewModuleSortingOrderId));
			this.layerSortingOrderDataDic[layerId].curOffsetSortingOrder = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].m_viewModule != null)
				{
					Canvas component = list[i].m_viewModule.GetComponent<Canvas>();
					if (component != null)
					{
						this.layerSortingOrderDataDic[layerId].curOffsetSortingOrder += 10;
						int num = this.layerSortingOrderDataDic[layerId].baseSortingOrder + this.layerSortingOrderDataDic[layerId].curOffsetSortingOrder;
						component.sortingOrder = num;
						list[i].m_viewModule.SetViewModuleSortingOrder(num);
					}
				}
			}
		}

		private Dictionary<int, ViewModuleData> m_viewModuleDatas = new Dictionary<int, ViewModuleData>();

		private Dictionary<int, UILayers> m_layers = new Dictionary<int, UILayers>();

		[Header("调试开关")]
		[SerializeField]
		private bool IsDebug;

		[Header("View Setting")]
		[SerializeField]
		private UIPool m_uiPool;

		[SerializeField]
		private EventSystemManager m_eventSystemManager;

		[SerializeField]
		private ResourcesManager m_resourcesManager;

		[SerializeField]
		private Camera m_uiCamera;

		[SerializeField]
		private float uiMaxLifeTime = 60f;

		[Header("Mask Setting")]
		[SerializeField]
		private GameObject m_maskUI;

		[SerializeField]
		[Label]
		private int m_maskCount;

		public Func<int, string> m_funcAnalysisViewName;

		public GameObject[] m_layerObjects;

		private bool isInited;

		private static int openCount;

		private static int reuseCount;

		private float lastCheckTime;

		private List<ViewModuleData> list = new List<ViewModuleData>();

		private int m_netLoadingCount;

		public const int ViewIncreaseSortingOrder = 10;

		private Dictionary<int, ViewModuleManager.LayerSortingOrderData> layerSortingOrderDataDic = new Dictionary<int, ViewModuleManager.LayerSortingOrderData>();

		public class LayerSortingOrderData
		{
			public int baseSortingOrder;

			public int curOffsetSortingOrder;
		}
	}
}
