using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Guild_guildGiftModel : BaseLocalModel
	{
		public Guild_guildGift GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Guild_guildGift> GetAllElements()
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

		public static readonly string fileName = "Guild_guildGift";

		private Guild_guildGiftModelImpl modelImpl = new Guild_guildGiftModelImpl();
	}
}
