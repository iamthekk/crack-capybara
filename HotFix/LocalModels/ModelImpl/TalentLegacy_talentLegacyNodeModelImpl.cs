using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentLegacy_talentLegacyNodeModelImpl : BaseLocalModelImpl<TalentLegacy_talentLegacyNode, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentLegacy_talentLegacyNode();
		}

		protected override int GetBeanKey(TalentLegacy_talentLegacyNode bean)
		{
			return bean.id;
		}
	}
}
