using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Mining_bonusGameModelImpl : BaseLocalModelImpl<Mining_bonusGame, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Mining_bonusGame();
		}

		protected override int GetBeanKey(Mining_bonusGame bean)
		{
			return bean.id;
		}
	}
}
