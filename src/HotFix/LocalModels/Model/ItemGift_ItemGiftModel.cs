using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ItemGift_ItemGiftModel : BaseLocalModel
	{
		public ItemGift_ItemGift GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ItemGift_ItemGift> GetAllElements()
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

		public static readonly string fileName = "ItemGift_ItemGift";

		private ItemGift_ItemGiftModelImpl modelImpl = new ItemGift_ItemGiftModelImpl();
	}
}
