using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class MainLevelReward_AFKrewardModel : BaseLocalModel
	{
		public MainLevelReward_AFKreward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<MainLevelReward_AFKreward> GetAllElements()
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

		public static readonly string fileName = "MainLevelReward_AFKreward";

		private MainLevelReward_AFKrewardModelImpl modelImpl = new MainLevelReward_AFKrewardModelImpl();
	}
}
