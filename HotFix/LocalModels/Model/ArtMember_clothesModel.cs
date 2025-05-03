using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtMember_clothesModel : BaseLocalModel
	{
		public ArtMember_clothes GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtMember_clothes> GetAllElements()
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

		public static readonly string fileName = "ArtMember_clothes";

		private ArtMember_clothesModelImpl modelImpl = new ArtMember_clothesModelImpl();
	}
}
