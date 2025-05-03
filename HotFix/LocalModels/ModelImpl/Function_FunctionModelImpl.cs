using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Function_FunctionModelImpl : BaseLocalModelImpl<Function_Function, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Function_Function();
		}

		protected override int GetBeanKey(Function_Function bean)
		{
			return bean.id;
		}
	}
}
