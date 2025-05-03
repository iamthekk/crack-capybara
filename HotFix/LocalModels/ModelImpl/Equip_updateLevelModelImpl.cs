using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Equip_updateLevelModelImpl : BaseLocalModelImpl<Equip_updateLevel, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Equip_updateLevel();
		}

		protected override int GetBeanKey(Equip_updateLevel bean)
		{
			return bean.id;
		}
	}
}
