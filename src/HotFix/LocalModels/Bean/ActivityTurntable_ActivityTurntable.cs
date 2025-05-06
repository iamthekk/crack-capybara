using System;

namespace LocalModels.Bean
{
	public class ActivityTurntable_ActivityTurntable : BaseLocalBean
	{
		public int id { get; set; }

		public int OpenTime { get; set; }

		public int EndTime { get; set; }

		public int priceId { get; set; }

		public int singlePrice { get; set; }

		public int tenPrice { get; set; }

		public string[] poolTimes { get; set; }

		public string[] pool { get; set; }

		public string[] rate { get; set; }

		public string[] showRate { get; set; }

		public int miniPityCount { get; set; }

		public int miniPityRate { get; set; }

		public string[] limitItems { get; set; }

		public int bigPityCount { get; set; }

		public int timesRewardID { get; set; }

		public string ProdMailTempId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.OpenTime = base.readInt();
			this.EndTime = base.readInt();
			this.priceId = base.readInt();
			this.singlePrice = base.readInt();
			this.tenPrice = base.readInt();
			this.poolTimes = base.readArraystring();
			this.pool = base.readArraystring();
			this.rate = base.readArraystring();
			this.showRate = base.readArraystring();
			this.miniPityCount = base.readInt();
			this.miniPityRate = base.readInt();
			this.limitItems = base.readArraystring();
			this.bigPityCount = base.readInt();
			this.timesRewardID = base.readInt();
			this.ProdMailTempId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ActivityTurntable_ActivityTurntable();
		}
	}
}
