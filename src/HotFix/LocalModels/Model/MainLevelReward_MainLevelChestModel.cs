using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class MainLevelReward_MainLevelChestModel : BaseLocalModel
	{
		public MainLevelReward_MainLevelChest GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<MainLevelReward_MainLevelChest> GetAllElements()
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

		public static readonly string fileName = "MainLevelReward_MainLevelChest";

		private MainLevelReward_MainLevelChestModelImpl modelImpl = new MainLevelReward_MainLevelChestModelImpl();
	}
}
