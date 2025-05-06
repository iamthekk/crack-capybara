using System;

namespace HotFix.Client
{
	public class CSkill_Knife : CSkillBase
	{
		protected override void OnPlay()
		{
		}

		protected override void OnPlayComplete()
		{
			if (this.curStartEffectBase != null)
			{
				this.m_startEffectFactory.RemoveNode(this.curStartEffectBase);
			}
		}

		protected override void OnReadParameters(string parameters)
		{
		}
	}
}
