using System;

namespace LocalModels.Bean
{
	public class ServerList_serverGrop : BaseLocalBean
	{
		public int id { get; set; }

		public int[] range { get; set; }

		public string groupName { get; set; }

		public int sortId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.range = base.readArrayint();
			this.groupName = base.readLocalString();
			this.sortId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ServerList_serverGrop();
		}
	}
}
