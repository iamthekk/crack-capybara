using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class RogueDungeon_endEventModelImpl : BaseLocalModelImpl<RogueDungeon_endEvent, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new RogueDungeon_endEvent();
		}

		protected override int GetBeanKey(RogueDungeon_endEvent bean)
		{
			return bean.id;
		}
	}
}
