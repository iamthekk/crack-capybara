using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TalentLegacy_careerModel : BaseLocalModel
	{
		public TalentLegacy_career GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TalentLegacy_career> GetAllElements()
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

		public static readonly string fileName = "TalentLegacy_career";

		private TalentLegacy_careerModelImpl modelImpl = new TalentLegacy_careerModelImpl();
	}
}
