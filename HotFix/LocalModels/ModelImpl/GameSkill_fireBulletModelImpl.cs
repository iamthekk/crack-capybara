using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class GameSkill_fireBulletModelImpl : BaseLocalModelImpl<GameSkill_fireBullet, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new GameSkill_fireBullet();
		}

		protected override int GetBeanKey(GameSkill_fireBullet bean)
		{
			return bean.id;
		}
	}
}
