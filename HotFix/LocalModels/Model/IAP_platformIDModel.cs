using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IAP_platformIDModel : BaseLocalModel
	{
		public IAP_platformID GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IAP_platformID> GetAllElements()
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

		public static readonly string fileName = "IAP_platformID";

		private IAP_platformIDModelImpl modelImpl = new IAP_platformIDModelImpl();
	}
}
