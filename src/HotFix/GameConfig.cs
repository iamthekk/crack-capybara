using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.GameTestTools;
using LocalModels.Bean;
using LocalModels.Model;

namespace HotFix
{
	public class GameConfig : Singleton<GameConfig>
	{
		public static int GameEvent_NonFood_LostHp { get; private set; }

		public static int Angel_Recover_HpRate { get; private set; }

		public static int[] GameEvent_SlotTrain_Prices { get; private set; }

		public static int GameEvent_SlotTrain_Refresh_Price { get; private set; }

		public static int GameEvent_RefreshSkill_AdId { get; private set; } = 10;

		public int Tower_RankSingleCount { get; set; } = 30;

		public int Tower_RankMaxPage { get; set; } = 30;

		public int Tower_RecoverInterval { get; set; } = 60;

		public int Tower_RecoverMax { get; set; } = 1;

		public int ActivityWeek_RankSingleCount { get; set; } = 30;

		public int ActivityWeek_RankMaxPage { get; set; } = 30;

		public int WorldBoss_RankSingleCount { get; set; } = 10;

		public int WorldBoss_RankMaxPage { get; set; } = 30;

		public static int HangUp_Drop_Time { get; private set; }

		public static int HangUp_Max_Time { get; private set; }

		public static int HangUp_AD_Time { get; private set; }

		public static int HangUp_AD_ID { get; private set; } = 11;

		public static int Mining_Ticket_ID { get; private set; }

		public static int Mining_Ticket_RecoverLimit { get; private set; }

		public static int Mining_Ticket_RecoverInterval { get; private set; }

		public static int Mining_Draw_ItemId { get; private set; }

		public static int Mining_Draw_Cost { get; private set; }

		public static int Mining_Max_Rate { get; private set; }

		public static int Mining_AD_ItemNum { get; private set; }

		public static int Privilege_Card_Frame_Item { get; private set; }

		public static int RogueDungeon_Start_SkillSourceID { get; private set; }

		public static int RogueDungeon_Start_RandomSkillNum { get; private set; }

		public static int RogueDungeon_Start_SelectSkillNum { get; private set; }

		public static int RogueDungeon_Start_SelectRound { get; private set; }

		public static int RogueDungeon_Floor_SkillSourceID { get; private set; }

		public static int RogueDungeon_Floor_RandomSkillNum { get; private set; }

		public static int RogueDungeon_Floor_SelectSkillNum { get; private set; }

		public static int RogueDungeon_Floor_SelectRound { get; private set; }

		public static int RogueDungeon_Floor_RecoverHP { get; private set; }

		public static int WorldBoss_SkillSourceID { get; private set; }

		public static int WorldBoss_RandomSkillNum { get; private set; }

		public static int WorldBoss_SelectSkillNum { get; private set; }

		public static int WorldBoss_SelectRound { get; private set; }

		public static int GuildBoss_SkillSourceID { get; private set; }

		public static int GuildBoss_RandomSkillNum { get; private set; }

		public static int GuildBoss_SelectSkillNum { get; private set; }

		public static int GuildBoss_SelectRound { get; private set; }

		public int GetUrlImageOutTime { get; private set; }

		public static bool NotExist_Test { get; private set; }

		public static int NotExist_Avatar_Id { get; private set; }

		public static int NotExist_Avatar_Icon_AtlasId { get; private set; }

		public static string NotExist_Avatar_Icon_Sprite { get; private set; }

		public static int NotExist_AvatarFrame_Id { get; private set; }

		public static int NotExist_AvatarFrame_Icon_AtlasId { get; private set; }

		public static string NotExist_AvatarFrame_Icon_Sprite { get; private set; }

		public static int NotExist_Title_Id { get; private set; }

		public static int NotExist_Item_Id { get; private set; }

		public static int NotExist_Item_Icon_AtlasId { get; private set; }

		public static string NotExist_Item_Icon_Sprite { get; private set; }

