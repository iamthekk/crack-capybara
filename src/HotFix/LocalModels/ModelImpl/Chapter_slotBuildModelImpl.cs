using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Chapter_slotBuildModelImpl : BaseLocalModelImpl<Chapter_slotBuild, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Chapter_slotBuild();
		}

		protected override int GetBeanKey(Chapter_slotBuild bean)
		{
			return bean.id;
		}
	}
}
