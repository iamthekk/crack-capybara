using System;

namespace LocalModels.Bean
{
	public class ChainPacks_ChainType : BaseLocalBean
	{
		public int id { get; set; }

		public int sort { get; set; }

		public string sprite_Banner { get; set; }

		public int atlasId_Title { get; set; }

		public string sprite_Title { get; set; }

		public string sprite_Bg { get; set; }

		public string sprite_Bg_Mask { get; set; }

		public string sprite_NodeBg_Current { get; set; }

		public string sprite_NodeBg_Other { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.sort = base.readInt();
			this.sprite_Banner = base.readLocalString();
			this.atlasId_Title = base.readInt();
			this.sprite_Title = base.readLocalString();
			this.sprite_Bg = base.readLocalString();
			this.sprite_Bg_Mask = base.readLocalString();
			this.sprite_NodeBg_Current = base.readLocalString();
			this.sprite_NodeBg_Other = base.readLocalString();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new ChainPacks_ChainType();
		}
	}
}
