using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class AutoProcess_skillRandomModelImpl : BaseLocalModelImpl<AutoProcess_skillRandom, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new AutoProcess_skillRandom();
		}

		protected override int GetBeanKey(AutoProcess_skillRandom bean)
		{
			return bean.id;
		}
	}
}
