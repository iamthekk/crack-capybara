using System;
using LocalModels.Bean;

namespace LocalModels.ModelImpl
{
	public class ArtBullet_BulletModelImpl : BaseLocalModelImpl<ArtBullet_Bullet, int>
	{
		protected override IBeanBuilder GetBuilder()
		{
			return new ArtBullet_Bullet();
		}

		protected override int GetBeanKey(ArtBullet_Bullet bean)
		{
			return bean.id;
		}
	}
}
