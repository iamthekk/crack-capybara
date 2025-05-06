using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameBullet_bulletModel : BaseLocalModel
	{
		public GameBullet_bullet GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameBullet_bullet> GetAllElements()
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

		public static readonly string fileName = "GameBullet_bullet";

		private GameBullet_bulletModelImpl modelImpl = new GameBullet_bulletModelImpl();
	}
}
