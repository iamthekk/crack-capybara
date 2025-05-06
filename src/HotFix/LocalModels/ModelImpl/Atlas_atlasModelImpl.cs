using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Atlas_atlasModelImpl : BaseLocalModelImpl<Atlas_atlas, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Atlas_atlas();
		}

		protected override int GetBeanKey(Atlas_atlas bean)
		{
			return bean.id;
		}
	}
}
