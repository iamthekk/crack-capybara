using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class LanguageRaft_languageTabModel : BaseLocalModel
	{
		public LanguageRaft_languageTab GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<LanguageRaft_languageTab> GetAllElements()
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

		public static readonly string fileName = "LanguageRaft_languageTab";

		private LanguageRaft_languageTabModelImpl modelImpl = new LanguageRaft_languageTabModelImpl();
	}
}
