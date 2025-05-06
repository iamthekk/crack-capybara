using System;

namespace LocalModels.Bean
{
	public class CommonActivity_DropObj : BaseLocalBean
	{
		public int Id { get; set; }

		public string ObjName { get; set; }

		public int ObjPrice1 { get; set; }

		public int ObjPrice2 { get; set; }

		public string[] ObjGoods { get; set; }

		public override bool readImpl()
		{
			this.Id = base.readInt();
			this.ObjName = base.readLocalString();
			this.ObjPrice1 = base.readInt();
			this.ObjPrice2 = base.readInt();
			this.ObjGoods = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CommonActivity_DropObj();
		}
	}
}
