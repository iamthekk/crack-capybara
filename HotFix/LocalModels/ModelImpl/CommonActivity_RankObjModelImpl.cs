using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_RankObjModelImpl : BaseLocalModelImpl<CommonActivity_RankObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_RankObj();
		}

		protected override int GetBeanKey(CommonActivity_RankObj bean)
		{
			return bean.id;
		}
	}
}
