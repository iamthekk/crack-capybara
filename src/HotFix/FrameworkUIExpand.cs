using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using Framework.ViewModule;

namespace HotFix
{
	public static class FrameworkUIExpand
	{
		public static void UIGrays2Recover(this CustomButton btn)
		{
			if (btn == null)
			{
				return;
			}
			UIGrays component = btn.GetComponent<UIGrays>();
			if (component != null)
			{
				component.Recovery();
			}
		}

		public static void UIGrays2Gray(this CustomButton btn)
		{
			if (btn == null)
			{
				return;
			}
			UIGrays component = btn.GetComponent<UIGrays>();
			if (component != null)
			{
				component.SetUIGray();
			}
		}

		public static void ShowWindowToQueue(this ViewModuleManager viewModuleManager, List<WindowQueueInfo> windowQueueInfos, Action callBack = null)
		{
			FrameworkUIExpand.m_waitShowViewList.Clear();
			for (int i = 0; i < windowQueueInfos.Count; i++)
			{
				FrameworkUIExpand.m_waitShowViewList.Add(windowQueueInfos[i].ViewName);
			}
			FrameworkUIExpand.m_waitFirstShowViewQueue.EnqueueRange(windowQueueInfos);
			FrameworkUIExpand.m_queueInfoCloseCallBack = callBack;
			GameApp.View.ShowWindowToQueue();
		}

		public static void ShowWindowToQueue(this ViewModuleManager viewModuleManager)
		{
			if (FrameworkUIExpand.m_waitFirstShowViewQueue.Count > 0)
			{
				WindowQueueInfo windowQueueInfo = FrameworkUIExpand.m_waitFirstShowViewQueue.Dequeue();
				GameApp.View.OpenView(windowQueueInfo.ViewName, windowQueueInfo.OpenData, windowQueueInfo.Layers, null, null);
				return;
			}
			Action queueInfoCloseCallBack = FrameworkUIExpand.m_queueInfoCloseCallBack;
			if (queueInfoCloseCallBack != null)
			{
				queueInfoCloseCallBack();
			}
			FrameworkUIExpand.m_queueInfoCloseCallBack = null;
			FrameworkUIExpand.m_waitShowViewList.Clear();
		}

		public static bool IsContainsQueue(ViewName viewName)
		{
			return FrameworkUIExpand.m_waitShowViewList.Contains(viewName);
		}

		public static void CloseWindowToQueue(this ViewModuleManager viewModuleManager)
		{
			FrameworkUIExpand.m_waitFirstShowViewQueue.Clear();
			FrameworkUIExpand.m_waitShowViewList.Clear();
		}

		public static bool IsHaveShowQueue()
		{
			return FrameworkUIExpand.m_waitFirstShowViewQueue.Count > 0;
		}

		private static Queue<WindowQueueInfo> m_waitFirstShowViewQueue = new Queue<WindowQueueInfo>();

		private static List<ViewName> m_waitShowViewList = new List<ViewName>();

		private static Action m_queueInfoCloseCallBack = null;
	}
}
