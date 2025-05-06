using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Module_moduleInfoModelImpl : BaseLocalModelImpl<Module_moduleInfo, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Module_moduleInfo();
		}

		protected override int GetBeanKey(Module_moduleInfo bean)
		{
			return bean.id;
		}
	}
}
