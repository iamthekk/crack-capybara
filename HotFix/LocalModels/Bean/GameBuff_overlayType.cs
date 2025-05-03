using System;

namespace LocalModels.Bean
{
	public class GameBuff_overlayType : BaseLocalBean
	{
		public int id { get; set; }

		public int sameResultOverlayType { get; set; }

		public int sameTimeOverlayType { get; set; }

		public int diffResultOverlayType { get; set; }

		public int diffTimeOverlayType { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.sameResultOverlayType = base.readInt();
			this.sameTimeOverlayType = base.readInt();
			this.diffResultOverlayType = base.readInt();
			this.diffTimeOverlayType = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameBuff_overlayType();
		}
	}
}
