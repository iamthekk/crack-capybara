using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Box_boxBaseModel : BaseLocalModel
	{
		public Box_boxBase GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Box_boxBase> GetAllElements()
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

		public static readonly string fileName = "Box_boxBase";

		private Box_boxBaseModelImpl modelImpl = new Box_boxBaseModelImpl();
	}
}
