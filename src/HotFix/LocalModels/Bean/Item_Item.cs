using System;

namespace LocalModels.Bean
{
	public class Item_Item : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasID { get; set; }

		public string icon { get; set; }

		public string smallicon { get; set; }

		public int iconSizeType { get; set; }

		public int quality { get; set; }

		public int inPackage { get; set; }

		public int stackable { get; set; }

		public string nameID { get; set; }

		public string describeID { get; set; }

		public string typeDescribeID { get; set; }

		public int itemType { get; set; }

		public string[] itemTypeParam { get; set; }

		public int itemGiftId { get; set; }

		public int propType { get; set; }

		public int showEffect { get; set; }

		public int[] redTypes { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasID = base.readInt();
			this.icon = base.readLocalString();
			this.smallicon = base.readLocalString();
			this.iconSizeType = base.readInt();
			this.quality = base.readInt();
			this.inPackage = base.readInt();
			this.stackable = base.readInt();
			this.nameID = base.readLocalString();
			this.describeID = base.readLocalString();
			this.typeDescribeID = base.readLocalString();
			this.itemType = base.readInt();
			this.itemTypeParam = base.readArraystring();
			this.itemGiftId = base.readInt();
			this.propType = base.readInt();
			this.showEffect = base.readInt();
			this.redTypes = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Item_Item();
		}
	}
}
