using System;
using System.Collections.Generic;
using System.Reflection;
using Proto.Guild;

namespace Dxx.Guild
{
	public class GuildSDKManager
	{
		public static GuildSDKManager Instance
		{
			get
			{
				return GuildSDKManager.mInstance;
			}
		}

		public bool IsInitOver
		{
			get
			{
				return this.mIsGuildInitOver;
			}
		}

		public bool IsDataInitOver
		{
			get
			{
				return this.GuildInfo != null && this.GuildInfo.HasGetGuildData;
			}
		}

		public GuildEventModule Event
		{
			get
			{
				if (this.mEvent == null)
				{
					this.mEvent = this.InternalGetModule<GuildEventModule>();
				}
				return this.mEvent;
			}
		}

		public GuildUserDataModule User
		{
			get
			{
				if (this.mUser == null)
				{
					this.mUser = this.InternalGetModule<GuildUserDataModule>();
				}
				return this.mUser;
			}
		}

		public GuildInfoDataModule GuildInfo
		{
			get
			{
				if (this.mGuildInfo == null)
				{
					this.mGuildInfo = this.InternalGetModule<GuildInfoDataModule>();
				}
				return this.mGuildInfo;
			}
		}

		public GuildNetwork Net
		{
			get
			{
				if (this.mNet == null)
				{
					this.mNet = this.InternalGetModule<GuildNetwork>();
				}
				return this.mNet;
			}
		}

		public GuildConfigModule Config
		{
			get
			{
				if (this.mConfig == null)
				{
					this.mConfig = this.InternalGetModule<GuildConfigModule>();
				}
				return this.mConfig;
			}
		}

		public GuildPermissionDataModule Permission
		{
			get
			{
				if (this.mPermission == null)
				{
					this.mPermission = this.InternalGetModule<GuildPermissionDataModule>();
				}
				return this.mPermission;
			}
		}

		public GuildListModule GuildList
		{
			get
			{
				if (this.mGuildList == null)
				{
					this.mGuildList = this.InternalGetModule<GuildListModule>();
				}
				return this.mGuildList;
			}
		}

		public GuildLogData GuildEventLog
		{
			get
			{
				if (this.mGuildEventLog == null)
				{
					this.mGuildEventLog = this.InternalGetModule<GuildLogData>();
				}
				return this.mGuildEventLog;
			}
		}

		public GuildTaskDataModule GuildTask
		{
			get
			{
				if (this.mGuildTask == null)
				{
					this.mGuildTask = this.InternalGetModule<GuildTaskDataModule>();
				}
				return this.mGuildTask;
			}
		}

		public GuildShopDataModule GuildShop
		{
			get
			{
				if (this.mGuildShop == null)
				{
					this.mGuildShop = this.InternalGetModule<GuildShopDataModule>();
				}
				return this.mGuildShop;
			}
		}

		public GuildActivityDataModule GuildActivity
		{
			get
			{
				if (this.mGuildActivity == null)
				{
					this.mGuildActivity = this.InternalGetModule<GuildActivityDataModule>();
				}
				return this.mGuildActivity;
			}
		}

		public GuildDonationDataModule GuildDonation
		{
			get
			{
				if (this.mGuildDonation == null)
				{
					this.mGuildDonation = this.InternalGetModule<GuildDonationDataModule>();
				}
				return this.mGuildDonation;
			}
		}

		public static bool Init(GuildInitConfig config)
		{
			if (GuildSDKManager.mInstance != null && GuildSDKManager.mInstance.IsInitOver)
			{
				HLog.LogError("GuildSDKManager has already been initialized！");
				return false;
			}
			GuildSDKManager.mInstance = new GuildSDKManager();
			GuildSDKManager.mInstance.LastInitConfig = config;
			GuildInternalModuleCreator guildInternalModuleCreator = new GuildInternalModuleCreator();
			List<IGuildModule> list = new List<IGuildModule>();
			if (!guildInternalModuleCreator.CreateGuildModules(GuildSDKManager.mInstance, config, list))
			{
				HLog.LogError("GuildSDKManager init fail when create internal modules！");
				return false;
			}
			for (int i = 0; i < list.Count; i++)
			{
				IGuildModule guildModule = list[i];
				if (!guildModule.Init(config))
				{
					HLog.LogError(string.Format("GuildSDKManager init fail when init module({0})！", guildModule.ModuleName));
					return false;
				}
			}
			GuildSDKManager.mInstance.mIsGuildInitOver = true;
			return true;
		}

		public static void DeInit()
		{
			foreach (KeyValuePair<int, IGuildModule> keyValuePair in GuildSDKManager.mInstance.mAllModules)
			{
				IGuildModule value = keyValuePair.Value;
				if (value != null)
				{
					value.UnInit();
				}
			}
			GuildSDKManager.mInstance = null;
		}

		public IGuildModule GetModule(int modulename)
		{
			if (!this.IsInitOver)
			{
				HLog.LogError("GetModule fail when doesn't initialization over!");
				return null;
			}
			IGuildModule guildModule;
			if (this.mAllModules.TryGetValue(modulename, out guildModule))
			{
				return guildModule;
			}
			HLog.LogError(string.Format("Module({0}) doesn't exist!", modulename));
			return null;
		}

		public T GetModule<T>()
		{
			if (!this.IsInitOver)
			{
				HLog.LogError("GetModule fail when doesn't initialization over!");
				return default(T);
			}
			return this.InternalGetModule<T>();
		}

