using System;
using Framework.Logic.Modules;

namespace Framework.Logic.UI
{
	public static class CustomTextTool
	{
		public static void SetCustomTextExtension()
		{
			CustomText.AutoSpaceCheck = new Func<bool>(CustomTextTool.AutoSpaceCheck);
			CustomLanguageText.AutoSpaceCheck = new Func<bool>(CustomTextTool.AutoSpaceCheck);
		}

		private static bool AutoSpaceCheck()
		{
			LanguageDataModule dataModule = GameApp.Data.GetDataModule(DataName.LanguageDataModule);
			if (dataModule != null)
			{
				LanguageType getCurrentLanguageType = dataModule.GetCurrentLanguageType;
				if (getCurrentLanguageType == LanguageType.ChineseSimplified || getCurrentLanguageType == LanguageType.ChineseTraditional || getCurrentLanguageType == LanguageType.Japanese || getCurrentLanguageType == LanguageType.Korean || getCurrentLanguageType == LanguageType.Thai)
				{
					return true;
				}
			}
			return false;
		}
	}
}
