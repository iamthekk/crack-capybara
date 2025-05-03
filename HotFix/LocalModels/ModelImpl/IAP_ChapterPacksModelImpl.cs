using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class IAP_ChapterPacksModelImpl : BaseLocalModelImpl<IAP_ChapterPacks, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new IAP_ChapterPacks();
		}

		protected override int GetBeanKey(IAP_ChapterPacks bean)
		{
			return bean.id;
		}
	}
}
