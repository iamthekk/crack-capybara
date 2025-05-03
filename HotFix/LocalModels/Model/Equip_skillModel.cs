using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Equip_skillModel : BaseLocalModel
	{
		public Equip_skill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Equip_skill> GetAllElements()
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

		public static readonly string fileName = "Equip_skill";

		private Equip_skillModelImpl modelImpl = new Equip_skillModelImpl();
	}
}
