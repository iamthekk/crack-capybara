using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class MonsterCfg_monsterCfgModelImpl : BaseLocalModelImpl<MonsterCfg_monsterCfg, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new MonsterCfg_monsterCfg();
		}

		protected override int GetBeanKey(MonsterCfg_monsterCfg bean)
		{
			return bean.id;
		}
	}
}
