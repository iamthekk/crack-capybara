using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Mount_advanceMountModel : BaseLocalModel
	{
		public Mount_advanceMount GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Mount_advanceMount> GetAllElements()
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

		public static readonly string fileName = "Mount_advanceMount";

		private Mount_advanceMountModelImpl modelImpl = new Mount_advanceMountModelImpl();
	}
}
