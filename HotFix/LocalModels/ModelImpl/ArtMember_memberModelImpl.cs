using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtMember_memberModelImpl : BaseLocalModelImpl<ArtMember_member, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtMember_member();
		}

		protected override int GetBeanKey(ArtMember_member bean)
		{
			return bean.id;
		}
	}
}
