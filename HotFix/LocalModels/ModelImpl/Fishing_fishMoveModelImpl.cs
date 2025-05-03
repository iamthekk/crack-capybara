using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Fishing_fishMoveModelImpl : BaseLocalModelImpl<Fishing_fishMove, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Fishing_fishMove();
		}

		protected override int GetBeanKey(Fishing_fishMove bean)
		{
			return bean.id;
		}
	}
}
