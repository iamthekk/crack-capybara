using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class BattleMain_waveModelImpl : BaseLocalModelImpl<BattleMain_wave, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new BattleMain_wave();
		}

		protected override int GetBeanKey(BattleMain_wave bean)
		{
			return bean.ID;
		}
	}
}
