using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class LanguageCN_languagetableModelImpl : BaseLocalModelImpl<LanguageCN_languagetable, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new LanguageCN_languagetable();
		}

		protected override string GetBeanKey(LanguageCN_languagetable bean)
		{
			return bean.id;
		}
	}
}
