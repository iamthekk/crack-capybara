using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtSagecraft_SagecraftModelImpl : BaseLocalModelImpl<ArtSagecraft_Sagecraft, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtSagecraft_Sagecraft();
		}

		protected override int GetBeanKey(ArtSagecraft_Sagecraft bean)
		{
			return bean.id;
		}
	}
}
