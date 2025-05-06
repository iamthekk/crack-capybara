using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentNew_talentEvolutionModelImpl : BaseLocalModelImpl<TalentNew_talentEvolution, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentNew_talentEvolution();
		}

		protected override int GetBeanKey(TalentNew_talentEvolution bean)
		{
			return bean.id;
		}
	}
}
