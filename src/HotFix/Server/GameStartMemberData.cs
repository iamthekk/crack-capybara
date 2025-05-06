using System;
using System.Collections.Generic;

namespace Server
{
	public class GameStartMemberData
	{
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Format("m_instanceId:{0}, ", this.m_instanceId),
				string.Format("m_curHp:{0}, ", this.m_curHp),
				string.Format("m_curHp:{0}, ", this.m_maxHp),
				string.Format("m_curRecharge:{0}, ", this.m_curRecharge),
				string.Format("m_maxRecharge:{0} ", this.m_maxRecharge),
				string.Format("m_attack:{0}, ", this.m_attack),
				string.Format("m_defense:{0}, ", this.m_defense),
				"\n"
			});
		}

		public int m_instanceId;

		public FP m_curHp;

		public FP m_maxHp;

		public FP m_curRecharge;

		public FP m_maxRecharge;

		public Dictionary<int, FP> m_curLegacyPower = new Dictionary<int, FP>();

		public Dictionary<int, FP> m_maxLegacyPower = new Dictionary<int, FP>();

		public FP m_attack;

		public FP m_defense;

		public bool m_isUsedRevive;

		public List<int> m_skillIds = new List<int>();
	}
}
