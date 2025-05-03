using System;

namespace LocalModels.Bean
{
	public class ServerList_serverList : BaseLocalBean
	{
		public int id { get; set; }

		public string desc { get; set; }

		public int[] range { get; set; }

		public string[] mark { get; set; }

		public int conditionCountMax { get; set; }

		public int conditionCountMin { get; set; }

		public int conditionDay { get; set; }

		public int[] statusNew { get; set; }

		public int[] statusFull { get; set; }

		public int sortId { get; set; }

		public string serverPrefix { get; set; }

		public string nameId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.desc = base.readLocalString();
			this.range = base.readArrayint();
			this.mark = base.readArraystring();
			this.conditionCountMax = base.readInt();
			this.conditionCountMin = base.readInt();
			this.conditionDay = base.readInt();
			this.statusNew = base.readArrayint();
			this.statusFull = base.readArrayint();
			this.sortId = base.readInt();
			this.serverPrefix = base.readLocalString();
			this.nameId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ServerList_serverList();
		}
	}
}
