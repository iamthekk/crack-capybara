using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Shop_SummonPoolModel : BaseLocalModel
	{
		public Shop_SummonPool GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Shop_SummonPool> GetAllElements()
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

		public static readonly string fileName = "Shop_SummonPool";

		private Shop_SummonPoolModelImpl modelImpl = new Shop_SummonPoolModelImpl();
	}
}
