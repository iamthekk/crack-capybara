using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Guild_guildConstModel : BaseLocalModel
	{
		public Guild_guildConst GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Guild_guildConst> GetAllElements()
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

		public static readonly string fileName = "Guild_guildConst";

		private Guild_guildConstModelImpl modelImpl = new Guild_guildConstModelImpl();
	}
}
