using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameCamera_ShakeModelImpl : BaseLocalModelImpl<GameCamera_Shake, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameCamera_Shake();
		}

		protected override int GetBeanKey(GameCamera_Shake bean)
		{
			return bean.ID;
		}
	}
}
