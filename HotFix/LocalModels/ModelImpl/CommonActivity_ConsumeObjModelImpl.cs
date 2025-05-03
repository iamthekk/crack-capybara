using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_ConsumeObjModelImpl : BaseLocalModelImpl<CommonActivity_ConsumeObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_ConsumeObj();
		}

		protected override int GetBeanKey(CommonActivity_ConsumeObj bean)
		{
			return bean.Id;
		}
	}
}
