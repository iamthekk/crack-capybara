using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mount_mountLevelModel : BaseLocalModel
	{
		public Mount_mountLevel GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mount_mountLevel> GetAllElements()
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

		public static readonly string fileName = "Mount_mountLevel";

		private Mount_mountLevelModelImpl modelImpl = new Mount_mountLevelModelImpl();
	}
}
