using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IAP_PushPacksModel : BaseLocalModel
	{
		public IAP_PushPacks GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IAP_PushPacks> GetAllElements()
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

		public static readonly string fileName = "IAP_PushPacks";

		private IAP_PushPacksModelImpl modelImpl = new IAP_PushPacksModelImpl();
	}
}
