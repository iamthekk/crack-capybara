using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.ModelImpl;

namespace LocalModels.Model
{
	public class ArtSkill_SkillModel : BaseLocalModel
	{
		public ArtSkill_Skill GetElementById(int id)
		{
			return this.modelImpl.GetElementById(id);
		}

		public IList<ArtSkill_Skill> GetAllElements()
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

		public static readonly string fileName = "ArtSkill_Skill";

		private ArtSkill_SkillModelImpl modelImpl = new ArtSkill_SkillModelImpl();
	}
}
