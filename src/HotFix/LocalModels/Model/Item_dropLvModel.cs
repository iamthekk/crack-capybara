using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Item_dropLvModel : BaseLocalModel
	{
		public Item_dropLv GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Item_dropLv> GetAllElements()
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

		public static readonly string fileName = "Item_dropLv";

		private Item_dropLvModelImpl modelImpl = new Item_dropLvModelImpl();
	}
}
