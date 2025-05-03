using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Shop_EquipActivityModel : BaseLocalModel
	{
		public Shop_EquipActivity GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Shop_EquipActivity> GetAllElements()
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

		public static readonly string fileName = "Shop_EquipActivity";

		private Shop_EquipActivityModelImpl modelImpl = new Shop_EquipActivityModelImpl();
	}
}
