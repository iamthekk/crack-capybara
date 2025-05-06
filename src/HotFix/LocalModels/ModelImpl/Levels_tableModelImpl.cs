using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Levels_tableModelImpl : BaseLocalModelImpl<Levels_table, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Levels_table();
		}

		protected override int GetBeanKey(Levels_table bean)
		{
			return bean.id;
		}
	}
}
