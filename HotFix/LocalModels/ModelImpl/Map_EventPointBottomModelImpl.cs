using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Map_EventPointBottomModelImpl : BaseLocalModelImpl<Map_EventPointBottom, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Map_EventPointBottom();
		}

		protected override int GetBeanKey(Map_EventPointBottom bean)
		{
			return bean.id;
		}
	}
}
