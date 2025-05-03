using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Avatar_SceneSkinModel : BaseLocalModel
	{
		public Avatar_SceneSkin GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Avatar_SceneSkin> GetAllElements()
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

		public static readonly string fileName = "Avatar_SceneSkin";

		private Avatar_SceneSkinModelImpl modelImpl = new Avatar_SceneSkinModelImpl();
	}
}
