using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Guide_guideModel : BaseLocalModel
	{
		public Guide_guide GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Guide_guide> GetAllElements()
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

		public static readonly string fileName = "Guide_guide";

		private Guide_guideModelImpl modelImpl = new Guide_guideModelImpl();
	}
}
