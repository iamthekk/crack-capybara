using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_boxBuildModelImpl : BaseLocalModelImpl<Chapter_boxBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_boxBuild();
		}

		protected override int GetBeanKey(Chapter_boxBuild bean)
		{
			return bean.id;
		}
	}
}
