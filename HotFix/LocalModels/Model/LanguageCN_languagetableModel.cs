using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class LanguageCN_languagetableModel : BaseLocalModel
	{
		public LanguageCN_languagetable GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<LanguageCN_languagetable> GetAllElements()
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

		public static readonly string fileName = "LanguageCN_languagetable";

		private LanguageCN_languagetableModelImpl modelImpl = new LanguageCN_languagetableModelImpl();
	}
}
