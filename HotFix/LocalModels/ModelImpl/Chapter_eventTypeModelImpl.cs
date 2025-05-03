using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_eventTypeModelImpl : BaseLocalModelImpl<Chapter_eventType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_eventType();
		}

		protected override int GetBeanKey(Chapter_eventType bean)
		{
			return bean.id;
		}
	}
}
