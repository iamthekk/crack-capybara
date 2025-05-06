using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mount_mountLevelModelImpl : BaseLocalModelImpl<Mount_mountLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mount_mountLevel();
		}

		protected override int GetBeanKey(Mount_mountLevel bean)
		{
			return bean.id;
		}
	}
}
