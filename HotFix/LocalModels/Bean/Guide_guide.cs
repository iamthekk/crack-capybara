using System;

namespace LocalModels.Bean
{
	public class Guide_guide : BaseLocalBean
	{
		public int id { get; set; }

		public int guideGroup { get; set; }

		public int canBreak { get; set; }

		public int weight { get; set; }

		public int groupOver { get; set; }

		public int autoLockScroll { get; set; }

		public int isLockScreen { get; set; }

		public string guideTarget { get; set; }

		public string[] guideTrigger { get; set; }

		public string[] guideCondition { get; set; }

		public string[] guideComplete { get; set; }

		public string[] guideStyles { get; set; }

		public int additional { get; set; }

		public string[] guideAction { get; set; }

		public int overOnStart { get; set; }

		public int showDelay { get; set; }

		public override bool readImpl()
		{
			this.id = base.readInt();
			this.guideGroup = base.readInt();
			this.canBreak = base.readInt();
			this.weight = base.readInt();
			this.groupOver = base.readInt();
			this.autoLockScroll = base.readInt();
			this.isLockScreen = base.readInt();
			this.guideTarget = base.readLocalString();
			this.guideTrigger = base.readArraystring();
			this.guideCondition = base.readArraystring();
			this.guideComplete = base.readArraystring();
			this.guideStyles = base.readArraystring();
			this.additional = base.readInt();
			this.guideAction = base.readArraystring();
			this.overOnStart = base.readInt();
			this.showDelay = base.readInt();
			return true;
		}

		public override BaseLocalBean createBean()
		{
			return new Guide_guide();
		}
	}
}
