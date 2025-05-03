using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtHover_hoverModelImpl : BaseLocalModelImpl<ArtHover_hover, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtHover_hover();
		}

		protected override int GetBeanKey(ArtHover_hover bean)
		{
			return bean.id;
		}
	}
}
