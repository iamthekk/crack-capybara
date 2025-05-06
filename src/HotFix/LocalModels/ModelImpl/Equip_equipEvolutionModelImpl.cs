using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_equipEvolutionModelImpl : BaseLocalModelImpl<Equip_equipEvolution, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_equipEvolution();
		}

		protected override int GetBeanKey(Equip_equipEvolution bean)
		{
			return bean.id;
		}
	}
}
