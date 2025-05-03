using System;

namespace LocalModels.Bean
{
	public class CommonActivity_ConsumeObj : BaseLocalBean
	{
		public int Id { get; set; }

		public int ObjGroup { get; set; }

		public string ObjName { get; set; }

		public int ObjNum { get; set; }

		public int ObjType { get; set; }

		public string[] ObjReward { get; set; }

		public override bool readImpl()
		{
			this.Id = base.readInt();
			this.ObjGroup = base.readInt();
			this.ObjName = base.readLocalString();
			this.ObjNum = base.readInt();
			this.ObjType = base.readInt();
			this.ObjReward = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CommonActivity_ConsumeObj();
		}
	}
}
