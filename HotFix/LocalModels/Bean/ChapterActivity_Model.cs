using System;

namespace LocalModels.Bean
{
	public class ChapterActivity_Model : BaseLocalBean
	{
		public int id { get; set; }

		public string path { get; set; }

		public float scale { get; set; }

		public float offsetY { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.path = base.readLocalString();
			this.scale = base.readFloat();
			this.offsetY = base.readFloat();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChapterActivity_Model();
		}
	}
}
