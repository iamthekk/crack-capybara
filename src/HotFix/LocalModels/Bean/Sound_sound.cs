using System;

namespace LocalModels.Bean
{
	public class Sound_sound : BaseLocalBean
	{
		public int ID { get; set; }

		public string path { get; set; }

		public float volume { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.path = base.readLocalString();
			this.volume = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Sound_sound();
		}
	}
}
