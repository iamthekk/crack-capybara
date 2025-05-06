using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtBullet_BulletModel : BaseLocalModel
	{
		public ArtBullet_Bullet GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtBullet_Bullet> GetAllElements()
		{
			return this.modelImpl.GetAllElement();
		}

		public override void Initialise(string name, byte[] assetBytes)
		{
			base.Initialise(name, assetBytes);
			if (assetBytes == null)
			{
				return;
			}
			this.modelImpl.Initialise(name, assetBytes);
		}

		public override void DeInitialise()
		{
			this.modelImpl.DeInitialise();
			base.DeInitialise();
		}

		public static readonly string fileName = "ArtBullet_Bullet";

		private ArtBullet_BulletModelImpl modelImpl = new ArtBullet_BulletModelImpl();
	}
}
