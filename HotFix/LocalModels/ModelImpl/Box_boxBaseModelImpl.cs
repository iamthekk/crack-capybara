using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Box_boxBaseModelImpl : BaseLocalModelImpl<Box_boxBase, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Box_boxBase();
		}

		protected override int GetBeanKey(Box_boxBase bean)
		{
			return bean.id;
		}
	}
}
