using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Language_languagetableModelImpl : BaseLocalModelImpl<Language_languagetable, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Language_languagetable();
		}

		protected override int GetBeanKey(Language_languagetable bean)
		{
			return bean.id;
		}
	}
}
