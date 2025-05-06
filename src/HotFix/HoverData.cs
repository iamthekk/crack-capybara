using System;
using System.Collections.Generic;
using Server;
using UnityEngine;

namespace HotFix
{
	public class HoverData
	{
		public int id { get; private set; }

		public MemberCamp targetCamp { get; private set; }

		public long currentHp { get; private set; }

		public long maxHp { get; private set; }

		public long currentRecharge { get; private set; }

		public long maxRecharge { get; private set; }

		public bool isShowRecharge { get; private set; }

		public long currentLegacyPower { get; private set; }

		public long maxLegacyPower { get; private set; }

		public HoverData(int id, Transform target, MemberCamp camp)
		{
			this.id = id;
			this.target = target;
			this.targetCamp = camp;
		}

		public void SetStateData(long curHp, long maxHp, long curRech, long maxRech, bool isShowRecharge, Dictionary<int, FP> curLegacyPower, Dictionary<int, FP> maxLegacyPower)
		{
			this.currentHp = curHp;
			this.maxHp = maxHp;
			this.currentRecharge = curRech;
			this.maxRecharge = maxRech;
			this.isShowRecharge = isShowRecharge;
		}

		public Transform target;
	}
}
