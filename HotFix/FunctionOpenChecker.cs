using System;
using Framework;
using Framework.ViewModule;

namespace HotFix
{
	public class FunctionOpenChecker : BaseViewPopChecker
	{
		public FunctionOpenChecker(MainStateViewPopController controller)
		{
			this.mController = controller;
		}

		public override bool Check()
		{
			if (this.mFirstCheck)
			{
				this.mFirstCheck = false;
				if (!Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen())
				{
					return false;
				}
			}
			ViewModuleData viewModuleData = GameApp.View.GetViewModuleData(213);
			if (viewModuleData != null && (viewModuleData.m_viewState == 1 || viewModuleData.m_viewState == 2))
			{
				return true;
			}
			if (Singleton<GameFunctionController>.Instance.HasWaitedNewFunction())
			{
				GameApp.View.OpenView(ViewName.FunctionOpenViewModule, null, 2, null, null);
				return true;
			}
			return viewModuleData != null && (viewModuleData.m_viewState == 1 || viewModuleData.m_viewState == 2);
		}

		private bool mFirstCheck = true;

		private MainStateViewPopController mController;
	}
}
