using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_eventResModelImpl : BaseLocalModelImpl<Chapter_eventRes, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_eventRes();
		}

		protected override int GetBeanKey(Chapter_eventRes bean)
		{
			return bean.id;
		}
	}
}
