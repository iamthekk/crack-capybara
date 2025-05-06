using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Collection_collectionStarModel : BaseLocalModel
	{
		public Collection_collectionStar GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Collection_collectionStar> GetAllElements()
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

		public static readonly string fileName = "Collection_collectionStar";

		private Collection_collectionStarModelImpl modelImpl = new Collection_collectionStarModelImpl();
	}
}
