using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class MonsterCfg_Old_monsterCfgModelImpl : BaseLocalModelImpl<MonsterCfg_Old_monsterCfg, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new MonsterCfg_Old_monsterCfg();
		}

		protected override int GetBeanKey(MonsterCfg_Old_monsterCfg bean)
		{
			return bean.id;
		}
	}
}
