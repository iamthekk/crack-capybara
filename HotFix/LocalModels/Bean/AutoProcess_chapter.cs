using System;

namespace LocalModels.Bean
{
	public class AutoProcess_chapter : BaseLocalBean
	{
		public int ID { get; set; }

		public int NameLanguageId { get; set; }

		public string PrefabPath { get; set; }

		public string PreviewPrefabPath { get; set; }

		public string GodAttack { get; set; }

		public int battleSceneID { get; set; }

		public int[] monster { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readInt();
			this.NameLanguageId = base.readInt();
			this.PrefabPath = base.readLocalString();
			this.PreviewPrefabPath = base.readLocalString();
			this.GodAttack = base.readLocalString();
			this.battleSceneID = base.readInt();
			this.monster = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new AutoProcess_chapter();
		}
	}
}
