using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class SevenDay_SevenDayActiveRewardModel : BaseLocalModel
	{
		public SevenDay_SevenDayActiveReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<SevenDay_SevenDayActiveReward> GetAllElements()
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

		public static readonly string fileName = "SevenDay_SevenDayActiveReward";

		private SevenDay_SevenDayActiveRewardModelImpl modelImpl = new SevenDay_SevenDayActiveRewardModelImpl();
	}
}
