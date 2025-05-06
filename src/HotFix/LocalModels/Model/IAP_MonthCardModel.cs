using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class IAP_MonthCardModel : BaseLocalModel
	{
		public IAP_MonthCard GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<IAP_MonthCard> GetAllElements()
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

		public static readonly string fileName = "IAP_MonthCard";

		private IAP_MonthCardModelImpl modelImpl = new IAP_MonthCardModelImpl();
	}
}
