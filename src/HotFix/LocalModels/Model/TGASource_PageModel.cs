using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class TGASource_PageModel : BaseLocalModel
	{
		public TGASource_Page GetElementById(string id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<TGASource_Page> GetAllElements()
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

		public static readonly string fileName = "TGASource_Page";

		private TGASource_PageModelImpl modelImpl = new TGASource_PageModelImpl();
	}
}
