using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsWaveChange : BaseEventArgs
	{
		public int CurWave { get; private set; }

		public int MaxWave { get; private set; }

		public void SetData(int curWave, int maxWave)
		{
			this.CurWave = curWave;
			this.MaxWave = maxWave;
		}

		public override void Clear()
		{
		}
	}
}
