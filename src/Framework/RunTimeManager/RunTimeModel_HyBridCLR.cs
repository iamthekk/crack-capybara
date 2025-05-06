using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Framework.Aes;
using Framework.Logic.Modules;
using Framework.Platfrom;
using HybridCLR;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Framework.RunTimeManager
{
	public class RunTimeModel_HyBridCLR : BaseRunTimeModel
	{
		public RunTimeModel_HyBridCLR()
		{
			this.m_aotMetaAssemblyFiles.Add("Assets/_Resources/Dll/HotFix.dll.bytes");
			this.clrDlls.Add("HotFix.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = false,
				name = "HotFix.dll",
				bytes = null
			});
			this.clrDlls.Add("mscorlib.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = true,
				name = "mscorlib.dll",
				bytes = null
			});
			this.clrDlls.Add("System.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = true,
				name = "System.dll",
				bytes = null
			});
			this.clrDlls.Add("System.Core.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = true,
				name = "System.Core.dll",
				bytes = null
			});
			this.clrDlls.Add("Framework.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = true,
				name = "Framework.dll",
				bytes = null
			});
			this.clrDlls.Add("UnityEngine.Purchasing.dll", new RunTimeModel_HyBridCLR.CLRData
			{
				isAotDll = true,
				name = "UnityEngine.Purchasing.dll",
				bytes = null
			});
		}

		public Task PreLoad()
		{
			if (this.ispreload)
			{
				return this.preLoadTask;
			}
			this.ispreload = true;
			this.m_handleAot = GameApp.Resources.LoadAssetsAsync<TextAsset>(this.m_aotMetaAssemblyFiles, null, 1, true);
			this.m_handleAot.Completed += delegate(AsyncOperationHandle<IList<TextAsset>> x)
			{
				if (this.m_handleAot.Status != 1)
				{
					HLog.LogError("[HyBrideCLR_Model] load fail!!!");
				}
				for (int i = 0; i < this.m_handleAot.Result.Count; i++)
				{
					TextAsset textAsset = this.m_handleAot.Result[i];
					RunTimeModel_HyBridCLR.CLRData clrdata;
					if (!(textAsset == null) && this.clrDlls.TryGetValue(textAsset.name, out clrdata))
					{
						if (textAsset.name.Equals("HotFix.dll"))
						{
							byte[] array = AesTools.AesDecrypt(textAsset.bytes);
							clrdata.bytes = array;
						}
						else
						{
							clrdata.bytes = textAsset.bytes;
						}
					}
				}
			};
			this.preLoadTask = this.m_handleAot.Task;
			return this.preLoadTask;
		}

		public override async void Load()
		{
			this.m_isLoadingFinished = false;
			await this.PreLoad();
			await TaskExpand.Run(new Action(this.LoadAOTAndAssembly));
			this.OnStarUp();
			this.m_isLoadingFinished = true;
			if (this.m_onFinished != null)
			{
				this.m_onFinished();
			}
		}

		private async void LoadAOTAndAssembly()
		{
			this.LoadMetadataForAOTAssemblies(this.clrDlls.Values.ToList<RunTimeModel_HyBridCLR.CLRData>());
			this.LoadAssemblyAsync();
		}

		private void LoadAssemblyAsync()
		{
			RunTimeModel_HyBridCLR.CLRData clrdata;
			if (!this.clrDlls.TryGetValue("HotFix.dll", out clrdata) || clrdata.bytes == null)
			{
				HLog.LogError("[HyBrideCLR_Model] dll 未加载");
				return;
			}
			this.m_hotAssembly = Assembly.Load(clrdata.bytes);
			if (this.m_hotAssembly == null)
			{
				HLog.LogError("[HyBrideCLR_Model] dll 未加载");
			}
			this.m_mainClass = this.m_hotAssembly.CreateInstance("HotFix.Main");
			this.m_appType = this.m_hotAssembly.GetType("HotFix.Main");
			this.m_onStarUp = this.m_appType.GetMethod("OnStarUp");
			this.m_onShutDown = this.m_appType.GetMethod("OnShutDown");
			this.m_applicationFocus = this.m_appType.GetMethod("OnApplicationFocus");
			this.m_onApplicationPause = this.m_appType.GetMethod("OnApplicationPause");
			this.m_onApplicationQuit = this.m_appType.GetMethod("OnApplicationQuit");
			this.m_getLanguageInfoByID = this.m_appType.GetMethod("GetLanguageInfoByID", new Type[]
			{
				typeof(LanguageType),
				typeof(string)
			});
		}

		private async Task LoadAssembly()
		{
			string path = "Assets/_Resources/Dll/HotFix.dll.bytes";
			this.m_handleHotAssembly = Addressables.LoadAssetAsync<TextAsset>(path);
			await this.m_handleHotAssembly.Task;
			if (this.m_handleHotAssembly.Status != 1)
			{
				HLog.LogError("[HyBrideCLR_Model] load fail!!!" + path);
			}
			this.m_hotAssembly = Assembly.Load(this.m_handleHotAssembly.Result.bytes);
			if (this.m_hotAssembly == null)
			{
				HLog.LogError("[HyBrideCLR_Model] dll 未加载");
			}
			this.m_mainClass = this.m_hotAssembly.CreateInstance("HotFix.Main");
			this.m_appType = this.m_hotAssembly.GetType("HotFix.Main");
			this.m_onStarUp = this.m_appType.GetMethod("OnStarUp");
			this.m_onShutDown = this.m_appType.GetMethod("OnShutDown");
			this.m_applicationFocus = this.m_appType.GetMethod("OnApplicationFocus");
			this.m_onApplicationPause = this.m_appType.GetMethod("OnApplicationPause");
			this.m_onApplicationQuit = this.m_appType.GetMethod("OnApplicationQuit");
			this.m_getLanguageInfoByID = this.m_appType.GetMethod("GetLanguageInfoByID", new Type[]
			{
				typeof(LanguageType),
				typeof(string)
			});
			await Task.CompletedTask;
		}

		private async Task LoadAots()
		{
			this.m_handleAot = GameApp.Resources.LoadAssetsAsync<TextAsset>(this.m_aotMetaAssemblyFiles, null, 1, true);
			await this.m_handleAot.Task;
			if (this.m_handleAot.Status != 1)
			{
				HLog.LogError("[HyBrideCLR_Model] Main.HyBridCLR.LoadAOT  loadError!!!");
			}
			this.LoadMetadataForAOTAssemblies(this.m_handleAot.Result);
		}

		public override void OnFixedUpdate()
		{
		}

		public override void OnStarUp()
		{
			if (this.m_onStarUp == null)
			{
				return;
			}
			this.m_onStarUp.Invoke(this.m_mainClass, null);
		}

		public Task WaitPreLoad()
		{
			return this.preLoadTask;
		}

		public override async void OnShutDown()
		{
			if (this.m_onShutDown != null)
			{
				this.m_onShutDown.Invoke(this.m_mainClass, null);
			}
			if (this.preLoadTask != null && !this.preLoadTask.IsCompleted)
			{
				await this.preLoadTask;
			}
			this.preLoadTask = null;
			this.m_mainClass = null;
			this.m_appType = null;
			this.m_onStarUp = null;
			this.m_onShutDown = null;
			this.m_applicationFocus = null;
			this.m_onApplicationPause = null;
			this.m_onApplicationQuit = null;
			this.m_getLanguageInfoByID = null;
			this.m_hotAssembly = null;
			if (this.m_handleHotAssembly.IsValid())
			{
				GameApp.Resources.Release<TextAsset>(this.m_handleHotAssembly);
			}
			if (this.m_handleAot.IsValid())
			{
				GameApp.Resources.Release<IList<TextAsset>>(this.m_handleAot);
			}
			this.m_isLoadingFinished = false;
		}

		public override void OnApplicationFocus(bool hasFocus)
		{
			if (!this.m_isLoadingFinished)
			{
				return;
			}
			if (this.m_applicationFocus == null)
			{
				return;
			}
			this.m_applicationFocus.Invoke(this.m_mainClass, new object[] { hasFocus });
		}

		public override void OnApplicationPause(bool pauseStatus)
		{
			if (!this.m_isLoadingFinished)
			{
				return;
			}
			if (this.m_onApplicationPause == null)
			{
				return;
			}
			this.m_onApplicationPause.Invoke(this.m_mainClass, new object[] { pauseStatus });
		}

		public override bool IsOnApplicaitonQuitValid()
		{
			return this.m_isLoadingFinished && this.m_onApplicationQuit != null;
		}

		public override void OnApplicationQuit()
		{
			if (!this.m_isLoadingFinished)
			{
				return;
			}
			if (this.m_onApplicationQuit == null)
			{
				return;
			}
			this.m_onApplicationQuit.Invoke(this.m_mainClass, null);
		}

		public override string GetLanguageInfoByID(LanguageType languageType, int id)
		{
			return this.GetLanguageInfoByID(languageType, id.ToString());
		}

		public override string GetLanguageInfoByID(LanguageType languageType, string id)
		{
			if (this.m_getLanguageInfoByID == null)
			{
				return string.Empty;
			}
			return (string)this.m_getLanguageInfoByID.Invoke(this.m_mainClass, new object[] { languageType, id });
		}

		private void LoadMetadataForAOTAssemblies(IList<TextAsset> textAssets)
		{
			HomologousImageMode homologousImageMode = 1;
			for (int i = 0; i < textAssets.Count; i++)
			{
				TextAsset textAsset = textAssets[i];
				if (!(textAsset == null))
				{
					RuntimeApi.LoadMetadataForAOTAssembly(textAsset.bytes, homologousImageMode);
				}
			}
		}

		private void LoadMetadataForAOTAssemblies(IList<RunTimeModel_HyBridCLR.CLRData> pClrDatas)
		{
			HomologousImageMode homologousImageMode = 1;
			for (int i = 0; i < pClrDatas.Count; i++)
			{
				RunTimeModel_HyBridCLR.CLRData clrdata = pClrDatas[i];
				if (clrdata.isAotDll && clrdata.bytes != null)
				{
					RuntimeApi.LoadMetadataForAOTAssembly(clrdata.bytes, homologousImageMode);
				}
			}
		}

		private List<object> m_aotMetaAssemblyFiles = new List<object> { "Assets/_Resources/Dll/mscorlib.dll.bytes", "Assets/_Resources/Dll/System.dll.bytes", "Assets/_Resources/Dll/System.Core.dll.bytes", "Assets/_Resources/Dll/Framework.dll.bytes", "Assets/_Resources/Dll/UnityEngine.Purchasing.dll.bytes" };

		private Assembly m_hotAssembly;

		private AsyncOperationHandle<TextAsset> m_handleHotAssembly;

		private AsyncOperationHandle<IList<TextAsset>> m_handleAot;

		private object m_mainClass;

		private Type m_appType;

		private MethodInfo m_onStarUp;

		private MethodInfo m_onShutDown;

		private MethodInfo m_applicationFocus;

		private MethodInfo m_onApplicationPause;

		private MethodInfo m_onApplicationQuit;

		private MethodInfo m_getLanguageInfoByID;

		private const string hotFixFile = "Assets/_Resources/Dll/HotFix.dll.bytes";

		private const string hotFixFileName = "HotFix.dll";

		private Dictionary<string, RunTimeModel_HyBridCLR.CLRData> clrDlls = new Dictionary<string, RunTimeModel_HyBridCLR.CLRData>();

		private Task preLoadTask;

		private bool ispreload;

		public class CLRData
		{
			public bool isAotDll;

			public string name;

			public byte[] bytes;
		}
	}
}
