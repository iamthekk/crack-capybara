using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentLegacy_talentLegacyEffectModelImpl : BaseLocalModelImpl<TalentLegacy_talentLegacyEffect, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentLegacy_talentLegacyEffect();
		}

		protected override int GetBeanKey(TalentLegacy_talentLegacyEffect bean)
		{
			return bean.id;
		}
	}
}
