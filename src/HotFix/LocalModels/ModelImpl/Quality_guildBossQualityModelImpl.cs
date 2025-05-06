using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class Quality_guildBossQualityModelImpl : BaseLocalModelImpl<Quality_guildBossQuality, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new Quality_guildBossQuality();
		}

		protected override int GetBeanKey(Quality_guildBossQuality bean)
		{
			return bean.id;
		}
	}
}
