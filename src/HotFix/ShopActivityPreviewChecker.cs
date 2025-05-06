using System;
using Framework;
using Framework.ViewModule;

namespace HotFix
{
	public class ShopActivityPreviewChecker : BaseViewPopChecker
	{
		public override bool Check()
		{
			if (MainState.EnterCount > 1)
			{
				return false;
			}
			if (!Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.IAPShop, false))
			{
				GameApp.Data.GetDataModule(DataName.IAPDataModule).SetShowSUpPoolPreviewCount();
				return false;
			}
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			int num;
			if (!dataModule.CanShowSUpPoolPreview(out num))
			{
				return false;
			}
			if (GuideController.Instance.CurrentGuide != null)
			{
				return true;
			}
			ViewModuleData viewModuleData = GameApp.View.GetViewModuleData(201);
			if (viewModuleData != null && viewModuleData.m_viewState == 2)
			{
				MainViewModule mainViewModule = viewModuleData.m_viewModule as MainViewModule;
				if (mainViewModule != null && mainViewModule.GetCurrentPageEnum() != UIMainPageName.Battle)
				{
					return true;
				}
			}
			ViewModuleData viewModuleData2 = GameApp.View.GetViewModuleData(1037);
			if (viewModuleData2 != null && (viewModuleData2.m_viewState == 1 || viewModuleData2.m_viewState == 2))
			{
				return true;
			}
			int num2;
			if (dataModule.CanShowSUpPoolPreview(out num2))
			{
				IAPDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				if (dataModule2 != null)
				{
					dataModule2.SetShowSUpPoolPreviewCount();
				}
				GameApp.View.OpenView(ViewName.ShopActivitySUpPoolPreviewViewModule, null, 1, null, null);
				return true;
			}
			return viewModuleData2 != null && (viewModuleData2.m_viewState == 1 || viewModuleData2.m_viewState == 2);
		}
	}
}
