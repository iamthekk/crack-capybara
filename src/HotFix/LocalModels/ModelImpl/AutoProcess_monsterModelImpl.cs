using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class AutoProcess_monsterModelImpl : BaseLocalModelImpl<AutoProcess_monster, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new AutoProcess_monster();
		}

		protected override int GetBeanKey(AutoProcess_monster bean)
		{
			return bean.id;
		}
	}
}
