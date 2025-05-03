using System;

namespace LocalModels.Bean
{
	public class CrossArena_CrossArenaChallengeListRule : BaseLocalBean
	{
		public int ID { get; set; }

		public int[] Rank { get; set; }

		public float[] pos1 { get; set; }

		public float[] pos2 { get; set; }

		public float[] pos3 { get; set; }

		public float[] pos5 { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.Rank = base.readArrayint();
			this.pos1 = base.readArrayfloat();
			this.pos2 = base.readArrayfloat();
			this.pos3 = base.readArrayfloat();
			this.pos5 = base.readArrayfloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CrossArena_CrossArenaChallengeListRule();
		}
	}
}
