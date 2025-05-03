using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ServerList_chatGropModel : BaseLocalModel
	{
		public ServerList_chatGrop GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ServerList_chatGrop> GetAllElements()
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

		public static readonly string fileName = "ServerList_chatGrop";

		private ServerList_chatGropModelImpl modelImpl = new ServerList_chatGropModelImpl();
	}
}
