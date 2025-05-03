using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IAP_BattlePassRewardModel : BaseLocalModel
	{
		public IAP_BattlePassReward GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IAP_BattlePassReward> GetAllElements()
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

		public static readonly string fileName = "IAP_BattlePassReward";

		private IAP_BattlePassRewardModelImpl modelImpl = new IAP_BattlePassRewardModelImpl();
	}
}
