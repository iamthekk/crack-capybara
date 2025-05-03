using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class RogueDungeon_monsterEntryModelImpl : BaseLocalModelImpl<RogueDungeon_monsterEntry, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new RogueDungeon_monsterEntry();
		}

		protected override int GetBeanKey(RogueDungeon_monsterEntry bean)
		{
			return bean.id;
		}
	}
}
