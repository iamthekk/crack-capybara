using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class LanguageRaft_languageTabModelImpl : BaseLocalModelImpl<LanguageRaft_languageTab, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new LanguageRaft_languageTab();
		}

		protected override int GetBeanKey(LanguageRaft_languageTab bean)
		{
			return bean.id;
		}
	}
}
