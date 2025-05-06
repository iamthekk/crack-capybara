using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Fishing_fishMoveModel : BaseLocalModel
	{
		public Fishing_fishMove GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Fishing_fishMove> GetAllElements()
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

		public static readonly string fileName = "Fishing_fishMove";

		private Fishing_fishMoveModelImpl modelImpl = new Fishing_fishMoveModelImpl();
	}
}
