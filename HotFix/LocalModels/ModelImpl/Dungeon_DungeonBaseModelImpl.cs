using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Dungeon_DungeonBaseModelImpl : BaseLocalModelImpl<Dungeon_DungeonBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Dungeon_DungeonBase();
		}

		protected override int GetBeanKey(Dungeon_DungeonBase bean)
		{
			return bean.id;
		}
	}
}
