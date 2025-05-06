using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class LanguageRaft_languagetableModel : BaseLocalModel
	{
		public LanguageRaft_languagetable GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<LanguageRaft_languagetable> GetAllElements()
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

		public static readonly string fileName = "LanguageRaft_languagetable";

		private LanguageRaft_languagetableModelImpl modelImpl = new LanguageRaft_languagetableModelImpl();
	}
}
