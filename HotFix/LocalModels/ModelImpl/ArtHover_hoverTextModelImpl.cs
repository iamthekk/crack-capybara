using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtHover_hoverTextModelImpl : BaseLocalModelImpl<ArtHover_hoverText, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtHover_hoverText();
		}

		protected override int GetBeanKey(ArtHover_hoverText bean)
		{
			return bean.id;
		}
	}
}
