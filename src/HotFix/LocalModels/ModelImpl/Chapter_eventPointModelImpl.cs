using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_eventPointModelImpl : BaseLocalModelImpl<Chapter_eventPoint, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_eventPoint();
		}

		protected override int GetBeanKey(Chapter_eventPoint bean)
		{
			return bean.id;
		}
	}
}
