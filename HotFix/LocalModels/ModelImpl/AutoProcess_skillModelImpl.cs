using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class AutoProcess_skillModelImpl : BaseLocalModelImpl<AutoProcess_skill, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new AutoProcess_skill();
		}

		protected override int GetBeanKey(AutoProcess_skill bean)
		{
			return bean.id;
		}
	}
}
