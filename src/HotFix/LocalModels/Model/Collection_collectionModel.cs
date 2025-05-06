using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Collection_collectionModel : BaseLocalModel
	{
		public Collection_collection GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Collection_collection> GetAllElements()
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

		public static readonly string fileName = "Collection_collection";

		private Collection_collectionModelImpl modelImpl = new Collection_collectionModelImpl();
	}
}
