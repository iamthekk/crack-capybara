using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameBullet_bulletTypeModelImpl : BaseLocalModelImpl<GameBullet_bulletType, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameBullet_bulletType();
		}

		protected override int GetBeanKey(GameBullet_bulletType bean)
		{
			return bean.id;
		}
	}
}
