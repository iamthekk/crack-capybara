using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class LanguageRaft_languagetableModelImpl : BaseLocalModelImpl<LanguageRaft_languagetable, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new LanguageRaft_languagetable();
		}

		protected override string GetBeanKey(LanguageRaft_languagetable bean)
		{
			return bean.id;
		}
	}
}
