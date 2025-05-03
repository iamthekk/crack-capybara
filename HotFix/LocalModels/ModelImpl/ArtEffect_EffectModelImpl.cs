using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtEffect_EffectModelImpl : BaseLocalModelImpl<ArtEffect_Effect, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtEffect_Effect();
		}

		protected override int GetBeanKey(ArtEffect_Effect bean)
		{
			return bean.id;
		}
	}
}
