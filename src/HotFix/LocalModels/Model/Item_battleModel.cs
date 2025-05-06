using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Item_battleModel : BaseLocalModel
	{
		public Item_battle GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Item_battle> GetAllElements()
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

		public static readonly string fileName = "Item_battle";

		private Item_battleModelImpl modelImpl = new Item_battleModelImpl();
	}
}
