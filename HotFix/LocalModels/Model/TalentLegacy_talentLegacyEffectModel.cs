using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TalentLegacy_talentLegacyEffectModel : BaseLocalModel
	{
		public TalentLegacy_talentLegacyEffect GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TalentLegacy_talentLegacyEffect> GetAllElements()
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

		public static readonly string fileName = "TalentLegacy_talentLegacyEffect";

		private TalentLegacy_talentLegacyEffectModelImpl modelImpl = new TalentLegacy_talentLegacyEffectModelImpl();
	}
}
