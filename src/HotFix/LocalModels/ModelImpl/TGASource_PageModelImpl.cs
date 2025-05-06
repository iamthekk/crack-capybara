using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TGASource_PageModelImpl : BaseLocalModelImpl<TGASource_Page, string>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TGASource_Page();
		}

		protected override string GetBeanKey(TGASource_Page bean)
		{
			return bean.id;
		}
	}
}
