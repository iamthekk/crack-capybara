using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtHit_HitModelImpl : BaseLocalModelImpl<ArtHit_Hit, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtHit_Hit();
		}

		protected override int GetBeanKey(ArtHit_Hit bean)
		{
			return bean.id;
		}
	}
}
