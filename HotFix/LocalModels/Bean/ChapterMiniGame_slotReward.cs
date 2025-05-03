using System;

namespace LocalModels.Bean
{
	public class ChapterMiniGame_slotReward : BaseLocalBean
	{
		public int id { get; set; }

		public int symbolType { get; set; }

		public int weight2 { get; set; }

		public int weight3 { get; set; }

		public string[] reward2 { get; set; }

		public string reward2txt { get; set; }

		public string[] reward3 { get; set; }

		public string reward3txt { get; set; }

		public int atlasRegular { get; set; }

		public string iconRegular { get; set; }

		public int atlasBlur { get; set; }

		public string iconBlur { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.symbolType = base.readInt();
			this.weight2 = base.readInt();
			this.weight3 = base.readInt();
			this.reward2 = base.readArraystring();
			this.reward2txt = base.readLocalString();
			this.reward3 = base.readArraystring();
			this.reward3txt = base.readLocalString();
			this.atlasRegular = base.readInt();
			this.iconRegular = base.readLocalString();
			this.atlasBlur = base.readInt();
			this.iconBlur = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterMiniGame_slotReward();
		}
	}
}
