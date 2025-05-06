using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class BattleMain_waveModel : BaseLocalModel
	{
		public BattleMain_wave GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<BattleMain_wave> GetAllElements()
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

		public static readonly string fileName = "BattleMain_wave";

		private BattleMain_waveModelImpl modelImpl = new BattleMain_waveModelImpl();
	}
}
