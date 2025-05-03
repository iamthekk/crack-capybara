using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Guild_guildTaskModel : BaseLocalModel
	{
		public Guild_guildTask GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Guild_guildTask> GetAllElements()
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

		public static readonly string fileName = "Guild_guildTask";

		private Guild_guildTaskModelImpl modelImpl = new Guild_guildTaskModelImpl();
	}
}
