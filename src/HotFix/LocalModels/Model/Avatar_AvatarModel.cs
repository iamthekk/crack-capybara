using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Avatar_AvatarModel : BaseLocalModel
	{
		public Avatar_Avatar GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Avatar_Avatar> GetAllElements()
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

		public static readonly string fileName = "Avatar_Avatar";

		private Avatar_AvatarModelImpl modelImpl = new Avatar_AvatarModelImpl();
	}
}
