using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ChestList_ChestRewardModel : BaseLocalModel
	{
		public ChestList_ChestReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ChestList_ChestReward> GetAllElements()
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

		public static readonly string fileName = "ChestList_ChestReward";

		private ChestList_ChestRewardModelImpl modelImpl = new ChestList_ChestRewardModelImpl();
	}
}
