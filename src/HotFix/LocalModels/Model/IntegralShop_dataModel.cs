using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IntegralShop_dataModel : BaseLocalModel
	{
		public IntegralShop_data GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IntegralShop_data> GetAllElements()
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

		public static readonly string fileName = "IntegralShop_data";

		private IntegralShop_dataModelImpl modelImpl = new IntegralShop_dataModelImpl();
	}
}
