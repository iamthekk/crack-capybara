using System;

namespace LocalModels.Bean
{
	public class CommonActivity_CommonActivity : BaseLocalBean
	{
		public int Id { get; set; }

		public int SortID { get; set; }

		public int Type { get; set; }

		public int RankID { get; set; }

		public int QuestID { get; set; }

		public int PayID { get; set; }

		public int ShopID { get; set; }

		public string[] FinalReward { get; set; }

		public string[] packageDrop { get; set; }

		public string[] removeItem { get; set; }

		public string[] reparation { get; set; }

		public string prodMailReparation { get; set; }

		public string testMailReparation { get; set; }

		public int OpenTime { get; set; }

		public int EndTime { get; set; }

		public int Round { get; set; }

		public int JumpInterface { get; set; }

		public string JumpInterfaceName { get; set; }

		public string ActvDes { get; set; }

		public int[] Currency { get; set; }

		public string Prod_MailTempId { get; set; }

		public string[] Param { get; set; }

		public int OutputItem { get; set; }

		public int atlasID { get; set; }

		public string Name { get; set; }

		public string banner { get; set; }

		public string banner2 { get; set; }

		public override bool readImpl()
		{
			this.Id = base.readInt();
			this.SortID = base.readInt();
			this.Type = base.readInt();
			this.RankID = base.readInt();
			this.QuestID = base.readInt();
			this.PayID = base.readInt();
			this.ShopID = base.readInt();
			this.FinalReward = base.readArraystring();
			this.packageDrop = base.readArraystring();
			this.removeItem = base.readArraystring();
			this.reparation = base.readArraystring();
			this.prodMailReparation = base.readLocalString();
			this.testMailReparation = base.readLocalString();
			this.OpenTime = base.readInt();
			this.EndTime = base.readInt();
			this.Round = base.readInt();
			this.JumpInterface = base.readInt();
			this.JumpInterfaceName = base.readLocalString();
			this.ActvDes = base.readLocalString();
			this.Currency = base.readArrayint();
			this.Prod_MailTempId = base.readLocalString();
			this.Param = base.readArraystring();
			this.OutputItem = base.readInt();
			this.atlasID = base.readInt();
			this.Name = base.readLocalString();
			this.banner = base.readLocalString();
			this.banner2 = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new CommonActivity_CommonActivity();
		}
	}
}
