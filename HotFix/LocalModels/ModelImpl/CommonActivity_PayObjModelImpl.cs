using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_PayObjModelImpl : BaseLocalModelImpl<CommonActivity_PayObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_PayObj();
		}

		protected override int GetBeanKey(CommonActivity_PayObj bean)
		{
			return bean.id;
		}
	}
}
