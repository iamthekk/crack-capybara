using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtMember_clothesModelImpl : BaseLocalModelImpl<ArtMember_clothes, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtMember_clothes();
		}

		protected override int GetBeanKey(ArtMember_clothes bean)
		{
			return bean.id;
		}
	}
}
