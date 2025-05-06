using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Vip_vipModel : BaseLocalModel
	{
		public Vip_vip GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Vip_vip> GetAllElements()
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

		public static readonly string fileName = "Vip_vip";

		private Vip_vipModelImpl modelImpl = new Vip_vipModelImpl();
	}
}
