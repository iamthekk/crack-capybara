using System;
using Framework;

namespace HotFix
{
	public class GameManager : Singleton<GameManager>
	{
		public int SpeedUpRate { get; private set; } = 1;

		public float GameSpeed { get; private set; } = 1f;

		public void SetSpeedRate(int rate)
		{
			this.SpeedUpRate = rate;
			this.SetGameSpeed(rate);
		}

		public void ActiveGameSpeed(bool isActive)
		{
			if (isActive)
			{
				this.SetGameSpeed(this.SpeedUpRate);
				return;
			}
			this.SetGameSpeed(1);
		}

		public int PvESpeedUpRate { get; private set; } = 1;

		public void SetPvESpeedRate(int rate)
		{
			this.PvESpeedUpRate = rate;
			this.SetGameSpeed(rate);
		}

		public void ActivePvESpeed(bool isActive)
		{
			if (isActive)
			{
				this.SetGameSpeed(this.PvESpeedUpRate);
				return;
			}
			GameApp.SetTimeScale(1f);
		}

		public int PvPSpeedUpRate { get; private set; } = 1;

		public void SetPvPSpeedRate(int rate)
		{
			this.PvPSpeedUpRate = rate;
			this.SetGameSpeed(rate);
		}

		public void ActivePvPSpeed(bool isActive)
		{
			if (isActive)
			{
				this.SetGameSpeed(this.PvPSpeedUpRate);
				return;
			}
			GameApp.SetTimeScale(1f);
		}

		public void ActiveSweepSpeed(bool isActive)
		{
			if (isActive)
			{
				GameApp.SetTimeScale(5f);
				return;
			}
			GameApp.SetTimeScale(1f);
		}

		private void SetGameSpeed(int rate)
		{
			if (rate == 2)
			{
				this.GameSpeed = 1.2f;
			}
			else if (rate == 3)
			{
				this.GameSpeed = 1.5f;
			}
			else
			{
				this.GameSpeed = 1f;
			}
			GameApp.SetTimeScale(this.GameSpeed);
		}

		public void ResetSpeed()
		{
			this.SpeedUpRate = 1;
		}
	}
}
