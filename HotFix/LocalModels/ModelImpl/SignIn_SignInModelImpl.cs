using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class SignIn_SignInModelImpl : BaseLocalModelImpl<SignIn_SignIn, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new SignIn_SignIn();
		}

		protected override int GetBeanKey(SignIn_SignIn bean)
		{
			return bean.ID;
		}
	}
}
