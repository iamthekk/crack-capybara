using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TGASource_IAPModel : BaseLocalModel
	{
		public TGASource_IAP GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TGASource_IAP> GetAllElements()
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

		public static readonly string fileName = "TGASource_IAP";

		private TGASource_IAPModelImpl modelImpl = new TGASource_IAPModelImpl();
	}
}
