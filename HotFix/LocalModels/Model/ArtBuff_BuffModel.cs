using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtBuff_BuffModel : BaseLocalModel
	{
		public ArtBuff_Buff GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtBuff_Buff> GetAllElements()
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

		public static readonly string fileName = "ArtBuff_Buff";

		private ArtBuff_BuffModelImpl modelImpl = new ArtBuff_BuffModelImpl();
	}
}
