using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameCamera_ShakeModel : BaseLocalModel
	{
		public GameCamera_Shake GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameCamera_Shake> GetAllElements()
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

		public static readonly string fileName = "GameCamera_Shake";

		private GameCamera_ShakeModelImpl modelImpl = new GameCamera_ShakeModelImpl();
	}
}
