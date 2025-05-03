using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtMap_MapModelImpl : BaseLocalModelImpl<ArtMap_Map, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtMap_Map();
		}

		protected override int GetBeanKey(ArtMap_Map bean)
		{
			return bean.id;
		}
	}
}
