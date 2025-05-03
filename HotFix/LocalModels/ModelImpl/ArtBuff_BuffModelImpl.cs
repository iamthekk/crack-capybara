using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtBuff_BuffModelImpl : BaseLocalModelImpl<ArtBuff_Buff, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtBuff_Buff();
		}

		protected override int GetBeanKey(ArtBuff_Buff bean)
		{
			return bean.id;
		}
	}
}
