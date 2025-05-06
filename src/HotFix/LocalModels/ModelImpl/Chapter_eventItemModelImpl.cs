using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_eventItemModelImpl : BaseLocalModelImpl<Chapter_eventItem, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_eventItem();
		}

		protected override int GetBeanKey(Chapter_eventItem bean)
		{
			return bean.id;
		}
	}
}
