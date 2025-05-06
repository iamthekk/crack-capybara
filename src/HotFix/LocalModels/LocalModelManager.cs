using System;
using System.Collections.Generic;
using LocalModels.Bean;
using LocalModels.Model;

namespace LocalModels
{
	public class LocalModelManager : BaseLocalModelManager
	{
		public Guide_guideModel GetGuide_guideModelInstance()
		{
			return this.Guide_guideModelInstance;
		}

		public IList<Guide_guide> GetGuide_guideElements()
		{
			return this.Guide_guideModelInstance.GetAllElements();
		}

		public Guide_guide GetGuide_guide(int key)
		{
			return this.Guide_guideModelInstance.GetElementById(key);
		}

		public GameSkill_skillModel GetGameSkill_skillModelInstance()
		{
			return this.GameSkill_skillModelInstance;
		}

		public IList<GameSkill_skill> GetGameSkill_skillElements()
		{
			return this.GameSkill_skillModelInstance.GetAllElements();
		}

		public GameSkill_skill GetGameSkill_skill(int key)
		{
			return this.GameSkill_skillModelInstance.GetElementById(key);
		}

		public ActivityTurntable_TurntableRewardModel GetActivityTurntable_TurntableRewardModelInstance()
		{
			return this.ActivityTurntable_TurntableRewardModelInstance;
		}

		public IList<ActivityTurntable_TurntableReward> GetActivityTurntable_TurntableRewardElements()
		{
			return this.ActivityTurntable_TurntableRewardModelInstance.GetAllElements();
		}

		public ActivityTurntable_TurntableReward GetActivityTurntable_TurntableReward(int key)
		{
			return this.ActivityTurntable_TurntableRewardModelInstance.GetElementById(key);
		}

		public AutoProcess_monsterModel GetAutoProcess_monsterModelInstance()
		{
			return this.AutoProcess_monsterModelInstance;
		}

		public IList<AutoProcess_monster> GetAutoProcess_monsterElements()
		{
			return this.AutoProcess_monsterModelInstance.GetAllElements();
		}

		public AutoProcess_monster GetAutoProcess_monster(int key)
		{
			return this.AutoProcess_monsterModelInstance.GetElementById(key);
		}

		public ArtSkin_equipSkinModel GetArtSkin_equipSkinModelInstance()
		{
			return this.ArtSkin_equipSkinModelInstance;
		}

		public IList<ArtSkin_equipSkin> GetArtSkin_equipSkinElements()
		{
			return this.ArtSkin_equipSkinModelInstance.GetAllElements();
		}

		public ArtSkin_equipSkin GetArtSkin_equipSkin(int key)
		{
			return this.ArtSkin_equipSkinModelInstance.GetElementById(key);
		}

		public ArtMember_clothesModel GetArtMember_clothesModelInstance()
		{
			return this.ArtMember_clothesModelInstance;
		}

		public IList<ArtMember_clothes> GetArtMember_clothesElements()
		{
			return this.ArtMember_clothesModelInstance.GetAllElements();
		}

		public ArtMember_clothes GetArtMember_clothes(int key)
		{
			return this.ArtMember_clothesModelInstance.GetElementById(key);
		}

		public Artifact_advanceArtifactModel GetArtifact_advanceArtifactModelInstance()
		{
			return this.Artifact_advanceArtifactModelInstance;
		}

		public IList<Artifact_advanceArtifact> GetArtifact_advanceArtifactElements()
		{
			return this.Artifact_advanceArtifactModelInstance.GetAllElements();
		}

		public Artifact_advanceArtifact GetArtifact_advanceArtifact(int key)
		{
			return this.Artifact_advanceArtifactModelInstance.GetElementById(key);
		}

		public GameMember_gameHoverModel GetGameMember_gameHoverModelInstance()
		{
			return this.GameMember_gameHoverModelInstance;
		}

		public IList<GameMember_gameHover> GetGameMember_gameHoverElements()
		{
			return this.GameMember_gameHoverModelInstance.GetAllElements();
		}

		public GameMember_gameHover GetGameMember_gameHover(int key)
		{
			return this.GameMember_gameHoverModelInstance.GetElementById(key);
		}

		public Vip_vipModel GetVip_vipModelInstance()
		{
			return this.Vip_vipModelInstance;
		}

		public IList<Vip_vip> GetVip_vipElements()
		{
			return this.Vip_vipModelInstance.GetAllElements();
		}

		public Vip_vip GetVip_vip(int key)
		{
			return this.Vip_vipModelInstance.GetElementById(key);
		}

		public Equip_equipModel GetEquip_equipModelInstance()
		{
			return this.Equip_equipModelInstance;
		}

		public IList<Equip_equip> GetEquip_equipElements()
		{
			return this.Equip_equipModelInstance.GetAllElements();
		}

		public Equip_equip GetEquip_equip(int key)
		{
			return this.Equip_equipModelInstance.GetElementById(key);
		}

		public IAP_GiftPacksModel GetIAP_GiftPacksModelInstance()
		{
			return this.IAP_GiftPacksModelInstance;
		}

		public IList<IAP_GiftPacks> GetIAP_GiftPacksElements()
		{
			return this.IAP_GiftPacksModelInstance.GetAllElements();
		}

		public IAP_GiftPacks GetIAP_GiftPacks(int key)
		{
			return this.IAP_GiftPacksModelInstance.GetElementById(key);
		}

		public Shop_ShopSellModel GetShop_ShopSellModelInstance()
		{
			return this.Shop_ShopSellModelInstance;
		}

		public IList<Shop_ShopSell> GetShop_ShopSellElements()
		{
			return this.Shop_ShopSellModelInstance.GetAllElements();
		}

		public Shop_ShopSell GetShop_ShopSell(int key)
		{
			return this.Shop_ShopSellModelInstance.GetElementById(key);
		}

		public Language_languagetableModel GetLanguage_languagetableModelInstance()
		{
			return this.Language_languagetableModelInstance;
		}

		public IList<Language_languagetable> GetLanguage_languagetableElements()
		{
			return this.Language_languagetableModelInstance.GetAllElements();
		}

		public Language_languagetable GetLanguage_languagetable(int key)
		{
			return this.Language_languagetableModelInstance.GetElementById(key);
		}

		public TGASource_GetModel GetTGASource_GetModelInstance()
		{
			return this.TGASource_GetModelInstance;
		}

		public IList<TGASource_Get> GetTGASource_GetElements()
		{
			return this.TGASource_GetModelInstance.GetAllElements();
		}

