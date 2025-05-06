using System;
using UnityEngine;

namespace Framework.UnityGlobalManager
{
	public class UnityGlobalManager : MonoBehaviour
	{
		public void SetManager(IUnityGlobalManager manager)
		{
			this.m_manager = manager;
		}

		public CurveScriptable Curve
		{
			get
			{
				if (this.m_manager != null)
				{
					return this.m_manager.GetCurve();
				}
				return null;
			}
		}

		public GameObject GetGlobalGameObject(string path)
		{
			if (this.m_manager != null)
			{
				return this.m_manager.GetGlobalGameObject(path);
			}
			return null;
		}

		public void Load(Action finished)
		{
			if (this.m_manager != null)
			{
				this.m_manager.Load(finished);
			}
		}

		public void UnLoad(Action finished)
		{
			if (this.m_manager != null)
			{
				this.m_manager.UnLoad(finished);
			}
		}

		public void OnInit()
		{
		}

		public void OnDeInit()
		{
			this.m_manager = null;
		}

		private IUnityGlobalManager m_manager;
	}
}
