using System;
using UnityEngine;

namespace Framework.ViewModule
{
	public class ViewModuleData
	{
		public ViewModuleData(int viewID, GameObject gameObject = null, DestoryType destoryType = DestoryType.Dont)
		{
			this.m_id = viewID;
			this.m_gameObject = gameObject;
			this.m_destoryType = destoryType;
		}

		public ViewModuleData(int viewID, BaseViewModuleLoader loader, string assetPath, DestoryType destoryType = DestoryType.Auto)
		{
			this.m_id = viewID;
			this.m_loader = loader;
			this.m_assetPath = assetPath;
			this.m_destoryType = destoryType;
		}

		public int m_id;

		public string m_assetPath;

		public GameObject m_gameObject;

		public BaseViewModuleLoader m_loader;

		public GameObject m_prefab;

		public BaseViewModule m_viewModule;

		public DestoryType m_destoryType;

		public ViewState m_viewState;

		public int layerId;

		public float lastCloseTime;
	}
}
