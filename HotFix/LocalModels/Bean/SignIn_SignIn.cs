using System;

namespace LocalModels.Bean
{
	public class SignIn_SignIn : BaseLocalBean
	{
		public int ID { get; set; }

		public int MinDays { get; set; }

		public int MaxDays { get; set; }

		public string[] Day1 { get; set; }

		public string[] Day2 { get; set; }

		public string[] Day3 { get; set; }

		public string[] Day4 { get; set; }

		public string[] Day5 { get; set; }

		public string[] Day6 { get; set; }

		public string[] Day7 { get; set; }

		public string colour { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.MinDays = base.readInt();
			this.MaxDays = base.readInt();
			this.Day1 = base.readArraystring();
			this.Day2 = base.readArraystring();
			this.Day3 = base.readArraystring();
			this.Day4 = base.readArraystring();
			this.Day5 = base.readArraystring();
			this.Day6 = base.readArraystring();
			this.Day7 = base.readArraystring();
			this.colour = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new SignIn_SignIn();
		}
	}
}
