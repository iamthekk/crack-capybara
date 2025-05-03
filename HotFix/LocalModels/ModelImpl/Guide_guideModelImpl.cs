using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Guide_guideModelImpl : BaseLocalModelImpl<Guide_guide, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Guide_guide();
		}

		protected override int GetBeanKey(Guide_guide bean)
		{
			return bean.id;
		}
	}
}
