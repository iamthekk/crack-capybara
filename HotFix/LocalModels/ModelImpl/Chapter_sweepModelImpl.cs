using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_sweepModelImpl : BaseLocalModelImpl<Chapter_sweep, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_sweep();
		}

		protected override int GetBeanKey(Chapter_sweep bean)
		{
			return bean.id;
		}
	}
}
