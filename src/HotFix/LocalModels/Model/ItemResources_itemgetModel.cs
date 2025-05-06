using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ItemResources_itemgetModel : BaseLocalModel
	{
		public ItemResources_itemget GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ItemResources_itemget> GetAllElements()
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

		public static readonly string fileName = "ItemResources_itemget";

		private ItemResources_itemgetModelImpl modelImpl = new ItemResources_itemgetModelImpl();
	}
}
