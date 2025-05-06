using System;

namespace LocalModels.Bean
{
	public class IAP_MonthCard : BaseLocalBean
	{
		public int id { get; set; }

		public int duration { get; set; }

		public int alarmClock { get; set; }

		public string postID { get; set; }

		public string[] products { get; set; }

		public string[] productsPerDay { get; set; }

		public int rebate { get; set; }

		public string[] tips { get; set; }

		public string[] rewarTips { get; set; }

		public string[] privilege { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.duration = base.readInt();
			this.alarmClock = base.readInt();
			this.postID = base.readLocalString();
			this.products = base.readArraystring();
			this.productsPerDay = base.readArraystring();
			this.rebate = base.readInt();
			this.tips = base.readArraystring();
			this.rewarTips = base.readArraystring();
			this.privilege = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_MonthCard();
		}
	}
}
