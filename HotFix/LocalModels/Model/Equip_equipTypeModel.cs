using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Equip_equipTypeModel : BaseLocalModel
	{
		public Equip_equipType GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Equip_equipType> GetAllElements()
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

		public static readonly string fileName = "Equip_equipType";

		private Equip_equipTypeModelImpl modelImpl = new Equip_equipTypeModelImpl();
	}
}