		private T InternalGetModule<T>()
		{
			string fullName = typeof(T).FullName;
			IGuildModule guildModule;
			if (GuildSDKManager.mInstance.mAllModulesByTypeName.TryGetValue(fullName, out guildModule) && guildModule is T)
			{
				return (T)((object)guildModule);
			}
			HLog.LogError("GetModule<T(" + fullName + ")> fail when module doesn't registed!");
			return default(T);
		}

		public bool RegModule(int modulename, IGuildModule module)
		{
			if (!this.IsInitOver)
			{
				HLog.LogError("RegModule fail when mInstance doesn't initialization over!");
				return false;
			}
			return this.InternalRegModule(modulename, module);
		}

		internal bool InternalRegModule(int modulename, IGuildModule module)
		{
			if (module == null)
			{
				return false;
			}
			IGuildModule guildModule;
			if (this.mAllModules.TryGetValue(modulename, out guildModule))
			{
				HLog.LogError(string.Format("Module({0} has already registed!", modulename));
				return false;
			}
			this.mAllModules[modulename] = module;
			string fullName = module.GetType().FullName;
			this.mAllModulesByTypeName[fullName] = module;
			if (module is GuildEventModule)
			{
				this.mEvent = (GuildEventModule)module;
			}
			else if (this.mNet != null)
			{
				this.mNet = (GuildNetwork)module;
			}
			else if (this.mConfig != null)
			{
				this.mConfig = (GuildConfigModule)module;
			}
			if (this.mIsGuildInitOver)
			{
				module.Init(this.LastInitConfig);
			}
			return true;
		}

		public bool UnRegModule(int modulename, IGuildModule module = null)
		{
			if (!this.IsInitOver)
			{
				HLog.LogError("UnRegModule fail when mInstance doesn't initialization over!");
				return false;
			}
			return this.InternalUnRegModule(modulename, module);
		}

		internal bool InternalUnRegModule(int modulename, IGuildModule module = null)
		{
			IGuildModule guildModule;
			if (this.mAllModules.TryGetValue(modulename, out guildModule))
			{
				if (module != null && module != guildModule)
				{
					HLog.LogError(string.Format("Module({0} doesn't match when UnRegModule()!", modulename));
					return false;
				}
				this.mAllModules.Remove(modulename);
				if (guildModule != null)
				{
					FieldInfo fieldInfo = this._GetModuleFieldInfo(guildModule);
					if (fieldInfo != null)
					{
						fieldInfo.SetValue(this, null);
					}
					this.mAllModulesByTypeName.Remove(guildModule.GetType().FullName);
				}
			}
			return true;
		}

		private FieldInfo _GetModuleFieldInfo(IGuildModule module)
		{
			Type type = base.GetType();
			Type type2 = module.GetType();
			BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			foreach (FieldInfo fieldInfo in type.GetFields(bindingFlags))
			{
				if (type2.FullName == fieldInfo.FieldType.FullName)
				{
					return fieldInfo;
				}
				if (type2.IsSubclassOf(fieldInfo.FieldType))
				{
					return fieldInfo;
				}
			}
			return null;
		}

		public ulong GetCustomRefreshTime()
		{
			if (this.GuildInfo.HasGuild)
			{
				GuildShopGroup shopGroup = this.GuildShop.GetShopGroup(1);
				if (shopGroup != null)
				{
					return shopGroup.ShopRefreshTime;
				}
			}
			return 0UL;
		}

		public void OpenGuild()
		{
			if (this.IsDataInitOver && this.GuildInfo.HasGuild)
			{
				GuildProxy.UI.OpenMainGuild();
				return;
			}
			GuildProxy.UI.SetNetShow(true);
			GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse resp)
			{
				if (result && this.IsDataInitOver)
				{
					if (this.GuildInfo.HasGuild)
					{
						GuildProxy.UI.OpenMainGuild();
					}
					else
					{
						GuildProxy.UI.OpenMainGuildInfo();
					}
				}
				GuildProxy.UI.SetNetShow(false);
			});
		}

		public bool CheckQuitGuildTime()
		{
			if (this.GuildInfo.QuitGuildTimeStamp <= 0L)
			{
				return true;
			}
			long num = (long)GuildProxy.Table.GetGuildConstTable(139).TypeInt - (GuildProxy.Net.ServerTime() - this.GuildInfo.QuitGuildTimeStamp);
			if (num > 0L)
			{
				TimeSpan timeSpan = new TimeSpan(num * 1000L * 10000L);
				GuildProxy.UI.ShowTips(GuildProxy.Language.GetInfoByID3("guild_join_cd", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
				return false;
			}
			return true;
		}

		private static GuildSDKManager mInstance;

		private Dictionary<int, IGuildModule> mAllModules = new Dictionary<int, IGuildModule>();

		private Dictionary<string, IGuildModule> mAllModulesByTypeName = new Dictionary<string, IGuildModule>();

		public GuildInitConfig LastInitConfig;

		public bool mIsGuildInitOver;

		private GuildEventModule mEvent;

		private GuildUserDataModule mUser;

		private GuildInfoDataModule mGuildInfo;

		private GuildNetwork mNet;

		private GuildConfigModule mConfig;

		private GuildPermissionDataModule mPermission;

		private GuildListModule mGuildList;

		private GuildLogData mGuildEventLog;

		private GuildTaskDataModule mGuildTask;

		private GuildShopDataModule mGuildShop;

		private GuildActivityDataModule mGuildActivity;

		private GuildDonationDataModule mGuildDonation;
	}
}
