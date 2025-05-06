using System;

namespace Server
{
	public class SkillTrigger_Undo : BaseSkillTrigger
	{
		public override int GetName()
		{
			return 999;
		}

		public override bool IsCanPlay(SkillTriggerData triggerData)
		{
			return false;
		}
	}
}
