using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IntegralShop_goodsModel : BaseLocalModel
	{
		public IntegralShop_goods GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IntegralShop_goods> GetAllElements()
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

		public static readonly string fileName = "IntegralShop_goods";

		private IntegralShop_goodsModelImpl modelImpl = new IntegralShop_goodsModelImpl();
	}
}
