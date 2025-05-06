using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IAP_PurchaseModel : BaseLocalModel
	{
		public IAP_Purchase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IAP_Purchase> GetAllElements()
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

		public static readonly string fileName = "IAP_Purchase";

		private IAP_PurchaseModelImpl modelImpl = new IAP_PurchaseModelImpl();
	}
}
