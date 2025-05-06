using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class FishData
	{
		public int fishId { get; private set; }

		public int fishWeight { get; private set; }

		public int floatWeight { get; private set; }

		public FishData(int id, int fishWeight, int floatWeight)
		{
			this.fishId = id;
			this.fishWeight = fishWeight;
			this.floatWeight = floatWeight;
		}

		public Fishing_fish Config
		{
			get
			{
				if (this._config == null)
				{
					this._config = GameApp.Table.GetManager().GetFishing_fishModelInstance().GetElementById(this.fishId);
				}
				return this._config;
			}
		}

		private Fishing_fish _config;
	}
}
