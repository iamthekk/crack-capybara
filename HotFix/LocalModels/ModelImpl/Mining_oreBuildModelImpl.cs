using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_oreBuildModelImpl : BaseLocalModelImpl<Mining_oreBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_oreBuild();
		}

		protected override int GetBeanKey(Mining_oreBuild bean)
		{
			return bean.id;
		}
	}
}
