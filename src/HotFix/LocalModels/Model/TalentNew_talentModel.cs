using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TalentNew_talentModel : BaseLocalModel
	{
		public TalentNew_talent GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TalentNew_talent> GetAllElements()
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

		public static readonly string fileName = "TalentNew_talent";

		private TalentNew_talentModelImpl modelImpl = new TalentNew_talentModelImpl();
	}
}
