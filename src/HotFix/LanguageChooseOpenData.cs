using System;
using Framework.Logic.Modules;

namespace HotFix
{
	public class LanguageChooseOpenData
	{
		public LanguageType DefaultLanguage;

		public Action<LanguageType> Callback;
	}
}
