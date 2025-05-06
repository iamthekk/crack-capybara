using System;
using Framework;
using Proto.Pet;

namespace HotFix
{
	public static class PetOpenEggHelper
	{
		public static void OnPetBoxFreeClick(EPetBoxType petBoxType)
		{
			AdData adData = GameApp.Data.GetDataModule(DataName.AdDataModule).GetAdData(8);
			if (adData == null)
			{
				return;
			}
			if (adData.GetRemainTimes() <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("pet_draw_free_unenough"));
				return;
			}
			AdBridge.PlayRewardVideo(8, delegate(bool isSuccess)
			{
				if (isSuccess)
				{
					NetworkUtils.Pet.PetDrawRequest((int)petBoxType, delegate(PetDrawResponse response)
					{
						if (response != null && response.Code == 0)
						{
							PetDataModule dataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
							GameApp.SDK.Analyze.Track_PetBoxOpen(dataModule.m_petDrawExpData.DrawLevel, GameTGACostCurrency.Ad, 0, "普通宠物蛋15抽", response.ShowPet);
						}
					});
				}
			});
		}

		public static void OnPetBoxAdvanceClickImpl(EPetBoxType petBoxType)
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			long itemDataCountByid = dataModule.GetItemDataCountByid(11UL);
			long itemDataCountByid2 = dataModule.GetItemDataCountByid(2UL);
			int ticketCost = 0;
			int diamondCost = 0;
			if (petBoxType == EPetBoxType.Draw35)
			{
				ticketCost = Singleton<GameConfig>.Instance.Pet35DrawTicketCost;
				diamondCost = Singleton<GameConfig>.Instance.Pet35DrawDiamondCost;
			}
			else
			{
				ticketCost = Singleton<GameConfig>.Instance.Pet15DrawTicketCost;
				diamondCost = Singleton<GameConfig>.Instance.Pet15DrawDiamondCost;
			}
			if (itemDataCountByid >= (long)ticketCost)
			{
				NetworkUtils.Pet.PetDrawRequest((int)petBoxType, delegate(PetDrawResponse response)
				{
					if (response != null && response.Code == 0)
					{
						PetDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.PetDataModule);
						string text = "";
						if (petBoxType == EPetBoxType.Draw15)
						{
							text = "高级宠物蛋15抽";
						}
						else if (petBoxType == EPetBoxType.Draw35)
						{
							text = "高级宠物蛋35抽";
						}
						GameApp.SDK.Analyze.Track_PetBoxOpen(dataModule2.m_petDrawExpData.DrawLevel, GameTGACostCurrency.PetEgg, ticketCost, text, response.ShowPet);
					}
				});
				return;
			}
			if (itemDataCountByid2 >= (long)diamondCost)
			{
				RememberTipCommonViewModule.TryRunRememberTip(new RememberTipCommonViewModule.OpenData
				{
					rememberTipType = RememberTipType.PetDraw,
					contentStr = Singleton<LanguageManager>.Instance.GetInfoByID("remember_tip_1", new object[] { diamondCost }),
					onConfirmCallback = delegate
					{
						NetworkUtils.Pet.PetDrawRequest((int)petBoxType, delegate(PetDrawResponse response)
						{
							if (response != null && response.Code == 0)
							{
								PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
								string text2 = "";
								if (petBoxType == EPetBoxType.Draw15)
								{
									text2 = "高级宠物蛋15抽";
								}
								else if (petBoxType == EPetBoxType.Draw35)
								{
									text2 = "高级宠物蛋35抽";
								}
								GameApp.SDK.Analyze.Track_PetBoxOpen(dataModule3.m_petDrawExpData.DrawLevel, GameTGACostCurrency.Gem, diamondCost, text2, response.ShowPet);
							}
						});
					}
				});
				return;
			}
			GameApp.View.ShowItemNotEnoughTip(2, true);
		}
	}
}
