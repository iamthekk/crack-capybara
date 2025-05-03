using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class TalentNew_talentModelImpl : BaseLocalModelImpl<TalentNew_talent, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new TalentNew_talent();
		}

		protected override int GetBeanKey(TalentNew_talent bean)
		{
			return bean.id;
		}
	}
}
