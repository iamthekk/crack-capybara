using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mining_oreBuildModel : BaseLocalModel
	{
		public Mining_oreBuild GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mining_oreBuild> GetAllElements()
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

		public static readonly string fileName = "Mining_oreBuild";

		private Mining_oreBuildModelImpl modelImpl = new Mining_oreBuildModelImpl();
	}
}
