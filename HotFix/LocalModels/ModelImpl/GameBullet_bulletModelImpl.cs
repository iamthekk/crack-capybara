using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameBullet_bulletModelImpl : BaseLocalModelImpl<GameBullet_bullet, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameBullet_bullet();
		}

		protected override int GetBeanKey(GameBullet_bullet bean)
		{
			return bean.id;
		}
	}
}
