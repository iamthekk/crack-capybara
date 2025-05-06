using System;

namespace LocalModels.Bean
{
	public class Equip_equipCompose : BaseLocalBean
	{
		public int id { get; set; }

		public int composeTo { get; set; }

		public int qualityPlus { get; set; }

		public int composeNeed1 { get; set; }

		public int composeNeed2 { get; set; }

		public int composeNeed3 { get; set; }

		public int composeItem1 { get; set; }

		public int composeItem2 { get; set; }

		public int composeItem3 { get; set; }

		public int composeItem4 { get; set; }

		public int[] RecycleSelfquality { get; set; }

		public int[] RecycleSelf { get; set; }

		public string[] Recycle1New { get; set; }

		public string[] Recycle2New { get; set; }

		public string[] Recycle3New { get; set; }

		public string[] Recycle4New { get; set; }

		public string[] Recycle1 { get; set; }

		public string[] Recycle2 { get; set; }

		public string[] Recycle3 { get; set; }

		public string[] Recycle4 { get; set; }

		public int qualityAttributes { get; set; }

		public int qualityColor { get; set; }

		public string nameID { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.composeTo = base.readInt();
			this.qualityPlus = base.readInt();
			this.composeNeed1 = base.readInt();
			this.composeNeed2 = base.readInt();
			this.composeNeed3 = base.readInt();
			this.composeItem1 = base.readInt();
			this.composeItem2 = base.readInt();
			this.composeItem3 = base.readInt();
			this.composeItem4 = base.readInt();
			this.RecycleSelfquality = base.readArrayint();
			this.RecycleSelf = base.readArrayint();
			this.Recycle1New = base.readArraystring();
			this.Recycle2New = base.readArraystring();
			this.Recycle3New = base.readArraystring();
			this.Recycle4New = base.readArraystring();
			this.Recycle1 = base.readArraystring();
			this.Recycle2 = base.readArraystring();
			this.Recycle3 = base.readArraystring();
			this.Recycle4 = base.readArraystring();
			this.qualityAttributes = base.readInt();
			this.qualityColor = base.readInt();
			this.nameID = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Equip_equipCompose();
		}
	}
}
