using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Equip_equipModel : BaseLocalModel
	{
		public Equip_equip GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Equip_equip> GetAllElements()
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

		public static readonly string fileName = "Equip_equip";

		private Equip_equipModelImpl modelImpl = new Equip_equipModelImpl();
	}
}
