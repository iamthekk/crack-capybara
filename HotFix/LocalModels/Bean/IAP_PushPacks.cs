using System;

namespace LocalModels.Bean
{
	public class IAP_PushPacks : BaseLocalBean
	{
		public int id { get; set; }

		public int packType { get; set; }

		public int group { get; set; }

		public string[] products { get; set; }

		public string[] products2 { get; set; }

		public string[] products3 { get; set; }

		public int priority { get; set; }

		public string parameters { get; set; }

		public string nameID { get; set; }

		public string descID { get; set; }

		public int valueNum { get; set; }

		public string[] valueDescID { get; set; }

		public int iconAtlasID { get; set; }

		public string iconName { get; set; }

		public int isEffect { get; set; }

		public string nodePrefab { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.packType = base.readInt();
			this.group = base.readInt();
			this.products = base.readArraystring();
			this.products2 = base.readArraystring();
			this.products3 = base.readArraystring();
			this.priority = base.readInt();
			this.parameters = base.readLocalString();
			this.nameID = base.readLocalString();
			this.descID = base.readLocalString();
			this.valueNum = base.readInt();
			this.valueDescID = base.readArraystring();
			this.iconAtlasID = base.readInt();
			this.iconName = base.readLocalString();
			this.isEffect = base.readInt();
			this.nodePrefab = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new IAP_PushPacks();
		}
	}
}
