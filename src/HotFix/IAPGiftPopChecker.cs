using System;
using System.Collections.Generic;
using Framework;

namespace HotFix
{
	public class IAPGiftPopChecker : BaseViewPopChecker
	{
		public override bool Check()
		{
			if (this.isFirstCheck)
			{
				this.isFirstCheck = false;
				IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				this.enableDataList = dataModule.ChapterGift.CheckAndResetNewEnableData();
			}
			return false;
		}

		private bool isFirstCheck = true;

		private List<IAPChapterGift.Data> enableDataList;
	}
}
