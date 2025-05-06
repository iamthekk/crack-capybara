using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class HeroLevelup_HeroLevelupModelImpl : BaseLocalModelImpl<HeroLevelup_HeroLevelup, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new HeroLevelup_HeroLevelup();
		}

		protected override int GetBeanKey(HeroLevelup_HeroLevelup bean)
		{
			return bean.ID;
		}
	}
}
