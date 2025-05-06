using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_cardFlippingBase : BaseLocalBean
	{
		public int id { get; set; }

		public string[] reward1 { get; set; }

		public string[] iconRes1 { get; set; }

		public string[] iconFrameRes1 { get; set; }

		public string[] reward2 { get; set; }

		public string[] iconRes2 { get; set; }

		public string[] iconFrameRes2 { get; set; }

		public string[] reward3 { get; set; }

		public string[] iconRes3 { get; set; }

		public string[] iconFrameRes3 { get; set; }

		public int[] weights { get; set; }

		public string[] rewardTextIds { get; set; }

		public string languageId { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.reward1 = base.readArraystring();
			this.iconRes1 = base.readArraystring();
			this.iconFrameRes1 = base.readArraystring();
			this.reward2 = base.readArraystring();
			this.iconRes2 = base.readArraystring();
			this.iconFrameRes2 = base.readArraystring();
			this.reward3 = base.readArraystring();
			this.iconRes3 = base.readArraystring();
			this.iconFrameRes3 = base.readArraystring();
			this.weights = base.readArrayint();
			this.rewardTextIds = base.readArraystring();
			this.languageId = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_cardFlippingBase();
		}
	}
}
