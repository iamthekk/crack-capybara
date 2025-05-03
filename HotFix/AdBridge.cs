using System;
using Framework;

namespace HotFix
{
	public static class AdBridge
	{
		public static void PlayRewardVideo(int adId, Action<bool> callback)
		{
			AdBridge.<>c__DisplayClass1_0 CS$<>8__locals1 = new AdBridge.<>c__DisplayClass1_0();
			CS$<>8__locals1.callback = callback;
			if (!GameApp.Data.GetDataModule(DataName.IAPDataModule).MonthCard.IsActivePrivilege(CardPrivilegeKind.NoAd))
			{
				CS$<>8__locals1.tgaSource = GameTGATools.ADSourceName(adId);
				WatchADViewModule.OpenData openData = new WatchADViewModule.OpenData();
				openData.adId = adId;
				openData.onWatch = new Action(CS$<>8__locals1.<PlayRewardVideo>g__Watch|0);
				openData.onJump = new Action(CS$<>8__locals1.<PlayRewardVideo>g__Jump|1);
				GameApp.View.OpenView(ViewName.WatchAdViewModule, openData, 1, null, null);
				return;
			}
			Action<bool> callback2 = CS$<>8__locals1.callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(true);
		}

		private static Action<bool> onRewardVideoEndCallback;
	}
}
