using System;
using Framework;
using Framework.ViewModule;

namespace HotFix
{
	public class RateViewChecker : BaseViewPopChecker
	{
		public override bool Check()
		{
			if (!GameApp.SDK.GetCloudDataValue<bool>("IfShowRate", true))
			{
				return false;
			}
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (dataModule == null)
			{
				return false;
			}
			if (dataModule.ChapterID < 4)
			{
				return false;
			}
			if (PlayerPrefsKeys.GetIsRateShow())
			{
				return false;
			}
			ViewModuleData viewModuleData = GameApp.View.GetViewModuleData(1019);
			if (viewModuleData == null)
			{
				return false;
			}
			if (viewModuleData.m_viewState == 1 || viewModuleData.m_viewState == 2)
			{
				return true;
			}
			GameApp.View.OpenView(ViewName.RateViewModule, null, 2, null, null);
			return true;
		}
	}
}
