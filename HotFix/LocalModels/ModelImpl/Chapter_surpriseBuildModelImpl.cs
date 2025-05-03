using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_surpriseBuildModelImpl : BaseLocalModelImpl<Chapter_surpriseBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_surpriseBuild();
		}

		protected override int GetBeanKey(Chapter_surpriseBuild bean)
		{
			return bean.id;
		}
	}
}
