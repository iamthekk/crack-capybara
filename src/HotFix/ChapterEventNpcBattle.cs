using System;
using Framework;

namespace HotFix
{
	public class ChapterEventNpcBattle : ChapterEventBattle
	{
		public override void OnInit()
		{
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			GameEventDataNpcBattle gameEventDataNpcBattle = this.currentData as GameEventDataNpcBattle;
			if (gameEventDataNpcBattle != null)
			{
				dataModule.SetBattleSeed(gameEventDataNpcBattle.poolData.randomSeed);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_MonsterGroupFight, null);
		}
	}
}
