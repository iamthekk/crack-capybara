using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_skillExpModelImpl : BaseLocalModelImpl<Chapter_skillExp, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_skillExp();
		}

		protected override int GetBeanKey(Chapter_skillExp bean)
		{
			return bean.id;
		}
	}
}
