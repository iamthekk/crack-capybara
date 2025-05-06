using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Language_languagetableModel : BaseLocalModel
	{
		public Language_languagetable GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Language_languagetable> GetAllElements()
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

		public static readonly string fileName = "Language_languagetable";

		private Language_languagetableModelImpl modelImpl = new Language_languagetableModelImpl();
	}
}
