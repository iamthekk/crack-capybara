using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentLegacy_careerModelImpl : BaseLocalModelImpl<TalentLegacy_career, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentLegacy_career();
		}

		protected override int GetBeanKey(TalentLegacy_career bean)
		{
			return bean.id;
		}
	}
}