		public static int NotExist_Item_Quality_AtlasId { get; private set; }

		public static string NotExist_Item_Quality_Sprite { get; private set; }

		public static int NotExist_Pet_Id { get; private set; }

		public static int NotExist_Pet_Icon_AtlasId { get; private set; }

		public static string NotExist_Pet_Icon_Sprite { get; private set; }

		public static int NotExist_Pet_Quality_AtlasId { get; private set; }

		public static string NotExist_Pet_Quality_Sprite { get; private set; }

		public static int NotExist_Mount_Id { get; private set; }

		public static int NotExist_Mount_Icon_AtlasId { get; private set; }

		public static string NotExist_Mount_Icon_Sprite { get; private set; }

		public static int NotExist_Mount_Quality_AtlasId { get; private set; }

		public static string NotExist_Mount_Quality_Sprite { get; private set; }

		public static int NotExist_Artifact_Id { get; private set; }

		public static int NotExist_Artifact_Icon_AtlasId { get; private set; }

		public static string NotExist_Artifact_Icon_Sprite { get; private set; }

		public static int NotExist_Artifact_Quality_AtlasId { get; private set; }

		public static string NotExist_Artifact_Quality_Sprite { get; private set; }

		public static int NotExist_Equip_Weapon_Id { get; private set; }

		public static int NotExist_Equip_Clothes_Id { get; private set; }

		public static int NotExist_Equip_Ring_Id { get; private set; }

		public static int NotExist_Equip_Accessory_Id { get; private set; }

		[GameTestMethod("版本兼容", "测试不存在的数据（开）", "", 0)]
		private static void SetNotExistTestOpen()
		{
			GameConfig.NotExist_Test = true;
		}

		[GameTestMethod("版本兼容", "测试不存在的数据（关）", "", 0)]
		private static void SetNotExistTestClose()
		{
			GameConfig.NotExist_Test = false;
		}

		public int ShopEquipSUpExchangeCount { get; private set; }

