using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class GameConfig_ConfigModel : BaseLocalModel
	{
		public GameConfig_Config GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<GameConfig_Config> GetAllElements()
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

		public static readonly string fileName = "GameConfig_Config";

		private GameConfig_ConfigModelImpl modelImpl = new GameConfig_ConfigModelImpl();
	}
}
