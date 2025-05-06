using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Sociality_ReportModelImpl : BaseLocalModelImpl<Sociality_Report, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Sociality_Report();
		}

		protected override int GetBeanKey(Sociality_Report bean)
		{
			return bean.id;
		}
	}
}
