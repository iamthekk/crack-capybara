using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mount_mountStageModelImpl : BaseLocalModelImpl<Mount_mountStage, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mount_mountStage();
		}

		protected override int GetBeanKey(Mount_mountStage bean)
		{
			return bean.id;
		}
	}
}
