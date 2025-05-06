using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Const_ConstModelImpl : BaseLocalModelImpl<Const_Const, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Const_Const();
		}

		protected override string GetBeanKey(Const_Const bean)
		{
			return bean.id;
		}
	}
}
