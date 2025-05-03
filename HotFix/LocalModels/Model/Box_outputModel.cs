using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Box_outputModel : BaseLocalModel
	{
		public Box_output GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Box_output> GetAllElements()
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

		public static readonly string fileName = "Box_output";

		private Box_outputModelImpl modelImpl = new Box_outputModelImpl();
	}
}
