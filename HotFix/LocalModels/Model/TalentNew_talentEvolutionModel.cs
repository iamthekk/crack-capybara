using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TalentNew_talentEvolutionModel : BaseLocalModel
	{
		public TalentNew_talentEvolution GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TalentNew_talentEvolution> GetAllElements()
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

		public static readonly string fileName = "TalentNew_talentEvolution";

		private TalentNew_talentEvolutionModelImpl modelImpl = new TalentNew_talentEvolutionModelImpl();
	}
}
