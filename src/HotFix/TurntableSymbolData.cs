using System;
using LocalModels.Bean;

namespace HotFix
{
	public class TurntableSymbolData
	{
		public TurntableSymbolData(ChapterActivity_ActvTurntableDetail cfg, int index)
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

		public ChapterActivity_ActvTurntableDetail cfg;

		public int index;

		public float totalWeightReal;

		public float totalWeightShow;

		public float offsetAngle;

		public float areaAngle;
	}
}