		public TGASource_Get GetTGASource_Get(int key)
		{
			return this.TGASource_GetModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossMonsterModel GetGuildBOSS_guildBossMonsterModelInstance()
		{
			return this.GuildBOSS_guildBossMonsterModelInstance;
		}

		public IList<GuildBOSS_guildBossMonster> GetGuildBOSS_guildBossMonsterElements()
		{
			return this.GuildBOSS_guildBossMonsterModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossMonster GetGuildBOSS_guildBossMonster(int key)
		{
			return this.GuildBOSS_guildBossMonsterModelInstance.GetElementById(key);
		}

		public Module_moduleInfoModel GetModule_moduleInfoModelInstance()
		{
			return this.Module_moduleInfoModelInstance;
		}

		public IList<Module_moduleInfo> GetModule_moduleInfoElements()
		{
			return this.Module_moduleInfoModelInstance.GetAllElements();
		}

		public Module_moduleInfo GetModule_moduleInfo(int key)
		{
			return this.Module_moduleInfoModelInstance.GetElementById(key);
		}

		public GuildRace_levelModel GetGuildRace_levelModelInstance()
		{
			return this.GuildRace_levelModelInstance;
		}

		public IList<GuildRace_level> GetGuildRace_levelElements()
		{
			return this.GuildRace_levelModelInstance.GetAllElements();
		}

		public GuildRace_level GetGuildRace_level(int key)
		{
			return this.GuildRace_levelModelInstance.GetElementById(key);
		}

		public ChapterActivity_ChapterObjModel GetChapterActivity_ChapterObjModelInstance()
		{
			return this.ChapterActivity_ChapterObjModelInstance;
		}

		public IList<ChapterActivity_ChapterObj> GetChapterActivity_ChapterObjElements()
		{
			return this.ChapterActivity_ChapterObjModelInstance.GetAllElements();
		}

		public ChapterActivity_ChapterObj GetChapterActivity_ChapterObj(int key)
		{
			return this.ChapterActivity_ChapterObjModelInstance.GetElementById(key);
		}

		public ArtBullet_BulletModel GetArtBullet_BulletModelInstance()
		{
			return this.ArtBullet_BulletModelInstance;
		}

		public IList<ArtBullet_Bullet> GetArtBullet_BulletElements()
		{
			return this.ArtBullet_BulletModelInstance.GetAllElements();
		}

		public ArtBullet_Bullet GetArtBullet_Bullet(int key)
		{
			return this.ArtBullet_BulletModelInstance.GetElementById(key);
		}

		public Collection_collectionModel GetCollection_collectionModelInstance()
		{
			return this.Collection_collectionModelInstance;
		}

		public IList<Collection_collection> GetCollection_collectionElements()
		{
			return this.Collection_collectionModelInstance.GetAllElements();
		}

		public Collection_collection GetCollection_collection(int key)
		{
			return this.Collection_collectionModelInstance.GetElementById(key);
		}

		public Task_WeeklyActiveModel GetTask_WeeklyActiveModelInstance()
		{
			return this.Task_WeeklyActiveModelInstance;
		}

		public IList<Task_WeeklyActive> GetTask_WeeklyActiveElements()
		{
			return this.Task_WeeklyActiveModelInstance.GetAllElements();
		}

		public Task_WeeklyActive GetTask_WeeklyActive(int key)
		{
			return this.Task_WeeklyActiveModelInstance.GetElementById(key);
		}

		public GameBullet_bulletTypeModel GetGameBullet_bulletTypeModelInstance()
		{
			return this.GameBullet_bulletTypeModelInstance;
		}

		public IList<GameBullet_bulletType> GetGameBullet_bulletTypeElements()
		{
			return this.GameBullet_bulletTypeModelInstance.GetAllElements();
		}

		public GameBullet_bulletType GetGameBullet_bulletType(int key)
		{
			return this.GameBullet_bulletTypeModelInstance.GetElementById(key);
		}

		public Atlas_atlasModel GetAtlas_atlasModelInstance()
		{
			return this.Atlas_atlasModelInstance;
		}

		public IList<Atlas_atlas> GetAtlas_atlasElements()
		{
			return this.Atlas_atlasModelInstance.GetAllElements();
		}

		public Atlas_atlas GetAtlas_atlas(int key)
		{
			return this.Atlas_atlasModelInstance.GetElementById(key);
		}

		public Dungeon_DungeonBaseModel GetDungeon_DungeonBaseModelInstance()
		{
			return this.Dungeon_DungeonBaseModelInstance;
		}

		public IList<Dungeon_DungeonBase> GetDungeon_DungeonBaseElements()
		{
			return this.Dungeon_DungeonBaseModelInstance.GetAllElements();
		}

		public Dungeon_DungeonBase GetDungeon_DungeonBase(int key)
		{
			return this.Dungeon_DungeonBaseModelInstance.GetElementById(key);
		}

		public Achievements_AchievementsModel GetAchievements_AchievementsModelInstance()
		{
			return this.Achievements_AchievementsModelInstance;
		}

		public IList<Achievements_Achievements> GetAchievements_AchievementsElements()
		{
			return this.Achievements_AchievementsModelInstance.GetAllElements();
		}

		public Achievements_Achievements GetAchievements_Achievements(int key)
		{
			return this.Achievements_AchievementsModelInstance.GetElementById(key);
		}

		public Mining_oreBuildModel GetMining_oreBuildModelInstance()
		{
			return this.Mining_oreBuildModelInstance;
		}

		public IList<Mining_oreBuild> GetMining_oreBuildElements()
		{
			return this.Mining_oreBuildModelInstance.GetAllElements();
		}

		public Mining_oreBuild GetMining_oreBuild(int key)
		{
			return this.Mining_oreBuildModelInstance.GetElementById(key);
		}

		public RogueDungeon_endEventModel GetRogueDungeon_endEventModelInstance()
		{
			return this.RogueDungeon_endEventModelInstance;
		}

		public IList<RogueDungeon_endEvent> GetRogueDungeon_endEventElements()
		{
			return this.RogueDungeon_endEventModelInstance.GetAllElements();
		}

		public RogueDungeon_endEvent GetRogueDungeon_endEvent(int key)
		{
			return this.RogueDungeon_endEventModelInstance.GetElementById(key);
		}

		public GameMember_memberModel GetGameMember_memberModelInstance()
		{
			return this.GameMember_memberModelInstance;
		}

		public IList<GameMember_member> GetGameMember_memberElements()
		{
			return this.GameMember_memberModelInstance.GetAllElements();
		}

		public GameMember_member GetGameMember_member(int key)
		{
			return this.GameMember_memberModelInstance.GetElementById(key);
		}

		public Equip_equipTypeModel GetEquip_equipTypeModelInstance()
		{
			return this.Equip_equipTypeModelInstance;
		}

		public IList<Equip_equipType> GetEquip_equipTypeElements()
		{
			return this.Equip_equipTypeModelInstance.GetAllElements();
		}

		public Equip_equipType GetEquip_equipType(int key)
		{
			return this.Equip_equipTypeModelInstance.GetElementById(key);
		}

		public ArtBuff_BuffModel GetArtBuff_BuffModelInstance()
		{
			return this.ArtBuff_BuffModelInstance;
		}

		public IList<ArtBuff_Buff> GetArtBuff_BuffElements()
		{
			return this.ArtBuff_BuffModelInstance.GetAllElements();
		}

		public ArtBuff_Buff GetArtBuff_Buff(int key)
		{
			return this.ArtBuff_BuffModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossBoxModel GetGuildBOSS_guildBossBoxModelInstance()
		{
			return this.GuildBOSS_guildBossBoxModelInstance;
		}

		public IList<GuildBOSS_guildBossBox> GetGuildBOSS_guildBossBoxElements()
		{
			return this.GuildBOSS_guildBossBoxModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossBox GetGuildBOSS_guildBossBox(int key)
		{
			return this.GuildBOSS_guildBossBoxModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_paySlotRewardModel GetChapterMiniGame_paySlotRewardModelInstance()
		{
			return this.ChapterMiniGame_paySlotRewardModelInstance;
		}

		public IList<ChapterMiniGame_paySlotReward> GetChapterMiniGame_paySlotRewardElements()
		{
			return this.ChapterMiniGame_paySlotRewardModelInstance.GetAllElements();
		}

		public ChapterMiniGame_paySlotReward GetChapterMiniGame_paySlotReward(int key)
		{
			return this.ChapterMiniGame_paySlotRewardModelInstance.GetElementById(key);
		}

		public SevenDay_SevenDayActiveRewardModel GetSevenDay_SevenDayActiveRewardModelInstance()
		{
			return this.SevenDay_SevenDayActiveRewardModelInstance;
		}

		public IList<SevenDay_SevenDayActiveReward> GetSevenDay_SevenDayActiveRewardElements()
		{
			return this.SevenDay_SevenDayActiveRewardModelInstance.GetAllElements();
		}

		public SevenDay_SevenDayActiveReward GetSevenDay_SevenDayActiveReward(int key)
		{
			return this.SevenDay_SevenDayActiveRewardModelInstance.GetElementById(key);
		}

		public Vip_dataModel GetVip_dataModelInstance()
		{
			return this.Vip_dataModelInstance;
		}

		public IList<Vip_data> GetVip_dataElements()
		{
			return this.Vip_dataModelInstance.GetAllElements();
		}

		public Vip_data GetVip_data(int key)
		{
			return this.Vip_dataModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaChallengeListRuleModel GetCrossArena_CrossArenaChallengeListRuleModelInstance()
		{
			return this.CrossArena_CrossArenaChallengeListRuleModelInstance;
		}

		public IList<CrossArena_CrossArenaChallengeListRule> GetCrossArena_CrossArenaChallengeListRuleElements()
		{
			return this.CrossArena_CrossArenaChallengeListRuleModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaChallengeListRule GetCrossArena_CrossArenaChallengeListRule(int key)
		{
			return this.CrossArena_CrossArenaChallengeListRuleModelInstance.GetElementById(key);
		}

		public TowerChallenge_TowerLevelModel GetTowerChallenge_TowerLevelModelInstance()
		{
			return this.TowerChallenge_TowerLevelModelInstance;
		}

		public IList<TowerChallenge_TowerLevel> GetTowerChallenge_TowerLevelElements()
		{
			return this.TowerChallenge_TowerLevelModelInstance.GetAllElements();
		}

		public TowerChallenge_TowerLevel GetTowerChallenge_TowerLevel(int key)
		{
			return this.TowerChallenge_TowerLevelModelInstance.GetElementById(key);
		}

		public WorldBoss_RankRewardModel GetWorldBoss_RankRewardModelInstance()
		{
			return this.WorldBoss_RankRewardModelInstance;
		}

		public IList<WorldBoss_RankReward> GetWorldBoss_RankRewardElements()
		{
			return this.WorldBoss_RankRewardModelInstance.GetAllElements();
		}

		public WorldBoss_RankReward GetWorldBoss_RankReward(int key)
		{
			return this.WorldBoss_RankRewardModelInstance.GetElementById(key);
		}

		public ChainPacks_PushChainActvModel GetChainPacks_PushChainActvModelInstance()
		{
			return this.ChainPacks_PushChainActvModelInstance;
		}

		public IList<ChainPacks_PushChainActv> GetChainPacks_PushChainActvElements()
		{
			return this.ChainPacks_PushChainActvModelInstance.GetAllElements();
		}

		public ChainPacks_PushChainActv GetChainPacks_PushChainActv(int key)
		{
			return this.ChainPacks_PushChainActvModelInstance.GetElementById(key);
		}

		public SevenDay_SevenDayPayModel GetSevenDay_SevenDayPayModelInstance()
		{
			return this.SevenDay_SevenDayPayModelInstance;
		}

		public IList<SevenDay_SevenDayPay> GetSevenDay_SevenDayPayElements()
		{
			return this.SevenDay_SevenDayPayModelInstance.GetAllElements();
		}

		public SevenDay_SevenDayPay GetSevenDay_SevenDayPay(int key)
		{
			return this.SevenDay_SevenDayPayModelInstance.GetElementById(key);
		}

		public Sociality_ReportModel GetSociality_ReportModelInstance()
		{
			return this.Sociality_ReportModelInstance;
		}

		public IList<Sociality_Report> GetSociality_ReportElements()
		{
			return this.Sociality_ReportModelInstance.GetAllElements();
		}

		public Sociality_Report GetSociality_Report(int key)
		{
			return this.Sociality_ReportModelInstance.GetElementById(key);
		}

		public AutoProcess_chapterModel GetAutoProcess_chapterModelInstance()
		{
			return this.AutoProcess_chapterModelInstance;
		}

		public IList<AutoProcess_chapter> GetAutoProcess_chapterElements()
		{
			return this.AutoProcess_chapterModelInstance.GetAllElements();
		}

		public AutoProcess_chapter GetAutoProcess_chapter(int key)
		{
			return this.AutoProcess_chapterModelInstance.GetElementById(key);
		}

		public Attribute_AttrTextModel GetAttribute_AttrTextModelInstance()
		{
			return this.Attribute_AttrTextModelInstance;
		}

		public IList<Attribute_AttrText> GetAttribute_AttrTextElements()
		{
			return this.Attribute_AttrTextModelInstance.GetAllElements();
		}

		public Attribute_AttrText GetAttribute_AttrText(string key)
		{
			return this.Attribute_AttrTextModelInstance.GetElementById(key);
		}

		public GameSkillBuild_skillBuildModel GetGameSkillBuild_skillBuildModelInstance()
		{
			return this.GameSkillBuild_skillBuildModelInstance;
		}

		public IList<GameSkillBuild_skillBuild> GetGameSkillBuild_skillBuildElements()
		{
			return this.GameSkillBuild_skillBuildModelInstance.GetAllElements();
		}

		public GameSkillBuild_skillBuild GetGameSkillBuild_skillBuild(int key)
		{
			return this.GameSkillBuild_skillBuildModelInstance.GetElementById(key);
		}

		public Shop_ShopActivityModel GetShop_ShopActivityModelInstance()
		{
			return this.Shop_ShopActivityModelInstance;
		}

		public IList<Shop_ShopActivity> GetShop_ShopActivityElements()
		{
			return this.Shop_ShopActivityModelInstance.GetAllElements();
		}

		public Shop_ShopActivity GetShop_ShopActivity(int key)
		{
			return this.Shop_ShopActivityModelInstance.GetElementById(key);
		}

		public IAP_BattlePassRewardModel GetIAP_BattlePassRewardModelInstance()
		{
			return this.IAP_BattlePassRewardModelInstance;
		}

		public IList<IAP_BattlePassReward> GetIAP_BattlePassRewardElements()
		{
			return this.IAP_BattlePassRewardModelInstance.GetAllElements();
		}

		public IAP_BattlePassReward GetIAP_BattlePassReward(int key)
		{
			return this.IAP_BattlePassRewardModelInstance.GetElementById(key);
		}

		public ServerList_serverGropModel GetServerList_serverGropModelInstance()
		{
			return this.ServerList_serverGropModelInstance;
		}

		public IList<ServerList_serverGrop> GetServerList_serverGropElements()
		{
			return this.ServerList_serverGropModelInstance.GetAllElements();
		}

		public ServerList_serverGrop GetServerList_serverGrop(int key)
		{
			return this.ServerList_serverGropModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_singleSlotRewardModel GetChapterMiniGame_singleSlotRewardModelInstance()
		{
			return this.ChapterMiniGame_singleSlotRewardModelInstance;
		}

		public IList<ChapterMiniGame_singleSlotReward> GetChapterMiniGame_singleSlotRewardElements()
		{
			return this.ChapterMiniGame_singleSlotRewardModelInstance.GetAllElements();
		}

		public ChapterMiniGame_singleSlotReward GetChapterMiniGame_singleSlotReward(int key)
		{
			return this.ChapterMiniGame_singleSlotRewardModelInstance.GetElementById(key);
		}

		public CommonActivity_ConsumeObjModel GetCommonActivity_ConsumeObjModelInstance()
		{
			return this.CommonActivity_ConsumeObjModelInstance;
		}

		public IList<CommonActivity_ConsumeObj> GetCommonActivity_ConsumeObjElements()
		{
			return this.CommonActivity_ConsumeObjModelInstance.GetAllElements();
		}

		public CommonActivity_ConsumeObj GetCommonActivity_ConsumeObj(int key)
		{
			return this.CommonActivity_ConsumeObjModelInstance.GetElementById(key);
		}

		public Mining_showRateModel GetMining_showRateModelInstance()
		{
			return this.Mining_showRateModelInstance;
		}

		public IList<Mining_showRate> GetMining_showRateElements()
		{
			return this.Mining_showRateModelInstance.GetAllElements();
		}

		public Mining_showRate GetMining_showRate(int key)
		{
			return this.Mining_showRateModelInstance.GetElementById(key);
		}

		public ArtMap_MapModel GetArtMap_MapModelInstance()
		{
			return this.ArtMap_MapModelInstance;
		}

		public IList<ArtMap_Map> GetArtMap_MapElements()
		{
			return this.ArtMap_MapModelInstance.GetAllElements();
		}

		public ArtMap_Map GetArtMap_Map(int key)
		{
			return this.ArtMap_MapModelInstance.GetElementById(key);
		}

		public Chapter_stageUpgradeModel GetChapter_stageUpgradeModelInstance()
		{
			return this.Chapter_stageUpgradeModelInstance;
		}

		public IList<Chapter_stageUpgrade> GetChapter_stageUpgradeElements()
		{
			return this.Chapter_stageUpgradeModelInstance.GetAllElements();
		}

		public Chapter_stageUpgrade GetChapter_stageUpgrade(int key)
		{
			return this.Chapter_stageUpgradeModelInstance.GetElementById(key);
		}

		public Chapter_surpriseBuildModel GetChapter_surpriseBuildModelInstance()
		{
			return this.Chapter_surpriseBuildModelInstance;
		}

		public IList<Chapter_surpriseBuild> GetChapter_surpriseBuildElements()
		{
			return this.Chapter_surpriseBuildModelInstance.GetAllElements();
		}

		public Chapter_surpriseBuild GetChapter_surpriseBuild(int key)
		{
			return this.Chapter_surpriseBuildModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossTaskModel GetGuildBOSS_guildBossTaskModelInstance()
		{
			return this.GuildBOSS_guildBossTaskModelInstance;
		}

		public IList<GuildBOSS_guildBossTask> GetGuildBOSS_guildBossTaskElements()
		{
			return this.GuildBOSS_guildBossTaskModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossTask GetGuildBOSS_guildBossTask(int key)
		{
			return this.GuildBOSS_guildBossTaskModelInstance.GetElementById(key);
		}

		public Guild_guildGiftLevelModel GetGuild_guildGiftLevelModelInstance()
		{
			return this.Guild_guildGiftLevelModelInstance;
		}

		public IList<Guild_guildGiftLevel> GetGuild_guildGiftLevelElements()
		{
			return this.Guild_guildGiftLevelModelInstance.GetAllElements();
		}

		public Guild_guildGiftLevel GetGuild_guildGiftLevel(int key)
		{
			return this.Guild_guildGiftLevelModelInstance.GetElementById(key);
		}

		public IntegralShop_dataModel GetIntegralShop_dataModelInstance()
		{
			return this.IntegralShop_dataModelInstance;
		}

		public IList<IntegralShop_data> GetIntegralShop_dataElements()
		{
			return this.IntegralShop_dataModelInstance.GetAllElements();
		}

		public IntegralShop_data GetIntegralShop_data(int key)
		{
			return this.IntegralShop_dataModelInstance.GetElementById(key);
		}

		public TalentLegacy_talentLegacyNodeModel GetTalentLegacy_talentLegacyNodeModelInstance()
		{
			return this.TalentLegacy_talentLegacyNodeModelInstance;
		}

		public IList<TalentLegacy_talentLegacyNode> GetTalentLegacy_talentLegacyNodeElements()
		{
			return this.TalentLegacy_talentLegacyNodeModelInstance.GetAllElements();
		}

		public TalentLegacy_talentLegacyNode GetTalentLegacy_talentLegacyNode(int key)
		{
			return this.TalentLegacy_talentLegacyNodeModelInstance.GetElementById(key);
		}

		public Shop_AdModel GetShop_AdModelInstance()
		{
			return this.Shop_AdModelInstance;
		}

		public IList<Shop_Ad> GetShop_AdElements()
		{
			return this.Shop_AdModelInstance.GetAllElements();
		}

		public Shop_Ad GetShop_Ad(int key)
		{
			return this.Shop_AdModelInstance.GetElementById(key);
		}

		public Pet_PetEntryModel GetPet_PetEntryModelInstance()
		{
			return this.Pet_PetEntryModelInstance;
		}

		public IList<Pet_PetEntry> GetPet_PetEntryElements()
		{
			return this.Pet_PetEntryModelInstance.GetAllElements();
		}

		public Pet_PetEntry GetPet_PetEntry(int key)
		{
			return this.Pet_PetEntryModelInstance.GetElementById(key);
		}

		public Guild_guildEventModel GetGuild_guildEventModelInstance()
		{
			return this.Guild_guildEventModelInstance;
		}

		public IList<Guild_guildEvent> GetGuild_guildEventElements()
		{
			return this.Guild_guildEventModelInstance.GetAllElements();
		}

		public Guild_guildEvent GetGuild_guildEvent(int key)
		{
			return this.Guild_guildEventModelInstance.GetElementById(key);
		}

		public IAP_MonthCardModel GetIAP_MonthCardModelInstance()
		{
			return this.IAP_MonthCardModelInstance;
		}

		public IList<IAP_MonthCard> GetIAP_MonthCardElements()
		{
			return this.IAP_MonthCardModelInstance.GetAllElements();
		}

		public IAP_MonthCard GetIAP_MonthCard(int key)
		{
			return this.IAP_MonthCardModelInstance.GetElementById(key);
		}

		public Fishing_fishingModel GetFishing_fishingModelInstance()
		{
			return this.Fishing_fishingModelInstance;
		}

		public IList<Fishing_fishing> GetFishing_fishingElements()
		{
			return this.Fishing_fishingModelInstance.GetAllElements();
		}

		public Fishing_fishing GetFishing_fishing(int key)
		{
			return this.Fishing_fishingModelInstance.GetElementById(key);
		}

		public Fishing_fishMoveModel GetFishing_fishMoveModelInstance()
		{
			return this.Fishing_fishMoveModelInstance;
		}

		public IList<Fishing_fishMove> GetFishing_fishMoveElements()
		{
			return this.Fishing_fishMoveModelInstance.GetAllElements();
		}

		public Fishing_fishMove GetFishing_fishMove(int key)
		{
			return this.Fishing_fishMoveModelInstance.GetElementById(key);
		}

		public IAP_BattlePassModel GetIAP_BattlePassModelInstance()
		{
			return this.IAP_BattlePassModelInstance;
		}

		public IList<IAP_BattlePass> GetIAP_BattlePassElements()
		{
			return this.IAP_BattlePassModelInstance.GetAllElements();
		}

		public IAP_BattlePass GetIAP_BattlePass(int key)
		{
			return this.IAP_BattlePassModelInstance.GetElementById(key);
		}

		public ChapterActivity_BattlepassModel GetChapterActivity_BattlepassModelInstance()
		{
			return this.ChapterActivity_BattlepassModelInstance;
		}

		public IList<ChapterActivity_Battlepass> GetChapterActivity_BattlepassElements()
		{
			return this.ChapterActivity_BattlepassModelInstance.GetAllElements();
		}

		public ChapterActivity_Battlepass GetChapterActivity_Battlepass(int key)
		{
			return this.ChapterActivity_BattlepassModelInstance.GetElementById(key);
		}

		public ArtHover_hoverModel GetArtHover_hoverModelInstance()
		{
			return this.ArtHover_hoverModelInstance;
		}

		public IList<ArtHover_hover> GetArtHover_hoverElements()
		{
			return this.ArtHover_hoverModelInstance.GetAllElements();
		}

		public ArtHover_hover GetArtHover_hover(int key)
		{
			return this.ArtHover_hoverModelInstance.GetElementById(key);
		}

		public WorldBoss_SubsectionModel GetWorldBoss_SubsectionModelInstance()
		{
			return this.WorldBoss_SubsectionModelInstance;
		}

		public IList<WorldBoss_Subsection> GetWorldBoss_SubsectionElements()
		{
			return this.WorldBoss_SubsectionModelInstance.GetAllElements();
		}

		public WorldBoss_Subsection GetWorldBoss_Subsection(int key)
		{
			return this.WorldBoss_SubsectionModelInstance.GetElementById(key);
		}

		public Guild_guildPowerModel GetGuild_guildPowerModelInstance()
		{
			return this.Guild_guildPowerModelInstance;
		}

		public IList<Guild_guildPower> GetGuild_guildPowerElements()
		{
			return this.Guild_guildPowerModelInstance.GetAllElements();
		}

		public Guild_guildPower GetGuild_guildPower(int key)
		{
			return this.Guild_guildPowerModelInstance.GetElementById(key);
		}

		public RogueDungeon_monsterEntryModel GetRogueDungeon_monsterEntryModelInstance()
		{
			return this.RogueDungeon_monsterEntryModelInstance;
		}

		public IList<RogueDungeon_monsterEntry> GetRogueDungeon_monsterEntryElements()
		{
			return this.RogueDungeon_monsterEntryModelInstance.GetAllElements();
		}

		public RogueDungeon_monsterEntry GetRogueDungeon_monsterEntry(int key)
		{
			return this.RogueDungeon_monsterEntryModelInstance.GetElementById(key);
		}

		public Quality_guildBossQualityModel GetQuality_guildBossQualityModelInstance()
		{
			return this.Quality_guildBossQualityModelInstance;
		}

		public IList<Quality_guildBossQuality> GetQuality_guildBossQualityElements()
		{
			return this.Quality_guildBossQualityModelInstance.GetAllElements();
		}

		public Quality_guildBossQuality GetQuality_guildBossQuality(int key)
		{
			return this.Quality_guildBossQualityModelInstance.GetElementById(key);
		}

		public Relic_starUpModel GetRelic_starUpModelInstance()
		{
			return this.Relic_starUpModelInstance;
		}

		public IList<Relic_starUp> GetRelic_starUpElements()
		{
			return this.Relic_starUpModelInstance.GetAllElements();
		}

		public Relic_starUp GetRelic_starUp(int key)
		{
			return this.Relic_starUpModelInstance.GetElementById(key);
		}

		public LanguageCN_languagetableModel GetLanguageCN_languagetableModelInstance()
		{
			return this.LanguageCN_languagetableModelInstance;
		}

		public IList<LanguageCN_languagetable> GetLanguageCN_languagetableElements()
		{
			return this.LanguageCN_languagetableModelInstance.GetAllElements();
		}

		public LanguageCN_languagetable GetLanguageCN_languagetable(string key)
		{
			return this.LanguageCN_languagetableModelInstance.GetElementById(key);
		}

		public Artifact_artifactStageModel GetArtifact_artifactStageModelInstance()
		{
			return this.Artifact_artifactStageModelInstance;
		}

		public IList<Artifact_artifactStage> GetArtifact_artifactStageElements()
		{
			return this.Artifact_artifactStageModelInstance.GetAllElements();
		}

		public Artifact_artifactStage GetArtifact_artifactStage(int key)
		{
			return this.Artifact_artifactStageModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_paySlotBaseModel GetChapterMiniGame_paySlotBaseModelInstance()
		{
			return this.ChapterMiniGame_paySlotBaseModelInstance;
		}

		public IList<ChapterMiniGame_paySlotBase> GetChapterMiniGame_paySlotBaseElements()
		{
			return this.ChapterMiniGame_paySlotBaseModelInstance.GetAllElements();
		}

		public ChapterMiniGame_paySlotBase GetChapterMiniGame_paySlotBase(int key)
		{
			return this.ChapterMiniGame_paySlotBaseModelInstance.GetElementById(key);
		}

		public ItemResources_itemgetModel GetItemResources_itemgetModelInstance()
		{
			return this.ItemResources_itemgetModelInstance;
		}

		public IList<ItemResources_itemget> GetItemResources_itemgetElements()
		{
			return this.ItemResources_itemgetModelInstance.GetAllElements();
		}

		public ItemResources_itemget GetItemResources_itemget(int key)
		{
			return this.ItemResources_itemgetModelInstance.GetElementById(key);
		}

		public Collection_collectionSuitModel GetCollection_collectionSuitModelInstance()
		{
			return this.Collection_collectionSuitModelInstance;
		}

		public IList<Collection_collectionSuit> GetCollection_collectionSuitElements()
		{
			return this.Collection_collectionSuitModelInstance.GetAllElements();
		}

		public Collection_collectionSuit GetCollection_collectionSuit(int key)
		{
			return this.Collection_collectionSuitModelInstance.GetElementById(key);
		}

		public Dungeon_DungeonLevelModel GetDungeon_DungeonLevelModelInstance()
		{
			return this.Dungeon_DungeonLevelModelInstance;
		}

		public IList<Dungeon_DungeonLevel> GetDungeon_DungeonLevelElements()
		{
			return this.Dungeon_DungeonLevelModelInstance.GetAllElements();
		}

		public Dungeon_DungeonLevel GetDungeon_DungeonLevel(int key)
		{
			return this.Dungeon_DungeonLevelModelInstance.GetElementById(key);
		}

		public Guild_guildSignInModel GetGuild_guildSignInModelInstance()
		{
			return this.Guild_guildSignInModelInstance;
		}

		public IList<Guild_guildSignIn> GetGuild_guildSignInElements()
		{
			return this.Guild_guildSignInModelInstance.GetAllElements();
		}

		public Guild_guildSignIn GetGuild_guildSignIn(int key)
		{
			return this.Guild_guildSignInModelInstance.GetElementById(key);
		}

		public GuildBOSS_rankRewardsModel GetGuildBOSS_rankRewardsModelInstance()
		{
			return this.GuildBOSS_rankRewardsModelInstance;
		}

		public IList<GuildBOSS_rankRewards> GetGuildBOSS_rankRewardsElements()
		{
			return this.GuildBOSS_rankRewardsModelInstance.GetAllElements();
		}

		public GuildBOSS_rankRewards GetGuildBOSS_rankRewards(int key)
		{
			return this.GuildBOSS_rankRewardsModelInstance.GetElementById(key);
		}

		public CommonActivity_RankObjModel GetCommonActivity_RankObjModelInstance()
		{
			return this.CommonActivity_RankObjModelInstance;
		}

		public IList<CommonActivity_RankObj> GetCommonActivity_RankObjElements()
		{
			return this.CommonActivity_RankObjModelInstance.GetAllElements();
		}

		public CommonActivity_RankObj GetCommonActivity_RankObj(int key)
		{
			return this.CommonActivity_RankObjModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaLevelModel GetCrossArena_CrossArenaLevelModelInstance()
		{
			return this.CrossArena_CrossArenaLevelModelInstance;
		}

		public IList<CrossArena_CrossArenaLevel> GetCrossArena_CrossArenaLevelElements()
		{
			return this.CrossArena_CrossArenaLevelModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaLevel GetCrossArena_CrossArenaLevel(int key)
		{
			return this.CrossArena_CrossArenaLevelModelInstance.GetElementById(key);
		}

		public TalentLegacy_talentLegacyEffectModel GetTalentLegacy_talentLegacyEffectModelInstance()
		{
			return this.TalentLegacy_talentLegacyEffectModelInstance;
		}

		public IList<TalentLegacy_talentLegacyEffect> GetTalentLegacy_talentLegacyEffectElements()
		{
			return this.TalentLegacy_talentLegacyEffectModelInstance.GetAllElements();
		}

		public TalentLegacy_talentLegacyEffect GetTalentLegacy_talentLegacyEffect(int key)
		{
			return this.TalentLegacy_talentLegacyEffectModelInstance.GetElementById(key);
		}

		public ArtSkill_SkillModel GetArtSkill_SkillModelInstance()
		{
			return this.ArtSkill_SkillModelInstance;
		}

		public IList<ArtSkill_Skill> GetArtSkill_SkillElements()
		{
			return this.ArtSkill_SkillModelInstance.GetAllElements();
		}

		public ArtSkill_Skill GetArtSkill_Skill(int key)
		{
			return this.ArtSkill_SkillModelInstance.GetElementById(key);
		}

		public Emoji_EmojiModel GetEmoji_EmojiModelInstance()
		{
			return this.Emoji_EmojiModelInstance;
		}

		public IList<Emoji_Emoji> GetEmoji_EmojiElements()
		{
			return this.Emoji_EmojiModelInstance.GetAllElements();
		}

		public Emoji_Emoji GetEmoji_Emoji(int key)
		{
			return this.Emoji_EmojiModelInstance.GetElementById(key);
		}

		public IAP_ChapterPacksModel GetIAP_ChapterPacksModelInstance()
		{
			return this.IAP_ChapterPacksModelInstance;
		}

		public IList<IAP_ChapterPacks> GetIAP_ChapterPacksElements()
		{
			return this.IAP_ChapterPacksModelInstance.GetAllElements();
		}

		public IAP_ChapterPacks GetIAP_ChapterPacks(int key)
		{
			return this.IAP_ChapterPacksModelInstance.GetElementById(key);
		}

		public ChapterActivity_ModelModel GetChapterActivity_ModelModelInstance()
		{
			return this.ChapterActivity_ModelModelInstance;
		}

		public IList<ChapterActivity_Model> GetChapterActivity_ModelElements()
		{
			return this.ChapterActivity_ModelModelInstance.GetAllElements();
		}

		public ChapterActivity_Model GetChapterActivity_Model(int key)
		{
			return this.ChapterActivity_ModelModelInstance.GetElementById(key);
		}

		public Relic_updateLevelModel GetRelic_updateLevelModelInstance()
		{
			return this.Relic_updateLevelModelInstance;
		}

		public IList<Relic_updateLevel> GetRelic_updateLevelElements()
		{
			return this.Relic_updateLevelModelInstance.GetAllElements();
		}

		public Relic_updateLevel GetRelic_updateLevel(int key)
		{
			return this.Relic_updateLevelModelInstance.GetElementById(key);
		}

		public Mining_qualityModelModel GetMining_qualityModelModelInstance()
		{
			return this.Mining_qualityModelModelInstance;
		}

		public IList<Mining_qualityModel> GetMining_qualityModelElements()
		{
			return this.Mining_qualityModelModelInstance.GetAllElements();
		}

		public Mining_qualityModel GetMining_qualityModel(int key)
		{
			return this.Mining_qualityModelModelInstance.GetElementById(key);
		}

		public IAP_PurchaseModel GetIAP_PurchaseModelInstance()
		{
			return this.IAP_PurchaseModelInstance;
		}

		public IList<IAP_Purchase> GetIAP_PurchaseElements()
		{
			return this.IAP_PurchaseModelInstance.GetAllElements();
		}

		public IAP_Purchase GetIAP_Purchase(int key)
		{
			return this.IAP_PurchaseModelInstance.GetElementById(key);
		}

		public GameMember_npcFunctionModel GetGameMember_npcFunctionModelInstance()
		{
			return this.GameMember_npcFunctionModelInstance;
		}

		public IList<GameMember_npcFunction> GetGameMember_npcFunctionElements()
		{
			return this.GameMember_npcFunctionModelInstance.GetAllElements();
		}

		public GameMember_npcFunction GetGameMember_npcFunction(int key)
		{
			return this.GameMember_npcFunctionModelInstance.GetElementById(key);
		}

		public ArtMember_morphModel GetArtMember_morphModelInstance()
		{
			return this.ArtMember_morphModelInstance;
		}

		public IList<ArtMember_morph> GetArtMember_morphElements()
		{
			return this.ArtMember_morphModelInstance.GetAllElements();
		}

		public ArtMember_morph GetArtMember_morph(int key)
		{
			return this.ArtMember_morphModelInstance.GetElementById(key);
		}

		public Fishing_fishRodModel GetFishing_fishRodModelInstance()
		{
			return this.Fishing_fishRodModelInstance;
		}

		public IList<Fishing_fishRod> GetFishing_fishRodElements()
		{
			return this.Fishing_fishRodModelInstance.GetAllElements();
		}

		public Fishing_fishRod GetFishing_fishRod(int key)
		{
			return this.Fishing_fishRodModelInstance.GetElementById(key);
		}

		public ArtHit_HitModel GetArtHit_HitModelInstance()
		{
			return this.ArtHit_HitModelInstance;
		}

		public IList<ArtHit_Hit> GetArtHit_HitElements()
		{
			return this.ArtHit_HitModelInstance.GetAllElements();
		}

		public ArtHit_Hit GetArtHit_Hit(int key)
		{
			return this.ArtHit_HitModelInstance.GetElementById(key);
		}

		public Guild_guildcontributeModel GetGuild_guildcontributeModelInstance()
		{
			return this.Guild_guildcontributeModelInstance;
		}

		public IList<Guild_guildcontribute> GetGuild_guildcontributeElements()
		{
			return this.Guild_guildcontributeModelInstance.GetAllElements();
		}

		public Guild_guildcontribute GetGuild_guildcontribute(int key)
		{
			return this.Guild_guildcontributeModelInstance.GetElementById(key);
		}

		public TGASource_ADModel GetTGASource_ADModelInstance()
		{
			return this.TGASource_ADModelInstance;
		}

		public IList<TGASource_AD> GetTGASource_ADElements()
		{
			return this.TGASource_ADModelInstance.GetAllElements();
		}

		public TGASource_AD GetTGASource_AD(int key)
		{
			return this.TGASource_ADModelInstance.GetElementById(key);
		}

		public SevenDay_SevenDayTaskModel GetSevenDay_SevenDayTaskModelInstance()
		{
			return this.SevenDay_SevenDayTaskModelInstance;
		}

		public IList<SevenDay_SevenDayTask> GetSevenDay_SevenDayTaskElements()
		{
			return this.SevenDay_SevenDayTaskModelInstance.GetAllElements();
		}

		public SevenDay_SevenDayTask GetSevenDay_SevenDayTask(int key)
		{
			return this.SevenDay_SevenDayTaskModelInstance.GetElementById(key);
		}

		public BattleMain_waveModel GetBattleMain_waveModelInstance()
		{
			return this.BattleMain_waveModelInstance;
		}

		public IList<BattleMain_wave> GetBattleMain_waveElements()
		{
			return this.BattleMain_waveModelInstance.GetAllElements();
		}

		public BattleMain_wave GetBattleMain_wave(int key)
		{
			return this.BattleMain_waveModelInstance.GetElementById(key);
		}

		public TalentNew_talentMegaStageModel GetTalentNew_talentMegaStageModelInstance()
		{
			return this.TalentNew_talentMegaStageModelInstance;
		}

		public IList<TalentNew_talentMegaStage> GetTalentNew_talentMegaStageElements()
		{
			return this.TalentNew_talentMegaStageModelInstance.GetAllElements();
		}

		public TalentNew_talentMegaStage GetTalentNew_talentMegaStage(int key)
		{
			return this.TalentNew_talentMegaStageModelInstance.GetElementById(key);
		}

		public Avatar_AvatarModel GetAvatar_AvatarModelInstance()
		{
			return this.Avatar_AvatarModelInstance;
		}

		public IList<Avatar_Avatar> GetAvatar_AvatarElements()
		{
			return this.Avatar_AvatarModelInstance.GetAllElements();
		}

		public Avatar_Avatar GetAvatar_Avatar(int key)
		{
			return this.Avatar_AvatarModelInstance.GetElementById(key);
		}

		public Pet_petCollectionModel GetPet_petCollectionModelInstance()
		{
			return this.Pet_petCollectionModelInstance;
		}

		public IList<Pet_petCollection> GetPet_petCollectionElements()
		{
			return this.Pet_petCollectionModelInstance.GetAllElements();
		}

		public Pet_petCollection GetPet_petCollection(int key)
		{
			return this.Pet_petCollectionModelInstance.GetElementById(key);
		}

		public RogueDungeon_rogueDungeonModel GetRogueDungeon_rogueDungeonModelInstance()
		{
			return this.RogueDungeon_rogueDungeonModelInstance;
		}

		public IList<RogueDungeon_rogueDungeon> GetRogueDungeon_rogueDungeonElements()
		{
			return this.RogueDungeon_rogueDungeonModelInstance.GetAllElements();
		}

		public RogueDungeon_rogueDungeon GetRogueDungeon_rogueDungeon(int key)
		{
			return this.RogueDungeon_rogueDungeonModelInstance.GetElementById(key);
		}

		public Chapter_slotBuildModel GetChapter_slotBuildModelInstance()
		{
			return this.Chapter_slotBuildModelInstance;
		}

		public IList<Chapter_slotBuild> GetChapter_slotBuildElements()
		{
			return this.Chapter_slotBuildModelInstance.GetAllElements();
		}

		public Chapter_slotBuild GetChapter_slotBuild(int key)
		{
			return this.Chapter_slotBuildModelInstance.GetElementById(key);
		}

		public ChapterActivity_BattlepassQualityModel GetChapterActivity_BattlepassQualityModelInstance()
		{
			return this.ChapterActivity_BattlepassQualityModelInstance;
		}

		public IList<ChapterActivity_BattlepassQuality> GetChapterActivity_BattlepassQualityElements()
		{
			return this.ChapterActivity_BattlepassQualityModelInstance.GetAllElements();
		}

		public ChapterActivity_BattlepassQuality GetChapterActivity_BattlepassQuality(int key)
		{
			return this.ChapterActivity_BattlepassQualityModelInstance.GetElementById(key);
		}

		public ChapterActivity_ChapterActivityModel GetChapterActivity_ChapterActivityModelInstance()
		{
			return this.ChapterActivity_ChapterActivityModelInstance;
		}

		public IList<ChapterActivity_ChapterActivity> GetChapterActivity_ChapterActivityElements()
		{
			return this.ChapterActivity_ChapterActivityModelInstance.GetAllElements();
		}

		public ChapterActivity_ChapterActivity GetChapterActivity_ChapterActivity(int key)
		{
			return this.ChapterActivity_ChapterActivityModelInstance.GetElementById(key);
		}

		public IAP_DiamondPacksModel GetIAP_DiamondPacksModelInstance()
		{
			return this.IAP_DiamondPacksModelInstance;
		}

		public IList<IAP_DiamondPacks> GetIAP_DiamondPacksElements()
		{
			return this.IAP_DiamondPacksModelInstance.GetAllElements();
		}

		public IAP_DiamondPacks GetIAP_DiamondPacks(int key)
		{
			return this.IAP_DiamondPacksModelInstance.GetElementById(key);
		}

		public MainLevelReward_MainLevelChestModel GetMainLevelReward_MainLevelChestModelInstance()
		{
			return this.MainLevelReward_MainLevelChestModelInstance;
		}

		public IList<MainLevelReward_MainLevelChest> GetMainLevelReward_MainLevelChestElements()
		{
			return this.MainLevelReward_MainLevelChestModelInstance.GetAllElements();
		}

		public MainLevelReward_MainLevelChest GetMainLevelReward_MainLevelChest(int key)
		{
			return this.MainLevelReward_MainLevelChestModelInstance.GetElementById(key);
		}

		public Shop_SummonPoolModel GetShop_SummonPoolModelInstance()
		{
			return this.Shop_SummonPoolModelInstance;
		}

		public IList<Shop_SummonPool> GetShop_SummonPoolElements()
		{
			return this.Shop_SummonPoolModelInstance.GetAllElements();
		}

		public Shop_SummonPool GetShop_SummonPool(int key)
		{
			return this.Shop_SummonPoolModelInstance.GetElementById(key);
		}

		public Pet_PetTrainingProbModel GetPet_PetTrainingProbModelInstance()
		{
			return this.Pet_PetTrainingProbModelInstance;
		}

		public IList<Pet_PetTrainingProb> GetPet_PetTrainingProbElements()
		{
			return this.Pet_PetTrainingProbModelInstance.GetAllElements();
		}

		public Pet_PetTrainingProb GetPet_PetTrainingProb(int key)
		{
			return this.Pet_PetTrainingProbModelInstance.GetElementById(key);
		}

		public TalentNew_talentEvolutionModel GetTalentNew_talentEvolutionModelInstance()
		{
			return this.TalentNew_talentEvolutionModelInstance;
		}

		public IList<TalentNew_talentEvolution> GetTalentNew_talentEvolutionElements()
		{
			return this.TalentNew_talentEvolutionModelInstance.GetAllElements();
		}

		public TalentNew_talentEvolution GetTalentNew_talentEvolution(int key)
		{
			return this.TalentNew_talentEvolutionModelInstance.GetElementById(key);
		}

		public MonsterCfgOld_monsterCfgOldModel GetMonsterCfgOld_monsterCfgOldModelInstance()
		{
			return this.MonsterCfgOld_monsterCfgOldModelInstance;
		}

		public IList<MonsterCfgOld_monsterCfgOld> GetMonsterCfgOld_monsterCfgOldElements()
		{
			return this.MonsterCfgOld_monsterCfgOldModelInstance.GetAllElements();
		}

		public MonsterCfgOld_monsterCfgOld GetMonsterCfgOld_monsterCfgOld(int key)
		{
			return this.MonsterCfgOld_monsterCfgOldModelInstance.GetElementById(key);
		}

		public TGASource_PageModel GetTGASource_PageModelInstance()
		{
			return this.TGASource_PageModelInstance;
		}

		public IList<TGASource_Page> GetTGASource_PageElements()
		{
			return this.TGASource_PageModelInstance.GetAllElements();
		}

		public TGASource_Page GetTGASource_Page(string key)
		{
			return this.TGASource_PageModelInstance.GetElementById(key);
		}

		public Map_mapModel GetMap_mapModelInstance()
		{
			return this.Map_mapModelInstance;
		}

		public IList<Map_map> GetMap_mapElements()
		{
			return this.Map_mapModelInstance.GetAllElements();
		}

		public Map_map GetMap_map(int key)
		{
			return this.Map_mapModelInstance.GetElementById(key);
		}

		public AutoProcess_skillModel GetAutoProcess_skillModelInstance()
		{
			return this.AutoProcess_skillModelInstance;
		}

		public IList<AutoProcess_skill> GetAutoProcess_skillElements()
		{
			return this.AutoProcess_skillModelInstance.GetAllElements();
		}

		public AutoProcess_skill GetAutoProcess_skill(int key)
		{
			return this.AutoProcess_skillModelInstance.GetElementById(key);
		}

		public ActivityTurntable_TurntablePayModel GetActivityTurntable_TurntablePayModelInstance()
		{
			return this.ActivityTurntable_TurntablePayModelInstance;
		}

		public IList<ActivityTurntable_TurntablePay> GetActivityTurntable_TurntablePayElements()
		{
			return this.ActivityTurntable_TurntablePayModelInstance.GetAllElements();
		}

		public ActivityTurntable_TurntablePay GetActivityTurntable_TurntablePay(int key)
		{
			return this.ActivityTurntable_TurntablePayModelInstance.GetElementById(key);
		}

		public ChapterActivity_ActvTurntableRewardModel GetChapterActivity_ActvTurntableRewardModelInstance()
		{
			return this.ChapterActivity_ActvTurntableRewardModelInstance;
		}

		public IList<ChapterActivity_ActvTurntableReward> GetChapterActivity_ActvTurntableRewardElements()
		{
			return this.ChapterActivity_ActvTurntableRewardModelInstance.GetAllElements();
		}

		public ChapterActivity_ActvTurntableReward GetChapterActivity_ActvTurntableReward(int key)
		{
			return this.ChapterActivity_ActvTurntableRewardModelInstance.GetElementById(key);
		}

		public ChestList_ChestRewardModel GetChestList_ChestRewardModelInstance()
		{
			return this.ChestList_ChestRewardModelInstance;
		}

		public IList<ChestList_ChestReward> GetChestList_ChestRewardElements()
		{
			return this.ChestList_ChestRewardModelInstance.GetAllElements();
		}

		public ChestList_ChestReward GetChestList_ChestReward(int key)
		{
			return this.ChestList_ChestRewardModelInstance.GetElementById(key);
		}

		public Map_floatingRandomModel GetMap_floatingRandomModelInstance()
		{
			return this.Map_floatingRandomModelInstance;
		}

		public IList<Map_floatingRandom> GetMap_floatingRandomElements()
		{
			return this.Map_floatingRandomModelInstance.GetAllElements();
		}

		public Map_floatingRandom GetMap_floatingRandom(int key)
		{
			return this.Map_floatingRandomModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_turntableRewardModel GetChapterMiniGame_turntableRewardModelInstance()
		{
			return this.ChapterMiniGame_turntableRewardModelInstance;
		}

		public IList<ChapterMiniGame_turntableReward> GetChapterMiniGame_turntableRewardElements()
		{
			return this.ChapterMiniGame_turntableRewardModelInstance.GetAllElements();
		}

		public ChapterMiniGame_turntableReward GetChapterMiniGame_turntableReward(int key)
		{
			return this.ChapterMiniGame_turntableRewardModelInstance.GetElementById(key);
		}

		public Const_ConstModel GetConst_ConstModelInstance()
		{
			return this.Const_ConstModelInstance;
		}

		public IList<Const_Const> GetConst_ConstElements()
		{
			return this.Const_ConstModelInstance.GetAllElements();
		}

		public Const_Const GetConst_Const(string key)
		{
			return this.Const_ConstModelInstance.GetElementById(key);
		}

		public GameSkill_skillConditionModel GetGameSkill_skillConditionModelInstance()
		{
			return this.GameSkill_skillConditionModelInstance;
		}

		public IList<GameSkill_skillCondition> GetGameSkill_skillConditionElements()
		{
			return this.GameSkill_skillConditionModelInstance.GetAllElements();
		}

		public GameSkill_skillCondition GetGameSkill_skillCondition(int key)
		{
			return this.GameSkill_skillConditionModelInstance.GetElementById(key);
		}

		public ArtRide_RideModel GetArtRide_RideModelInstance()
		{
			return this.ArtRide_RideModelInstance;
		}

		public IList<ArtRide_Ride> GetArtRide_RideElements()
		{
			return this.ArtRide_RideModelInstance.GetAllElements();
		}

		public ArtRide_Ride GetArtRide_Ride(int key)
		{
			return this.ArtRide_RideModelInstance.GetElementById(key);
		}

		public ChainPacks_ChainPacksModel GetChainPacks_ChainPacksModelInstance()
		{
			return this.ChainPacks_ChainPacksModelInstance;
		}

		public IList<ChainPacks_ChainPacks> GetChainPacks_ChainPacksElements()
		{
			return this.ChainPacks_ChainPacksModelInstance.GetAllElements();
		}

		public ChainPacks_ChainPacks GetChainPacks_ChainPacks(int key)
		{
			return this.ChainPacks_ChainPacksModelInstance.GetElementById(key);
		}

		public TalentLegacy_careerModel GetTalentLegacy_careerModelInstance()
		{
			return this.TalentLegacy_careerModelInstance;
		}

		public IList<TalentLegacy_career> GetTalentLegacy_careerElements()
		{
			return this.TalentLegacy_careerModelInstance.GetAllElements();
		}

		public TalentLegacy_career GetTalentLegacy_career(int key)
		{
			return this.TalentLegacy_careerModelInstance.GetElementById(key);
		}

		public LanguageRaft_languagetableModel GetLanguageRaft_languagetableModelInstance()
		{
			return this.LanguageRaft_languagetableModelInstance;
		}

		public IList<LanguageRaft_languagetable> GetLanguageRaft_languagetableElements()
		{
			return this.LanguageRaft_languagetableModelInstance.GetAllElements();
		}

		public LanguageRaft_languagetable GetLanguageRaft_languagetable(string key)
		{
			return this.LanguageRaft_languagetableModelInstance.GetElementById(key);
		}

		public AutoProcess_skillRandomModel GetAutoProcess_skillRandomModelInstance()
		{
			return this.AutoProcess_skillRandomModelInstance;
		}

		public IList<AutoProcess_skillRandom> GetAutoProcess_skillRandomElements()
		{
			return this.AutoProcess_skillRandomModelInstance.GetAllElements();
		}

		public AutoProcess_skillRandom GetAutoProcess_skillRandom(int key)
		{
			return this.AutoProcess_skillRandomModelInstance.GetElementById(key);
		}

		public ServerList_serverListModel GetServerList_serverListModelInstance()
		{
			return this.ServerList_serverListModelInstance;
		}

		public IList<ServerList_serverList> GetServerList_serverListElements()
		{
			return this.ServerList_serverListModelInstance.GetAllElements();
		}

		public ServerList_serverList GetServerList_serverList(int key)
		{
			return this.ServerList_serverListModelInstance.GetElementById(key);
		}

		public IAP_platformIDModel GetIAP_platformIDModelInstance()
		{
			return this.IAP_platformIDModelInstance;
		}

		public IList<IAP_platformID> GetIAP_platformIDElements()
		{
			return this.IAP_platformIDModelInstance.GetAllElements();
		}

		public IAP_platformID GetIAP_platformID(int key)
		{
			return this.IAP_platformIDModelInstance.GetElementById(key);
		}

		public IAP_LevelFundRewardModel GetIAP_LevelFundRewardModelInstance()
		{
			return this.IAP_LevelFundRewardModelInstance;
		}

		public IList<IAP_LevelFundReward> GetIAP_LevelFundRewardElements()
		{
			return this.IAP_LevelFundRewardModelInstance.GetAllElements();
		}

		public IAP_LevelFundReward GetIAP_LevelFundReward(int key)
		{
			return this.IAP_LevelFundRewardModelInstance.GetElementById(key);
		}

		public ArtSagecraft_SagecraftModel GetArtSagecraft_SagecraftModelInstance()
		{
			return this.ArtSagecraft_SagecraftModelInstance;
		}

		public IList<ArtSagecraft_Sagecraft> GetArtSagecraft_SagecraftElements()
		{
			return this.ArtSagecraft_SagecraftModelInstance.GetAllElements();
		}

		public ArtSagecraft_Sagecraft GetArtSagecraft_Sagecraft(int key)
		{
			return this.ArtSagecraft_SagecraftModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossAttrModel GetGuildBOSS_guildBossAttrModelInstance()
		{
			return this.GuildBOSS_guildBossAttrModelInstance;
		}

		public IList<GuildBOSS_guildBossAttr> GetGuildBOSS_guildBossAttrElements()
		{
			return this.GuildBOSS_guildBossAttrModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossAttr GetGuildBOSS_guildBossAttr(int key)
		{
			return this.GuildBOSS_guildBossAttrModelInstance.GetElementById(key);
		}

		public GameBuff_buffTypeModel GetGameBuff_buffTypeModelInstance()
		{
			return this.GameBuff_buffTypeModelInstance;
		}

		public IList<GameBuff_buffType> GetGameBuff_buffTypeElements()
		{
			return this.GameBuff_buffTypeModelInstance.GetAllElements();
		}

		public GameBuff_buffType GetGameBuff_buffType(int key)
		{
			return this.GameBuff_buffTypeModelInstance.GetElementById(key);
		}

		public Pet_petSummonModel GetPet_petSummonModelInstance()
		{
			return this.Pet_petSummonModelInstance;
		}

		public IList<Pet_petSummon> GetPet_petSummonElements()
		{
			return this.Pet_petSummonModelInstance.GetAllElements();
		}

		public Pet_petSummon GetPet_petSummon(int key)
		{
			return this.Pet_petSummonModelInstance.GetElementById(key);
		}

		public Chapter_skillExpModel GetChapter_skillExpModelInstance()
		{
			return this.Chapter_skillExpModelInstance;
		}

		public IList<Chapter_skillExp> GetChapter_skillExpElements()
		{
			return this.Chapter_skillExpModelInstance.GetAllElements();
		}

		public Chapter_skillExp GetChapter_skillExp(int key)
		{
			return this.Chapter_skillExpModelInstance.GetElementById(key);
		}

		public Mining_oreQualityModel GetMining_oreQualityModelInstance()
		{
			return this.Mining_oreQualityModelInstance;
		}

		public IList<Mining_oreQuality> GetMining_oreQualityElements()
		{
			return this.Mining_oreQualityModelInstance.GetAllElements();
		}

		public Mining_oreQuality GetMining_oreQuality(int key)
		{
			return this.Mining_oreQualityModelInstance.GetElementById(key);
		}

		public ArtMember_memberModel GetArtMember_memberModelInstance()
		{
			return this.ArtMember_memberModelInstance;
		}

		public IList<ArtMember_member> GetArtMember_memberElements()
		{
			return this.ArtMember_memberModelInstance.GetAllElements();
		}

		public ArtMember_member GetArtMember_member(int key)
		{
			return this.ArtMember_memberModelInstance.GetElementById(key);
		}

		public MainLevelReward_AFKrewardModel GetMainLevelReward_AFKrewardModelInstance()
		{
			return this.MainLevelReward_AFKrewardModelInstance;
		}

		public IList<MainLevelReward_AFKreward> GetMainLevelReward_AFKrewardElements()
		{
			return this.MainLevelReward_AFKrewardModelInstance.GetAllElements();
		}

		public MainLevelReward_AFKreward GetMainLevelReward_AFKreward(int key)
		{
			return this.MainLevelReward_AFKrewardModelInstance.GetElementById(key);
		}

		public NewWorld_newWorldRankModel GetNewWorld_newWorldRankModelInstance()
		{
			return this.NewWorld_newWorldRankModelInstance;
		}

		public IList<NewWorld_newWorldRank> GetNewWorld_newWorldRankElements()
		{
			return this.NewWorld_newWorldRankModelInstance.GetAllElements();
		}

		public NewWorld_newWorldRank GetNewWorld_newWorldRank(int key)
		{
			return this.NewWorld_newWorldRankModelInstance.GetElementById(key);
		}

		public CommonActivity_CommonActivityModel GetCommonActivity_CommonActivityModelInstance()
		{
			return this.CommonActivity_CommonActivityModelInstance;
		}

		public IList<CommonActivity_CommonActivity> GetCommonActivity_CommonActivityElements()
		{
			return this.CommonActivity_CommonActivityModelInstance.GetAllElements();
		}

		public CommonActivity_CommonActivity GetCommonActivity_CommonActivity(int key)
		{
			return this.CommonActivity_CommonActivityModelInstance.GetElementById(key);
		}

		public Pet_petLevelModel GetPet_petLevelModelInstance()
		{
			return this.Pet_petLevelModelInstance;
		}

		public IList<Pet_petLevel> GetPet_petLevelElements()
		{
			return this.Pet_petLevelModelInstance.GetAllElements();
		}

		public Pet_petLevel GetPet_petLevel(int key)
		{
			return this.Pet_petLevelModelInstance.GetElementById(key);
		}

		public Function_FunctionModel GetFunction_FunctionModelInstance()
		{
			return this.Function_FunctionModelInstance;
		}

		public IList<Function_Function> GetFunction_FunctionElements()
		{
			return this.Function_FunctionModelInstance.GetAllElements();
		}

		public Function_Function GetFunction_Function(int key)
		{
			return this.Function_FunctionModelInstance.GetElementById(key);
		}

		public Mining_oreResModel GetMining_oreResModelInstance()
		{
			return this.Mining_oreResModelInstance;
		}

		public IList<Mining_oreRes> GetMining_oreResElements()
		{
			return this.Mining_oreResModelInstance.GetAllElements();
		}

		public Mining_oreRes GetMining_oreRes(int key)
		{
			return this.Mining_oreResModelInstance.GetElementById(key);
		}

		public BattleMain_skillModel GetBattleMain_skillModelInstance()
		{
			return this.BattleMain_skillModelInstance;
		}

		public IList<BattleMain_skill> GetBattleMain_skillElements()
		{
			return this.BattleMain_skillModelInstance.GetAllElements();
		}

		public BattleMain_skill GetBattleMain_skill(int key)
		{
			return this.BattleMain_skillModelInstance.GetElementById(key);
		}

		public TowerChallenge_TowerModel GetTowerChallenge_TowerModelInstance()
		{
			return this.TowerChallenge_TowerModelInstance;
		}

		public IList<TowerChallenge_Tower> GetTowerChallenge_TowerElements()
		{
			return this.TowerChallenge_TowerModelInstance.GetAllElements();
		}

		public TowerChallenge_Tower GetTowerChallenge_Tower(int key)
		{
			return this.TowerChallenge_TowerModelInstance.GetElementById(key);
		}

		public ItemResources_jumpResourceModel GetItemResources_jumpResourceModelInstance()
		{
			return this.ItemResources_jumpResourceModelInstance;
		}

		public IList<ItemResources_jumpResource> GetItemResources_jumpResourceElements()
		{
			return this.ItemResources_jumpResourceModelInstance.GetAllElements();
		}

		public ItemResources_jumpResource GetItemResources_jumpResource(int key)
		{
			return this.ItemResources_jumpResourceModelInstance.GetElementById(key);
		}

		public ArtAnimation_animationModel GetArtAnimation_animationModelInstance()
		{
			return this.ArtAnimation_animationModelInstance;
		}

		public IList<ArtAnimation_animation> GetArtAnimation_animationElements()
		{
			return this.ArtAnimation_animationModelInstance.GetAllElements();
		}

		public ArtAnimation_animation GetArtAnimation_animation(string key)
		{
			return this.ArtAnimation_animationModelInstance.GetElementById(key);
		}

		public Quality_equipQualityModel GetQuality_equipQualityModelInstance()
		{
			return this.Quality_equipQualityModelInstance;
		}

		public IList<Quality_equipQuality> GetQuality_equipQualityElements()
		{
			return this.Quality_equipQualityModelInstance.GetAllElements();
		}

		public Quality_equipQuality GetQuality_equipQuality(int key)
		{
			return this.Quality_equipQualityModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossModel GetGuildBOSS_guildBossModelInstance()
		{
			return this.GuildBOSS_guildBossModelInstance;
		}

		public IList<GuildBOSS_guildBoss> GetGuildBOSS_guildBossElements()
		{
			return this.GuildBOSS_guildBossModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBoss GetGuildBOSS_guildBoss(int key)
		{
			return this.GuildBOSS_guildBossModelInstance.GetElementById(key);
		}

		public Item_ItemModel GetItem_ItemModelInstance()
		{
			return this.Item_ItemModelInstance;
		}

		public IList<Item_Item> GetItem_ItemElements()
		{
			return this.Item_ItemModelInstance.GetAllElements();
		}

		public Item_Item GetItem_Item(int key)
		{
			return this.Item_ItemModelInstance.GetElementById(key);
		}

		public Pet_PetTrainingModel GetPet_PetTrainingModelInstance()
		{
			return this.Pet_PetTrainingModelInstance;
		}

		public IList<Pet_PetTraining> GetPet_PetTrainingElements()
		{
			return this.Pet_PetTrainingModelInstance.GetAllElements();
		}

		public Pet_PetTraining GetPet_PetTraining(int key)
		{
			return this.Pet_PetTrainingModelInstance.GetElementById(key);
		}

		public Pet_petModel GetPet_petModelInstance()
		{
			return this.Pet_petModelInstance;
		}

		public IList<Pet_pet> GetPet_petElements()
		{
			return this.Pet_petModelInstance.GetAllElements();
		}

		public Pet_pet GetPet_pet(int key)
		{
			return this.Pet_petModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaTimeModel GetCrossArena_CrossArenaTimeModelInstance()
		{
			return this.CrossArena_CrossArenaTimeModelInstance;
		}

		public IList<CrossArena_CrossArenaTime> GetCrossArena_CrossArenaTimeElements()
		{
			return this.CrossArena_CrossArenaTimeModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaTime GetCrossArena_CrossArenaTime(int key)
		{
			return this.CrossArena_CrossArenaTimeModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaRobotModel GetCrossArena_CrossArenaRobotModelInstance()
		{
			return this.CrossArena_CrossArenaRobotModelInstance;
		}

		public IList<CrossArena_CrossArenaRobot> GetCrossArena_CrossArenaRobotElements()
		{
			return this.CrossArena_CrossArenaRobotModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaRobot GetCrossArena_CrossArenaRobot(int key)
		{
			return this.CrossArena_CrossArenaRobotModelInstance.GetElementById(key);
		}

		public TGASource_IAPModel GetTGASource_IAPModelInstance()
		{
			return this.TGASource_IAPModelInstance;
		}

		public IList<TGASource_IAP> GetTGASource_IAPElements()
		{
			return this.TGASource_IAPModelInstance.GetAllElements();
		}

		public TGASource_IAP GetTGASource_IAP(int key)
		{
			return this.TGASource_IAPModelInstance.GetElementById(key);
		}

		public Guild_guildConstModel GetGuild_guildConstModelInstance()
		{
			return this.Guild_guildConstModelInstance;
		}

		public IList<Guild_guildConst> GetGuild_guildConstElements()
		{
			return this.Guild_guildConstModelInstance.GetAllElements();
		}

		public Guild_guildConst GetGuild_guildConst(int key)
		{
			return this.Guild_guildConstModelInstance.GetElementById(key);
		}

		public GameBuff_buffModel GetGameBuff_buffModelInstance()
		{
			return this.GameBuff_buffModelInstance;
		}

		public IList<GameBuff_buff> GetGameBuff_buffElements()
		{
			return this.GameBuff_buffModelInstance.GetAllElements();
		}

		public GameBuff_buff GetGameBuff_buff(int key)
		{
			return this.GameBuff_buffModelInstance.GetElementById(key);
		}

		public Box_boxBaseModel GetBox_boxBaseModelInstance()
		{
			return this.Box_boxBaseModelInstance;
		}

		public IList<Box_boxBase> GetBox_boxBaseElements()
		{
			return this.Box_boxBaseModelInstance.GetAllElements();
		}

		public Box_boxBase GetBox_boxBase(int key)
		{
			return this.Box_boxBaseModelInstance.GetElementById(key);
		}

		public ArtHover_hoverTextModel GetArtHover_hoverTextModelInstance()
		{
			return this.ArtHover_hoverTextModelInstance;
		}

		public IList<ArtHover_hoverText> GetArtHover_hoverTextElements()
		{
			return this.ArtHover_hoverTextModelInstance.GetAllElements();
		}

		public ArtHover_hoverText GetArtHover_hoverText(int key)
		{
			return this.ArtHover_hoverTextModelInstance.GetElementById(key);
		}

		public Chapter_boxBuildModel GetChapter_boxBuildModelInstance()
		{
			return this.Chapter_boxBuildModelInstance;
		}

		public IList<Chapter_boxBuild> GetChapter_boxBuildElements()
		{
			return this.Chapter_boxBuildModelInstance.GetAllElements();
		}

		public Chapter_boxBuild GetChapter_boxBuild(int key)
		{
			return this.Chapter_boxBuildModelInstance.GetElementById(key);
		}

		public GameSkill_skillTypeModel GetGameSkill_skillTypeModelInstance()
		{
			return this.GameSkill_skillTypeModelInstance;
		}

		public IList<GameSkill_skillType> GetGameSkill_skillTypeElements()
		{
			return this.GameSkill_skillTypeModelInstance.GetAllElements();
		}

		public GameSkill_skillType GetGameSkill_skillType(int key)
		{
			return this.GameSkill_skillTypeModelInstance.GetElementById(key);
		}

		public NewWorld_newWorldTaskModel GetNewWorld_newWorldTaskModelInstance()
		{
			return this.NewWorld_newWorldTaskModelInstance;
		}

		public IList<NewWorld_newWorldTask> GetNewWorld_newWorldTaskElements()
		{
			return this.NewWorld_newWorldTaskModelInstance.GetAllElements();
		}

		public NewWorld_newWorldTask GetNewWorld_newWorldTask(int key)
		{
			return this.NewWorld_newWorldTaskModelInstance.GetElementById(key);
		}

		public Guild_guildGiftModel GetGuild_guildGiftModelInstance()
		{
			return this.Guild_guildGiftModelInstance;
		}

		public IList<Guild_guildGift> GetGuild_guildGiftElements()
		{
			return this.Guild_guildGiftModelInstance.GetAllElements();
		}

		public Guild_guildGift GetGuild_guildGift(int key)
		{
			return this.Guild_guildGiftModelInstance.GetElementById(key);
		}

		public Chapter_eventPointModel GetChapter_eventPointModelInstance()
		{
			return this.Chapter_eventPointModelInstance;
		}

		public IList<Chapter_eventPoint> GetChapter_eventPointElements()
		{
			return this.Chapter_eventPointModelInstance.GetAllElements();
		}

		public Chapter_eventPoint GetChapter_eventPoint(int key)
		{
			return this.Chapter_eventPointModelInstance.GetElementById(key);
		}

		public Chapter_sweepModel GetChapter_sweepModelInstance()
		{
			return this.Chapter_sweepModelInstance;
		}

		public IList<Chapter_sweep> GetChapter_sweepElements()
		{
			return this.Chapter_sweepModelInstance.GetAllElements();
		}

		public Chapter_sweep GetChapter_sweep(int key)
		{
			return this.Chapter_sweepModelInstance.GetElementById(key);
		}

		public ActivityTurntable_ActivityTurntableModel GetActivityTurntable_ActivityTurntableModelInstance()
		{
			return this.ActivityTurntable_ActivityTurntableModelInstance;
		}

		public IList<ActivityTurntable_ActivityTurntable> GetActivityTurntable_ActivityTurntableElements()
		{
			return this.ActivityTurntable_ActivityTurntableModelInstance.GetAllElements();
		}

		public ActivityTurntable_ActivityTurntable GetActivityTurntable_ActivityTurntable(int key)
		{
			return this.ActivityTurntable_ActivityTurntableModelInstance.GetElementById(key);
		}

		public Task_DailyTaskModel GetTask_DailyTaskModelInstance()
		{
			return this.Task_DailyTaskModelInstance;
		}

		public IList<Task_DailyTask> GetTask_DailyTaskElements()
		{
			return this.Task_DailyTaskModelInstance.GetAllElements();
		}

		public Task_DailyTask GetTask_DailyTask(int key)
		{
			return this.Task_DailyTaskModelInstance.GetElementById(key);
		}

		public Artifact_artifactLevelModel GetArtifact_artifactLevelModelInstance()
		{
			return this.Artifact_artifactLevelModelInstance;
		}

		public IList<Artifact_artifactLevel> GetArtifact_artifactLevelElements()
		{
			return this.Artifact_artifactLevelModelInstance.GetAllElements();
		}

		public Artifact_artifactLevel GetArtifact_artifactLevel(int key)
		{
			return this.Artifact_artifactLevelModelInstance.GetElementById(key);
		}

		public Sound_soundModel GetSound_soundModelInstance()
		{
			return this.Sound_soundModelInstance;
		}

		public IList<Sound_sound> GetSound_soundElements()
		{
			return this.Sound_soundModelInstance.GetAllElements();
		}

		public Sound_sound GetSound_sound(int key)
		{
			return this.Sound_soundModelInstance.GetElementById(key);
		}

		public IntegralShop_goodsModel GetIntegralShop_goodsModelInstance()
		{
			return this.IntegralShop_goodsModelInstance;
		}

		public IList<IntegralShop_goods> GetIntegralShop_goodsElements()
		{
			return this.IntegralShop_goodsModelInstance.GetAllElements();
		}

		public IntegralShop_goods GetIntegralShop_goods(int key)
		{
			return this.IntegralShop_goodsModelInstance.GetElementById(key);
		}

		public Ride_RideModel GetRide_RideModelInstance()
		{
			return this.Ride_RideModelInstance;
		}

		public IList<Ride_Ride> GetRide_RideElements()
		{
			return this.Ride_RideModelInstance.GetAllElements();
		}

		public Ride_Ride GetRide_Ride(int key)
		{
			return this.Ride_RideModelInstance.GetElementById(key);
		}

		public Chapter_eventTypeModel GetChapter_eventTypeModelInstance()
		{
			return this.Chapter_eventTypeModelInstance;
		}

		public IList<Chapter_eventType> GetChapter_eventTypeElements()
		{
			return this.Chapter_eventTypeModelInstance.GetAllElements();
		}

		public Chapter_eventType GetChapter_eventType(int key)
		{
			return this.Chapter_eventTypeModelInstance.GetElementById(key);
		}

		public ItemGift_ItemGiftModel GetItemGift_ItemGiftModelInstance()
		{
			return this.ItemGift_ItemGiftModelInstance;
		}

		public IList<ItemGift_ItemGift> GetItemGift_ItemGiftElements()
		{
			return this.ItemGift_ItemGiftModelInstance.GetAllElements();
		}

		public ItemGift_ItemGift GetItemGift_ItemGift(int key)
		{
			return this.ItemGift_ItemGiftModelInstance.GetElementById(key);
		}

		public ChapterActivity_RankActivityModel GetChapterActivity_RankActivityModelInstance()
		{
			return this.ChapterActivity_RankActivityModelInstance;
		}

		public IList<ChapterActivity_RankActivity> GetChapterActivity_RankActivityElements()
		{
			return this.ChapterActivity_RankActivityModelInstance.GetAllElements();
		}

		public ChapterActivity_RankActivity GetChapterActivity_RankActivity(int key)
		{
			return this.ChapterActivity_RankActivityModelInstance.GetElementById(key);
		}

		public GameMember_skinModel GetGameMember_skinModelInstance()
		{
			return this.GameMember_skinModelInstance;
		}

		public IList<GameMember_skin> GetGameMember_skinElements()
		{
			return this.GameMember_skinModelInstance.GetAllElements();
		}

		public GameMember_skin GetGameMember_skin(int key)
		{
			return this.GameMember_skinModelInstance.GetElementById(key);
		}

		public TicketExchange_ExchangeModel GetTicketExchange_ExchangeModelInstance()
		{
			return this.TicketExchange_ExchangeModelInstance;
		}

		public IList<TicketExchange_Exchange> GetTicketExchange_ExchangeElements()
		{
			return this.TicketExchange_ExchangeModelInstance.GetAllElements();
		}

		public TicketExchange_Exchange GetTicketExchange_Exchange(int key)
		{
			return this.TicketExchange_ExchangeModelInstance.GetElementById(key);
		}

		public Quality_petQualityModel GetQuality_petQualityModelInstance()
		{
			return this.Quality_petQualityModelInstance;
		}

		public IList<Quality_petQuality> GetQuality_petQualityElements()
		{
			return this.Quality_petQualityModelInstance.GetAllElements();
		}

		public Quality_petQuality GetQuality_petQuality(int key)
		{
			return this.Quality_petQualityModelInstance.GetElementById(key);
		}

		public Avatar_SceneSkinModel GetAvatar_SceneSkinModelInstance()
		{
			return this.Avatar_SceneSkinModelInstance;
		}

		public IList<Avatar_SceneSkin> GetAvatar_SceneSkinElements()
		{
			return this.Avatar_SceneSkinModelInstance.GetAllElements();
		}

		public Avatar_SceneSkin GetAvatar_SceneSkin(int key)
		{
			return this.Avatar_SceneSkinModelInstance.GetElementById(key);
		}

		public Guild_guildDonationModel GetGuild_guildDonationModelInstance()
		{
			return this.Guild_guildDonationModelInstance;
		}

		public IList<Guild_guildDonation> GetGuild_guildDonationElements()
		{
			return this.Guild_guildDonationModelInstance.GetAllElements();
		}

		public Guild_guildDonation GetGuild_guildDonation(int key)
		{
			return this.Guild_guildDonationModelInstance.GetElementById(key);
		}

		public ChapterActivity_ActvTurntableDetailModel GetChapterActivity_ActvTurntableDetailModelInstance()
		{
			return this.ChapterActivity_ActvTurntableDetailModelInstance;
		}

		public IList<ChapterActivity_ActvTurntableDetail> GetChapterActivity_ActvTurntableDetailElements()
		{
			return this.ChapterActivity_ActvTurntableDetailModelInstance.GetAllElements();
		}

		public ChapterActivity_ActvTurntableDetail GetChapterActivity_ActvTurntableDetail(int key)
		{
			return this.ChapterActivity_ActvTurntableDetailModelInstance.GetElementById(key);
		}

		public BattleMain_chapterModel GetBattleMain_chapterModelInstance()
		{
			return this.BattleMain_chapterModelInstance;
		}

		public IList<BattleMain_chapter> GetBattleMain_chapterElements()
		{
			return this.BattleMain_chapterModelInstance.GetAllElements();
		}

		public BattleMain_chapter GetBattleMain_chapter(int key)
		{
			return this.BattleMain_chapterModelInstance.GetElementById(key);
		}

		public GameBuff_overlayTypeModel GetGameBuff_overlayTypeModelInstance()
		{
			return this.GameBuff_overlayTypeModelInstance;
		}

		public IList<GameBuff_overlayType> GetGameBuff_overlayTypeElements()
		{
			return this.GameBuff_overlayTypeModelInstance.GetAllElements();
		}

		public GameBuff_overlayType GetGameBuff_overlayType(int key)
		{
			return this.GameBuff_overlayTypeModelInstance.GetElementById(key);
		}

		public GuildRace_baseRaceModel GetGuildRace_baseRaceModelInstance()
		{
			return this.GuildRace_baseRaceModelInstance;
		}

		public IList<GuildRace_baseRace> GetGuildRace_baseRaceElements()
		{
			return this.GuildRace_baseRaceModelInstance.GetAllElements();
		}

		public GuildRace_baseRace GetGuildRace_baseRace(int key)
		{
			return this.GuildRace_baseRaceModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildSubectionModel GetGuildBOSS_guildSubectionModelInstance()
		{
			return this.GuildBOSS_guildSubectionModelInstance;
		}

		public IList<GuildBOSS_guildSubection> GetGuildBOSS_guildSubectionElements()
		{
			return this.GuildBOSS_guildSubectionModelInstance.GetAllElements();
		}

		public GuildBOSS_guildSubection GetGuildBOSS_guildSubection(int key)
		{
			return this.GuildBOSS_guildSubectionModelInstance.GetElementById(key);
		}

		public Guild_guilddairyModel GetGuild_guilddairyModelInstance()
		{
			return this.Guild_guilddairyModelInstance;
		}

		public IList<Guild_guilddairy> GetGuild_guilddairyElements()
		{
			return this.Guild_guilddairyModelInstance.GetAllElements();
		}

		public Guild_guilddairy GetGuild_guilddairy(int key)
		{
			return this.Guild_guilddairyModelInstance.GetElementById(key);
		}

		public ChainPacks_ChainActvModel GetChainPacks_ChainActvModelInstance()
		{
			return this.ChainPacks_ChainActvModelInstance;
		}

		public IList<ChainPacks_ChainActv> GetChainPacks_ChainActvElements()
		{
			return this.ChainPacks_ChainActvModelInstance.GetAllElements();
		}

		public ChainPacks_ChainActv GetChainPacks_ChainActv(int key)
		{
			return this.ChainPacks_ChainActvModelInstance.GetElementById(key);
		}

		public Guild_guildShopModel GetGuild_guildShopModelInstance()
		{
			return this.Guild_guildShopModelInstance;
		}

		public IList<Guild_guildShop> GetGuild_guildShopElements()
		{
			return this.Guild_guildShopModelInstance.GetAllElements();
		}

		public Guild_guildShop GetGuild_guildShop(int key)
		{
			return this.Guild_guildShopModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_turntableBaseModel GetChapterMiniGame_turntableBaseModelInstance()
		{
			return this.ChapterMiniGame_turntableBaseModelInstance;
		}

		public IList<ChapterMiniGame_turntableBase> GetChapterMiniGame_turntableBaseElements()
		{
			return this.ChapterMiniGame_turntableBaseModelInstance.GetAllElements();
		}

		public ChapterMiniGame_turntableBase GetChapterMiniGame_turntableBase(int key)
		{
			return this.ChapterMiniGame_turntableBaseModelInstance.GetElementById(key);
		}

		public Relic_dataModel GetRelic_dataModelInstance()
		{
			return this.Relic_dataModelInstance;
		}

		public IList<Relic_data> GetRelic_dataElements()
		{
			return this.Relic_dataModelInstance.GetAllElements();
		}

		public Relic_data GetRelic_data(int key)
		{
			return this.Relic_dataModelInstance.GetElementById(key);
		}

		public Collection_collectionStarModel GetCollection_collectionStarModelInstance()
		{
			return this.Collection_collectionStarModelInstance;
		}

		public IList<Collection_collectionStar> GetCollection_collectionStarElements()
		{
			return this.Collection_collectionStarModelInstance.GetAllElements();
		}

		public Collection_collectionStar GetCollection_collectionStar(int key)
		{
			return this.Collection_collectionStarModelInstance.GetElementById(key);
		}

		public LanguageRaft_languageTabModel GetLanguageRaft_languageTabModelInstance()
		{
			return this.LanguageRaft_languageTabModelInstance;
		}

		public IList<LanguageRaft_languageTab> GetLanguageRaft_languageTabElements()
		{
			return this.LanguageRaft_languageTabModelInstance.GetAllElements();
		}

		public LanguageRaft_languageTab GetLanguageRaft_languageTab(int key)
		{
			return this.LanguageRaft_languageTabModelInstance.GetElementById(key);
		}

		public Shop_ShopModel GetShop_ShopModelInstance()
		{
			return this.Shop_ShopModelInstance;
		}

		public IList<Shop_Shop> GetShop_ShopElements()
		{
			return this.Shop_ShopModelInstance.GetAllElements();
		}

		public Shop_Shop GetShop_Shop(int key)
		{
			return this.Shop_ShopModelInstance.GetElementById(key);
		}

		public IAP_pushIapModel GetIAP_pushIapModelInstance()
		{
			return this.IAP_pushIapModelInstance;
		}

		public IList<IAP_pushIap> GetIAP_pushIapElements()
		{
			return this.IAP_pushIapModelInstance.GetAllElements();
		}

		public IAP_pushIap GetIAP_pushIap(int key)
		{
			return this.IAP_pushIapModelInstance.GetElementById(key);
		}

		public GameSkill_skillSelectModel GetGameSkill_skillSelectModelInstance()
		{
			return this.GameSkill_skillSelectModelInstance;
		}

		public IList<GameSkill_skillSelect> GetGameSkill_skillSelectElements()
		{
			return this.GameSkill_skillSelectModelInstance.GetAllElements();
		}

		public GameSkill_skillSelect GetGameSkill_skillSelect(int key)
		{
			return this.GameSkill_skillSelectModelInstance.GetElementById(key);
		}

		public CommonActivity_DropObjModel GetCommonActivity_DropObjModelInstance()
		{
			return this.CommonActivity_DropObjModelInstance;
		}

		public IList<CommonActivity_DropObj> GetCommonActivity_DropObjElements()
		{
			return this.CommonActivity_DropObjModelInstance.GetAllElements();
		}

		public CommonActivity_DropObj GetCommonActivity_DropObj(int key)
		{
			return this.CommonActivity_DropObjModelInstance.GetElementById(key);
		}

		public Chapter_eventResModel GetChapter_eventResModelInstance()
		{
			return this.Chapter_eventResModelInstance;
		}

		public IList<Chapter_eventRes> GetChapter_eventResElements()
		{
			return this.Chapter_eventResModelInstance.GetAllElements();
		}

		public Chapter_eventRes GetChapter_eventRes(int key)
		{
			return this.Chapter_eventResModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaModel GetCrossArena_CrossArenaModelInstance()
		{
			return this.CrossArena_CrossArenaModelInstance;
		}

		public IList<CrossArena_CrossArena> GetCrossArena_CrossArenaElements()
		{
			return this.CrossArena_CrossArenaModelInstance.GetAllElements();
		}

		public CrossArena_CrossArena GetCrossArena_CrossArena(int key)
		{
			return this.CrossArena_CrossArenaModelInstance.GetElementById(key);
		}

		public Equip_skillModel GetEquip_skillModelInstance()
		{
			return this.Equip_skillModelInstance;
		}

		public IList<Equip_skill> GetEquip_skillElements()
		{
			return this.Equip_skillModelInstance.GetAllElements();
		}

		public Equip_skill GetEquip_skill(int key)
		{
			return this.Equip_skillModelInstance.GetElementById(key);
		}

		public GameSkill_skillAnimationModel GetGameSkill_skillAnimationModelInstance()
		{
			return this.GameSkill_skillAnimationModelInstance;
		}

		public IList<GameSkill_skillAnimation> GetGameSkill_skillAnimationElements()
		{
			return this.GameSkill_skillAnimationModelInstance.GetAllElements();
		}

		public GameSkill_skillAnimation GetGameSkill_skillAnimation(int key)
		{
			return this.GameSkill_skillAnimationModelInstance.GetElementById(key);
		}

		public Chapter_eventItemModel GetChapter_eventItemModelInstance()
		{
			return this.Chapter_eventItemModelInstance;
		}

		public IList<Chapter_eventItem> GetChapter_eventItemElements()
		{
			return this.Chapter_eventItemModelInstance.GetAllElements();
		}

		public Chapter_eventItem GetChapter_eventItem(int key)
		{
			return this.Chapter_eventItemModelInstance.GetElementById(key);
		}

		public WorldBoss_RewardModel GetWorldBoss_RewardModelInstance()
		{
			return this.WorldBoss_RewardModelInstance;
		}

		public IList<WorldBoss_Reward> GetWorldBoss_RewardElements()
		{
			return this.WorldBoss_RewardModelInstance.GetAllElements();
		}

		public WorldBoss_Reward GetWorldBoss_Reward(int key)
		{
			return this.WorldBoss_RewardModelInstance.GetElementById(key);
		}

		public ActivityTurntable_TurntableQuestModel GetActivityTurntable_TurntableQuestModelInstance()
		{
			return this.ActivityTurntable_TurntableQuestModelInstance;
		}

		public IList<ActivityTurntable_TurntableQuest> GetActivityTurntable_TurntableQuestElements()
		{
			return this.ActivityTurntable_TurntableQuestModelInstance.GetAllElements();
		}

		public ActivityTurntable_TurntableQuest GetActivityTurntable_TurntableQuest(int key)
		{
			return this.ActivityTurntable_TurntableQuestModelInstance.GetElementById(key);
		}

		public ArtEffect_EffectModel GetArtEffect_EffectModelInstance()
		{
			return this.ArtEffect_EffectModelInstance;
		}

		public IList<ArtEffect_Effect> GetArtEffect_EffectElements()
		{
			return this.ArtEffect_EffectModelInstance.GetAllElements();
		}

		public ArtEffect_Effect GetArtEffect_Effect(int key)
		{
			return this.ArtEffect_EffectModelInstance.GetElementById(key);
		}

		public WorldBoss_WorldBossBoxModel GetWorldBoss_WorldBossBoxModelInstance()
		{
			return this.WorldBoss_WorldBossBoxModelInstance;
		}

		public IList<WorldBoss_WorldBossBox> GetWorldBoss_WorldBossBoxElements()
		{
			return this.WorldBoss_WorldBossBoxModelInstance.GetAllElements();
		}

		public WorldBoss_WorldBossBox GetWorldBoss_WorldBossBox(int key)
		{
			return this.WorldBoss_WorldBossBoxModelInstance.GetElementById(key);
		}

		public GuildRace_opentimeModel GetGuildRace_opentimeModelInstance()
		{
			return this.GuildRace_opentimeModelInstance;
		}

		public IList<GuildRace_opentime> GetGuildRace_opentimeElements()
		{
			return this.GuildRace_opentimeModelInstance.GetAllElements();
		}

		public GuildRace_opentime GetGuildRace_opentime(int key)
		{
			return this.GuildRace_opentimeModelInstance.GetElementById(key);
		}

		public Chapter_attributeBuildModel GetChapter_attributeBuildModelInstance()
		{
			return this.Chapter_attributeBuildModelInstance;
		}

		public IList<Chapter_attributeBuild> GetChapter_attributeBuildElements()
		{
			return this.Chapter_attributeBuildModelInstance.GetAllElements();
		}

		public Chapter_attributeBuild GetChapter_attributeBuild(int key)
		{
			return this.Chapter_attributeBuildModelInstance.GetElementById(key);
		}

		public Shop_EquipActivityModel GetShop_EquipActivityModelInstance()
		{
			return this.Shop_EquipActivityModelInstance;
		}

		public IList<Shop_EquipActivity> GetShop_EquipActivityElements()
		{
			return this.Shop_EquipActivityModelInstance.GetAllElements();
		}

		public Shop_EquipActivity GetShop_EquipActivity(int key)
		{
			return this.Shop_EquipActivityModelInstance.GetElementById(key);
		}

		public Item_dropLvModel GetItem_dropLvModelInstance()
		{
			return this.Item_dropLvModelInstance;
		}

		public IList<Item_dropLv> GetItem_dropLvElements()
		{
			return this.Item_dropLvModelInstance.GetAllElements();
		}

		public Item_dropLv GetItem_dropLv(int key)
		{
			return this.Item_dropLvModelInstance.GetElementById(key);
		}

		public ChapterActivity_BattlepassRewardModel GetChapterActivity_BattlepassRewardModelInstance()
		{
			return this.ChapterActivity_BattlepassRewardModelInstance;
		}

		public IList<ChapterActivity_BattlepassReward> GetChapterActivity_BattlepassRewardElements()
		{
			return this.ChapterActivity_BattlepassRewardModelInstance.GetAllElements();
		}

		public ChapterActivity_BattlepassReward GetChapterActivity_BattlepassReward(int key)
		{
			return this.ChapterActivity_BattlepassRewardModelInstance.GetElementById(key);
		}

		public CommonActivity_ShopObjModel GetCommonActivity_ShopObjModelInstance()
		{
			return this.CommonActivity_ShopObjModelInstance;
		}

		public IList<CommonActivity_ShopObj> GetCommonActivity_ShopObjElements()
		{
			return this.CommonActivity_ShopObjModelInstance.GetAllElements();
		}

		public CommonActivity_ShopObj GetCommonActivity_ShopObj(int key)
		{
			return this.CommonActivity_ShopObjModelInstance.GetElementById(key);
		}

		public Pet_petLevelEffectModel GetPet_petLevelEffectModelInstance()
		{
			return this.Pet_petLevelEffectModelInstance;
		}

		public IList<Pet_petLevelEffect> GetPet_petLevelEffectElements()
		{
			return this.Pet_petLevelEffectModelInstance.GetAllElements();
		}

		public Pet_petLevelEffect GetPet_petLevelEffect(int key)
		{
			return this.Pet_petLevelEffectModelInstance.GetElementById(key);
		}

		public CommonActivity_PayObjModel GetCommonActivity_PayObjModelInstance()
		{
			return this.CommonActivity_PayObjModelInstance;
		}

		public IList<CommonActivity_PayObj> GetCommonActivity_PayObjElements()
		{
			return this.CommonActivity_PayObjModelInstance.GetAllElements();
		}

		public CommonActivity_PayObj GetCommonActivity_PayObj(int key)
		{
			return this.CommonActivity_PayObjModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossStepModel GetGuildBOSS_guildBossStepModelInstance()
		{
			return this.GuildBOSS_guildBossStepModelInstance;
		}

		public IList<GuildBOSS_guildBossStep> GetGuildBOSS_guildBossStepElements()
		{
			return this.GuildBOSS_guildBossStepModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossStep GetGuildBOSS_guildBossStep(int key)
		{
			return this.GuildBOSS_guildBossStepModelInstance.GetElementById(key);
		}

		public Guild_guildStyleModel GetGuild_guildStyleModelInstance()
		{
			return this.Guild_guildStyleModelInstance;
		}

		public IList<Guild_guildStyle> GetGuild_guildStyleElements()
		{
			return this.Guild_guildStyleModelInstance.GetAllElements();
		}

		public Guild_guildStyle GetGuild_guildStyle(int key)
		{
			return this.Guild_guildStyleModelInstance.GetElementById(key);
		}

		public WorldBoss_WorldBossModel GetWorldBoss_WorldBossModelInstance()
		{
			return this.WorldBoss_WorldBossModelInstance;
		}

		public IList<WorldBoss_WorldBoss> GetWorldBoss_WorldBossElements()
		{
			return this.WorldBoss_WorldBossModelInstance.GetAllElements();
		}

		public WorldBoss_WorldBoss GetWorldBoss_WorldBoss(int key)
		{
			return this.WorldBoss_WorldBossModelInstance.GetElementById(key);
		}

		public GameSkill_hitEffectModel GetGameSkill_hitEffectModelInstance()
		{
			return this.GameSkill_hitEffectModelInstance;
		}

		public IList<GameSkill_hitEffect> GetGameSkill_hitEffectElements()
		{
			return this.GameSkill_hitEffectModelInstance.GetAllElements();
		}

		public GameSkill_hitEffect GetGameSkill_hitEffect(int key)
		{
			return this.GameSkill_hitEffectModelInstance.GetElementById(key);
		}

		public Relic_relicModel GetRelic_relicModelInstance()
		{
			return this.Relic_relicModelInstance;
		}

		public IList<Relic_relic> GetRelic_relicElements()
		{
			return this.Relic_relicModelInstance.GetAllElements();
		}

		public Relic_relic GetRelic_relic(int key)
		{
			return this.Relic_relicModelInstance.GetElementById(key);
		}

		public GameSkillBuild_skillRandomModel GetGameSkillBuild_skillRandomModelInstance()
		{
			return this.GameSkillBuild_skillRandomModelInstance;
		}

		public IList<GameSkillBuild_skillRandom> GetGameSkillBuild_skillRandomElements()
		{
			return this.GameSkillBuild_skillRandomModelInstance.GetAllElements();
		}

		public GameSkillBuild_skillRandom GetGameSkillBuild_skillRandom(int key)
		{
			return this.GameSkillBuild_skillRandomModelInstance.GetElementById(key);
		}

		public Task_DailyActiveModel GetTask_DailyActiveModelInstance()
		{
			return this.Task_DailyActiveModelInstance;
		}

		public IList<Task_DailyActive> GetTask_DailyActiveElements()
		{
			return this.Task_DailyActiveModelInstance.GetAllElements();
		}

		public Task_DailyActive GetTask_DailyActive(int key)
		{
			return this.Task_DailyActiveModelInstance.GetElementById(key);
		}

		public MonsterCfg_Old_monsterCfgModel GetMonsterCfg_Old_monsterCfgModelInstance()
		{
			return this.MonsterCfg_Old_monsterCfgModelInstance;
		}

		public IList<MonsterCfg_Old_monsterCfg> GetMonsterCfg_Old_monsterCfgElements()
		{
			return this.MonsterCfg_Old_monsterCfgModelInstance.GetAllElements();
		}

		public MonsterCfg_Old_monsterCfg GetMonsterCfg_Old_monsterCfg(int key)
		{
			return this.MonsterCfg_Old_monsterCfgModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_slotTrainBuildModel GetChapterMiniGame_slotTrainBuildModelInstance()
		{
			return this.ChapterMiniGame_slotTrainBuildModelInstance;
		}

		public IList<ChapterMiniGame_slotTrainBuild> GetChapterMiniGame_slotTrainBuildElements()
		{
			return this.ChapterMiniGame_slotTrainBuildModelInstance.GetAllElements();
		}

		public ChapterMiniGame_slotTrainBuild GetChapterMiniGame_slotTrainBuild(int key)
		{
			return this.ChapterMiniGame_slotTrainBuildModelInstance.GetElementById(key);
		}

		public GameMember_aiTypeModel GetGameMember_aiTypeModelInstance()
		{
			return this.GameMember_aiTypeModelInstance;
		}

		public IList<GameMember_aiType> GetGameMember_aiTypeElements()
		{
			return this.GameMember_aiTypeModelInstance.GetAllElements();
		}

		public GameMember_aiType GetGameMember_aiType(int key)
		{
			return this.GameMember_aiTypeModelInstance.GetElementById(key);
		}

		public Mining_miningBaseModel GetMining_miningBaseModelInstance()
		{
			return this.Mining_miningBaseModelInstance;
		}

		public IList<Mining_miningBase> GetMining_miningBaseElements()
		{
			return this.Mining_miningBaseModelInstance.GetAllElements();
		}

		public Mining_miningBase GetMining_miningBase(int key)
		{
			return this.Mining_miningBaseModelInstance.GetElementById(key);
		}

		public Mount_advanceMountModel GetMount_advanceMountModelInstance()
		{
			return this.Mount_advanceMountModelInstance;
		}

		public IList<Mount_advanceMount> GetMount_advanceMountElements()
		{
			return this.Mount_advanceMountModelInstance.GetAllElements();
		}

		public Mount_advanceMount GetMount_advanceMount(int key)
		{
			return this.Mount_advanceMountModelInstance.GetElementById(key);
		}

		public TalentNew_talentModel GetTalentNew_talentModelInstance()
		{
			return this.TalentNew_talentModelInstance;
		}

		public IList<TalentNew_talent> GetTalentNew_talentElements()
		{
			return this.TalentNew_talentModelInstance.GetAllElements();
		}

		public TalentNew_talent GetTalentNew_talent(int key)
		{
			return this.TalentNew_talentModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_slotRewardModel GetChapterMiniGame_slotRewardModelInstance()
		{
			return this.ChapterMiniGame_slotRewardModelInstance;
		}

		public IList<ChapterMiniGame_slotReward> GetChapterMiniGame_slotRewardElements()
		{
			return this.ChapterMiniGame_slotRewardModelInstance.GetAllElements();
		}

		public ChapterMiniGame_slotReward GetChapterMiniGame_slotReward(int key)
		{
			return this.ChapterMiniGame_slotRewardModelInstance.GetElementById(key);
		}

		public Avatar_SkinModel GetAvatar_SkinModelInstance()
		{
			return this.Avatar_SkinModelInstance;
		}

		public IList<Avatar_Skin> GetAvatar_SkinElements()
		{
			return this.Avatar_SkinModelInstance.GetAllElements();
		}

		public Avatar_Skin GetAvatar_Skin(int key)
		{
			return this.Avatar_SkinModelInstance.GetElementById(key);
		}

		public MonsterCfg_monsterCfgModel GetMonsterCfg_monsterCfgModelInstance()
		{
			return this.MonsterCfg_monsterCfgModelInstance;
		}

		public IList<MonsterCfg_monsterCfg> GetMonsterCfg_monsterCfgElements()
		{
			return this.MonsterCfg_monsterCfgModelInstance.GetAllElements();
		}

		public MonsterCfg_monsterCfg GetMonsterCfg_monsterCfg(int key)
		{
			return this.MonsterCfg_monsterCfgModelInstance.GetElementById(key);
		}

		public Shop_SummonModel GetShop_SummonModelInstance()
		{
			return this.Shop_SummonModelInstance;
		}

		public IList<Shop_Summon> GetShop_SummonElements()
		{
			return this.Shop_SummonModelInstance.GetAllElements();
		}

		public Shop_Summon GetShop_Summon(int key)
		{
			return this.Shop_SummonModelInstance.GetElementById(key);
		}

		public ChainPacks_ChainTypeModel GetChainPacks_ChainTypeModelInstance()
		{
			return this.ChainPacks_ChainTypeModelInstance;
		}

		public IList<ChainPacks_ChainType> GetChainPacks_ChainTypeElements()
		{
			return this.ChainPacks_ChainTypeModelInstance.GetAllElements();
		}

		public ChainPacks_ChainType GetChainPacks_ChainType(int key)
		{
			return this.ChainPacks_ChainTypeModelInstance.GetElementById(key);
		}

		public Guild_guildLevelModel GetGuild_guildLevelModelInstance()
		{
			return this.Guild_guildLevelModelInstance;
		}

		public IList<Guild_guildLevel> GetGuild_guildLevelElements()
		{
			return this.Guild_guildLevelModelInstance.GetAllElements();
		}

		public Guild_guildLevel GetGuild_guildLevel(int key)
		{
			return this.Guild_guildLevelModelInstance.GetElementById(key);
		}

		public Chapter_chapterModel GetChapter_chapterModelInstance()
		{
			return this.Chapter_chapterModelInstance;
		}

		public IList<Chapter_chapter> GetChapter_chapterElements()
		{
			return this.Chapter_chapterModelInstance.GetAllElements();
		}

		public Chapter_chapter GetChapter_chapter(int key)
		{
			return this.Chapter_chapterModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaRobotNameModel GetCrossArena_CrossArenaRobotNameModelInstance()
		{
			return this.CrossArena_CrossArenaRobotNameModelInstance;
		}

		public IList<CrossArena_CrossArenaRobotName> GetCrossArena_CrossArenaRobotNameElements()
		{
			return this.CrossArena_CrossArenaRobotNameModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaRobotName GetCrossArena_CrossArenaRobotName(int key)
		{
			return this.CrossArena_CrossArenaRobotNameModelInstance.GetElementById(key);
		}

		public Fishing_fishModel GetFishing_fishModelInstance()
		{
			return this.Fishing_fishModelInstance;
		}

		public IList<Fishing_fish> GetFishing_fishElements()
		{
			return this.Fishing_fishModelInstance.GetAllElements();
		}

		public Fishing_fish GetFishing_fish(int key)
		{
			return this.Fishing_fishModelInstance.GetElementById(key);
		}

		public Item_dropModel GetItem_dropModelInstance()
		{
			return this.Item_dropModelInstance;
		}

		public IList<Item_drop> GetItem_dropElements()
		{
			return this.Item_dropModelInstance.GetAllElements();
		}

		public Item_drop GetItem_drop(int key)
		{
			return this.Item_dropModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_slotBaseModel GetChapterMiniGame_slotBaseModelInstance()
		{
			return this.ChapterMiniGame_slotBaseModelInstance;
		}

		public IList<ChapterMiniGame_slotBase> GetChapterMiniGame_slotBaseElements()
		{
			return this.ChapterMiniGame_slotBaseModelInstance.GetAllElements();
		}

		public ChapterMiniGame_slotBase GetChapterMiniGame_slotBase(int key)
		{
			return this.ChapterMiniGame_slotBaseModelInstance.GetElementById(key);
		}

		public ServerList_chatGropModel GetServerList_chatGropModelInstance()
		{
			return this.ServerList_chatGropModelInstance;
		}

		public IList<ServerList_chatGrop> GetServerList_chatGropElements()
		{
			return this.ServerList_chatGropModelInstance.GetAllElements();
		}

		public ServerList_chatGrop GetServerList_chatGrop(int key)
		{
			return this.ServerList_chatGropModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_cardFlippingBaseModel GetChapterMiniGame_cardFlippingBaseModelInstance()
		{
			return this.ChapterMiniGame_cardFlippingBaseModelInstance;
		}

		public IList<ChapterMiniGame_cardFlippingBase> GetChapterMiniGame_cardFlippingBaseElements()
		{
			return this.ChapterMiniGame_cardFlippingBaseModelInstance.GetAllElements();
		}

		public ChapterMiniGame_cardFlippingBase GetChapterMiniGame_cardFlippingBase(int key)
		{
			return this.ChapterMiniGame_cardFlippingBaseModelInstance.GetElementById(key);
		}

		public Box_outputModel GetBox_outputModelInstance()
		{
			return this.Box_outputModelInstance;
		}

		public IList<Box_output> GetBox_outputElements()
		{
			return this.Box_outputModelInstance.GetAllElements();
		}

		public Box_output GetBox_output(int key)
		{
			return this.Box_outputModelInstance.GetElementById(key);
		}

		public Equip_equipComposeModel GetEquip_equipComposeModelInstance()
		{
			return this.Equip_equipComposeModelInstance;
		}

		public IList<Equip_equipCompose> GetEquip_equipComposeElements()
		{
			return this.Equip_equipComposeModelInstance.GetAllElements();
		}

		public Equip_equipCompose GetEquip_equipCompose(int key)
		{
			return this.Equip_equipComposeModelInstance.GetElementById(key);
		}

		public Quality_collectionQualityModel GetQuality_collectionQualityModelInstance()
		{
			return this.Quality_collectionQualityModelInstance;
		}

		public IList<Quality_collectionQuality> GetQuality_collectionQualityElements()
		{
			return this.Quality_collectionQualityModelInstance.GetAllElements();
		}

		public Quality_collectionQuality GetQuality_collectionQuality(int key)
		{
			return this.Quality_collectionQualityModelInstance.GetElementById(key);
		}

		public Guild_guildLanguageModel GetGuild_guildLanguageModelInstance()
		{
			return this.Guild_guildLanguageModelInstance;
		}

		public IList<Guild_guildLanguage> GetGuild_guildLanguageElements()
		{
			return this.Guild_guildLanguageModelInstance.GetAllElements();
		}

		public Guild_guildLanguage GetGuild_guildLanguage(int key)
		{
			return this.Guild_guildLanguageModelInstance.GetElementById(key);
		}

		public GuildBOSS_guildBossSeasonRewardModel GetGuildBOSS_guildBossSeasonRewardModelInstance()
		{
			return this.GuildBOSS_guildBossSeasonRewardModelInstance;
		}

		public IList<GuildBOSS_guildBossSeasonReward> GetGuildBOSS_guildBossSeasonRewardElements()
		{
			return this.GuildBOSS_guildBossSeasonRewardModelInstance.GetAllElements();
		}

		public GuildBOSS_guildBossSeasonReward GetGuildBOSS_guildBossSeasonReward(int key)
		{
			return this.GuildBOSS_guildBossSeasonRewardModelInstance.GetElementById(key);
		}

		public Mining_bonusGameModel GetMining_bonusGameModelInstance()
		{
			return this.Mining_bonusGameModelInstance;
		}

		public IList<Mining_bonusGame> GetMining_bonusGameElements()
		{
			return this.Mining_bonusGameModelInstance.GetAllElements();
		}

		public Mining_bonusGame GetMining_bonusGame(int key)
		{
			return this.Mining_bonusGameModelInstance.GetElementById(key);
		}

		public ChapterMiniGame_singleSlotModel GetChapterMiniGame_singleSlotModelInstance()
		{
			return this.ChapterMiniGame_singleSlotModelInstance;
		}

		public IList<ChapterMiniGame_singleSlot> GetChapterMiniGame_singleSlotElements()
		{
			return this.ChapterMiniGame_singleSlotModelInstance.GetAllElements();
		}

		public ChapterMiniGame_singleSlot GetChapterMiniGame_singleSlot(int key)
		{
			return this.ChapterMiniGame_singleSlotModelInstance.GetElementById(key);
		}

		public CrossArena_CrossArenaRewardModel GetCrossArena_CrossArenaRewardModelInstance()
		{
			return this.CrossArena_CrossArenaRewardModelInstance;
		}

		public IList<CrossArena_CrossArenaReward> GetCrossArena_CrossArenaRewardElements()
		{
			return this.CrossArena_CrossArenaRewardModelInstance.GetAllElements();
		}

		public CrossArena_CrossArenaReward GetCrossArena_CrossArenaReward(int key)
		{
			return this.CrossArena_CrossArenaRewardModelInstance.GetElementById(key);
		}

		public Pet_petSkillModel GetPet_petSkillModelInstance()
		{
			return this.Pet_petSkillModelInstance;
		}

		public IList<Pet_petSkill> GetPet_petSkillElements()
		{
			return this.Pet_petSkillModelInstance.GetAllElements();
		}

		public Pet_petSkill GetPet_petSkill(int key)
		{
			return this.Pet_petSkillModelInstance.GetElementById(key);
		}

		public ChapterActivity_ActvTurntableBaseModel GetChapterActivity_ActvTurntableBaseModelInstance()
		{
			return this.ChapterActivity_ActvTurntableBaseModelInstance;
		}

		public IList<ChapterActivity_ActvTurntableBase> GetChapterActivity_ActvTurntableBaseElements()
		{
			return this.ChapterActivity_ActvTurntableBaseModelInstance.GetAllElements();
		}

		public ChapterActivity_ActvTurntableBase GetChapterActivity_ActvTurntableBase(int key)
		{
			return this.ChapterActivity_ActvTurntableBaseModelInstance.GetElementById(key);
		}

		public Item_battleModel GetItem_battleModelInstance()
		{
			return this.Item_battleModelInstance;
		}

		public IList<Item_battle> GetItem_battleElements()
		{
			return this.Item_battleModelInstance.GetAllElements();
		}

		public Item_battle GetItem_battle(int key)
		{
			return this.Item_battleModelInstance.GetElementById(key);
		}

		public Mount_mountLevelModel GetMount_mountLevelModelInstance()
		{
			return this.Mount_mountLevelModelInstance;
		}

		public IList<Mount_mountLevel> GetMount_mountLevelElements()
		{
			return this.Mount_mountLevelModelInstance.GetAllElements();
		}

		public Mount_mountLevel GetMount_mountLevel(int key)
		{
			return this.Mount_mountLevelModelInstance.GetElementById(key);
		}

		public Equip_equipSkillModel GetEquip_equipSkillModelInstance()
		{
			return this.Equip_equipSkillModelInstance;
		}

		public IList<Equip_equipSkill> GetEquip_equipSkillElements()
		{
			return this.Equip_equipSkillModelInstance.GetAllElements();
		}

		public Equip_equipSkill GetEquip_equipSkill(int key)
		{
			return this.Equip_equipSkillModelInstance.GetElementById(key);
		}

		public Equip_equipEvolutionModel GetEquip_equipEvolutionModelInstance()
		{
			return this.Equip_equipEvolutionModelInstance;
		}

		public IList<Equip_equipEvolution> GetEquip_equipEvolutionElements()
		{
			return this.Equip_equipEvolutionModelInstance.GetAllElements();
		}

		public Equip_equipEvolution GetEquip_equipEvolution(int key)
		{
			return this.Equip_equipEvolutionModelInstance.GetElementById(key);
		}

		public Quality_itemQualityModel GetQuality_itemQualityModelInstance()
		{
			return this.Quality_itemQualityModelInstance;
		}

		public IList<Quality_itemQuality> GetQuality_itemQualityElements()
		{
			return this.Quality_itemQualityModelInstance.GetAllElements();
		}

		public Quality_itemQuality GetQuality_itemQuality(int key)
		{
			return this.Quality_itemQualityModelInstance.GetElementById(key);
		}

		public Collection_starColorModel GetCollection_starColorModelInstance()
		{
			return this.Collection_starColorModelInstance;
		}

		public IList<Collection_starColor> GetCollection_starColorElements()
		{
			return this.Collection_starColorModelInstance.GetAllElements();
		}

		public Collection_starColor GetCollection_starColor(int key)
		{
			return this.Collection_starColorModelInstance.GetElementById(key);
		}

		public Fishing_areaModel GetFishing_areaModelInstance()
		{
			return this.Fishing_areaModelInstance;
		}

		public IList<Fishing_area> GetFishing_areaElements()
		{
			return this.Fishing_areaModelInstance.GetAllElements();
		}

		public Fishing_area GetFishing_area(int key)
		{
			return this.Fishing_areaModelInstance.GetElementById(key);
		}

		public ChapterActivity_RankRewardModel GetChapterActivity_RankRewardModelInstance()
		{
			return this.ChapterActivity_RankRewardModelInstance;
		}

		public IList<ChapterActivity_RankReward> GetChapterActivity_RankRewardElements()
		{
			return this.ChapterActivity_RankRewardModelInstance.GetAllElements();
		}

		public ChapterActivity_RankReward GetChapterActivity_RankReward(int key)
		{
			return this.ChapterActivity_RankRewardModelInstance.GetElementById(key);
		}

		public Map_EventPointBottomModel GetMap_EventPointBottomModelInstance()
		{
			return this.Map_EventPointBottomModelInstance;
		}

		public IList<Map_EventPointBottom> GetMap_EventPointBottomElements()
		{
			return this.Map_EventPointBottomModelInstance.GetAllElements();
		}

		public Map_EventPointBottom GetMap_EventPointBottom(int key)
		{
			return this.Map_EventPointBottomModelInstance.GetElementById(key);
		}

		public Equip_updateLevelModel GetEquip_updateLevelModelInstance()
		{
			return this.Equip_updateLevelModelInstance;
		}

		public IList<Equip_updateLevel> GetEquip_updateLevelElements()
		{
			return this.Equip_updateLevelModelInstance.GetAllElements();
		}

		public Equip_updateLevel GetEquip_updateLevel(int key)
		{
			return this.Equip_updateLevelModelInstance.GetElementById(key);
		}

		public IAP_LevelFundModel GetIAP_LevelFundModelInstance()
		{
			return this.IAP_LevelFundModelInstance;
		}

		public IList<IAP_LevelFund> GetIAP_LevelFundElements()
		{
			return this.IAP_LevelFundModelInstance.GetAllElements();
		}

		public IAP_LevelFund GetIAP_LevelFund(int key)
		{
			return this.IAP_LevelFundModelInstance.GetElementById(key);
		}

		public SignIn_SignInModel GetSignIn_SignInModelInstance()
		{
			return this.SignIn_SignInModelInstance;
		}

		public IList<SignIn_SignIn> GetSignIn_SignInElements()
		{
			return this.SignIn_SignInModelInstance.GetAllElements();
		}

		public SignIn_SignIn GetSignIn_SignIn(int key)
		{
			return this.SignIn_SignInModelInstance.GetElementById(key);
		}

		public Mount_mountStageModel GetMount_mountStageModelInstance()
		{
			return this.Mount_mountStageModelInstance;
		}

		public IList<Mount_mountStage> GetMount_mountStageElements()
		{
			return this.Mount_mountStageModelInstance.GetAllElements();
		}

		public Mount_mountStage GetMount_mountStage(int key)
		{
			return this.Mount_mountStageModelInstance.GetElementById(key);
		}

		public Relic_groupModel GetRelic_groupModelInstance()
		{
			return this.Relic_groupModelInstance;
		}

		public IList<Relic_group> GetRelic_groupElements()
		{
			return this.Relic_groupModelInstance.GetAllElements();
		}

		public Relic_group GetRelic_group(int key)
		{
			return this.Relic_groupModelInstance.GetElementById(key);
		}

		public ChestList_ChestListModel GetChestList_ChestListModelInstance()
		{
			return this.ChestList_ChestListModelInstance;
		}

		public IList<ChestList_ChestList> GetChestList_ChestListElements()
		{
			return this.ChestList_ChestListModelInstance.GetAllElements();
		}

		public ChestList_ChestList GetChestList_ChestList(int key)
		{
			return this.ChestList_ChestListModelInstance.GetElementById(key);
		}

		public GameConfig_ConfigModel GetGameConfig_ConfigModelInstance()
		{
			return this.GameConfig_ConfigModelInstance;
		}

		public IList<GameConfig_Config> GetGameConfig_ConfigElements()
		{
			return this.GameConfig_ConfigModelInstance.GetAllElements();
		}

		public GameConfig_Config GetGameConfig_Config(int key)
		{
			return this.GameConfig_ConfigModelInstance.GetElementById(key);
		}

		public GameSkillBuild_skillTagModel GetGameSkillBuild_skillTagModelInstance()
		{
			return this.GameSkillBuild_skillTagModelInstance;
		}

		public IList<GameSkillBuild_skillTag> GetGameSkillBuild_skillTagElements()
		{
			return this.GameSkillBuild_skillTagModelInstance.GetAllElements();
		}

		public GameSkillBuild_skillTag GetGameSkillBuild_skillTag(int key)
		{
			return this.GameSkillBuild_skillTagModelInstance.GetElementById(key);
		}

		public GameSkill_fireBulletModel GetGameSkill_fireBulletModelInstance()
		{
			return this.GameSkill_fireBulletModelInstance;
		}

		public IList<GameSkill_fireBullet> GetGameSkill_fireBulletElements()
		{
			return this.GameSkill_fireBulletModelInstance.GetAllElements();
		}

		public GameSkill_fireBullet GetGameSkill_fireBullet(int key)
		{
			return this.GameSkill_fireBulletModelInstance.GetElementById(key);
		}

		public IAP_PushPacksModel GetIAP_PushPacksModelInstance()
		{
			return this.IAP_PushPacksModelInstance;
		}

		public IList<IAP_PushPacks> GetIAP_PushPacksElements()
		{
			return this.IAP_PushPacksModelInstance.GetAllElements();
		}

		public IAP_PushPacks GetIAP_PushPacks(int key)
		{
			return this.IAP_PushPacksModelInstance.GetElementById(key);
		}

		public Scene_tableModel GetScene_tableModelInstance()
		{
			return this.Scene_tableModelInstance;
		}

		public IList<Scene_table> GetScene_tableElements()
		{
			return this.Scene_tableModelInstance.GetAllElements();
		}

		public Scene_table GetScene_table(int key)
		{
			return this.Scene_tableModelInstance.GetElementById(key);
		}

		public GameSkillBuild_firstModel GetGameSkillBuild_firstModelInstance()
		{
			return this.GameSkillBuild_firstModelInstance;
		}

		public IList<GameSkillBuild_first> GetGameSkillBuild_firstElements()
		{
			return this.GameSkillBuild_firstModelInstance.GetAllElements();
		}

		public GameSkillBuild_first GetGameSkillBuild_first(int key)
		{
			return this.GameSkillBuild_firstModelInstance.GetElementById(key);
		}

		public Levels_tableModel GetLevels_tableModelInstance()
		{
			return this.Levels_tableModelInstance;
		}

		public IList<Levels_table> GetLevels_tableElements()
		{
			return this.Levels_tableModelInstance.GetAllElements();
		}

		public Levels_table GetLevels_table(int key)
		{
			return this.Levels_tableModelInstance.GetElementById(key);
		}

		public GameBullet_bulletModel GetGameBullet_bulletModelInstance()
		{
			return this.GameBullet_bulletModelInstance;
		}

		public IList<GameBullet_bullet> GetGameBullet_bulletElements()
		{
			return this.GameBullet_bulletModelInstance.GetAllElements();
		}

		public GameBullet_bullet GetGameBullet_bullet(int key)
		{
			return this.GameBullet_bulletModelInstance.GetElementById(key);
		}

		public GameCamera_ShakeModel GetGameCamera_ShakeModelInstance()
		{
			return this.GameCamera_ShakeModelInstance;
		}

		public IList<GameCamera_Shake> GetGameCamera_ShakeElements()
		{
			return this.GameCamera_ShakeModelInstance.GetAllElements();
		}

		public GameCamera_Shake GetGameCamera_Shake(int key)
		{
			return this.GameCamera_ShakeModelInstance.GetElementById(key);
		}

		public HeroLevelup_HeroLevelupModel GetHeroLevelup_HeroLevelupModelInstance()
		{
			return this.HeroLevelup_HeroLevelupModelInstance;
		}

		public IList<HeroLevelup_HeroLevelup> GetHeroLevelup_HeroLevelupElements()
		{
			return this.HeroLevelup_HeroLevelupModelInstance.GetAllElements();
		}

		public HeroLevelup_HeroLevelup GetHeroLevelup_HeroLevelup(int key)
		{
			return this.HeroLevelup_HeroLevelupModelInstance.GetElementById(key);
		}

		public TGASource_CostModel GetTGASource_CostModelInstance()
		{
			return this.TGASource_CostModelInstance;
		}

		public IList<TGASource_Cost> GetTGASource_CostElements()
		{
			return this.TGASource_CostModelInstance.GetAllElements();
		}

		public TGASource_Cost GetTGASource_Cost(int key)
		{
			return this.TGASource_CostModelInstance.GetElementById(key);
		}

		public Guild_guildTaskModel GetGuild_guildTaskModelInstance()
		{
			return this.Guild_guildTaskModelInstance;
		}

		public IList<Guild_guildTask> GetGuild_guildTaskElements()
		{
			return this.Guild_guildTaskModelInstance.GetAllElements();
		}

		public Guild_guildTask GetGuild_guildTask(int key)
		{
			return this.Guild_guildTaskModelInstance.GetElementById(key);
		}

		public override void InitialiseLocalModels()
		{
			base.InitialiseLocalModels();
			this.m_localModels.Clear();
			this.m_ids.Clear();
			this.Guide_guideModelInstance = new Guide_guideModel();
			this.m_localModels[Guide_guideModel.fileName] = this.Guide_guideModelInstance;
			this.m_ids[0] = this.Guide_guideModelInstance;
			this.GameSkill_skillModelInstance = new GameSkill_skillModel();
			this.m_localModels[GameSkill_skillModel.fileName] = this.GameSkill_skillModelInstance;
			this.m_ids[1] = this.GameSkill_skillModelInstance;
			this.ActivityTurntable_TurntableRewardModelInstance = new ActivityTurntable_TurntableRewardModel();
			this.m_localModels[ActivityTurntable_TurntableRewardModel.fileName] = this.ActivityTurntable_TurntableRewardModelInstance;
			this.m_ids[2] = this.ActivityTurntable_TurntableRewardModelInstance;
			this.AutoProcess_monsterModelInstance = new AutoProcess_monsterModel();
			this.m_localModels[AutoProcess_monsterModel.fileName] = this.AutoProcess_monsterModelInstance;
			this.m_ids[3] = this.AutoProcess_monsterModelInstance;
			this.ArtSkin_equipSkinModelInstance = new ArtSkin_equipSkinModel();
			this.m_localModels[ArtSkin_equipSkinModel.fileName] = this.ArtSkin_equipSkinModelInstance;
			this.m_ids[4] = this.ArtSkin_equipSkinModelInstance;
			this.ArtMember_clothesModelInstance = new ArtMember_clothesModel();
			this.m_localModels[ArtMember_clothesModel.fileName] = this.ArtMember_clothesModelInstance;
			this.m_ids[5] = this.ArtMember_clothesModelInstance;
			this.Artifact_advanceArtifactModelInstance = new Artifact_advanceArtifactModel();
			this.m_localModels[Artifact_advanceArtifactModel.fileName] = this.Artifact_advanceArtifactModelInstance;
			this.m_ids[6] = this.Artifact_advanceArtifactModelInstance;
			this.GameMember_gameHoverModelInstance = new GameMember_gameHoverModel();
			this.m_localModels[GameMember_gameHoverModel.fileName] = this.GameMember_gameHoverModelInstance;
			this.m_ids[7] = this.GameMember_gameHoverModelInstance;
			this.Vip_vipModelInstance = new Vip_vipModel();
			this.m_localModels[Vip_vipModel.fileName] = this.Vip_vipModelInstance;
			this.m_ids[8] = this.Vip_vipModelInstance;
			this.Equip_equipModelInstance = new Equip_equipModel();
			this.m_localModels[Equip_equipModel.fileName] = this.Equip_equipModelInstance;
			this.m_ids[9] = this.Equip_equipModelInstance;
			this.IAP_GiftPacksModelInstance = new IAP_GiftPacksModel();
			this.m_localModels[IAP_GiftPacksModel.fileName] = this.IAP_GiftPacksModelInstance;
			this.m_ids[10] = this.IAP_GiftPacksModelInstance;
			this.Shop_ShopSellModelInstance = new Shop_ShopSellModel();
			this.m_localModels[Shop_ShopSellModel.fileName] = this.Shop_ShopSellModelInstance;
			this.m_ids[11] = this.Shop_ShopSellModelInstance;
			this.Language_languagetableModelInstance = new Language_languagetableModel();
			this.m_localModels[Language_languagetableModel.fileName] = this.Language_languagetableModelInstance;
			this.m_ids[12] = this.Language_languagetableModelInstance;
			this.TGASource_GetModelInstance = new TGASource_GetModel();
			this.m_localModels[TGASource_GetModel.fileName] = this.TGASource_GetModelInstance;
			this.m_ids[13] = this.TGASource_GetModelInstance;
			this.GuildBOSS_guildBossMonsterModelInstance = new GuildBOSS_guildBossMonsterModel();
			this.m_localModels[GuildBOSS_guildBossMonsterModel.fileName] = this.GuildBOSS_guildBossMonsterModelInstance;
			this.m_ids[14] = this.GuildBOSS_guildBossMonsterModelInstance;
			this.Module_moduleInfoModelInstance = new Module_moduleInfoModel();
			this.m_localModels[Module_moduleInfoModel.fileName] = this.Module_moduleInfoModelInstance;
			this.m_ids[15] = this.Module_moduleInfoModelInstance;
			this.GuildRace_levelModelInstance = new GuildRace_levelModel();
			this.m_localModels[GuildRace_levelModel.fileName] = this.GuildRace_levelModelInstance;
			this.m_ids[16] = this.GuildRace_levelModelInstance;
			this.ChapterActivity_ChapterObjModelInstance = new ChapterActivity_ChapterObjModel();
			this.m_localModels[ChapterActivity_ChapterObjModel.fileName] = this.ChapterActivity_ChapterObjModelInstance;
			this.m_ids[17] = this.ChapterActivity_ChapterObjModelInstance;
			this.ArtBullet_BulletModelInstance = new ArtBullet_BulletModel();
			this.m_localModels[ArtBullet_BulletModel.fileName] = this.ArtBullet_BulletModelInstance;
			this.m_ids[18] = this.ArtBullet_BulletModelInstance;
			this.Collection_collectionModelInstance = new Collection_collectionModel();
			this.m_localModels[Collection_collectionModel.fileName] = this.Collection_collectionModelInstance;
			this.m_ids[19] = this.Collection_collectionModelInstance;
			this.Task_WeeklyActiveModelInstance = new Task_WeeklyActiveModel();
			this.m_localModels[Task_WeeklyActiveModel.fileName] = this.Task_WeeklyActiveModelInstance;
			this.m_ids[20] = this.Task_WeeklyActiveModelInstance;
			this.GameBullet_bulletTypeModelInstance = new GameBullet_bulletTypeModel();
			this.m_localModels[GameBullet_bulletTypeModel.fileName] = this.GameBullet_bulletTypeModelInstance;
			this.m_ids[21] = this.GameBullet_bulletTypeModelInstance;
			this.Atlas_atlasModelInstance = new Atlas_atlasModel();
			this.m_localModels[Atlas_atlasModel.fileName] = this.Atlas_atlasModelInstance;
			this.m_ids[22] = this.Atlas_atlasModelInstance;
			this.Dungeon_DungeonBaseModelInstance = new Dungeon_DungeonBaseModel();
			this.m_localModels[Dungeon_DungeonBaseModel.fileName] = this.Dungeon_DungeonBaseModelInstance;
			this.m_ids[23] = this.Dungeon_DungeonBaseModelInstance;
			this.Achievements_AchievementsModelInstance = new Achievements_AchievementsModel();
			this.m_localModels[Achievements_AchievementsModel.fileName] = this.Achievements_AchievementsModelInstance;
			this.m_ids[24] = this.Achievements_AchievementsModelInstance;
			this.Mining_oreBuildModelInstance = new Mining_oreBuildModel();
			this.m_localModels[Mining_oreBuildModel.fileName] = this.Mining_oreBuildModelInstance;
			this.m_ids[25] = this.Mining_oreBuildModelInstance;
			this.RogueDungeon_endEventModelInstance = new RogueDungeon_endEventModel();
			this.m_localModels[RogueDungeon_endEventModel.fileName] = this.RogueDungeon_endEventModelInstance;
			this.m_ids[26] = this.RogueDungeon_endEventModelInstance;
			this.GameMember_memberModelInstance = new GameMember_memberModel();
			this.m_localModels[GameMember_memberModel.fileName] = this.GameMember_memberModelInstance;
			this.m_ids[27] = this.GameMember_memberModelInstance;
			this.Equip_equipTypeModelInstance = new Equip_equipTypeModel();
			this.m_localModels[Equip_equipTypeModel.fileName] = this.Equip_equipTypeModelInstance;
			this.m_ids[28] = this.Equip_equipTypeModelInstance;
			this.ArtBuff_BuffModelInstance = new ArtBuff_BuffModel();
			this.m_localModels[ArtBuff_BuffModel.fileName] = this.ArtBuff_BuffModelInstance;
			this.m_ids[29] = this.ArtBuff_BuffModelInstance;
			this.GuildBOSS_guildBossBoxModelInstance = new GuildBOSS_guildBossBoxModel();
			this.m_localModels[GuildBOSS_guildBossBoxModel.fileName] = this.GuildBOSS_guildBossBoxModelInstance;
			this.m_ids[30] = this.GuildBOSS_guildBossBoxModelInstance;
			this.ChapterMiniGame_paySlotRewardModelInstance = new ChapterMiniGame_paySlotRewardModel();
			this.m_localModels[ChapterMiniGame_paySlotRewardModel.fileName] = this.ChapterMiniGame_paySlotRewardModelInstance;
			this.m_ids[31] = this.ChapterMiniGame_paySlotRewardModelInstance;
			this.SevenDay_SevenDayActiveRewardModelInstance = new SevenDay_SevenDayActiveRewardModel();
			this.m_localModels[SevenDay_SevenDayActiveRewardModel.fileName] = this.SevenDay_SevenDayActiveRewardModelInstance;
			this.m_ids[32] = this.SevenDay_SevenDayActiveRewardModelInstance;
			this.Vip_dataModelInstance = new Vip_dataModel();
			this.m_localModels[Vip_dataModel.fileName] = this.Vip_dataModelInstance;
			this.m_ids[33] = this.Vip_dataModelInstance;
			this.CrossArena_CrossArenaChallengeListRuleModelInstance = new CrossArena_CrossArenaChallengeListRuleModel();
			this.m_localModels[CrossArena_CrossArenaChallengeListRuleModel.fileName] = this.CrossArena_CrossArenaChallengeListRuleModelInstance;
			this.m_ids[34] = this.CrossArena_CrossArenaChallengeListRuleModelInstance;
			this.TowerChallenge_TowerLevelModelInstance = new TowerChallenge_TowerLevelModel();
			this.m_localModels[TowerChallenge_TowerLevelModel.fileName] = this.TowerChallenge_TowerLevelModelInstance;
			this.m_ids[35] = this.TowerChallenge_TowerLevelModelInstance;
			this.WorldBoss_RankRewardModelInstance = new WorldBoss_RankRewardModel();
			this.m_localModels[WorldBoss_RankRewardModel.fileName] = this.WorldBoss_RankRewardModelInstance;
			this.m_ids[36] = this.WorldBoss_RankRewardModelInstance;
			this.ChainPacks_PushChainActvModelInstance = new ChainPacks_PushChainActvModel();
			this.m_localModels[ChainPacks_PushChainActvModel.fileName] = this.ChainPacks_PushChainActvModelInstance;
			this.m_ids[37] = this.ChainPacks_PushChainActvModelInstance;
			this.SevenDay_SevenDayPayModelInstance = new SevenDay_SevenDayPayModel();
			this.m_localModels[SevenDay_SevenDayPayModel.fileName] = this.SevenDay_SevenDayPayModelInstance;
			this.m_ids[38] = this.SevenDay_SevenDayPayModelInstance;
			this.Sociality_ReportModelInstance = new Sociality_ReportModel();
			this.m_localModels[Sociality_ReportModel.fileName] = this.Sociality_ReportModelInstance;
			this.m_ids[39] = this.Sociality_ReportModelInstance;
			this.AutoProcess_chapterModelInstance = new AutoProcess_chapterModel();
			this.m_localModels[AutoProcess_chapterModel.fileName] = this.AutoProcess_chapterModelInstance;
			this.m_ids[40] = this.AutoProcess_chapterModelInstance;
			this.Attribute_AttrTextModelInstance = new Attribute_AttrTextModel();
			this.m_localModels[Attribute_AttrTextModel.fileName] = this.Attribute_AttrTextModelInstance;
			this.m_ids[41] = this.Attribute_AttrTextModelInstance;
			this.GameSkillBuild_skillBuildModelInstance = new GameSkillBuild_skillBuildModel();
			this.m_localModels[GameSkillBuild_skillBuildModel.fileName] = this.GameSkillBuild_skillBuildModelInstance;
			this.m_ids[42] = this.GameSkillBuild_skillBuildModelInstance;
			this.Shop_ShopActivityModelInstance = new Shop_ShopActivityModel();
			this.m_localModels[Shop_ShopActivityModel.fileName] = this.Shop_ShopActivityModelInstance;
			this.m_ids[43] = this.Shop_ShopActivityModelInstance;
			this.IAP_BattlePassRewardModelInstance = new IAP_BattlePassRewardModel();
			this.m_localModels[IAP_BattlePassRewardModel.fileName] = this.IAP_BattlePassRewardModelInstance;
			this.m_ids[44] = this.IAP_BattlePassRewardModelInstance;
			this.ServerList_serverGropModelInstance = new ServerList_serverGropModel();
			this.m_localModels[ServerList_serverGropModel.fileName] = this.ServerList_serverGropModelInstance;
			this.m_ids[45] = this.ServerList_serverGropModelInstance;
			this.ChapterMiniGame_singleSlotRewardModelInstance = new ChapterMiniGame_singleSlotRewardModel();
			this.m_localModels[ChapterMiniGame_singleSlotRewardModel.fileName] = this.ChapterMiniGame_singleSlotRewardModelInstance;
			this.m_ids[46] = this.ChapterMiniGame_singleSlotRewardModelInstance;
			this.CommonActivity_ConsumeObjModelInstance = new CommonActivity_ConsumeObjModel();
			this.m_localModels[CommonActivity_ConsumeObjModel.fileName] = this.CommonActivity_ConsumeObjModelInstance;
			this.m_ids[47] = this.CommonActivity_ConsumeObjModelInstance;
			this.Mining_showRateModelInstance = new Mining_showRateModel();
			this.m_localModels[Mining_showRateModel.fileName] = this.Mining_showRateModelInstance;
			this.m_ids[48] = this.Mining_showRateModelInstance;
			this.ArtMap_MapModelInstance = new ArtMap_MapModel();
			this.m_localModels[ArtMap_MapModel.fileName] = this.ArtMap_MapModelInstance;
			this.m_ids[49] = this.ArtMap_MapModelInstance;
			this.Chapter_stageUpgradeModelInstance = new Chapter_stageUpgradeModel();
			this.m_localModels[Chapter_stageUpgradeModel.fileName] = this.Chapter_stageUpgradeModelInstance;
			this.m_ids[50] = this.Chapter_stageUpgradeModelInstance;
			this.Chapter_surpriseBuildModelInstance = new Chapter_surpriseBuildModel();
			this.m_localModels[Chapter_surpriseBuildModel.fileName] = this.Chapter_surpriseBuildModelInstance;
			this.m_ids[51] = this.Chapter_surpriseBuildModelInstance;
			this.GuildBOSS_guildBossTaskModelInstance = new GuildBOSS_guildBossTaskModel();
			this.m_localModels[GuildBOSS_guildBossTaskModel.fileName] = this.GuildBOSS_guildBossTaskModelInstance;
			this.m_ids[52] = this.GuildBOSS_guildBossTaskModelInstance;
			this.Guild_guildGiftLevelModelInstance = new Guild_guildGiftLevelModel();
			this.m_localModels[Guild_guildGiftLevelModel.fileName] = this.Guild_guildGiftLevelModelInstance;
			this.m_ids[53] = this.Guild_guildGiftLevelModelInstance;
			this.IntegralShop_dataModelInstance = new IntegralShop_dataModel();
			this.m_localModels[IntegralShop_dataModel.fileName] = this.IntegralShop_dataModelInstance;
			this.m_ids[54] = this.IntegralShop_dataModelInstance;
			this.TalentLegacy_talentLegacyNodeModelInstance = new TalentLegacy_talentLegacyNodeModel();
			this.m_localModels[TalentLegacy_talentLegacyNodeModel.fileName] = this.TalentLegacy_talentLegacyNodeModelInstance;
			this.m_ids[55] = this.TalentLegacy_talentLegacyNodeModelInstance;
			this.Shop_AdModelInstance = new Shop_AdModel();
			this.m_localModels[Shop_AdModel.fileName] = this.Shop_AdModelInstance;
			this.m_ids[56] = this.Shop_AdModelInstance;
			this.Pet_PetEntryModelInstance = new Pet_PetEntryModel();
			this.m_localModels[Pet_PetEntryModel.fileName] = this.Pet_PetEntryModelInstance;
			this.m_ids[57] = this.Pet_PetEntryModelInstance;
			this.Guild_guildEventModelInstance = new Guild_guildEventModel();
			this.m_localModels[Guild_guildEventModel.fileName] = this.Guild_guildEventModelInstance;
			this.m_ids[58] = this.Guild_guildEventModelInstance;
			this.IAP_MonthCardModelInstance = new IAP_MonthCardModel();
			this.m_localModels[IAP_MonthCardModel.fileName] = this.IAP_MonthCardModelInstance;
			this.m_ids[59] = this.IAP_MonthCardModelInstance;
			this.Fishing_fishingModelInstance = new Fishing_fishingModel();
			this.m_localModels[Fishing_fishingModel.fileName] = this.Fishing_fishingModelInstance;
			this.m_ids[60] = this.Fishing_fishingModelInstance;
			this.Fishing_fishMoveModelInstance = new Fishing_fishMoveModel();
			this.m_localModels[Fishing_fishMoveModel.fileName] = this.Fishing_fishMoveModelInstance;
			this.m_ids[61] = this.Fishing_fishMoveModelInstance;
			this.IAP_BattlePassModelInstance = new IAP_BattlePassModel();
			this.m_localModels[IAP_BattlePassModel.fileName] = this.IAP_BattlePassModelInstance;
			this.m_ids[62] = this.IAP_BattlePassModelInstance;
			this.ChapterActivity_BattlepassModelInstance = new ChapterActivity_BattlepassModel();
			this.m_localModels[ChapterActivity_BattlepassModel.fileName] = this.ChapterActivity_BattlepassModelInstance;
			this.m_ids[63] = this.ChapterActivity_BattlepassModelInstance;
			this.ArtHover_hoverModelInstance = new ArtHover_hoverModel();
			this.m_localModels[ArtHover_hoverModel.fileName] = this.ArtHover_hoverModelInstance;
			this.m_ids[64] = this.ArtHover_hoverModelInstance;
			this.WorldBoss_SubsectionModelInstance = new WorldBoss_SubsectionModel();
			this.m_localModels[WorldBoss_SubsectionModel.fileName] = this.WorldBoss_SubsectionModelInstance;
			this.m_ids[65] = this.WorldBoss_SubsectionModelInstance;
			this.Guild_guildPowerModelInstance = new Guild_guildPowerModel();
			this.m_localModels[Guild_guildPowerModel.fileName] = this.Guild_guildPowerModelInstance;
			this.m_ids[66] = this.Guild_guildPowerModelInstance;
			this.RogueDungeon_monsterEntryModelInstance = new RogueDungeon_monsterEntryModel();
			this.m_localModels[RogueDungeon_monsterEntryModel.fileName] = this.RogueDungeon_monsterEntryModelInstance;
			this.m_ids[67] = this.RogueDungeon_monsterEntryModelInstance;
			this.Quality_guildBossQualityModelInstance = new Quality_guildBossQualityModel();
			this.m_localModels[Quality_guildBossQualityModel.fileName] = this.Quality_guildBossQualityModelInstance;
			this.m_ids[68] = this.Quality_guildBossQualityModelInstance;
			this.Relic_starUpModelInstance = new Relic_starUpModel();
			this.m_localModels[Relic_starUpModel.fileName] = this.Relic_starUpModelInstance;
			this.m_ids[69] = this.Relic_starUpModelInstance;
			this.LanguageCN_languagetableModelInstance = new LanguageCN_languagetableModel();
			this.m_localModels[LanguageCN_languagetableModel.fileName] = this.LanguageCN_languagetableModelInstance;
			this.m_ids[70] = this.LanguageCN_languagetableModelInstance;
			this.Artifact_artifactStageModelInstance = new Artifact_artifactStageModel();
			this.m_localModels[Artifact_artifactStageModel.fileName] = this.Artifact_artifactStageModelInstance;
			this.m_ids[71] = this.Artifact_artifactStageModelInstance;
			this.ChapterMiniGame_paySlotBaseModelInstance = new ChapterMiniGame_paySlotBaseModel();
			this.m_localModels[ChapterMiniGame_paySlotBaseModel.fileName] = this.ChapterMiniGame_paySlotBaseModelInstance;
			this.m_ids[72] = this.ChapterMiniGame_paySlotBaseModelInstance;
			this.ItemResources_itemgetModelInstance = new ItemResources_itemgetModel();
			this.m_localModels[ItemResources_itemgetModel.fileName] = this.ItemResources_itemgetModelInstance;
			this.m_ids[73] = this.ItemResources_itemgetModelInstance;
			this.Collection_collectionSuitModelInstance = new Collection_collectionSuitModel();
			this.m_localModels[Collection_collectionSuitModel.fileName] = this.Collection_collectionSuitModelInstance;
			this.m_ids[74] = this.Collection_collectionSuitModelInstance;
			this.Dungeon_DungeonLevelModelInstance = new Dungeon_DungeonLevelModel();
			this.m_localModels[Dungeon_DungeonLevelModel.fileName] = this.Dungeon_DungeonLevelModelInstance;
			this.m_ids[75] = this.Dungeon_DungeonLevelModelInstance;
			this.Guild_guildSignInModelInstance = new Guild_guildSignInModel();
			this.m_localModels[Guild_guildSignInModel.fileName] = this.Guild_guildSignInModelInstance;
			this.m_ids[76] = this.Guild_guildSignInModelInstance;
			this.GuildBOSS_rankRewardsModelInstance = new GuildBOSS_rankRewardsModel();
			this.m_localModels[GuildBOSS_rankRewardsModel.fileName] = this.GuildBOSS_rankRewardsModelInstance;
			this.m_ids[77] = this.GuildBOSS_rankRewardsModelInstance;
			this.CommonActivity_RankObjModelInstance = new CommonActivity_RankObjModel();
			this.m_localModels[CommonActivity_RankObjModel.fileName] = this.CommonActivity_RankObjModelInstance;
			this.m_ids[78] = this.CommonActivity_RankObjModelInstance;
			this.CrossArena_CrossArenaLevelModelInstance = new CrossArena_CrossArenaLevelModel();
			this.m_localModels[CrossArena_CrossArenaLevelModel.fileName] = this.CrossArena_CrossArenaLevelModelInstance;
			this.m_ids[79] = this.CrossArena_CrossArenaLevelModelInstance;
			this.TalentLegacy_talentLegacyEffectModelInstance = new TalentLegacy_talentLegacyEffectModel();
			this.m_localModels[TalentLegacy_talentLegacyEffectModel.fileName] = this.TalentLegacy_talentLegacyEffectModelInstance;
			this.m_ids[80] = this.TalentLegacy_talentLegacyEffectModelInstance;
			this.ArtSkill_SkillModelInstance = new ArtSkill_SkillModel();
			this.m_localModels[ArtSkill_SkillModel.fileName] = this.ArtSkill_SkillModelInstance;
			this.m_ids[81] = this.ArtSkill_SkillModelInstance;
			this.Emoji_EmojiModelInstance = new Emoji_EmojiModel();
			this.m_localModels[Emoji_EmojiModel.fileName] = this.Emoji_EmojiModelInstance;
			this.m_ids[82] = this.Emoji_EmojiModelInstance;
			this.IAP_ChapterPacksModelInstance = new IAP_ChapterPacksModel();
			this.m_localModels[IAP_ChapterPacksModel.fileName] = this.IAP_ChapterPacksModelInstance;
			this.m_ids[83] = this.IAP_ChapterPacksModelInstance;
			this.ChapterActivity_ModelModelInstance = new ChapterActivity_ModelModel();
			this.m_localModels[ChapterActivity_ModelModel.fileName] = this.ChapterActivity_ModelModelInstance;
			this.m_ids[84] = this.ChapterActivity_ModelModelInstance;
			this.Relic_updateLevelModelInstance = new Relic_updateLevelModel();
			this.m_localModels[Relic_updateLevelModel.fileName] = this.Relic_updateLevelModelInstance;
			this.m_ids[85] = this.Relic_updateLevelModelInstance;
			this.Mining_qualityModelModelInstance = new Mining_qualityModelModel();
			this.m_localModels[Mining_qualityModelModel.fileName] = this.Mining_qualityModelModelInstance;
			this.m_ids[86] = this.Mining_qualityModelModelInstance;
			this.IAP_PurchaseModelInstance = new IAP_PurchaseModel();
			this.m_localModels[IAP_PurchaseModel.fileName] = this.IAP_PurchaseModelInstance;
			this.m_ids[87] = this.IAP_PurchaseModelInstance;
			this.GameMember_npcFunctionModelInstance = new GameMember_npcFunctionModel();
			this.m_localModels[GameMember_npcFunctionModel.fileName] = this.GameMember_npcFunctionModelInstance;
			this.m_ids[88] = this.GameMember_npcFunctionModelInstance;
			this.ArtMember_morphModelInstance = new ArtMember_morphModel();
			this.m_localModels[ArtMember_morphModel.fileName] = this.ArtMember_morphModelInstance;
			this.m_ids[89] = this.ArtMember_morphModelInstance;
			this.Fishing_fishRodModelInstance = new Fishing_fishRodModel();
			this.m_localModels[Fishing_fishRodModel.fileName] = this.Fishing_fishRodModelInstance;
			this.m_ids[90] = this.Fishing_fishRodModelInstance;
			this.ArtHit_HitModelInstance = new ArtHit_HitModel();
			this.m_localModels[ArtHit_HitModel.fileName] = this.ArtHit_HitModelInstance;
			this.m_ids[91] = this.ArtHit_HitModelInstance;
			this.Guild_guildcontributeModelInstance = new Guild_guildcontributeModel();
			this.m_localModels[Guild_guildcontributeModel.fileName] = this.Guild_guildcontributeModelInstance;
			this.m_ids[92] = this.Guild_guildcontributeModelInstance;
			this.TGASource_ADModelInstance = new TGASource_ADModel();
			this.m_localModels[TGASource_ADModel.fileName] = this.TGASource_ADModelInstance;
			this.m_ids[93] = this.TGASource_ADModelInstance;
			this.SevenDay_SevenDayTaskModelInstance = new SevenDay_SevenDayTaskModel();
			this.m_localModels[SevenDay_SevenDayTaskModel.fileName] = this.SevenDay_SevenDayTaskModelInstance;
			this.m_ids[94] = this.SevenDay_SevenDayTaskModelInstance;
			this.BattleMain_waveModelInstance = new BattleMain_waveModel();
			this.m_localModels[BattleMain_waveModel.fileName] = this.BattleMain_waveModelInstance;
			this.m_ids[95] = this.BattleMain_waveModelInstance;
			this.TalentNew_talentMegaStageModelInstance = new TalentNew_talentMegaStageModel();
			this.m_localModels[TalentNew_talentMegaStageModel.fileName] = this.TalentNew_talentMegaStageModelInstance;
			this.m_ids[96] = this.TalentNew_talentMegaStageModelInstance;
			this.Avatar_AvatarModelInstance = new Avatar_AvatarModel();
			this.m_localModels[Avatar_AvatarModel.fileName] = this.Avatar_AvatarModelInstance;
			this.m_ids[97] = this.Avatar_AvatarModelInstance;
			this.Pet_petCollectionModelInstance = new Pet_petCollectionModel();
			this.m_localModels[Pet_petCollectionModel.fileName] = this.Pet_petCollectionModelInstance;
			this.m_ids[98] = this.Pet_petCollectionModelInstance;
			this.RogueDungeon_rogueDungeonModelInstance = new RogueDungeon_rogueDungeonModel();
			this.m_localModels[RogueDungeon_rogueDungeonModel.fileName] = this.RogueDungeon_rogueDungeonModelInstance;
			this.m_ids[99] = this.RogueDungeon_rogueDungeonModelInstance;
			this.Chapter_slotBuildModelInstance = new Chapter_slotBuildModel();
			this.m_localModels[Chapter_slotBuildModel.fileName] = this.Chapter_slotBuildModelInstance;
			this.m_ids[100] = this.Chapter_slotBuildModelInstance;
			this.ChapterActivity_BattlepassQualityModelInstance = new ChapterActivity_BattlepassQualityModel();
			this.m_localModels[ChapterActivity_BattlepassQualityModel.fileName] = this.ChapterActivity_BattlepassQualityModelInstance;
			this.m_ids[101] = this.ChapterActivity_BattlepassQualityModelInstance;
			this.ChapterActivity_ChapterActivityModelInstance = new ChapterActivity_ChapterActivityModel();
			this.m_localModels[ChapterActivity_ChapterActivityModel.fileName] = this.ChapterActivity_ChapterActivityModelInstance;
			this.m_ids[102] = this.ChapterActivity_ChapterActivityModelInstance;
			this.IAP_DiamondPacksModelInstance = new IAP_DiamondPacksModel();
			this.m_localModels[IAP_DiamondPacksModel.fileName] = this.IAP_DiamondPacksModelInstance;
			this.m_ids[103] = this.IAP_DiamondPacksModelInstance;
			this.MainLevelReward_MainLevelChestModelInstance = new MainLevelReward_MainLevelChestModel();
			this.m_localModels[MainLevelReward_MainLevelChestModel.fileName] = this.MainLevelReward_MainLevelChestModelInstance;
			this.m_ids[104] = this.MainLevelReward_MainLevelChestModelInstance;
			this.Shop_SummonPoolModelInstance = new Shop_SummonPoolModel();
			this.m_localModels[Shop_SummonPoolModel.fileName] = this.Shop_SummonPoolModelInstance;
			this.m_ids[105] = this.Shop_SummonPoolModelInstance;
			this.Pet_PetTrainingProbModelInstance = new Pet_PetTrainingProbModel();
			this.m_localModels[Pet_PetTrainingProbModel.fileName] = this.Pet_PetTrainingProbModelInstance;
			this.m_ids[106] = this.Pet_PetTrainingProbModelInstance;
			this.TalentNew_talentEvolutionModelInstance = new TalentNew_talentEvolutionModel();
			this.m_localModels[TalentNew_talentEvolutionModel.fileName] = this.TalentNew_talentEvolutionModelInstance;
			this.m_ids[107] = this.TalentNew_talentEvolutionModelInstance;
			this.MonsterCfgOld_monsterCfgOldModelInstance = new MonsterCfgOld_monsterCfgOldModel();
			this.m_localModels[MonsterCfgOld_monsterCfgOldModel.fileName] = this.MonsterCfgOld_monsterCfgOldModelInstance;
			this.m_ids[108] = this.MonsterCfgOld_monsterCfgOldModelInstance;
			this.TGASource_PageModelInstance = new TGASource_PageModel();
			this.m_localModels[TGASource_PageModel.fileName] = this.TGASource_PageModelInstance;
			this.m_ids[109] = this.TGASource_PageModelInstance;
			this.Map_mapModelInstance = new Map_mapModel();
			this.m_localModels[Map_mapModel.fileName] = this.Map_mapModelInstance;
			this.m_ids[110] = this.Map_mapModelInstance;
			this.AutoProcess_skillModelInstance = new AutoProcess_skillModel();
			this.m_localModels[AutoProcess_skillModel.fileName] = this.AutoProcess_skillModelInstance;
			this.m_ids[111] = this.AutoProcess_skillModelInstance;
			this.ActivityTurntable_TurntablePayModelInstance = new ActivityTurntable_TurntablePayModel();
			this.m_localModels[ActivityTurntable_TurntablePayModel.fileName] = this.ActivityTurntable_TurntablePayModelInstance;
			this.m_ids[112] = this.ActivityTurntable_TurntablePayModelInstance;
			this.ChapterActivity_ActvTurntableRewardModelInstance = new ChapterActivity_ActvTurntableRewardModel();
			this.m_localModels[ChapterActivity_ActvTurntableRewardModel.fileName] = this.ChapterActivity_ActvTurntableRewardModelInstance;
			this.m_ids[113] = this.ChapterActivity_ActvTurntableRewardModelInstance;
			this.ChestList_ChestRewardModelInstance = new ChestList_ChestRewardModel();
			this.m_localModels[ChestList_ChestRewardModel.fileName] = this.ChestList_ChestRewardModelInstance;
			this.m_ids[114] = this.ChestList_ChestRewardModelInstance;
			this.Map_floatingRandomModelInstance = new Map_floatingRandomModel();
			this.m_localModels[Map_floatingRandomModel.fileName] = this.Map_floatingRandomModelInstance;
			this.m_ids[115] = this.Map_floatingRandomModelInstance;
			this.ChapterMiniGame_turntableRewardModelInstance = new ChapterMiniGame_turntableRewardModel();
			this.m_localModels[ChapterMiniGame_turntableRewardModel.fileName] = this.ChapterMiniGame_turntableRewardModelInstance;
			this.m_ids[116] = this.ChapterMiniGame_turntableRewardModelInstance;
			this.Const_ConstModelInstance = new Const_ConstModel();
			this.m_localModels[Const_ConstModel.fileName] = this.Const_ConstModelInstance;
			this.m_ids[117] = this.Const_ConstModelInstance;
			this.GameSkill_skillConditionModelInstance = new GameSkill_skillConditionModel();
			this.m_localModels[GameSkill_skillConditionModel.fileName] = this.GameSkill_skillConditionModelInstance;
			this.m_ids[118] = this.GameSkill_skillConditionModelInstance;
			this.ArtRide_RideModelInstance = new ArtRide_RideModel();
			this.m_localModels[ArtRide_RideModel.fileName] = this.ArtRide_RideModelInstance;
			this.m_ids[119] = this.ArtRide_RideModelInstance;
			this.ChainPacks_ChainPacksModelInstance = new ChainPacks_ChainPacksModel();
			this.m_localModels[ChainPacks_ChainPacksModel.fileName] = this.ChainPacks_ChainPacksModelInstance;
			this.m_ids[120] = this.ChainPacks_ChainPacksModelInstance;
			this.TalentLegacy_careerModelInstance = new TalentLegacy_careerModel();
			this.m_localModels[TalentLegacy_careerModel.fileName] = this.TalentLegacy_careerModelInstance;
			this.m_ids[121] = this.TalentLegacy_careerModelInstance;
			this.LanguageRaft_languagetableModelInstance = new LanguageRaft_languagetableModel();
			this.m_localModels[LanguageRaft_languagetableModel.fileName] = this.LanguageRaft_languagetableModelInstance;
			this.m_ids[122] = this.LanguageRaft_languagetableModelInstance;
			this.AutoProcess_skillRandomModelInstance = new AutoProcess_skillRandomModel();
			this.m_localModels[AutoProcess_skillRandomModel.fileName] = this.AutoProcess_skillRandomModelInstance;
			this.m_ids[123] = this.AutoProcess_skillRandomModelInstance;
			this.ServerList_serverListModelInstance = new ServerList_serverListModel();
			this.m_localModels[ServerList_serverListModel.fileName] = this.ServerList_serverListModelInstance;
			this.m_ids[124] = this.ServerList_serverListModelInstance;
			this.IAP_platformIDModelInstance = new IAP_platformIDModel();
			this.m_localModels[IAP_platformIDModel.fileName] = this.IAP_platformIDModelInstance;
			this.m_ids[125] = this.IAP_platformIDModelInstance;
			this.IAP_LevelFundRewardModelInstance = new IAP_LevelFundRewardModel();
			this.m_localModels[IAP_LevelFundRewardModel.fileName] = this.IAP_LevelFundRewardModelInstance;
			this.m_ids[126] = this.IAP_LevelFundRewardModelInstance;
			this.ArtSagecraft_SagecraftModelInstance = new ArtSagecraft_SagecraftModel();
			this.m_localModels[ArtSagecraft_SagecraftModel.fileName] = this.ArtSagecraft_SagecraftModelInstance;
			this.m_ids[127] = this.ArtSagecraft_SagecraftModelInstance;
			this.GuildBOSS_guildBossAttrModelInstance = new GuildBOSS_guildBossAttrModel();
			this.m_localModels[GuildBOSS_guildBossAttrModel.fileName] = this.GuildBOSS_guildBossAttrModelInstance;
			this.m_ids[128] = this.GuildBOSS_guildBossAttrModelInstance;
			this.GameBuff_buffTypeModelInstance = new GameBuff_buffTypeModel();
			this.m_localModels[GameBuff_buffTypeModel.fileName] = this.GameBuff_buffTypeModelInstance;
			this.m_ids[129] = this.GameBuff_buffTypeModelInstance;
			this.Pet_petSummonModelInstance = new Pet_petSummonModel();
			this.m_localModels[Pet_petSummonModel.fileName] = this.Pet_petSummonModelInstance;
			this.m_ids[130] = this.Pet_petSummonModelInstance;
			this.Chapter_skillExpModelInstance = new Chapter_skillExpModel();
			this.m_localModels[Chapter_skillExpModel.fileName] = this.Chapter_skillExpModelInstance;
			this.m_ids[131] = this.Chapter_skillExpModelInstance;
			this.Mining_oreQualityModelInstance = new Mining_oreQualityModel();
			this.m_localModels[Mining_oreQualityModel.fileName] = this.Mining_oreQualityModelInstance;
			this.m_ids[132] = this.Mining_oreQualityModelInstance;
			this.ArtMember_memberModelInstance = new ArtMember_memberModel();
			this.m_localModels[ArtMember_memberModel.fileName] = this.ArtMember_memberModelInstance;
			this.m_ids[133] = this.ArtMember_memberModelInstance;
			this.MainLevelReward_AFKrewardModelInstance = new MainLevelReward_AFKrewardModel();
			this.m_localModels[MainLevelReward_AFKrewardModel.fileName] = this.MainLevelReward_AFKrewardModelInstance;
			this.m_ids[134] = this.MainLevelReward_AFKrewardModelInstance;
			this.NewWorld_newWorldRankModelInstance = new NewWorld_newWorldRankModel();
			this.m_localModels[NewWorld_newWorldRankModel.fileName] = this.NewWorld_newWorldRankModelInstance;
			this.m_ids[135] = this.NewWorld_newWorldRankModelInstance;
			this.CommonActivity_CommonActivityModelInstance = new CommonActivity_CommonActivityModel();
			this.m_localModels[CommonActivity_CommonActivityModel.fileName] = this.CommonActivity_CommonActivityModelInstance;
			this.m_ids[136] = this.CommonActivity_CommonActivityModelInstance;
			this.Pet_petLevelModelInstance = new Pet_petLevelModel();
			this.m_localModels[Pet_petLevelModel.fileName] = this.Pet_petLevelModelInstance;
			this.m_ids[137] = this.Pet_petLevelModelInstance;
			this.Function_FunctionModelInstance = new Function_FunctionModel();
			this.m_localModels[Function_FunctionModel.fileName] = this.Function_FunctionModelInstance;
			this.m_ids[138] = this.Function_FunctionModelInstance;
			this.Mining_oreResModelInstance = new Mining_oreResModel();
			this.m_localModels[Mining_oreResModel.fileName] = this.Mining_oreResModelInstance;
			this.m_ids[139] = this.Mining_oreResModelInstance;
			this.BattleMain_skillModelInstance = new BattleMain_skillModel();
			this.m_localModels[BattleMain_skillModel.fileName] = this.BattleMain_skillModelInstance;
			this.m_ids[140] = this.BattleMain_skillModelInstance;
			this.TowerChallenge_TowerModelInstance = new TowerChallenge_TowerModel();
			this.m_localModels[TowerChallenge_TowerModel.fileName] = this.TowerChallenge_TowerModelInstance;
			this.m_ids[141] = this.TowerChallenge_TowerModelInstance;
			this.ItemResources_jumpResourceModelInstance = new ItemResources_jumpResourceModel();
			this.m_localModels[ItemResources_jumpResourceModel.fileName] = this.ItemResources_jumpResourceModelInstance;
			this.m_ids[142] = this.ItemResources_jumpResourceModelInstance;
			this.ArtAnimation_animationModelInstance = new ArtAnimation_animationModel();
			this.m_localModels[ArtAnimation_animationModel.fileName] = this.ArtAnimation_animationModelInstance;
			this.m_ids[143] = this.ArtAnimation_animationModelInstance;
			this.Quality_equipQualityModelInstance = new Quality_equipQualityModel();
			this.m_localModels[Quality_equipQualityModel.fileName] = this.Quality_equipQualityModelInstance;
			this.m_ids[144] = this.Quality_equipQualityModelInstance;
			this.GuildBOSS_guildBossModelInstance = new GuildBOSS_guildBossModel();
			this.m_localModels[GuildBOSS_guildBossModel.fileName] = this.GuildBOSS_guildBossModelInstance;
			this.m_ids[145] = this.GuildBOSS_guildBossModelInstance;
			this.Item_ItemModelInstance = new Item_ItemModel();
			this.m_localModels[Item_ItemModel.fileName] = this.Item_ItemModelInstance;
			this.m_ids[146] = this.Item_ItemModelInstance;
			this.Pet_PetTrainingModelInstance = new Pet_PetTrainingModel();
			this.m_localModels[Pet_PetTrainingModel.fileName] = this.Pet_PetTrainingModelInstance;
			this.m_ids[147] = this.Pet_PetTrainingModelInstance;
			this.Pet_petModelInstance = new Pet_petModel();
			this.m_localModels[Pet_petModel.fileName] = this.Pet_petModelInstance;
			this.m_ids[148] = this.Pet_petModelInstance;
			this.CrossArena_CrossArenaTimeModelInstance = new CrossArena_CrossArenaTimeModel();
			this.m_localModels[CrossArena_CrossArenaTimeModel.fileName] = this.CrossArena_CrossArenaTimeModelInstance;
			this.m_ids[149] = this.CrossArena_CrossArenaTimeModelInstance;
			this.CrossArena_CrossArenaRobotModelInstance = new CrossArena_CrossArenaRobotModel();
			this.m_localModels[CrossArena_CrossArenaRobotModel.fileName] = this.CrossArena_CrossArenaRobotModelInstance;
			this.m_ids[150] = this.CrossArena_CrossArenaRobotModelInstance;
			this.TGASource_IAPModelInstance = new TGASource_IAPModel();
			this.m_localModels[TGASource_IAPModel.fileName] = this.TGASource_IAPModelInstance;
			this.m_ids[151] = this.TGASource_IAPModelInstance;
			this.Guild_guildConstModelInstance = new Guild_guildConstModel();
			this.m_localModels[Guild_guildConstModel.fileName] = this.Guild_guildConstModelInstance;
			this.m_ids[152] = this.Guild_guildConstModelInstance;
			this.GameBuff_buffModelInstance = new GameBuff_buffModel();
			this.m_localModels[GameBuff_buffModel.fileName] = this.GameBuff_buffModelInstance;
			this.m_ids[153] = this.GameBuff_buffModelInstance;
			this.Box_boxBaseModelInstance = new Box_boxBaseModel();
			this.m_localModels[Box_boxBaseModel.fileName] = this.Box_boxBaseModelInstance;
			this.m_ids[154] = this.Box_boxBaseModelInstance;
			this.ArtHover_hoverTextModelInstance = new ArtHover_hoverTextModel();
			this.m_localModels[ArtHover_hoverTextModel.fileName] = this.ArtHover_hoverTextModelInstance;
			this.m_ids[155] = this.ArtHover_hoverTextModelInstance;
			this.Chapter_boxBuildModelInstance = new Chapter_boxBuildModel();
			this.m_localModels[Chapter_boxBuildModel.fileName] = this.Chapter_boxBuildModelInstance;
			this.m_ids[156] = this.Chapter_boxBuildModelInstance;
			this.GameSkill_skillTypeModelInstance = new GameSkill_skillTypeModel();
			this.m_localModels[GameSkill_skillTypeModel.fileName] = this.GameSkill_skillTypeModelInstance;
			this.m_ids[157] = this.GameSkill_skillTypeModelInstance;
			this.NewWorld_newWorldTaskModelInstance = new NewWorld_newWorldTaskModel();
			this.m_localModels[NewWorld_newWorldTaskModel.fileName] = this.NewWorld_newWorldTaskModelInstance;
			this.m_ids[158] = this.NewWorld_newWorldTaskModelInstance;
			this.Guild_guildGiftModelInstance = new Guild_guildGiftModel();
			this.m_localModels[Guild_guildGiftModel.fileName] = this.Guild_guildGiftModelInstance;
			this.m_ids[159] = this.Guild_guildGiftModelInstance;
			this.Chapter_eventPointModelInstance = new Chapter_eventPointModel();
			this.m_localModels[Chapter_eventPointModel.fileName] = this.Chapter_eventPointModelInstance;
			this.m_ids[160] = this.Chapter_eventPointModelInstance;
			this.Chapter_sweepModelInstance = new Chapter_sweepModel();
			this.m_localModels[Chapter_sweepModel.fileName] = this.Chapter_sweepModelInstance;
			this.m_ids[161] = this.Chapter_sweepModelInstance;
			this.ActivityTurntable_ActivityTurntableModelInstance = new ActivityTurntable_ActivityTurntableModel();
			this.m_localModels[ActivityTurntable_ActivityTurntableModel.fileName] = this.ActivityTurntable_ActivityTurntableModelInstance;
			this.m_ids[162] = this.ActivityTurntable_ActivityTurntableModelInstance;
			this.Task_DailyTaskModelInstance = new Task_DailyTaskModel();
			this.m_localModels[Task_DailyTaskModel.fileName] = this.Task_DailyTaskModelInstance;
			this.m_ids[163] = this.Task_DailyTaskModelInstance;
			this.Artifact_artifactLevelModelInstance = new Artifact_artifactLevelModel();
			this.m_localModels[Artifact_artifactLevelModel.fileName] = this.Artifact_artifactLevelModelInstance;
			this.m_ids[164] = this.Artifact_artifactLevelModelInstance;
			this.Sound_soundModelInstance = new Sound_soundModel();
			this.m_localModels[Sound_soundModel.fileName] = this.Sound_soundModelInstance;
			this.m_ids[165] = this.Sound_soundModelInstance;
			this.IntegralShop_goodsModelInstance = new IntegralShop_goodsModel();
			this.m_localModels[IntegralShop_goodsModel.fileName] = this.IntegralShop_goodsModelInstance;
			this.m_ids[166] = this.IntegralShop_goodsModelInstance;
			this.Ride_RideModelInstance = new Ride_RideModel();
			this.m_localModels[Ride_RideModel.fileName] = this.Ride_RideModelInstance;
			this.m_ids[167] = this.Ride_RideModelInstance;
			this.Chapter_eventTypeModelInstance = new Chapter_eventTypeModel();
			this.m_localModels[Chapter_eventTypeModel.fileName] = this.Chapter_eventTypeModelInstance;
			this.m_ids[168] = this.Chapter_eventTypeModelInstance;
			this.ItemGift_ItemGiftModelInstance = new ItemGift_ItemGiftModel();
			this.m_localModels[ItemGift_ItemGiftModel.fileName] = this.ItemGift_ItemGiftModelInstance;
			this.m_ids[169] = this.ItemGift_ItemGiftModelInstance;
			this.ChapterActivity_RankActivityModelInstance = new ChapterActivity_RankActivityModel();
			this.m_localModels[ChapterActivity_RankActivityModel.fileName] = this.ChapterActivity_RankActivityModelInstance;
			this.m_ids[170] = this.ChapterActivity_RankActivityModelInstance;
			this.GameMember_skinModelInstance = new GameMember_skinModel();
			this.m_localModels[GameMember_skinModel.fileName] = this.GameMember_skinModelInstance;
			this.m_ids[171] = this.GameMember_skinModelInstance;
			this.TicketExchange_ExchangeModelInstance = new TicketExchange_ExchangeModel();
			this.m_localModels[TicketExchange_ExchangeModel.fileName] = this.TicketExchange_ExchangeModelInstance;
			this.m_ids[172] = this.TicketExchange_ExchangeModelInstance;
			this.Quality_petQualityModelInstance = new Quality_petQualityModel();
			this.m_localModels[Quality_petQualityModel.fileName] = this.Quality_petQualityModelInstance;
			this.m_ids[173] = this.Quality_petQualityModelInstance;
			this.Avatar_SceneSkinModelInstance = new Avatar_SceneSkinModel();
			this.m_localModels[Avatar_SceneSkinModel.fileName] = this.Avatar_SceneSkinModelInstance;
			this.m_ids[174] = this.Avatar_SceneSkinModelInstance;
			this.Guild_guildDonationModelInstance = new Guild_guildDonationModel();
			this.m_localModels[Guild_guildDonationModel.fileName] = this.Guild_guildDonationModelInstance;
			this.m_ids[175] = this.Guild_guildDonationModelInstance;
			this.ChapterActivity_ActvTurntableDetailModelInstance = new ChapterActivity_ActvTurntableDetailModel();
			this.m_localModels[ChapterActivity_ActvTurntableDetailModel.fileName] = this.ChapterActivity_ActvTurntableDetailModelInstance;
			this.m_ids[176] = this.ChapterActivity_ActvTurntableDetailModelInstance;
			this.BattleMain_chapterModelInstance = new BattleMain_chapterModel();
			this.m_localModels[BattleMain_chapterModel.fileName] = this.BattleMain_chapterModelInstance;
			this.m_ids[177] = this.BattleMain_chapterModelInstance;
			this.GameBuff_overlayTypeModelInstance = new GameBuff_overlayTypeModel();
			this.m_localModels[GameBuff_overlayTypeModel.fileName] = this.GameBuff_overlayTypeModelInstance;
			this.m_ids[178] = this.GameBuff_overlayTypeModelInstance;
			this.GuildRace_baseRaceModelInstance = new GuildRace_baseRaceModel();
			this.m_localModels[GuildRace_baseRaceModel.fileName] = this.GuildRace_baseRaceModelInstance;
			this.m_ids[179] = this.GuildRace_baseRaceModelInstance;
			this.GuildBOSS_guildSubectionModelInstance = new GuildBOSS_guildSubectionModel();
			this.m_localModels[GuildBOSS_guildSubectionModel.fileName] = this.GuildBOSS_guildSubectionModelInstance;
			this.m_ids[180] = this.GuildBOSS_guildSubectionModelInstance;
			this.Guild_guilddairyModelInstance = new Guild_guilddairyModel();
			this.m_localModels[Guild_guilddairyModel.fileName] = this.Guild_guilddairyModelInstance;
			this.m_ids[181] = this.Guild_guilddairyModelInstance;
			this.ChainPacks_ChainActvModelInstance = new ChainPacks_ChainActvModel();
			this.m_localModels[ChainPacks_ChainActvModel.fileName] = this.ChainPacks_ChainActvModelInstance;
			this.m_ids[182] = this.ChainPacks_ChainActvModelInstance;
			this.Guild_guildShopModelInstance = new Guild_guildShopModel();
			this.m_localModels[Guild_guildShopModel.fileName] = this.Guild_guildShopModelInstance;
			this.m_ids[183] = this.Guild_guildShopModelInstance;
			this.ChapterMiniGame_turntableBaseModelInstance = new ChapterMiniGame_turntableBaseModel();
			this.m_localModels[ChapterMiniGame_turntableBaseModel.fileName] = this.ChapterMiniGame_turntableBaseModelInstance;
			this.m_ids[184] = this.ChapterMiniGame_turntableBaseModelInstance;
			this.Relic_dataModelInstance = new Relic_dataModel();
			this.m_localModels[Relic_dataModel.fileName] = this.Relic_dataModelInstance;
			this.m_ids[185] = this.Relic_dataModelInstance;
			this.Collection_collectionStarModelInstance = new Collection_collectionStarModel();
			this.m_localModels[Collection_collectionStarModel.fileName] = this.Collection_collectionStarModelInstance;
			this.m_ids[186] = this.Collection_collectionStarModelInstance;
			this.LanguageRaft_languageTabModelInstance = new LanguageRaft_languageTabModel();
			this.m_localModels[LanguageRaft_languageTabModel.fileName] = this.LanguageRaft_languageTabModelInstance;
			this.m_ids[187] = this.LanguageRaft_languageTabModelInstance;
			this.Shop_ShopModelInstance = new Shop_ShopModel();
			this.m_localModels[Shop_ShopModel.fileName] = this.Shop_ShopModelInstance;
			this.m_ids[188] = this.Shop_ShopModelInstance;
			this.IAP_pushIapModelInstance = new IAP_pushIapModel();
			this.m_localModels[IAP_pushIapModel.fileName] = this.IAP_pushIapModelInstance;
			this.m_ids[189] = this.IAP_pushIapModelInstance;
			this.GameSkill_skillSelectModelInstance = new GameSkill_skillSelectModel();
			this.m_localModels[GameSkill_skillSelectModel.fileName] = this.GameSkill_skillSelectModelInstance;
			this.m_ids[190] = this.GameSkill_skillSelectModelInstance;
			this.CommonActivity_DropObjModelInstance = new CommonActivity_DropObjModel();
			this.m_localModels[CommonActivity_DropObjModel.fileName] = this.CommonActivity_DropObjModelInstance;
			this.m_ids[191] = this.CommonActivity_DropObjModelInstance;
			this.Chapter_eventResModelInstance = new Chapter_eventResModel();
			this.m_localModels[Chapter_eventResModel.fileName] = this.Chapter_eventResModelInstance;
			this.m_ids[192] = this.Chapter_eventResModelInstance;
			this.CrossArena_CrossArenaModelInstance = new CrossArena_CrossArenaModel();
			this.m_localModels[CrossArena_CrossArenaModel.fileName] = this.CrossArena_CrossArenaModelInstance;
			this.m_ids[193] = this.CrossArena_CrossArenaModelInstance;
			this.Equip_skillModelInstance = new Equip_skillModel();
			this.m_localModels[Equip_skillModel.fileName] = this.Equip_skillModelInstance;
			this.m_ids[194] = this.Equip_skillModelInstance;
			this.GameSkill_skillAnimationModelInstance = new GameSkill_skillAnimationModel();
			this.m_localModels[GameSkill_skillAnimationModel.fileName] = this.GameSkill_skillAnimationModelInstance;
			this.m_ids[195] = this.GameSkill_skillAnimationModelInstance;
			this.Chapter_eventItemModelInstance = new Chapter_eventItemModel();
			this.m_localModels[Chapter_eventItemModel.fileName] = this.Chapter_eventItemModelInstance;
			this.m_ids[196] = this.Chapter_eventItemModelInstance;
			this.WorldBoss_RewardModelInstance = new WorldBoss_RewardModel();
			this.m_localModels[WorldBoss_RewardModel.fileName] = this.WorldBoss_RewardModelInstance;
			this.m_ids[197] = this.WorldBoss_RewardModelInstance;
			this.ActivityTurntable_TurntableQuestModelInstance = new ActivityTurntable_TurntableQuestModel();
			this.m_localModels[ActivityTurntable_TurntableQuestModel.fileName] = this.ActivityTurntable_TurntableQuestModelInstance;
			this.m_ids[198] = this.ActivityTurntable_TurntableQuestModelInstance;
			this.ArtEffect_EffectModelInstance = new ArtEffect_EffectModel();
			this.m_localModels[ArtEffect_EffectModel.fileName] = this.ArtEffect_EffectModelInstance;
			this.m_ids[199] = this.ArtEffect_EffectModelInstance;
			this.WorldBoss_WorldBossBoxModelInstance = new WorldBoss_WorldBossBoxModel();
			this.m_localModels[WorldBoss_WorldBossBoxModel.fileName] = this.WorldBoss_WorldBossBoxModelInstance;
			this.m_ids[200] = this.WorldBoss_WorldBossBoxModelInstance;
			this.GuildRace_opentimeModelInstance = new GuildRace_opentimeModel();
			this.m_localModels[GuildRace_opentimeModel.fileName] = this.GuildRace_opentimeModelInstance;
			this.m_ids[201] = this.GuildRace_opentimeModelInstance;
			this.Chapter_attributeBuildModelInstance = new Chapter_attributeBuildModel();
			this.m_localModels[Chapter_attributeBuildModel.fileName] = this.Chapter_attributeBuildModelInstance;
			this.m_ids[202] = this.Chapter_attributeBuildModelInstance;
			this.Shop_EquipActivityModelInstance = new Shop_EquipActivityModel();
			this.m_localModels[Shop_EquipActivityModel.fileName] = this.Shop_EquipActivityModelInstance;
			this.m_ids[203] = this.Shop_EquipActivityModelInstance;
			this.Item_dropLvModelInstance = new Item_dropLvModel();
			this.m_localModels[Item_dropLvModel.fileName] = this.Item_dropLvModelInstance;
			this.m_ids[204] = this.Item_dropLvModelInstance;
			this.ChapterActivity_BattlepassRewardModelInstance = new ChapterActivity_BattlepassRewardModel();
			this.m_localModels[ChapterActivity_BattlepassRewardModel.fileName] = this.ChapterActivity_BattlepassRewardModelInstance;
			this.m_ids[205] = this.ChapterActivity_BattlepassRewardModelInstance;
			this.CommonActivity_ShopObjModelInstance = new CommonActivity_ShopObjModel();
			this.m_localModels[CommonActivity_ShopObjModel.fileName] = this.CommonActivity_ShopObjModelInstance;
			this.m_ids[206] = this.CommonActivity_ShopObjModelInstance;
			this.Pet_petLevelEffectModelInstance = new Pet_petLevelEffectModel();
			this.m_localModels[Pet_petLevelEffectModel.fileName] = this.Pet_petLevelEffectModelInstance;
			this.m_ids[207] = this.Pet_petLevelEffectModelInstance;
			this.CommonActivity_PayObjModelInstance = new CommonActivity_PayObjModel();
			this.m_localModels[CommonActivity_PayObjModel.fileName] = this.CommonActivity_PayObjModelInstance;
			this.m_ids[208] = this.CommonActivity_PayObjModelInstance;
			this.GuildBOSS_guildBossStepModelInstance = new GuildBOSS_guildBossStepModel();
			this.m_localModels[GuildBOSS_guildBossStepModel.fileName] = this.GuildBOSS_guildBossStepModelInstance;
			this.m_ids[209] = this.GuildBOSS_guildBossStepModelInstance;
			this.Guild_guildStyleModelInstance = new Guild_guildStyleModel();
			this.m_localModels[Guild_guildStyleModel.fileName] = this.Guild_guildStyleModelInstance;
			this.m_ids[210] = this.Guild_guildStyleModelInstance;
			this.WorldBoss_WorldBossModelInstance = new WorldBoss_WorldBossModel();
			this.m_localModels[WorldBoss_WorldBossModel.fileName] = this.WorldBoss_WorldBossModelInstance;
			this.m_ids[211] = this.WorldBoss_WorldBossModelInstance;
			this.GameSkill_hitEffectModelInstance = new GameSkill_hitEffectModel();
			this.m_localModels[GameSkill_hitEffectModel.fileName] = this.GameSkill_hitEffectModelInstance;
			this.m_ids[212] = this.GameSkill_hitEffectModelInstance;
			this.Relic_relicModelInstance = new Relic_relicModel();
			this.m_localModels[Relic_relicModel.fileName] = this.Relic_relicModelInstance;
			this.m_ids[213] = this.Relic_relicModelInstance;
			this.GameSkillBuild_skillRandomModelInstance = new GameSkillBuild_skillRandomModel();
			this.m_localModels[GameSkillBuild_skillRandomModel.fileName] = this.GameSkillBuild_skillRandomModelInstance;
			this.m_ids[214] = this.GameSkillBuild_skillRandomModelInstance;
			this.Task_DailyActiveModelInstance = new Task_DailyActiveModel();
			this.m_localModels[Task_DailyActiveModel.fileName] = this.Task_DailyActiveModelInstance;
			this.m_ids[215] = this.Task_DailyActiveModelInstance;
			this.MonsterCfg_Old_monsterCfgModelInstance = new MonsterCfg_Old_monsterCfgModel();
			this.m_localModels[MonsterCfg_Old_monsterCfgModel.fileName] = this.MonsterCfg_Old_monsterCfgModelInstance;
			this.m_ids[216] = this.MonsterCfg_Old_monsterCfgModelInstance;
			this.ChapterMiniGame_slotTrainBuildModelInstance = new ChapterMiniGame_slotTrainBuildModel();
			this.m_localModels[ChapterMiniGame_slotTrainBuildModel.fileName] = this.ChapterMiniGame_slotTrainBuildModelInstance;
			this.m_ids[217] = this.ChapterMiniGame_slotTrainBuildModelInstance;
			this.GameMember_aiTypeModelInstance = new GameMember_aiTypeModel();
			this.m_localModels[GameMember_aiTypeModel.fileName] = this.GameMember_aiTypeModelInstance;
			this.m_ids[218] = this.GameMember_aiTypeModelInstance;
			this.Mining_miningBaseModelInstance = new Mining_miningBaseModel();
			this.m_localModels[Mining_miningBaseModel.fileName] = this.Mining_miningBaseModelInstance;
			this.m_ids[219] = this.Mining_miningBaseModelInstance;
			this.Mount_advanceMountModelInstance = new Mount_advanceMountModel();
			this.m_localModels[Mount_advanceMountModel.fileName] = this.Mount_advanceMountModelInstance;
			this.m_ids[220] = this.Mount_advanceMountModelInstance;
			this.TalentNew_talentModelInstance = new TalentNew_talentModel();
			this.m_localModels[TalentNew_talentModel.fileName] = this.TalentNew_talentModelInstance;
			this.m_ids[221] = this.TalentNew_talentModelInstance;
			this.ChapterMiniGame_slotRewardModelInstance = new ChapterMiniGame_slotRewardModel();
			this.m_localModels[ChapterMiniGame_slotRewardModel.fileName] = this.ChapterMiniGame_slotRewardModelInstance;
			this.m_ids[222] = this.ChapterMiniGame_slotRewardModelInstance;
			this.Avatar_SkinModelInstance = new Avatar_SkinModel();
			this.m_localModels[Avatar_SkinModel.fileName] = this.Avatar_SkinModelInstance;
			this.m_ids[223] = this.Avatar_SkinModelInstance;
			this.MonsterCfg_monsterCfgModelInstance = new MonsterCfg_monsterCfgModel();
			this.m_localModels[MonsterCfg_monsterCfgModel.fileName] = this.MonsterCfg_monsterCfgModelInstance;
			this.m_ids[224] = this.MonsterCfg_monsterCfgModelInstance;
			this.Shop_SummonModelInstance = new Shop_SummonModel();
			this.m_localModels[Shop_SummonModel.fileName] = this.Shop_SummonModelInstance;
			this.m_ids[225] = this.Shop_SummonModelInstance;
			this.ChainPacks_ChainTypeModelInstance = new ChainPacks_ChainTypeModel();
			this.m_localModels[ChainPacks_ChainTypeModel.fileName] = this.ChainPacks_ChainTypeModelInstance;
			this.m_ids[226] = this.ChainPacks_ChainTypeModelInstance;
			this.Guild_guildLevelModelInstance = new Guild_guildLevelModel();
			this.m_localModels[Guild_guildLevelModel.fileName] = this.Guild_guildLevelModelInstance;
			this.m_ids[227] = this.Guild_guildLevelModelInstance;
			this.Chapter_chapterModelInstance = new Chapter_chapterModel();
			this.m_localModels[Chapter_chapterModel.fileName] = this.Chapter_chapterModelInstance;
			this.m_ids[228] = this.Chapter_chapterModelInstance;
			this.CrossArena_CrossArenaRobotNameModelInstance = new CrossArena_CrossArenaRobotNameModel();
			this.m_localModels[CrossArena_CrossArenaRobotNameModel.fileName] = this.CrossArena_CrossArenaRobotNameModelInstance;
			this.m_ids[229] = this.CrossArena_CrossArenaRobotNameModelInstance;
			this.Fishing_fishModelInstance = new Fishing_fishModel();
			this.m_localModels[Fishing_fishModel.fileName] = this.Fishing_fishModelInstance;
			this.m_ids[230] = this.Fishing_fishModelInstance;
			this.Item_dropModelInstance = new Item_dropModel();
			this.m_localModels[Item_dropModel.fileName] = this.Item_dropModelInstance;
			this.m_ids[231] = this.Item_dropModelInstance;
			this.ChapterMiniGame_slotBaseModelInstance = new ChapterMiniGame_slotBaseModel();
			this.m_localModels[ChapterMiniGame_slotBaseModel.fileName] = this.ChapterMiniGame_slotBaseModelInstance;
			this.m_ids[232] = this.ChapterMiniGame_slotBaseModelInstance;
			this.ServerList_chatGropModelInstance = new ServerList_chatGropModel();
			this.m_localModels[ServerList_chatGropModel.fileName] = this.ServerList_chatGropModelInstance;
			this.m_ids[233] = this.ServerList_chatGropModelInstance;
			this.ChapterMiniGame_cardFlippingBaseModelInstance = new ChapterMiniGame_cardFlippingBaseModel();
			this.m_localModels[ChapterMiniGame_cardFlippingBaseModel.fileName] = this.ChapterMiniGame_cardFlippingBaseModelInstance;
			this.m_ids[234] = this.ChapterMiniGame_cardFlippingBaseModelInstance;
			this.Box_outputModelInstance = new Box_outputModel();
			this.m_localModels[Box_outputModel.fileName] = this.Box_outputModelInstance;
			this.m_ids[235] = this.Box_outputModelInstance;
			this.Equip_equipComposeModelInstance = new Equip_equipComposeModel();
			this.m_localModels[Equip_equipComposeModel.fileName] = this.Equip_equipComposeModelInstance;
			this.m_ids[236] = this.Equip_equipComposeModelInstance;
			this.Quality_collectionQualityModelInstance = new Quality_collectionQualityModel();
			this.m_localModels[Quality_collectionQualityModel.fileName] = this.Quality_collectionQualityModelInstance;
			this.m_ids[237] = this.Quality_collectionQualityModelInstance;
			this.Guild_guildLanguageModelInstance = new Guild_guildLanguageModel();
			this.m_localModels[Guild_guildLanguageModel.fileName] = this.Guild_guildLanguageModelInstance;
			this.m_ids[238] = this.Guild_guildLanguageModelInstance;
			this.GuildBOSS_guildBossSeasonRewardModelInstance = new GuildBOSS_guildBossSeasonRewardModel();
			this.m_localModels[GuildBOSS_guildBossSeasonRewardModel.fileName] = this.GuildBOSS_guildBossSeasonRewardModelInstance;
			this.m_ids[239] = this.GuildBOSS_guildBossSeasonRewardModelInstance;
			this.Mining_bonusGameModelInstance = new Mining_bonusGameModel();
			this.m_localModels[Mining_bonusGameModel.fileName] = this.Mining_bonusGameModelInstance;
			this.m_ids[240] = this.Mining_bonusGameModelInstance;
			this.ChapterMiniGame_singleSlotModelInstance = new ChapterMiniGame_singleSlotModel();
			this.m_localModels[ChapterMiniGame_singleSlotModel.fileName] = this.ChapterMiniGame_singleSlotModelInstance;
			this.m_ids[241] = this.ChapterMiniGame_singleSlotModelInstance;
			this.CrossArena_CrossArenaRewardModelInstance = new CrossArena_CrossArenaRewardModel();
			this.m_localModels[CrossArena_CrossArenaRewardModel.fileName] = this.CrossArena_CrossArenaRewardModelInstance;
			this.m_ids[242] = this.CrossArena_CrossArenaRewardModelInstance;
			this.Pet_petSkillModelInstance = new Pet_petSkillModel();
			this.m_localModels[Pet_petSkillModel.fileName] = this.Pet_petSkillModelInstance;
			this.m_ids[243] = this.Pet_petSkillModelInstance;
			this.ChapterActivity_ActvTurntableBaseModelInstance = new ChapterActivity_ActvTurntableBaseModel();
			this.m_localModels[ChapterActivity_ActvTurntableBaseModel.fileName] = this.ChapterActivity_ActvTurntableBaseModelInstance;
			this.m_ids[244] = this.ChapterActivity_ActvTurntableBaseModelInstance;
			this.Item_battleModelInstance = new Item_battleModel();
			this.m_localModels[Item_battleModel.fileName] = this.Item_battleModelInstance;
			this.m_ids[245] = this.Item_battleModelInstance;
			this.Mount_mountLevelModelInstance = new Mount_mountLevelModel();
			this.m_localModels[Mount_mountLevelModel.fileName] = this.Mount_mountLevelModelInstance;
			this.m_ids[246] = this.Mount_mountLevelModelInstance;
			this.Equip_equipSkillModelInstance = new Equip_equipSkillModel();
			this.m_localModels[Equip_equipSkillModel.fileName] = this.Equip_equipSkillModelInstance;
			this.m_ids[247] = this.Equip_equipSkillModelInstance;
			this.Equip_equipEvolutionModelInstance = new Equip_equipEvolutionModel();
			this.m_localModels[Equip_equipEvolutionModel.fileName] = this.Equip_equipEvolutionModelInstance;
			this.m_ids[248] = this.Equip_equipEvolutionModelInstance;
			this.Quality_itemQualityModelInstance = new Quality_itemQualityModel();
			this.m_localModels[Quality_itemQualityModel.fileName] = this.Quality_itemQualityModelInstance;
			this.m_ids[249] = this.Quality_itemQualityModelInstance;
			this.Collection_starColorModelInstance = new Collection_starColorModel();
			this.m_localModels[Collection_starColorModel.fileName] = this.Collection_starColorModelInstance;
			this.m_ids[250] = this.Collection_starColorModelInstance;
			this.Fishing_areaModelInstance = new Fishing_areaModel();
			this.m_localModels[Fishing_areaModel.fileName] = this.Fishing_areaModelInstance;
			this.m_ids[251] = this.Fishing_areaModelInstance;
			this.ChapterActivity_RankRewardModelInstance = new ChapterActivity_RankRewardModel();
			this.m_localModels[ChapterActivity_RankRewardModel.fileName] = this.ChapterActivity_RankRewardModelInstance;
			this.m_ids[252] = this.ChapterActivity_RankRewardModelInstance;
			this.Map_EventPointBottomModelInstance = new Map_EventPointBottomModel();
			this.m_localModels[Map_EventPointBottomModel.fileName] = this.Map_EventPointBottomModelInstance;
			this.m_ids[253] = this.Map_EventPointBottomModelInstance;
			this.Equip_updateLevelModelInstance = new Equip_updateLevelModel();
			this.m_localModels[Equip_updateLevelModel.fileName] = this.Equip_updateLevelModelInstance;
			this.m_ids[254] = this.Equip_updateLevelModelInstance;
			this.IAP_LevelFundModelInstance = new IAP_LevelFundModel();
			this.m_localModels[IAP_LevelFundModel.fileName] = this.IAP_LevelFundModelInstance;
			this.m_ids[255] = this.IAP_LevelFundModelInstance;
			this.SignIn_SignInModelInstance = new SignIn_SignInModel();
			this.m_localModels[SignIn_SignInModel.fileName] = this.SignIn_SignInModelInstance;
			this.m_ids[256] = this.SignIn_SignInModelInstance;
			this.Mount_mountStageModelInstance = new Mount_mountStageModel();
			this.m_localModels[Mount_mountStageModel.fileName] = this.Mount_mountStageModelInstance;
			this.m_ids[257] = this.Mount_mountStageModelInstance;
			this.Relic_groupModelInstance = new Relic_groupModel();
			this.m_localModels[Relic_groupModel.fileName] = this.Relic_groupModelInstance;
			this.m_ids[258] = this.Relic_groupModelInstance;
			this.ChestList_ChestListModelInstance = new ChestList_ChestListModel();
			this.m_localModels[ChestList_ChestListModel.fileName] = this.ChestList_ChestListModelInstance;
			this.m_ids[259] = this.ChestList_ChestListModelInstance;
			this.GameConfig_ConfigModelInstance = new GameConfig_ConfigModel();
			this.m_localModels[GameConfig_ConfigModel.fileName] = this.GameConfig_ConfigModelInstance;
			this.m_ids[260] = this.GameConfig_ConfigModelInstance;
			this.GameSkillBuild_skillTagModelInstance = new GameSkillBuild_skillTagModel();
			this.m_localModels[GameSkillBuild_skillTagModel.fileName] = this.GameSkillBuild_skillTagModelInstance;
			this.m_ids[261] = this.GameSkillBuild_skillTagModelInstance;
			this.GameSkill_fireBulletModelInstance = new GameSkill_fireBulletModel();
			this.m_localModels[GameSkill_fireBulletModel.fileName] = this.GameSkill_fireBulletModelInstance;
			this.m_ids[262] = this.GameSkill_fireBulletModelInstance;
			this.IAP_PushPacksModelInstance = new IAP_PushPacksModel();
			this.m_localModels[IAP_PushPacksModel.fileName] = this.IAP_PushPacksModelInstance;
			this.m_ids[263] = this.IAP_PushPacksModelInstance;
			this.Scene_tableModelInstance = new Scene_tableModel();
			this.m_localModels[Scene_tableModel.fileName] = this.Scene_tableModelInstance;
			this.m_ids[264] = this.Scene_tableModelInstance;
			this.GameSkillBuild_firstModelInstance = new GameSkillBuild_firstModel();
			this.m_localModels[GameSkillBuild_firstModel.fileName] = this.GameSkillBuild_firstModelInstance;
			this.m_ids[265] = this.GameSkillBuild_firstModelInstance;
			this.Levels_tableModelInstance = new Levels_tableModel();
			this.m_localModels[Levels_tableModel.fileName] = this.Levels_tableModelInstance;
			this.m_ids[266] = this.Levels_tableModelInstance;
			this.GameBullet_bulletModelInstance = new GameBullet_bulletModel();
			this.m_localModels[GameBullet_bulletModel.fileName] = this.GameBullet_bulletModelInstance;
			this.m_ids[267] = this.GameBullet_bulletModelInstance;
			this.GameCamera_ShakeModelInstance = new GameCamera_ShakeModel();
			this.m_localModels[GameCamera_ShakeModel.fileName] = this.GameCamera_ShakeModelInstance;
			this.m_ids[268] = this.GameCamera_ShakeModelInstance;
			this.HeroLevelup_HeroLevelupModelInstance = new HeroLevelup_HeroLevelupModel();
			this.m_localModels[HeroLevelup_HeroLevelupModel.fileName] = this.HeroLevelup_HeroLevelupModelInstance;
			this.m_ids[269] = this.HeroLevelup_HeroLevelupModelInstance;
			this.TGASource_CostModelInstance = new TGASource_CostModel();
			this.m_localModels[TGASource_CostModel.fileName] = this.TGASource_CostModelInstance;
			this.m_ids[270] = this.TGASource_CostModelInstance;
			this.Guild_guildTaskModelInstance = new Guild_guildTaskModel();
			this.m_localModels[Guild_guildTaskModel.fileName] = this.Guild_guildTaskModelInstance;
			this.m_ids[271] = this.Guild_guildTaskModelInstance;
		}

		public override void DeInitialiseLocalModels()
		{
			base.DeInitialiseLocalModels();
		}

		private Guide_guideModel Guide_guideModelInstance;

		private GameSkill_skillModel GameSkill_skillModelInstance;

		private ActivityTurntable_TurntableRewardModel ActivityTurntable_TurntableRewardModelInstance;

		private AutoProcess_monsterModel AutoProcess_monsterModelInstance;

		private ArtSkin_equipSkinModel ArtSkin_equipSkinModelInstance;

		private ArtMember_clothesModel ArtMember_clothesModelInstance;

		private Artifact_advanceArtifactModel Artifact_advanceArtifactModelInstance;

		private GameMember_gameHoverModel GameMember_gameHoverModelInstance;

		private Vip_vipModel Vip_vipModelInstance;

		private Equip_equipModel Equip_equipModelInstance;

		private IAP_GiftPacksModel IAP_GiftPacksModelInstance;

		private Shop_ShopSellModel Shop_ShopSellModelInstance;

		private Language_languagetableModel Language_languagetableModelInstance;

		private TGASource_GetModel TGASource_GetModelInstance;

		private GuildBOSS_guildBossMonsterModel GuildBOSS_guildBossMonsterModelInstance;

		private Module_moduleInfoModel Module_moduleInfoModelInstance;

		private GuildRace_levelModel GuildRace_levelModelInstance;

		private ChapterActivity_ChapterObjModel ChapterActivity_ChapterObjModelInstance;

		private ArtBullet_BulletModel ArtBullet_BulletModelInstance;

		private Collection_collectionModel Collection_collectionModelInstance;

		private Task_WeeklyActiveModel Task_WeeklyActiveModelInstance;

		private GameBullet_bulletTypeModel GameBullet_bulletTypeModelInstance;

		private Atlas_atlasModel Atlas_atlasModelInstance;

		private Dungeon_DungeonBaseModel Dungeon_DungeonBaseModelInstance;

		private Achievements_AchievementsModel Achievements_AchievementsModelInstance;

		private Mining_oreBuildModel Mining_oreBuildModelInstance;

		private RogueDungeon_endEventModel RogueDungeon_endEventModelInstance;

		private GameMember_memberModel GameMember_memberModelInstance;

		private Equip_equipTypeModel Equip_equipTypeModelInstance;

		private ArtBuff_BuffModel ArtBuff_BuffModelInstance;

		private GuildBOSS_guildBossBoxModel GuildBOSS_guildBossBoxModelInstance;

		private ChapterMiniGame_paySlotRewardModel ChapterMiniGame_paySlotRewardModelInstance;

		private SevenDay_SevenDayActiveRewardModel SevenDay_SevenDayActiveRewardModelInstance;

		private Vip_dataModel Vip_dataModelInstance;

		private CrossArena_CrossArenaChallengeListRuleModel CrossArena_CrossArenaChallengeListRuleModelInstance;

		private TowerChallenge_TowerLevelModel TowerChallenge_TowerLevelModelInstance;

		private WorldBoss_RankRewardModel WorldBoss_RankRewardModelInstance;

		private ChainPacks_PushChainActvModel ChainPacks_PushChainActvModelInstance;

		private SevenDay_SevenDayPayModel SevenDay_SevenDayPayModelInstance;

		private Sociality_ReportModel Sociality_ReportModelInstance;

		private AutoProcess_chapterModel AutoProcess_chapterModelInstance;

		private Attribute_AttrTextModel Attribute_AttrTextModelInstance;

		private GameSkillBuild_skillBuildModel GameSkillBuild_skillBuildModelInstance;

		private Shop_ShopActivityModel Shop_ShopActivityModelInstance;

		private IAP_BattlePassRewardModel IAP_BattlePassRewardModelInstance;

		private ServerList_serverGropModel ServerList_serverGropModelInstance;

		private ChapterMiniGame_singleSlotRewardModel ChapterMiniGame_singleSlotRewardModelInstance;

		private CommonActivity_ConsumeObjModel CommonActivity_ConsumeObjModelInstance;

		private Mining_showRateModel Mining_showRateModelInstance;

		private ArtMap_MapModel ArtMap_MapModelInstance;

		private Chapter_stageUpgradeModel Chapter_stageUpgradeModelInstance;

		private Chapter_surpriseBuildModel Chapter_surpriseBuildModelInstance;

		private GuildBOSS_guildBossTaskModel GuildBOSS_guildBossTaskModelInstance;

		private Guild_guildGiftLevelModel Guild_guildGiftLevelModelInstance;

		private IntegralShop_dataModel IntegralShop_dataModelInstance;

		private TalentLegacy_talentLegacyNodeModel TalentLegacy_talentLegacyNodeModelInstance;

		private Shop_AdModel Shop_AdModelInstance;

		private Pet_PetEntryModel Pet_PetEntryModelInstance;

		private Guild_guildEventModel Guild_guildEventModelInstance;

		private IAP_MonthCardModel IAP_MonthCardModelInstance;

		private Fishing_fishingModel Fishing_fishingModelInstance;

		private Fishing_fishMoveModel Fishing_fishMoveModelInstance;

		private IAP_BattlePassModel IAP_BattlePassModelInstance;

		private ChapterActivity_BattlepassModel ChapterActivity_BattlepassModelInstance;

		private ArtHover_hoverModel ArtHover_hoverModelInstance;

		private WorldBoss_SubsectionModel WorldBoss_SubsectionModelInstance;

		private Guild_guildPowerModel Guild_guildPowerModelInstance;

		private RogueDungeon_monsterEntryModel RogueDungeon_monsterEntryModelInstance;

		private Quality_guildBossQualityModel Quality_guildBossQualityModelInstance;

		private Relic_starUpModel Relic_starUpModelInstance;

		private LanguageCN_languagetableModel LanguageCN_languagetableModelInstance;

		private Artifact_artifactStageModel Artifact_artifactStageModelInstance;

		private ChapterMiniGame_paySlotBaseModel ChapterMiniGame_paySlotBaseModelInstance;

		private ItemResources_itemgetModel ItemResources_itemgetModelInstance;

		private Collection_collectionSuitModel Collection_collectionSuitModelInstance;

		private Dungeon_DungeonLevelModel Dungeon_DungeonLevelModelInstance;

		private Guild_guildSignInModel Guild_guildSignInModelInstance;

		private GuildBOSS_rankRewardsModel GuildBOSS_rankRewardsModelInstance;

		private CommonActivity_RankObjModel CommonActivity_RankObjModelInstance;

		private CrossArena_CrossArenaLevelModel CrossArena_CrossArenaLevelModelInstance;

		private TalentLegacy_talentLegacyEffectModel TalentLegacy_talentLegacyEffectModelInstance;

		private ArtSkill_SkillModel ArtSkill_SkillModelInstance;

		private Emoji_EmojiModel Emoji_EmojiModelInstance;

		private IAP_ChapterPacksModel IAP_ChapterPacksModelInstance;

		private ChapterActivity_ModelModel ChapterActivity_ModelModelInstance;

		private Relic_updateLevelModel Relic_updateLevelModelInstance;

		private Mining_qualityModelModel Mining_qualityModelModelInstance;

		private IAP_PurchaseModel IAP_PurchaseModelInstance;

		private GameMember_npcFunctionModel GameMember_npcFunctionModelInstance;

		private ArtMember_morphModel ArtMember_morphModelInstance;

		private Fishing_fishRodModel Fishing_fishRodModelInstance;

		private ArtHit_HitModel ArtHit_HitModelInstance;

		private Guild_guildcontributeModel Guild_guildcontributeModelInstance;

		private TGASource_ADModel TGASource_ADModelInstance;

		private SevenDay_SevenDayTaskModel SevenDay_SevenDayTaskModelInstance;

		private BattleMain_waveModel BattleMain_waveModelInstance;

		private TalentNew_talentMegaStageModel TalentNew_talentMegaStageModelInstance;

		private Avatar_AvatarModel Avatar_AvatarModelInstance;

		private Pet_petCollectionModel Pet_petCollectionModelInstance;

		private RogueDungeon_rogueDungeonModel RogueDungeon_rogueDungeonModelInstance;

		private Chapter_slotBuildModel Chapter_slotBuildModelInstance;

		private ChapterActivity_BattlepassQualityModel ChapterActivity_BattlepassQualityModelInstance;

		private ChapterActivity_ChapterActivityModel ChapterActivity_ChapterActivityModelInstance;

		private IAP_DiamondPacksModel IAP_DiamondPacksModelInstance;

		private MainLevelReward_MainLevelChestModel MainLevelReward_MainLevelChestModelInstance;

		private Shop_SummonPoolModel Shop_SummonPoolModelInstance;

		private Pet_PetTrainingProbModel Pet_PetTrainingProbModelInstance;

		private TalentNew_talentEvolutionModel TalentNew_talentEvolutionModelInstance;

		private MonsterCfgOld_monsterCfgOldModel MonsterCfgOld_monsterCfgOldModelInstance;

		private TGASource_PageModel TGASource_PageModelInstance;

		private Map_mapModel Map_mapModelInstance;

		private AutoProcess_skillModel AutoProcess_skillModelInstance;

		private ActivityTurntable_TurntablePayModel ActivityTurntable_TurntablePayModelInstance;

		private ChapterActivity_ActvTurntableRewardModel ChapterActivity_ActvTurntableRewardModelInstance;

		private ChestList_ChestRewardModel ChestList_ChestRewardModelInstance;

		private Map_floatingRandomModel Map_floatingRandomModelInstance;

		private ChapterMiniGame_turntableRewardModel ChapterMiniGame_turntableRewardModelInstance;

		private Const_ConstModel Const_ConstModelInstance;

		private GameSkill_skillConditionModel GameSkill_skillConditionModelInstance;

		private ArtRide_RideModel ArtRide_RideModelInstance;

		private ChainPacks_ChainPacksModel ChainPacks_ChainPacksModelInstance;

		private TalentLegacy_careerModel TalentLegacy_careerModelInstance;

		private LanguageRaft_languagetableModel LanguageRaft_languagetableModelInstance;

		private AutoProcess_skillRandomModel AutoProcess_skillRandomModelInstance;

		private ServerList_serverListModel ServerList_serverListModelInstance;

		private IAP_platformIDModel IAP_platformIDModelInstance;

		private IAP_LevelFundRewardModel IAP_LevelFundRewardModelInstance;

		private ArtSagecraft_SagecraftModel ArtSagecraft_SagecraftModelInstance;

		private GuildBOSS_guildBossAttrModel GuildBOSS_guildBossAttrModelInstance;

		private GameBuff_buffTypeModel GameBuff_buffTypeModelInstance;

		private Pet_petSummonModel Pet_petSummonModelInstance;

		private Chapter_skillExpModel Chapter_skillExpModelInstance;

		private Mining_oreQualityModel Mining_oreQualityModelInstance;

		private ArtMember_memberModel ArtMember_memberModelInstance;

		private MainLevelReward_AFKrewardModel MainLevelReward_AFKrewardModelInstance;

		private NewWorld_newWorldRankModel NewWorld_newWorldRankModelInstance;

		private CommonActivity_CommonActivityModel CommonActivity_CommonActivityModelInstance;

		private Pet_petLevelModel Pet_petLevelModelInstance;

		private Function_FunctionModel Function_FunctionModelInstance;

		private Mining_oreResModel Mining_oreResModelInstance;

		private BattleMain_skillModel BattleMain_skillModelInstance;

		private TowerChallenge_TowerModel TowerChallenge_TowerModelInstance;

		private ItemResources_jumpResourceModel ItemResources_jumpResourceModelInstance;

		private ArtAnimation_animationModel ArtAnimation_animationModelInstance;

		private Quality_equipQualityModel Quality_equipQualityModelInstance;

		private GuildBOSS_guildBossModel GuildBOSS_guildBossModelInstance;

		private Item_ItemModel Item_ItemModelInstance;

		private Pet_PetTrainingModel Pet_PetTrainingModelInstance;

		private Pet_petModel Pet_petModelInstance;

		private CrossArena_CrossArenaTimeModel CrossArena_CrossArenaTimeModelInstance;

		private CrossArena_CrossArenaRobotModel CrossArena_CrossArenaRobotModelInstance;

		private TGASource_IAPModel TGASource_IAPModelInstance;

		private Guild_guildConstModel Guild_guildConstModelInstance;

		private GameBuff_buffModel GameBuff_buffModelInstance;

		private Box_boxBaseModel Box_boxBaseModelInstance;

		private ArtHover_hoverTextModel ArtHover_hoverTextModelInstance;

		private Chapter_boxBuildModel Chapter_boxBuildModelInstance;

		private GameSkill_skillTypeModel GameSkill_skillTypeModelInstance;

		private NewWorld_newWorldTaskModel NewWorld_newWorldTaskModelInstance;

		private Guild_guildGiftModel Guild_guildGiftModelInstance;

		private Chapter_eventPointModel Chapter_eventPointModelInstance;

		private Chapter_sweepModel Chapter_sweepModelInstance;

		private ActivityTurntable_ActivityTurntableModel ActivityTurntable_ActivityTurntableModelInstance;

		private Task_DailyTaskModel Task_DailyTaskModelInstance;

		private Artifact_artifactLevelModel Artifact_artifactLevelModelInstance;

		private Sound_soundModel Sound_soundModelInstance;

		private IntegralShop_goodsModel IntegralShop_goodsModelInstance;

		private Ride_RideModel Ride_RideModelInstance;

		private Chapter_eventTypeModel Chapter_eventTypeModelInstance;

		private ItemGift_ItemGiftModel ItemGift_ItemGiftModelInstance;

		private ChapterActivity_RankActivityModel ChapterActivity_RankActivityModelInstance;

		private GameMember_skinModel GameMember_skinModelInstance;

		private TicketExchange_ExchangeModel TicketExchange_ExchangeModelInstance;

		private Quality_petQualityModel Quality_petQualityModelInstance;

		private Avatar_SceneSkinModel Avatar_SceneSkinModelInstance;

		private Guild_guildDonationModel Guild_guildDonationModelInstance;

		private ChapterActivity_ActvTurntableDetailModel ChapterActivity_ActvTurntableDetailModelInstance;

		private BattleMain_chapterModel BattleMain_chapterModelInstance;

		private GameBuff_overlayTypeModel GameBuff_overlayTypeModelInstance;

		private GuildRace_baseRaceModel GuildRace_baseRaceModelInstance;

		private GuildBOSS_guildSubectionModel GuildBOSS_guildSubectionModelInstance;

		private Guild_guilddairyModel Guild_guilddairyModelInstance;

		private ChainPacks_ChainActvModel ChainPacks_ChainActvModelInstance;

		private Guild_guildShopModel Guild_guildShopModelInstance;

		private ChapterMiniGame_turntableBaseModel ChapterMiniGame_turntableBaseModelInstance;

		private Relic_dataModel Relic_dataModelInstance;

		private Collection_collectionStarModel Collection_collectionStarModelInstance;

		private LanguageRaft_languageTabModel LanguageRaft_languageTabModelInstance;

		private Shop_ShopModel Shop_ShopModelInstance;

		private IAP_pushIapModel IAP_pushIapModelInstance;

		private GameSkill_skillSelectModel GameSkill_skillSelectModelInstance;

		private CommonActivity_DropObjModel CommonActivity_DropObjModelInstance;

		private Chapter_eventResModel Chapter_eventResModelInstance;

		private CrossArena_CrossArenaModel CrossArena_CrossArenaModelInstance;

		private Equip_skillModel Equip_skillModelInstance;

		private GameSkill_skillAnimationModel GameSkill_skillAnimationModelInstance;

		private Chapter_eventItemModel Chapter_eventItemModelInstance;

		private WorldBoss_RewardModel WorldBoss_RewardModelInstance;

		private ActivityTurntable_TurntableQuestModel ActivityTurntable_TurntableQuestModelInstance;

		private ArtEffect_EffectModel ArtEffect_EffectModelInstance;

		private WorldBoss_WorldBossBoxModel WorldBoss_WorldBossBoxModelInstance;

		private GuildRace_opentimeModel GuildRace_opentimeModelInstance;

		private Chapter_attributeBuildModel Chapter_attributeBuildModelInstance;

		private Shop_EquipActivityModel Shop_EquipActivityModelInstance;

		private Item_dropLvModel Item_dropLvModelInstance;

		private ChapterActivity_BattlepassRewardModel ChapterActivity_BattlepassRewardModelInstance;

		private CommonActivity_ShopObjModel CommonActivity_ShopObjModelInstance;

		private Pet_petLevelEffectModel Pet_petLevelEffectModelInstance;

		private CommonActivity_PayObjModel CommonActivity_PayObjModelInstance;

		private GuildBOSS_guildBossStepModel GuildBOSS_guildBossStepModelInstance;

		private Guild_guildStyleModel Guild_guildStyleModelInstance;

		private WorldBoss_WorldBossModel WorldBoss_WorldBossModelInstance;

		private GameSkill_hitEffectModel GameSkill_hitEffectModelInstance;

		private Relic_relicModel Relic_relicModelInstance;

		private GameSkillBuild_skillRandomModel GameSkillBuild_skillRandomModelInstance;

		private Task_DailyActiveModel Task_DailyActiveModelInstance;

		private MonsterCfg_Old_monsterCfgModel MonsterCfg_Old_monsterCfgModelInstance;

		private ChapterMiniGame_slotTrainBuildModel ChapterMiniGame_slotTrainBuildModelInstance;

		private GameMember_aiTypeModel GameMember_aiTypeModelInstance;

		private Mining_miningBaseModel Mining_miningBaseModelInstance;

		private Mount_advanceMountModel Mount_advanceMountModelInstance;

		private TalentNew_talentModel TalentNew_talentModelInstance;

		private ChapterMiniGame_slotRewardModel ChapterMiniGame_slotRewardModelInstance;

		private Avatar_SkinModel Avatar_SkinModelInstance;

		private MonsterCfg_monsterCfgModel MonsterCfg_monsterCfgModelInstance;

		private Shop_SummonModel Shop_SummonModelInstance;

		private ChainPacks_ChainTypeModel ChainPacks_ChainTypeModelInstance;

		private Guild_guildLevelModel Guild_guildLevelModelInstance;

		private Chapter_chapterModel Chapter_chapterModelInstance;

		private CrossArena_CrossArenaRobotNameModel CrossArena_CrossArenaRobotNameModelInstance;

		private Fishing_fishModel Fishing_fishModelInstance;

		private Item_dropModel Item_dropModelInstance;

		private ChapterMiniGame_slotBaseModel ChapterMiniGame_slotBaseModelInstance;

		private ServerList_chatGropModel ServerList_chatGropModelInstance;

		private ChapterMiniGame_cardFlippingBaseModel ChapterMiniGame_cardFlippingBaseModelInstance;

		private Box_outputModel Box_outputModelInstance;

		private Equip_equipComposeModel Equip_equipComposeModelInstance;

		private Quality_collectionQualityModel Quality_collectionQualityModelInstance;

		private Guild_guildLanguageModel Guild_guildLanguageModelInstance;

		private GuildBOSS_guildBossSeasonRewardModel GuildBOSS_guildBossSeasonRewardModelInstance;

		private Mining_bonusGameModel Mining_bonusGameModelInstance;

		private ChapterMiniGame_singleSlotModel ChapterMiniGame_singleSlotModelInstance;

		private CrossArena_CrossArenaRewardModel CrossArena_CrossArenaRewardModelInstance;

		private Pet_petSkillModel Pet_petSkillModelInstance;

		private ChapterActivity_ActvTurntableBaseModel ChapterActivity_ActvTurntableBaseModelInstance;

		private Item_battleModel Item_battleModelInstance;

		private Mount_mountLevelModel Mount_mountLevelModelInstance;

		private Equip_equipSkillModel Equip_equipSkillModelInstance;

		private Equip_equipEvolutionModel Equip_equipEvolutionModelInstance;

		private Quality_itemQualityModel Quality_itemQualityModelInstance;

		private Collection_starColorModel Collection_starColorModelInstance;

		private Fishing_areaModel Fishing_areaModelInstance;

		private ChapterActivity_RankRewardModel ChapterActivity_RankRewardModelInstance;

		private Map_EventPointBottomModel Map_EventPointBottomModelInstance;

		private Equip_updateLevelModel Equip_updateLevelModelInstance;

		private IAP_LevelFundModel IAP_LevelFundModelInstance;

		private SignIn_SignInModel SignIn_SignInModelInstance;

		private Mount_mountStageModel Mount_mountStageModelInstance;

		private Relic_groupModel Relic_groupModelInstance;

		private ChestList_ChestListModel ChestList_ChestListModelInstance;

		private GameConfig_ConfigModel GameConfig_ConfigModelInstance;

		private GameSkillBuild_skillTagModel GameSkillBuild_skillTagModelInstance;

		private GameSkill_fireBulletModel GameSkill_fireBulletModelInstance;

		private IAP_PushPacksModel IAP_PushPacksModelInstance;

		private Scene_tableModel Scene_tableModelInstance;

		private GameSkillBuild_firstModel GameSkillBuild_firstModelInstance;

		private Levels_tableModel Levels_tableModelInstance;

		private GameBullet_bulletModel GameBullet_bulletModelInstance;

		private GameCamera_ShakeModel GameCamera_ShakeModelInstance;

		private HeroLevelup_HeroLevelupModel HeroLevelup_HeroLevelupModelInstance;

		private TGASource_CostModel TGASource_CostModelInstance;

		private Guild_guildTaskModel Guild_guildTaskModelInstance;
	}
}
