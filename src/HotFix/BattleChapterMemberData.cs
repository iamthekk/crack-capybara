using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public class BattleChapterMemberData
	{
		public int id { get; private set; }

		public List<int> skillIds { get; private set; }

		public FP hp { get; private set; }

		public FP recharge { get; private set; }

		public Dictionary<int, FP> legacyPower { get; private set; }

		public bool isUsedRevive { get; private set; }

		public BattleChapterMemberData(int id, List<int> skillIds, MemberAttributeData attribute, FP hp, FP recharge, bool isUsedRevive)
		{
			this.id = id;
			this.skillIds = skillIds;
			this.attributeData = attribute;
			this.hp = hp;
			this.recharge = recharge;
			this.isUsedRevive = isUsedRevive;
		}

		public MemberAttributeData attributeData;
	}
}
