using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_DropObjModelImpl : BaseLocalModelImpl<CommonActivity_DropObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_DropObj();
		}

		protected override int GetBeanKey(CommonActivity_DropObj bean)
		{
			return bean.Id;
		}
	}
}
