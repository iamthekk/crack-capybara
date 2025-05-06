using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Box_outputModelImpl : BaseLocalModelImpl<Box_output, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Box_output();
		}

		protected override int GetBeanKey(Box_output bean)
		{
			return bean.Id;
		}
	}
}
