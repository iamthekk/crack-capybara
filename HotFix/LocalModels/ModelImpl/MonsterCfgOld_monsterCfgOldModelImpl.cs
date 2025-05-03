using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class MonsterCfgOld_monsterCfgOldModelImpl : BaseLocalModelImpl<MonsterCfgOld_monsterCfgOld, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new MonsterCfgOld_monsterCfgOld();
		}

		protected override int GetBeanKey(MonsterCfgOld_monsterCfgOld bean)
		{
			return bean.id;
		}
	}
}
