using System;
using LocalModels.Bean;

namespace HotFix
{
	public class WheelSymbolData
	{
		public WheelSymbolData(ChapterMiniGame_turntableReward cfg, int index)
		{
			this.cfg = cfg;
			this.index = index;
		}

		public void Calc(float totalWeightReal, float totalWeightShow, float offset)
		{
			this.totalWeightReal = totalWeightReal;
			this.totalWeightShow = totalWeightShow;
			this.offsetAngle += offset;
			this.areaAngle = 360f * (float)this.cfg.showWeight / this.totalWeightShow;
		}

		public ChapterMiniGame_turntableReward cfg;

		public int index;

		public float totalWeightReal;

		public float totalWeightShow;

		public float offsetAngle;

		public float areaAngle;
	}
}
