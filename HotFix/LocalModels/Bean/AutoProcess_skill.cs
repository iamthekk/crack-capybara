using System;

namespace LocalModels.Bean
{
	public class AutoProcess_skill : BaseLocalBean
	{
		public int id { get; set; }

		public int SkillType { get; set; }

		public int SuitID { get; set; }

		public int SuitSkillID { get; set; }

		public int[] skills { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.SkillType = base.readInt();
			this.SuitID = base.readInt();
			this.SuitSkillID = base.readInt();
			this.skills = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new AutoProcess_skill();
		}
	}
}
