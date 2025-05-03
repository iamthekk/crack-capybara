using System;

namespace LocalModels.Bean
{
	public class Chapter_eventType : BaseLocalBean
	{
		public int id { get; set; }

		public int gameEventType { get; set; }

		public int[] events { get; set; }

		public int[] events_Android { get; set; }

		public int[] eventsV1_0_10 { get; set; }

		public int[] events_AndroidV1_0_10 { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.gameEventType = base.readInt();
			this.events = base.readArrayint();
			this.events_Android = base.readArrayint();
			this.eventsV1_0_10 = base.readArrayint();
			this.events_AndroidV1_0_10 = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Chapter_eventType();
		}
	}
}
