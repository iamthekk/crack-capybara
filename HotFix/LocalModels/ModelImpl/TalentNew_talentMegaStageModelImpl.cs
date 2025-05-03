using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentNew_talentMegaStageModelImpl : BaseLocalModelImpl<TalentNew_talentMegaStage, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentNew_talentMegaStage();
		}

		protected override int GetBeanKey(TalentNew_talentMegaStage bean)
		{
			return bean.id;
		}
	}
}
