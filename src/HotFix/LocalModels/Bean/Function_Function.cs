using System;

namespace LocalModels.Bean
{
	public class Function_Function : BaseLocalBean
	{
		public int id { get; set; }

		public int unlockType { get; set; }

		public string unlockArgs { get; set; }

		public int OpenTime { get; set; }

		public string nameID { get; set; }

		public string desID { get; set; }

		public int showView { get; set; }

		public int showIndex { get; set; }

		public int iconAtlasID { get; set; }

		public string iconName { get; set; }

		public string flyPos { get; set; }

		public string packVersion { get; set; }

		public int resVersion { get; set; }

		public string versionTime { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.unlockType = base.readInt();
			this.unlockArgs = base.readLocalString();
			this.OpenTime = base.readInt();
			this.nameID = base.readLocalString();
			this.desID = base.readLocalString();
			this.showView = base.readInt();
			this.showIndex = base.readInt();
			this.iconAtlasID = base.readInt();
			this.iconName = base.readLocalString();
			this.flyPos = base.readLocalString();
			this.packVersion = base.readLocalString();
			this.resVersion = base.readInt();
			this.versionTime = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Function_Function();
		}
	}
}
