using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Avatar_SkinModel : BaseLocalModel
	{
		public Avatar_Skin GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Avatar_Skin> GetAllElements()
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

		public static readonly string fileName = "Avatar_Skin";

		private Avatar_SkinModelImpl modelImpl = new Avatar_SkinModelImpl();
	}
}
