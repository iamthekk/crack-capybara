using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtAnimation_animationModelImpl : BaseLocalModelImpl<ArtAnimation_animation, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtAnimation_animation();
		}

		protected override string GetBeanKey(ArtAnimation_animation bean)
		{
			return bean.id;
		}
	}
}
