using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TowerChallenge_TowerModel : BaseLocalModel
	{
		public TowerChallenge_Tower GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TowerChallenge_Tower> GetAllElements()
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

		public static readonly string fileName = "TowerChallenge_Tower";

		private TowerChallenge_TowerModelImpl modelImpl = new TowerChallenge_TowerModelImpl();
	}
}
