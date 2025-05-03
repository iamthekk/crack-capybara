using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class CommonActivity_ShopObjModelImpl : BaseLocalModelImpl<CommonActivity_ShopObj, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new CommonActivity_ShopObj();
		}

		protected override int GetBeanKey(CommonActivity_ShopObj bean)
		{
			return bean.id;
		}
	}
}
