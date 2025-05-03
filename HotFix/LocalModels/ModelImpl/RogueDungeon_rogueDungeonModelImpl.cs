using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class RogueDungeon_rogueDungeonModelImpl : BaseLocalModelImpl<RogueDungeon_rogueDungeon, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new RogueDungeon_rogueDungeon();
		}

		protected override int GetBeanKey(RogueDungeon_rogueDungeon bean)
		{
			return bean.id;
		}
	}
}
