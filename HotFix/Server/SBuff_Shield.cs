using System;

namespace Server
{
	public class SBuff_Shield : SBuffBase
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		protected override void ReadParameters(string parameters)
		{
		}

		protected override void OnTrigger()
		{
			base.OnTrigger();
			this.m_owner.skillFactory.CheckPlay(SkillTriggerType.ShieldAfter, this);
		}
	}
}
