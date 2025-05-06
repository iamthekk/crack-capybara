using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class Equip_equipSkillModel : BaseLocalModel
	{
		public Equip_equipSkill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<Equip_equipSkill> GetAllElements()
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

		public static readonly string fileName = "Equip_equipSkill";

		private Equip_equipSkillModelImpl modelImpl = new Equip_equipSkillModelImpl();
	}
}
