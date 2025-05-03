using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_CommonActivityModelImpl : BaseLocalModelImpl<CommonActivity_CommonActivity, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_CommonActivity();
		}

		protected override int GetBeanKey(CommonActivity_CommonActivity bean)
		{
			return bean.Id;
		}
	}
}
