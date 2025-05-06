using System;

namespace Server
{
	public class SBuff_Fire : SBuffBase
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

		protected override void OnInitBefore()
		{
			MemberAttributeData attribute = this.m_attacker.memberData.attribute;
			this.m_buffData.m_overlayMax += attribute.FireBuffMaxLayerAdd.FloorToInt();
			int num = attribute.FireBuffAddRound.FloorToInt() - attribute.FireBuffReductionRound.FloorToInt();
			this.m_buffData.m_duration += num;
			base.SetDuration(this.m_buffData.m_duration);
		}
	}
}
