using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Dungeon_DungeonLevelModelImpl : BaseLocalModelImpl<Dungeon_DungeonLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Dungeon_DungeonLevel();
		}

		protected override int GetBeanKey(Dungeon_DungeonLevel bean)
		{
			return bean.id;
		}
	}
}
