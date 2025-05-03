using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameConfig_ConfigModelImpl : BaseLocalModelImpl<GameConfig_Config, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameConfig_Config();
		}

		protected override int GetBeanKey(GameConfig_Config bean)
		{
			return bean.ID;
		}
	}
}
