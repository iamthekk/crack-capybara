using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Collection_starColorModel : BaseLocalModel
	{
		public Collection_starColor GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Collection_starColor> GetAllElements()
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

		public static readonly string fileName = "Collection_starColor";

		private Collection_starColorModelImpl modelImpl = new Collection_starColorModelImpl();
	}
}
