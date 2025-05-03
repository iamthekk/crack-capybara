using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Vip_dataModel : BaseLocalModel
	{
		public Vip_data GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Vip_data> GetAllElements()
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

		public static readonly string fileName = "Vip_data";

		private Vip_dataModelImpl modelImpl = new Vip_dataModelImpl();
	}
}
