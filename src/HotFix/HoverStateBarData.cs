using System;

namespace HotFix
{
	public class HoverStateBarData
	{
		public void SetData(ClampData hpData, ClampData rechargeData, bool isShowRecharge)
		{
			this.hpData = hpData;
			this.rechargeData = rechargeData;
			this.isShowRecharge = isShowRecharge;
		}

		public ClampData hpData;

		public ClampData rechargeData;

		public bool isShowRecharge;
	}
}
