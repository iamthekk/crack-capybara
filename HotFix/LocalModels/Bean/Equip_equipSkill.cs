using System;

namespace LocalModels.Bean
{
	public class Equip_equipSkill : BaseLocalBean
	{
		public int id { get; set; }

		public int[] qualitySkill { get; set; }

		public int[] qualityUnlock { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.qualitySkill = base.readArrayint();
			this.qualityUnlock = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_equipSkill();
		}
	}
}
