using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Fishing_fishRodModelImpl : BaseLocalModelImpl<Fishing_fishRod, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Fishing_fishRod();
		}

		protected override int GetBeanKey(Fishing_fishRod bean)
		{
			return bean.id;
		}
	}
}
