using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtSkin_equipSkinModelImpl : BaseLocalModelImpl<ArtSkin_equipSkin, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtSkin_equipSkin();
		}

		protected override int GetBeanKey(ArtSkin_equipSkin bean)
		{
			return bean.id;
		}
	}
}
