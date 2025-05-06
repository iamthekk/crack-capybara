using System;

namespace LocalModels.Bean
{
	public class GameCamera_Shake : BaseLocalBean
	{
		public int ID { get; set; }

		public int shakeType { get; set; }

		public float delay { get; set; }

		public float duration { get; set; }

		public float power { get; set; }

		public int count { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.shakeType = base.readInt();
			this.delay = base.readFloat();
			this.duration = base.readFloat();
			this.power = base.readFloat();
			this.count = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameCamera_Shake();
		}
	}
}
