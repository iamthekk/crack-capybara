using System;
using System.Collections.Generic;
using Framework.EventSystem;
using UnityEngine;

namespace Framework.DataModule
{
	public class DataModuleManager : MonoBehaviour, IModuleManager
	{
		public void Init()
		{
			if (this.isInited)
			{
				return;
			}
			this.isInited = true;
		}

		public Dictionary<int, IDataModule> GetAllDataModules()
		{
			return this.m_dataModules;
		}

		public void RegisterDataModule(IDataModule dataModule)
		{
			if (dataModule != null)
			{
				int name = dataModule.GetName();
				this.m_dataModules.ContainsKey(name);
				dataModule.RegisterEvents(this.m_eventSystemManager);
				this.m_dataModules[name] = dataModule;
			}
		}

		public void UnRegisterDataModule(IDataModule dataModule)
		{
			if (dataModule != null)
			{
				dataModule.UnRegisterEvents(this.m_eventSystemManager);
				this.m_dataModules.Remove(dataModule.GetName());
			}
		}

		public void UnRegisterAllDataModule(params int[] ignoreIDs)
		{
			if (this.m_dataModules.Count == 0)
			{
				return;
			}
			HashSet<int> hashSet = ((ignoreIDs != null && ignoreIDs.Length != 0) ? new HashSet<int>(ignoreIDs) : null);
			List<IDataModule> list = new List<IDataModule>();
			foreach (KeyValuePair<int, IDataModule> keyValuePair in this.m_dataModules)
			{
				if (keyValuePair.Value != null && (hashSet == null || !hashSet.Contains(keyValuePair.Key)))
				{
					list.Add(keyValuePair.Value);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				IDataModule dataModule = list[i];
				if (dataModule != null)
				{
					this.UnRegisterDataModule(dataModule);
				}
			}
		}

		public T GetDataModule<T>(int name) where T : IDataModule
		{
			IDataModule dataModule;
			if (this.m_dataModules.TryGetValue(name, out dataModule))
			{
				return (T)((object)dataModule);
			}
			return default(T);
		}

		private Dictionary<int, IDataModule> m_dataModules = new Dictionary<int, IDataModule>();

		[SerializeField]
		private EventSystemManager m_eventSystemManager;

		private bool isInited;
	}
}
