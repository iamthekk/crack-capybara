using System;

namespace LocalModels.Bean
{
	public class ServerList_chatGrop : BaseLocalBean
	{
		public int id { get; set; }

		public int[] range { get; set; }

		public int groupId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.range = base.readArrayint();
			this.groupId = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ServerList_chatGrop();
		}
	}
}
