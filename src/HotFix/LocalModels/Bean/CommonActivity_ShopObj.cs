using System;

namespace LocalModels.Bean
{
	public class CommonActivity_ShopObj : BaseLocalBean
	{
		public int id { get; set; }

		public string Objnote { get; set; }

		public int ObjGroup { get; set; }

		public string ObjName { get; set; }

		public int objToplimit { get; set; }

		public int ObjPrice1 { get; set; }

		public int ObjPrice2 { get; set; }

		public string[] ObjGoods { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.Objnote = base.readLocalString();
			this.ObjGroup = base.readInt();
			this.ObjName = base.readLocalString();
			this.objToplimit = base.readInt();
			this.ObjPrice1 = base.readInt();
			this.ObjPrice2 = base.readInt();
			this.ObjGoods = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CommonActivity_ShopObj();
		}
	}
}
