using System;

namespace LocalModels.Bean
{
	public class GameSkillBuild_skillBuild : BaseLocalBean
	{
		public int id { get; set; }

		public int unlockChapter { get; set; }

		public int[] source { get; set; }

		public int groupId { get; set; }

		public int level { get; set; }

		public int[] needGroup { get; set; }

		public int quality { get; set; }

		public int[] cost { get; set; }

		public int weight { get; set; }

		public int tag { get; set; }

		public string attributes { get; set; }

		public int recoverHp { get; set; }

		public int skillId { get; set; }

		public int[] compose { get; set; }

		public int composeType { get; set; }

		public int[] showCompose { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.unlockChapter = base.readInt();
			this.source = base.readArrayint();
			this.groupId = base.readInt();
			this.level = base.readInt();
			this.needGroup = base.readArrayint();
			this.quality = base.readInt();
			this.cost = base.readArrayint();
			this.weight = base.readInt();
			this.tag = base.readInt();
			this.attributes = base.readLocalString();
			this.recoverHp = base.readInt();
			this.skillId = base.readInt();
			this.compose = base.readArrayint();
			this.composeType = base.readInt();
			this.showCompose = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameSkillBuild_skillBuild();
		}
	}
}
