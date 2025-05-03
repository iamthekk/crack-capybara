using System;

namespace LocalModels.Bean
{
	public class GameBuff_buff : BaseLocalBean
	{
		public int id { get; set; }

		public int atlasID { get; set; }

		public string spriteName { get; set; }

		public int hudLanguageID { get; set; }

		public int soundId { get; set; }

		public int freedType { get; set; }

		public int buffStateType { get; set; }

		public int prefabID { get; set; }

		public string transType { get; set; }

		public int removeEffectID { get; set; }

		public int removeEffectPos { get; set; }

		public int buffType { get; set; }

		public string parameters { get; set; }

		public int duration { get; set; }

		public int overlayType { get; set; }

		public int overlayMax { get; set; }

		public int addCountRound { get; set; }

		public string addAttributes { get; set; }

		public string addAttributesOnce { get; set; }

		public int[] triggerTags { get; set; }

		public string triggerAttributes { get; set; }

		public int[] triggerBuffs { get; set; }

		public int[] removeAddBuffs { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.atlasID = base.readInt();
			this.spriteName = base.readLocalString();
			this.hudLanguageID = base.readInt();
			this.soundId = base.readInt();
			this.freedType = base.readInt();
			this.buffStateType = base.readInt();
			this.prefabID = base.readInt();
			this.transType = base.readLocalString();
			this.removeEffectID = base.readInt();
			this.removeEffectPos = base.readInt();
			this.buffType = base.readInt();
			this.parameters = base.readLocalString();
			this.duration = base.readInt();
			this.overlayType = base.readInt();
			this.overlayMax = base.readInt();
			this.addCountRound = base.readInt();
			this.addAttributes = base.readLocalString();
			this.addAttributesOnce = base.readLocalString();
			this.triggerTags = base.readArrayint();
			this.triggerAttributes = base.readLocalString();
			this.triggerBuffs = base.readArrayint();
			this.removeAddBuffs = base.readArrayint();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new GameBuff_buff();
		}
	}
}
