using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_attributeBuildModelImpl : BaseLocalModelImpl<Chapter_attributeBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_attributeBuild();
		}

		protected override int GetBeanKey(Chapter_attributeBuild bean)
		{
			return bean.id;
		}
	}
}