		public void InitTableData()
		{
			GameConfig_ConfigModel gameConfig_ConfigModelInstance = GameApp.Table.GetManager().GetGameConfig_ConfigModelInstance();
			this.ShopEquipSUpExchangeCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(113).Value);
			this.Fitness_MaxCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1401).Value);
			this.MainCity_BoxDurations = gameConfig_ConfigModelInstance.GetElementById(1501).Value.GetListInt(',');
			this.MainCity_BoxLoopDurations = gameConfig_ConfigModelInstance.GetElementById(1502).Value.GetListInt(',');
			this.MainCity_BoxMaxIntegralCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1503).Value);
			this.MainCity_BoxIntegralDropIDs = gameConfig_ConfigModelInstance.GetElementById(1504).Value.GetListInt('|');
			this.MainCity_BoxMaxCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1505).Value);
			this.Sociality_RankSingleCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1601).Value);
			this.Sociality_RankMaxPage = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1602).Value);
			this.AvatarDefaultFrameId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1701).Value);
			this.AvatarDefaultId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1702).Value);
			this.ClothesDefaultBodyId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1711).Value);
			this.ClothesDefaultHeadId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1712).Value);
			this.ClothesDefaultAccessoryId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1713).Value);
			this.AvatarDefaultTitleId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1714).Value);
			this.NickName_MinLength = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1703).Value);
			this.NickName_MaxLength = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1704).Value);
			this.SlaveMaxCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1801).Value);
			this.HeartBeatInterval = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1901).Value);
			this.APMax = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2001).Value);
			this.APRecoverInterval = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2002).Value);
			this.APBattleCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2003).Value);
			this.APRevoltCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2004).Value);
			this.APLootCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2005).Value);
			this.Hangup_Gold_Duration = int.Parse(gameConfig_ConfigModelInstance.GetElementById(101).Value);
			this.Hangup_Dust_Duration = int.Parse(gameConfig_ConfigModelInstance.GetElementById(102).Value);
			this.Hangup_HeroExp_Duration = int.Parse(gameConfig_ConfigModelInstance.GetElementById(103).Value);
			this.Hangup_RedPoint_Duration = long.Parse(gameConfig_ConfigModelInstance.GetElementById(104).Value);
			this.Hangup_MinShowDuration = long.Parse(gameConfig_ConfigModelInstance.GetElementById(105).Value);
			this.Hangup_MaxDuration = long.Parse(gameConfig_ConfigModelInstance.GetElementById(106).Value);
			this.Tower_RankMaxPage = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1602).Value);
			this.Tower_RankSingleCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1601).Value);
			this.Tower_RecoverMax = int.Parse(gameConfig_ConfigModelInstance.GetElementById(3001).Value);
			this.Tower_RecoverInterval = int.Parse(gameConfig_ConfigModelInstance.GetElementById(3002).Value);
			this.BattlePass_DiamonScoreRate = float.Parse(gameConfig_ConfigModelInstance.GetElementById(802).Value);
			if (this.BattlePass_DiamonScoreRate <= 0f)
			{
				this.BattlePass_DiamonScoreRate = 10f;
			}
			GameConfig.NotExist_Avatar_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000001).Value);
			GameConfig.NotExist_AvatarFrame_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000002).Value);
			GameConfig.NotExist_Title_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000003).Value);
			string[] array = gameConfig_ConfigModelInstance.GetElementById(900000011).Value.Split(',', StringSplitOptions.None);
			GameConfig.NotExist_Item_Id = int.Parse(array[0]);
			GameConfig.NotExist_Item_Icon_AtlasId = int.Parse(array[1]);
			GameConfig.NotExist_Item_Icon_Sprite = array[2];
			GameConfig.NotExist_Item_Quality_AtlasId = int.Parse(array[3]);
			GameConfig.NotExist_Item_Quality_Sprite = array[4];
			string[] array2 = gameConfig_ConfigModelInstance.GetElementById(900000012).Value.Split(',', StringSplitOptions.None);
			GameConfig.NotExist_Pet_Id = int.Parse(array2[0]);
			GameConfig.NotExist_Pet_Icon_AtlasId = int.Parse(array2[1]);
			GameConfig.NotExist_Pet_Icon_Sprite = array2[2];
			GameConfig.NotExist_Pet_Quality_AtlasId = int.Parse(array2[3]);
			GameConfig.NotExist_Pet_Quality_Sprite = array2[4];
			string[] array3 = gameConfig_ConfigModelInstance.GetElementById(900000013).Value.Split(',', StringSplitOptions.None);
			GameConfig.NotExist_Mount_Id = int.Parse(array3[0]);
			GameConfig.NotExist_Mount_Icon_AtlasId = int.Parse(array3[1]);
			GameConfig.NotExist_Mount_Icon_Sprite = array3[2];
			GameConfig.NotExist_Mount_Quality_AtlasId = int.Parse(array3[3]);
			GameConfig.NotExist_Mount_Quality_Sprite = array3[4];
			string[] array4 = gameConfig_ConfigModelInstance.GetElementById(900000014).Value.Split(',', StringSplitOptions.None);
			GameConfig.NotExist_Artifact_Id = int.Parse(array4[0]);
			GameConfig.NotExist_Artifact_Icon_AtlasId = int.Parse(array4[1]);
			GameConfig.NotExist_Artifact_Icon_Sprite = array4[2];
			GameConfig.NotExist_Artifact_Quality_AtlasId = int.Parse(array4[3]);
			GameConfig.NotExist_Artifact_Quality_Sprite = array4[4];
			GameConfig.NotExist_Equip_Weapon_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000021).Value);
			GameConfig.NotExist_Equip_Clothes_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000022).Value);
			GameConfig.NotExist_Equip_Ring_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000023).Value);
			GameConfig.NotExist_Equip_Accessory_Id = int.Parse(gameConfig_ConfigModelInstance.GetElementById(900000024).Value);
			string[] array5 = gameConfig_ConfigModelInstance.GetElementById(3107).Value.Split('|', StringSplitOptions.None);
			this.CrossArena_RefreshOppListCost = new int[array5.Length];
			for (int i = 0; i < array5.Length; i++)
			{
				this.CrossArena_RefreshOppListCost[i] = int.Parse(array5[i]);
			}
			int num;
			if (int.TryParse(gameConfig_ConfigModelInstance.GetElementById(1003).Value, out num))
			{
				GameConfig.GameEvent_NonFood_LostHp = num;
			}
			int num2;
			if (int.TryParse(gameConfig_ConfigModelInstance.GetElementById(1004).Value, out num2))
			{
				GameConfig.Angel_Recover_HpRate = num2;
			}
			string[] array6 = GameApp.Table.GetManager().GetGameConfig_ConfigModelInstance().GetElementById(1005)
				.Value.Split('|', StringSplitOptions.None);
			GameConfig.GameEvent_SlotTrain_Prices = new int[array6.Length];
			for (int j = 0; j < array6.Length; j++)
			{
				GameConfig.GameEvent_SlotTrain_Prices[j] = int.Parse(array6[j]);
			}
			GameConfig.GameEvent_SlotTrain_Refresh_Price = int.Parse(GameApp.Table.GetManager().GetGameConfig_ConfigModelInstance().GetElementById(1006)
				.Value);
			this.PeAdDrawResultCountMin = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2101).Value);
			this.PeAdDrawResultCountMax = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2102).Value);
			this.Pet15DrawTicketCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2103).Value);
			this.Pet15DrawResultCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2104).Value);
			this.Pet15DrawDiamondCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2107).Value);
			this.Pet35DrawTicketCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2105).Value);
			this.Pet35DrawResultCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2106).Value);
			this.Pet35DrawDiamondCost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2108).Value);
			this.MaxDailyScore = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2201).Value);
			this.MaxWeeklyScore = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2202).Value);
			GameConfig.GameEvent_RefreshSkill_AdId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1007).Value);
			GameConfig.HangUp_Drop_Time = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8001).Value);
			GameConfig.HangUp_Max_Time = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8002).Value);
			GameConfig.HangUp_AD_Time = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8003).Value);
			GameConfig.HangUp_AD_ID = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8004).Value);
			GameConfig.Mining_Ticket_ID = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2021).Value);
			GameConfig.Mining_Ticket_RecoverLimit = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2022).Value);
			GameConfig.Mining_Ticket_RecoverInterval = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2023).Value);
			GameConfig.Mining_Draw_ItemId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2024).Value);
			GameConfig.Mining_Draw_Cost = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2025).Value);
			GameConfig.Mining_Max_Rate = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2028).Value);
			GameConfig.Mining_AD_ItemNum = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2029).Value);
			GameConfig.Privilege_Card_Frame_Item = int.Parse(gameConfig_ConfigModelInstance.GetElementById(1700).Value);
			string[] array7 = gameConfig_ConfigModelInstance.GetElementById(2301).Value.Split(",", StringSplitOptions.None);
			this.PetTrainingCostId = int.Parse(array7[0]);
			this.PetTrainingCostValue = int.Parse(array7[1]);
			this.PetTraningLockCostId = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2302).Value);
			string[] array8 = gameConfig_ConfigModelInstance.GetElementById(2303).Value.Split("|", StringSplitOptions.None);
			this.PetTrainingLockCostValueList = new List<int>();
			for (int k = 0; k < array8.Length; k++)
			{
				this.PetTrainingLockCostValueList.Add(int.Parse(array8[k]));
			}
			this.PetMaxLevel = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2304).Value);
			this.PetShowMaxCount = int.Parse(gameConfig_ConfigModelInstance.GetElementById(2305).Value);
			this.TalentLegacySpeedItemTime = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8505).Value);
			this.TalentLegacyGoldSpeedTime = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8504).Value.Split('|', StringSplitOptions.None)[1]);
			this.TalentLegacyGoldSpeedNum = int.Parse(gameConfig_ConfigModelInstance.GetElementById(8504).Value.Split('|', StringSplitOptions.None)[0].Split(',', StringSplitOptions.None)[1]);
			List<int> listInt = gameConfig_ConfigModelInstance.GetElementById(3201).Value.GetListInt('|');
			if (listInt.Count > 0)
			{
				GameConfig.RogueDungeon_Start_SkillSourceID = listInt[0];
			}
			if (listInt.Count > 1)
			{
				GameConfig.RogueDungeon_Start_RandomSkillNum = listInt[1];
			}
			if (listInt.Count > 2)
			{
				GameConfig.RogueDungeon_Start_SelectSkillNum = listInt[2];
			}
			if (listInt.Count > 3)
			{
				GameConfig.RogueDungeon_Start_SelectRound = listInt[3];
			}
			List<int> listInt2 = gameConfig_ConfigModelInstance.GetElementById(3202).Value.GetListInt('|');
			if (listInt2.Count > 0)
			{
				GameConfig.RogueDungeon_Floor_SkillSourceID = listInt2[0];
			}
			if (listInt2.Count > 1)
			{
				GameConfig.RogueDungeon_Floor_RandomSkillNum = listInt2[1];
			}
			if (listInt2.Count > 2)
			{
				GameConfig.RogueDungeon_Floor_SelectSkillNum = listInt2[2];
			}
			if (listInt2.Count > 3)
			{
				GameConfig.RogueDungeon_Floor_SelectRound = listInt2[3];
			}
			GameConfig.RogueDungeon_Floor_RecoverHP = int.Parse(gameConfig_ConfigModelInstance.GetElementById(3204).Value);
			List<int> listInt3 = gameConfig_ConfigModelInstance.GetElementById(3301).Value.GetListInt('|');
			if (listInt3.Count > 0)
			{
				GameConfig.WorldBoss_SkillSourceID = listInt3[0];
			}
			if (listInt3.Count > 1)
			{
				GameConfig.WorldBoss_RandomSkillNum = listInt3[1];
			}
			if (listInt3.Count > 2)
			{
				GameConfig.WorldBoss_SelectSkillNum = listInt3[2];
			}
			if (listInt3.Count > 3)
			{
				GameConfig.WorldBoss_SelectRound = listInt3[3];
			}
			List<int> listInt4 = gameConfig_ConfigModelInstance.GetElementById(3401).Value.GetListInt('|');
			if (listInt4.Count > 0)
			{
				GameConfig.GuildBoss_SkillSourceID = listInt4[0];
			}
			if (listInt4.Count > 1)
			{
				GameConfig.GuildBoss_RandomSkillNum = listInt4[1];
			}
			if (listInt4.Count > 2)
			{
				GameConfig.GuildBoss_SelectSkillNum = listInt4[2];
			}
			if (listInt4.Count > 3)
			{
				GameConfig.GuildBoss_SelectRound = listInt4[3];
			}
			GameConfig.Sweep_Free_Rates = gameConfig_ConfigModelInstance.GetElementById(9003).Value.GetListInt('|');
			GameConfig.Sweep_MonthCard_Rates = gameConfig_ConfigModelInstance.GetElementById(9004).Value.GetListInt('|');
			GameConfig.Chapter_Activity_Wheel_Rates = gameConfig_ConfigModelInstance.GetElementById(9005).Value.GetListInt('|');
			this.GetUrlImageOutTime = int.Parse(gameConfig_ConfigModelInstance.GetElementById(11001).Value);
			this.InitAttributeLinkData();
		}

		public static int GetSlotTrainPrice(int playCount)
		{
			if (playCount < GameConfig.GameEvent_SlotTrain_Prices.Length)
			{
				return GameConfig.GameEvent_SlotTrain_Prices[playCount];
			}
			return -1;
		}

		public long GetPetLockPassiveCostValue(int lockCount)
		{
			long num;
			if (lockCount <= 0)
			{
				num = 0L;
			}
			else if (lockCount >= this.PetTrainingLockCostValueList.Count)
			{
				num = (long)this.PetTrainingLockCostValueList[this.PetTrainingLockCostValueList.Count - 1];
			}
			else
			{
				num = (long)this.PetTrainingLockCostValueList[lockCount - 1];
			}
			return num;
		}

		private void InitAttributeLinkData()
		{
			this.AttributeLinkDict.Clear();
			IList<Attribute_AttrText> allElements = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Attribute_AttrText attribute_AttrText = allElements[i];
				if (attribute_AttrText != null && !attribute_AttrText.ID.IsEmpty())
				{
					if (!this.AttributeLinkDict.ContainsKey(attribute_AttrText.linkID))
					{
						this.AttributeLinkDict.Add(attribute_AttrText.linkID, attribute_AttrText.ID);
					}
					else
					{
						HLog.LogError("AttributeTable has same LinkID: ", attribute_AttrText.linkID.ToString());
					}
				}
			}
		}

		public string GetAttributeKeyByLinkID(int linkID)
		{
			return CollectionExtensions.GetValueOrDefault<int, string>(this.AttributeLinkDict, linkID, "");
		}

		public const int Game_MainPlayID = 10001;

		public const int Battle_FriendlyCount = 5;

		public const int Battle_EnemyCount = 5;

		public const int Battle_BuffsMax = 20;

		public const float GameBattle_EnterBattle_CameraX = 2.6f;

		public const float GameBattle_EnterBattleFish_CameraX = 2f;

		public const float GameBattle_EnterBattle_Time = 2f;

		public const float GameEvent_UpAndDown_Time = 0.7f;

		public static float GameBattle_FriendPoint_Distance = 0f;

		public static float GameBattle_EnemyPoint_Distance = 12f;

		public static float GameBattle_WavePoint_Distance = 8f;

		public static float GameBattle_NpcPoint_Height = 3f;

		public const float GameBattle_Point_Offset = 2f;

		public const float GameBattle_Raft_MoveSpeed = 2f;

		public const float GameBattle_Raft_FastMoveSpeed = 8f;

		public const float GameBattle_Player_MoveSpeed = 4f;

		public const float GameBattle_Player_FastMoveSpeed = 8f;

		public const float GameBattle_ArriveEnemy_Distance = 5.3f;

		public const float GameBattle_Tackle_Distance = 3.5f;

		public const float GameBattle_RemoveNpc_Distance = 15f;

		public const float GameBattle_BattleNpc_MoveSpeed = 5f;

		public const float GameBattle_RemoveBattleNpc_Distance = 15f;

		public const float GameBattle_EnemyRaft_MoveSpeed = 5f;

		public const float GameBattle_EnemyRaft_FastMoveSpeed = 12f;

		public const float GameBattle_Enemy_MoveSpeed = 6f;

		public const float GameBattle_Enemy_FastMoveSpeed = 12f;

		public const float GameBattle_MonsterExit_Distance = 6f;

		public const float GameBattle_MonsterExit_Offset = 4f;

		public const float GameBattle_MonsterExit_Speed = 2.57f;

		public const float ResetWorldPositionX = 1000f;

		public const string Map_SortingLayer_Background = "BackGround";

		public const string Map_SortingLayer_Member = "Member";

		public const int Map_OrderLayer_Ride = -10;

		public const int Map_OrderLayer_NormalNpc = -15;

		public const int Map_OrderLayer_FishShadow = -76;

		public const int Map_OrderLayer_Fish = 1;

		public int GameEvent_StageLoop = 20;

		public float GameEvent_Power_AddAttack = 1.015f;

		public float GameEvent_Power_AddHP = 1.045f;

		public int GameEvent_Shop_GoodsNum = 3;

		public const int GameEvent_LevelUP_RandomSkillNum = 3;

		public const int GameEvent_LevelUP_SelectSkillNum = 1;

		public const float GameEvent_Fishing_Height = 2.5f;

		public const float GameEvent_Fishing_Time = 0.5f;

		public const float GameEvent_PassingNpc_MoveSpeed = 1f;

		public const int GameBGM_Login = 1;

		public const int GameBGM_GuildBoss = 650;

		public const int GameAudio_UI_FlyBattleShell = 54;

		public const int GameAudio_UI_Win = 52;

		public const int GameAudio_UI_Settlement = 53;

		public const int GameAudio_UI_Fighting = 55;

		public const int GameAudio_UI_BattleLevelUp = 57;

		public const int GameAudio_UI_ShowSelectSkill = 58;

		public const int GameAudio_UI_ShowSurprise = 59;

		public const int GameAudio_UI_Slot_ShowReward = 62;

		public const int GameAudio_UI_UnlockSkill = 66;

		public const int GameBGM_CG = 3;

		public const int ChapterFinish_Win = 88;

		public const int ChapterFinish_Fail = 89;

		public const float GameSpeedUp = 1.2f;

		public const float GameSpeedUpFast = 1.5f;

		public const float SweepSpeed = 5f;

		public const int CurrencyID_Coin = 1;

		public const int CurrencyID_Diamond = 2;

		public const int CurrencyID_Power = 3;

		public const int CurrencyID_Shell = 51;

		public const int AtalsId_UICommonHot = 8;

		public const int AtlasId_IconPet = 29;

		public const int AtlasID_Battle = 101;

		public const int AtlasID_SkillIcon = 102;

		public const int AtlasID_Emoticons = 103;

		public const int AtlasID_EventItemIcon = 104;

		public const int AtlasID_CommonHot = 105;

		public const int AtlasID_FishIcon = 108;

		public const int AtlasID_FishRodIcon = 109;

		public const int AtlasID_TalentIcon = 110;

		public const int AtlasID_CommonIcon = 111;

		public const int AtlasID_IconBuff = 113;

		public const int AtlasID_SlotMachine = 114;

		public const int AtlasID_BattleNew = 115;

		public const int AtlasID_IconPet = 117;

		public const int AtlasID_IconMount = 123;

		public const int AtlasID_IconArtifact = 124;

		public const int AtlasID_IconChapterActivity = 120;

		public const int AtlasID_BattleButton = 121;

		public const int AtlasID_ChapterActivityRank = 122;

		public const int AtlasID_MountIcon = 123;

		public const int AtlasID_ArtifactIcon = 124;

		public int RedPoint_NoTipsTimeSec = 60;

		public int PeAdDrawResultCountMin = 15;

		public int PeAdDrawResultCountMax = 35;

		public int Pet15DrawTicketCost = 15;

		public int Pet15DrawDiamondCost = 30;

		public int Pet15DrawResultCount = 15;

		public int Pet35DrawTicketCost = 30;

		public int Pet35DrawDiamondCost = 60;

		public int Pet35DrawResultCount = 35;

		public int Hangup_Gold_Duration = 20;

		public int Hangup_Dust_Duration = 20;

		public int Hangup_HeroExp_Duration = 20;

		public long Hangup_MaxDuration = 86400L;

		public long Hangup_RedPoint_Duration = 600L;

		public long Hangup_MinShowDuration = 600L;

		public const int Ride_Raft_ID = 1001;

		public const string TITLE_COLOR_GRAY = "#D8D7EF";

		public const string TITLE_COLOR_GREEN = "#D3F24E";

		public const string TITLE_COLOR_BLUE = "#91EEF6";

		public const string TITLE_COLOR_PURPLE = "#FF604D";

		public const string TITLE_COLOR_ORANGE = "#FF9852";

		public const string TITLE_COLOR_RED = "#FF604D";

		public const string TEXT_COLOR_GRAY = "#807B92";

		public const string TEXT_COLOR_GREEN = "#3AA23A";

		public const string TEXT_COLOR_BLUE = "#4695D1";

		public const string TEXT_COLOR_PURPLE = "#9F5CEE";

		public const string TEXT_COLOR_ORANGE = "#F4883F";

		public const string TEXT_COLOR_RED = "#E55254";

		public int Fitness_MaxCount = 5;

		public List<int> MainCity_BoxDurations = new List<int>();

		public List<int> MainCity_BoxLoopDurations = new List<int>();

		public int MainCity_BoxMaxIntegralCount = 100;

		public List<int> MainCity_BoxIntegralDropIDs = new List<int>();

		public int MainCity_BoxMaxCount = 10;

		public int Equip_UnLockLevel1 = 1;

		public int Equip_UnLockLevel2 = 1;

		public int Equip_UnLockLevel3 = 1;

		public int Equip_UnLockLevel4 = 1;

		public int Equip_UnLockLevel5 = 30;

		public int Equip_UnLockLevel6 = 45;

		public int Equip_ExpOrGold_Rate = 100;

		public int Equip_OneClickMergeMaxComposeId = 4;

		public int Equip_MaxComposeId = 15;

		public int PetMaxLevel = 100;

		public int PetShowMaxCount = 8;

		public int PetTrainingCostId = 30;

		public int PetTrainingCostValue = 100;

		public int PetTraningLockCostId = 31;

		public List<int> PetTrainingLockCostValueList;

		public int Sociality_RankSingleCount = 30;

		public int Sociality_RankMaxPage = 30;

		public int AvatarDefaultId = 1;

		public int AvatarDefaultFrameId = 2;

		public int AvatarDefaultTitleId = 3;

		public int ClothesDefaultHeadId = 1;

		public int ClothesDefaultBodyId = 2;

		public int ClothesDefaultAccessoryId = 3;

		public int NickName_MinLength = 3;

		public int NickName_MaxLength = 18;

		public int SlaveMaxCount = 5;

		public int HeartBeatInterval = 5;

		public int APMax = 100;

		public int APRecoverInterval = 60;

		public int APBattleCost = 60;

		public int APRevoltCost = 60;

		public int APLootCost = 60;

		public int[] CrossArena_RefreshOppListCost = new int[] { 0, 20, 40, 80 };

		public float BattlePass_DiamonScoreRate = 10f;

		public int MaxDailyScore = 110;

		public int MaxWeeklyScore = 550;

		public const int Mining_AD_ID = 12;

		public int GuildChatInterval = 1;

		public int SceneChatInterval = 1;

		public int ServerChatInterval = 1;

		public int ChatCharCount = 100;

		public const int RogueDungeon_Battle_Interval = 2000;

		public bool IsPopNotice = true;

		public const int TalentLegacy_AD = 14;

		public const int TalentLegacySpeedItemId = 47;

		public int TalentLegacySpeedItemTime;

		public int TalentLegacyGoldSpeedNum;

		public int TalentLegacyGoldSpeedTime;

		public Dictionary<int, string> AttributeLinkDict = new Dictionary<int, string>();

		public const int NewWorld_Need_Chapter = 80;

		public const int NewWorld_Need_Talent = 74;

		public const int NewWorld_Need_Tower = 110010;

		public const long Clear_Local_Record_Time = 20L;

		public const bool ClickEnergyOpenNewUI = true;

		public static List<int> Sweep_Free_Rates;

		public static List<int> Sweep_MonthCard_Rates;

		public static List<int> Chapter_Activity_Wheel_Rates;
	}
}
