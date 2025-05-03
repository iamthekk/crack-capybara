using System;
using System.Threading.Tasks;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.State;
using Framework.ViewModule;
using UnityEngine;

namespace Framework.Logic.Modules
{
	public static class FrameworkExpand
	{
		public static void RegisterEvent(this EventSystemManager manager, LocalMessageName name, HandlerEvent handle)
		{
			manager.RegisterEvent((int)name, handle);
		}

		public static void UnRegisterEvent(this EventSystemManager manager, LocalMessageName name, HandlerEvent handle)
		{
			manager.UnRegisterEvent((int)name, handle);
		}

		public static void Dispatch(this EventSystemManager manager, object sender, LocalMessageName name, BaseEventArgs eventArgs)
		{
			manager.Dispatch(sender, (int)name, eventArgs);
		}

		public static void DispatchNow(this EventSystemManager manager, object sender, LocalMessageName name, BaseEventArgs eventArgs)
		{
			manager.DispatchNow(sender, (int)name, eventArgs);
		}

		public static T GetDataModule<T>(this DataModuleManager manager, DataName name) where T : IDataModule
		{
			return manager.GetDataModule<T>((int)name);
		}

		public static T GetViewModule<T>(this ViewModuleManager manager, ViewName name) where T : BaseViewModule
		{
			return manager.GetViewModule<T>((int)name);
		}

		public static async Task OpenView(this ViewModuleManager manager, ViewName name, object data = null, UILayers layer = UILayers.First, Action<GameObject> loadedCallBack = null, Action<GameObject> openedCallBack = null)
		{
			await manager.OpenView((int)name, data, layer, loadedCallBack, openedCallBack);
		}

		public static void CloseView(this ViewModuleManager manager, ViewName name)
		{
			manager.CloseView((int)name, null);
		}

		public static bool IsOpened(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsOpened((int)name);
		}

		public static bool IsLoading(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsLoading((int)name);
		}

		public static bool IsOpenedOrLoading(this ViewModuleManager manager, ViewName name)
		{
			return manager.IsOpenedOrLoading((int)name);
		}

		public static void ActiveState(this StateManager manager, StateName name)
		{
			manager.ActiveState((int)name);
		}

		public static T GetState<T>(this StateManager manager, StateName name) where T : State
		{
			return manager.GetState<T>((int)name);
		}

		public static string ToHex(this Color color)
		{
			return ColorUtility.ToHtmlStringRGBA(color);
		}
	}
}
