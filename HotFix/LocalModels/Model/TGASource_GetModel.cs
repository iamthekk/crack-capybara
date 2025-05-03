using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TGASource_GetModel : BaseLocalModel
	{
		public TGASource_Get GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TGASource_Get> GetAllElements()
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

		public static readonly string fileName = "TGASource_Get";

		private TGASource_GetModelImpl modelImpl = new TGASource_GetModelImpl();
	}
}
