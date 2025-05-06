using System;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;

namespace Framework.ViewModule
{
	public abstract class BaseViewModule : MonoBehaviour
	{
		public int m_viewName { get; private set; }

		public BaseViewModuleLoader Loader
		{
			get
			{
				return this.m_loader;
			}
		}

		public void SetViewData(int viewName)
		{
			this.m_viewName = viewName;
		}

		internal void SetLoader(BaseViewModuleLoader loader)
		{
			this.m_loader = loader;
		}

		public abstract void OnCreate(object data);

		public abstract void OnOpen(object data);

		public abstract void OnUpdate(float deltaTime, float unscaledDeltaTime);

		public abstract void OnClose();

		public abstract void OnDelete();

		public abstract void RegisterEvents(EventSystemManager manager);

		public abstract void UnRegisterEvents(EventSystemManager manager);

		public int ViewModuleSortingOrderId { get; private set; } = 1;

		public void SetViewModuleSortingOrder(int sortingOrder)
		{
			this.ViewModuleSortingOrderId = sortingOrder;
			this.InternalAutoSortingOrder();
		}

		public void InternalAutoSortingOrder()
		{
			this.LocalAutoSortingOrder(base.transform);
		}

		public void LocalAutoSortingOrder(Transform trans)
		{
			SortingOrderToolBase[] componentsInChildren = trans.GetComponentsInChildren<SortingOrderToolBase>(true);
			if (componentsInChildren != null && componentsInChildren.Length != 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].SetSortingOrder(this.ViewModuleSortingOrderId);
				}
			}
		}

		private BaseViewModuleLoader m_loader;
	}
}
