using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArena : BaseLocalBean
	{
		public int id { get; set; }

		public int ranktop { get; set; }

		public int rankbuttom { get; set; }

		public int baseWinScore { get; set; }

		public int baseLoseScore { get; set; }

		public float addition { get; set; }

		public int[] winScoreSection { get; set; }

		public int[] loseScoreSection { get; set; }

		public string[] dailyRewards { get; set; }

		public string[] seasonRewards { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.ranktop = base.readInt();
			this.rankbuttom = base.readInt();
			this.baseWinScore = base.readInt();
			this.baseLoseScore = base.readInt();
			this.addition = base.readFloat();
			this.winScoreSection = base.readArrayint();
			this.loseScoreSection = base.readArrayint();
			this.dailyRewards = base.readArraystring();
			this.seasonRewards = base.readArraystring();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArena();
		}
	}
}
