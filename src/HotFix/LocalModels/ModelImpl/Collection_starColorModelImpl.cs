using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Collection_starColorModelImpl : BaseLocalModelImpl<Collection_starColor, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Collection_starColor();
		}

		protected override int GetBeanKey(Collection_starColor bean)
		{
			return bean.id;
		}
	}
}
