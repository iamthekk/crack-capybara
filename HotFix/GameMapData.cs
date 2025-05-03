using System;
using Framework;

namespace HotFix
{
	public class GameMapData
	{
		public int CurrentBgm { get; private set; }

		public void SetCurrentBgm(int bgm)
		{
			this.CurrentBgm = bgm;
		}

		public GameMapData()
		{
			this.CurrentBgm = this.DefaultBgm;
		}

		public void PlayDefaultBgm()
		{
			if (this.CurrentBgm != this.DefaultBgm)
			{
				this.CurrentBgm = this.DefaultBgm;
				GameApp.Sound.PlayBGM(this.CurrentBgm, 1f);
			}
		}

		public void PlayBattleBgm()
		{
			if (this.CurrentBgm != this.BattleBgm)
			{
				this.CurrentBgm = this.BattleBgm;
				GameApp.Sound.PlayBGM(this.CurrentBgm, 1f);
			}
		}

		public void PlayBossBgm()
		{
			if (this.CurrentBgm != this.BossBgm)
			{
				this.CurrentBgm = this.BossBgm;
				GameApp.Sound.PlayBGM(this.CurrentBgm, 1f);
			}
		}

		public int MapId;

		public int RideId;

		public int DefaultBgm;

		public int BattleBgm;

		public int BossBgm;
	}
}
