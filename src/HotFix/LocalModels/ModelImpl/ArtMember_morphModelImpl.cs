using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtMember_morphModelImpl : BaseLocalModelImpl<ArtMember_morph, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtMember_morph();
		}

		protected override int GetBeanKey(ArtMember_morph bean)
		{
			return bean.id;
		}
	}
}
