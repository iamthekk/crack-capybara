using System;

namespace LocalModels.Bean
{
	public class Ride_Ride : BaseLocalBean
	{
		public int id { get; set; }

		public int model { get; set; }

		public string skin { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.model = base.readInt();
			this.skin = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Ride_Ride();
		}
	}
}
