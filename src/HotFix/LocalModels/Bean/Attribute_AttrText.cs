using System;

namespace LocalModels.Bean
{
	public class Attribute_AttrText : BaseLocalBean
	{
		public string ID { get; set; }

		public int linkID { get; set; }

		public int IsPower { get; set; }

		public float Value { get; set; }

		public string LanguageId { get; set; }

		public int SortID { get; set; }

		public int DisplayType { get; set; }

		public int iconAtlasID { get; set; }

		public string iconName { get; set; }

		public override bool readImpl()
		{
			this.ID = base.readLocalString();
			this.linkID = base.readInt();
			this.IsPower = base.readInt();
			this.Value = base.readFloat();
			this.LanguageId = base.readLocalString();
			this.SortID = base.readInt();
			this.DisplayType = base.readInt();
			this.iconAtlasID = base.readInt();
			this.iconName = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Attribute_AttrText();
		}
	}
}
