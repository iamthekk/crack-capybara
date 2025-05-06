using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dxx.Guild;
using Framework;
using Framework.SDKManager;
using Habby.Mail.Data;
using LocalModels.Bean;
using Proto.Common;
using Server;
using UnityEngine;

namespace HotFix
{
	public class GameTGATools
	{
		public static GameTGATools Ins
		{
			get
			{
				if (GameTGATools.mToolIns == null)
				{
					GameTGATools.mToolIns = new GameTGATools();
				}
				return GameTGATools.mToolIns;
			}
		}

		public static void CreateTGA()
		{
			GameTGATools.mToolIns = new GameTGATools();
		}

		public static void DestroyTGA()
		{
			GameTGATools gameTGATools = GameTGATools.mToolIns;
			if (gameTGATools != null)
			{
				gameTGATools.OnDestroy();
			}
			GameTGATools.mToolIns = null;
		}

		private GameTGATools()
		{
			this.ClearChapterTempData();
			this.ViewOpenTimes.Clear();
			if (this._timerCoroutine != null)
			{
				SDKManager sdk = GameApp.SDK;
				if (sdk != null)
				{
					sdk.StopCoroutine(this._timerCoroutine);
				}
				this._timerCoroutine = null;
			}
			SDKManager sdk2 = GameApp.SDK;
			this._timerCoroutine = ((sdk2 != null) ? sdk2.StartCoroutine(this.TimerUpdate()) : null);
			if (this.mLoginDataModule == null)
			{
				this.mLoginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			}
		}

		private void OnDestroy()
		{
			if (this._timerCoroutine != null)
			{
				SDKManager sdk = GameApp.SDK;
				if (sdk != null)
				{
					sdk.StopCoroutine(this._timerCoroutine);
				}
				this._timerCoroutine = null;
			}
		}

		public void SetPublicInfo(Dictionary<string, object> dic)
		{
			if (dic == null)
			{
				return;
			}
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
				if (dataModule != null)
				{
					int num = dataModule.ChapterID * 100 + dataModule.MaxStage;
					dic["max_chapter_day"] = num;
				}
				if (GameApp.NetWork != null && GameApp.NetWork.m_serverID > 0U)
				{
					dic["server"] = GameApp.NetWork.m_serverID;
				}
				LoginDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule2 != null)
				{
					dic["uid"] = dataModule2.userId;
					dic["ab_group"] = ((dataModule2.abVersion == 0U) ? "A" : "B");
					dic["gems_hold"] = dataModule2.userCurrency.Diamonds;
					dic["gold_hold"] = dataModule2.userCurrency.Coins;
					dic["nick_name"] = dataModule2.NickName;
					dic["total_login_days"] = dataModule2.AccountTotalLoginDays;
					if (dataModule2.AccountCreateTime > 0L)
					{
						DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime((double)DxxTools.Time.lastServerTimestamp);
						DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)dataModule2.AccountCreateTime);
						int num2 = (int)(dateTime - dateTime2).TotalDays;
						dic["register_days"] = num2 + 1;
					}
					if (dataModule2.habbyMailBind)
					{
						string habbyID = dataModule2.HabbyID;
						if (!string.IsNullOrEmpty(habbyID))
						{
							dic["habby_id"] = habbyID;
						}
					}
					dic["title"] = dataModule2.AvatarTitle;
				}
				TicketDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
				if (dataModule3 != null)
				{
					dic["stamina_hold"] = (int)dataModule3.GetTicket(UserTicketKind.UserLife).NewNum;
				}
				AddAttributeDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				if (dataModule4 != null)
				{
					dic["battle_power"] = GameTGATools.NumberOffset((long)dataModule4.Combat, 10);
				}
				HeroDataModule dataModule5 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
				if (dataModule4 != null && dataModule5 != null)
				{
					CardData cardData = new CardData();
					cardData.CloneFrom(dataModule5.MainCardData);
					cardData.UpdateAttribute(dataModule4.AttributeDatas);
					long hpMax4UI = cardData.m_memberAttributeData.GetHpMax4UI();
					long attack4UI = cardData.m_memberAttributeData.GetAttack4UI();
					long defence4UI = cardData.m_memberAttributeData.GetDefence4UI();
					dic["current_hp"] = GameTGATools.NumberOffset(hpMax4UI, 10);
					dic["current_atk"] = GameTGATools.NumberOffset(attack4UI, 10);
					dic["current_def"] = GameTGATools.NumberOffset(defence4UI, 10);
				}
				TalentDataModule dataModule6 = GameApp.Data.GetDataModule(DataName.TalentDataModule);
				if (dataModule6 != null)
				{
					dic["talent_level"] = dataModule6.talentProgressData.curLevel;
				}
				IAPDataModule dataModule7 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				if (dataModule7 != null)
				{
					dic["is_ad_free"] = dataModule7.MonthCard.IsActivation(IAPMonthCardData.CardType.NoAd);
					dic["is_month"] = dataModule7.MonthCard.IsActivation(IAPMonthCardData.CardType.Month);
					dic["is_lifetime"] = dataModule7.MonthCard.IsActivation(IAPMonthCardData.CardType.Lifetime);
					dic["total_iap"] = dataModule7.AccountTotalCharge;
				}
				AdDataModule dataModule8 = GameApp.Data.GetDataModule(DataName.AdDataModule);
				if (dataModule8 != null)
				{
					dic["total_ad"] = this.GetADCount();
				}
				TowerDataModule dataModule9 = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				if (dataModule9 != null)
				{
					int num3 = dataModule9.CalculateShouldChallengeLevelID(dataModule9.CompleteTowerLevelId);
					TowerChallenge_Tower towerConfigByLevelId = dataModule9.GetTowerConfigByLevelId(num3);
					int towerConfigNum = dataModule9.GetTowerConfigNum(towerConfigByLevelId);
					int levelNumByLevelId = dataModule9.GetLevelNumByLevelId(num3);
					dic["max_tower_stage"] = towerConfigNum * 100 + levelNumByLevelId;
				}
				WorldBossDataModule dataModule10 = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
				if (dataModule10 != null && dataModule10.SelfRank > 0)
				{
					WorldBoss_WorldBoss worldBoss_WorldBoss = GameApp.Table.GetManager().GetWorldBoss_WorldBoss(dataModule10.ChapterId);
					if (worldBoss_WorldBoss != null)
					{
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						dictionary["boss_id"] = dataModule10.ChapterId;
						dictionary["rank"] = dataModule10.SelfRank;
						dictionary["damge_total"] = dataModule10.TotalDamage;
						dictionary["group_id"] = dataModule10.GroupIndex;
						WorldBoss_Subsection worldBoss_Subsection = GameApp.Table.GetManager().GetWorldBoss_Subsection(dataModule10.RankLevel);
						if (worldBoss_Subsection != null)
						{
							dictionary["world_boss_grade"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, worldBoss_Subsection.languageId);
						}
						dic["word_boss_rank"] = dictionary;
					}
				}
				GuildShareDetailData guildDetailData = GuildSDKManager.Instance.GuildInfo.GuildDetailData;
				if (guildDetailData != null && !string.IsNullOrEmpty(guildDetailData.GuildID))
				{
					dic["guild_id"] = guildDetailData.GuildID;
				}
			}
		}

		public bool SetAllBagItems(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (!flag)
			{
				return true;
			}
			List<PropData> bagList = GameApp.Data.GetDataModule<PropDataModule>(118).GetBagList(PropShowType.eProp);
			List<object> list = new List<object>();
			foreach (PropData propData in bagList)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["item_id"] = propData.id;
				dictionary["item_name"] = this.GetItemName((int)propData.id);
				dictionary["item_count"] = propData.count;
				list.Add(dictionary);
			}
			if (list.Count > 0)
			{
				dic[key] = list;
				return true;
			}
			return false;
		}

		public bool SetAllCollections(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (!flag)
			{
				return true;
			}
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			List<object> list = new List<object>();
			foreach (CollectionData collectionData in dataModule.collectionDict.Values)
			{
				if (collectionData.collectionType == 1U)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["item_id"] = collectionData.itemId;
					dictionary["item_name"] = this.GetItemName(collectionData.itemId);
					dictionary["item_rarity"] = CollectionHelper.GetRarityName(collectionData.rarity);
					dictionary["item_level"] = collectionData.collectionStar;
					list.Add(dictionary);
				}
			}
			if (list.Count > 0)
			{
				dic[key] = list;
				return true;
			}
			return false;
		}

		public bool SetAllPets(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (!flag)
			{
				return true;
			}
			List<PetData> petList = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetList(EPetFilterType.All);
			List<object> list = new List<object>();
			foreach (PetData petData in petList)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["pet_id"] = petData.petId;
				dictionary["pet_name"] = this.GetItemName(petData.petId);
				Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(petData.quality);
				dictionary["pet_rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.nameID);
				dictionary["pet_grade"] = petData.level;
				dictionary["pet_shards"] = petData.fragmentCount;
				list.Add(dictionary);
			}
			if (list.Count > 0)
			{
				dic[key] = list;
				return true;
			}
			return false;
		}

		public void SetEquipPets(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				List<PetData> petList = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetList(EPetFilterType.Assist);
				List<object> list = new List<object>();
				foreach (PetData petData in petList)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["pet_id"] = petData.petId;
					dictionary["pet_name"] = this.GetItemName(petData.petId);
					Quality_petQuality elementById = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(petData.quality);
					dictionary["rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.nameID);
					dictionary["level"] = petData.level;
					List<MergeAttributeData> petLevelMergeAttributeData = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(petData.petId)
						.GetPetLevelMergeAttributeData(petData.level, GameApp.Table.GetManager());
					MemberAttributeData memberAttributeData = new MemberAttributeData();
					memberAttributeData.MergeAttributes(petLevelMergeAttributeData, false);
					dictionary["hp"] = memberAttributeData.GetHpMax4UI();
					dictionary["atk"] = memberAttributeData.GetAttack4UI();
					dictionary["def"] = memberAttributeData.GetDefence4UI();
					list.Add(dictionary);
				}
				if (list.Count > 0)
				{
					dic[key] = list;
				}
			}
		}

		public bool SetAllEquipments(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (!flag)
			{
				return true;
			}
			EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			List<object> list = new List<object>();
			foreach (EquipData equipData in dataModule.m_equipDatas.Values)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["equipment_id"] = equipData.id;
				dictionary["equipment_name"] = this.GetItemName((int)equipData.id);
				dictionary["equipment_rarity"] = this.ItemQualityToColor(equipData.qualityColor);
				dictionary["equipment_grade"] = equipData.composeId;
				dictionary["equipment_level"] = equipData.level;
				dictionary["count"] = equipData.count;
				list.Add(dictionary);
			}
			if (list.Count > 0)
			{
				dic[key] = list;
				return true;
			}
			return false;
		}

		public void SetEquipEquipments(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
				Dictionary<ulong, EquipData> equipDatas = dataModule.m_equipDatas;
				List<object> list = new List<object>();
				foreach (ulong num in dataModule.m_equipDressRowIds)
				{
					if (num > 0UL && equipDatas.ContainsKey(num))
					{
						EquipData equipData = equipDatas[num];
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						dictionary["equipment_id"] = equipData.id;
						dictionary["equipment_name"] = this.GetItemName((int)equipData.id);
						dictionary["equipment_rarity"] = this.ItemQualityToColor(equipData.qualityColor);
						dictionary["equipment_grade"] = equipData.composeId;
						dictionary["equipment_level"] = equipData.level;
						list.Add(dictionary);
					}
				}
				if (list.Count > 0)
				{
					dic[key] = list;
				}
			}
		}

		public void SetAllMounts(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				List<object> list = new List<object>();
				MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
				MountInfo mountInfo = dataModule.MountInfo;
				MountBasicData currentBasicData = dataModule.GetCurrentBasicData();
				if (mountInfo != null && currentBasicData != null)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["mount_id"] = currentBasicData.MemberConfig.id;
					dictionary["mount_name"] = this.GetItemName(currentBasicData.MemberConfig.id);
					dictionary["mount_stage"] = mountInfo.Stage;
					dictionary["mount_grade"] = mountInfo.Level;
					list.Add(dictionary);
				}
				List<MountAdvanceData> advanceDataList = dataModule.GetAdvanceDataList();
				if (advanceDataList != null && advanceDataList.Count > 0)
				{
					foreach (MountAdvanceData mountAdvanceData in advanceDataList)
					{
						if (mountAdvanceData.IsUnlock)
						{
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
							dictionary2["mount_id"] = mountAdvanceData.MemberConfig.id;
							dictionary2["mount_name"] = this.GetItemName(mountAdvanceData.MemberConfig.id);
							dictionary2["rare_star"] = mountAdvanceData.Star;
							dictionary2["mount_rarity"] = this.ItemQualityToColor(mountAdvanceData.Config.quality);
							list.Add(dictionary2);
						}
					}
				}
				if (list.Count > 0)
				{
					dic[key] = list;
				}
			}
		}

		public void SetAllArtifacts(Dictionary<string, object> dic, string key)
		{
			SDKManager sdk = GameApp.SDK;
			bool flag;
			if (sdk == null)
			{
				flag = false;
			}
			else
			{
				SDKManager.SDKTGA analyze = sdk.Analyze;
				bool? flag2 = ((analyze != null) ? new bool?(analyze.IsLogin) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				List<object> list = new List<object>();
				ArtifactDataModule dataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
				ArtifactInfo artifactInfo = dataModule.ArtifactInfo;
				ArtifactBasicData currentBasicData = dataModule.GetCurrentBasicData();
				if (artifactInfo != null && currentBasicData != null)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["legend_id"] = currentBasicData.ItemConfig.id;
					dictionary["legend_name"] = this.GetItemName(currentBasicData.ItemConfig.id);
					dictionary["legend_stage"] = artifactInfo.Stage;
					dictionary["legend_grade"] = artifactInfo.Level;
					list.Add(dictionary);
				}
				List<ArtifactAdvanceData> advanceDataList = dataModule.GetAdvanceDataList();
				if (advanceDataList != null && advanceDataList.Count > 0)
				{
					foreach (ArtifactAdvanceData artifactAdvanceData in advanceDataList)
					{
						if (artifactAdvanceData.IsUnlock)
						{
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
							dictionary2["legend_id"] = artifactAdvanceData.ItemConfig.id;
							dictionary2["legend_name"] = this.GetItemName(artifactAdvanceData.ItemConfig.id);
							dictionary2["rare_star"] = artifactAdvanceData.Star;
							dictionary2["legend_rarity"] = this.ItemQualityToColor(artifactAdvanceData.Config.quality);
							list.Add(dictionary2);
						}
					}
				}
				if (list.Count > 0)
				{
					dic[key] = list;
				}
			}
		}

		public void SetGameEventNodeAttParam(Dictionary<string, object> dic, string key, List<NodeAttParam> attList)
		{
			if (attList.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (NodeAttParam nodeAttParam in attList)
				{
					if (nodeAttParam.attType == GameEventAttType.AttackPercent || nodeAttParam.attType == GameEventAttType.RecoverHpRate || nodeAttParam.attType == GameEventAttType.HPMaxPercent || nodeAttParam.attType == GameEventAttType.DefencePercent || nodeAttParam.attType == GameEventAttType.CampHpRate)
					{
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						dictionary["buff_type"] = ((nodeAttParam.baseCount > 0.0) ? "buff" : "debuff");
						List<AttributeTypeData> attParamList = NodeAttParam.GetAttParamList(new List<NodeAttParam> { nodeAttParam });
						dictionary["name"] = attParamList[0].m_tgaValue;
						list.Add(dictionary);
					}
				}
				if (list.Count > 0)
				{
					dic[key] = list;
				}
			}
		}

		public void SetGameRewardProperty(Dictionary<string, object> dic, string key, List<NodeAttParam> attList, List<NodeScoreParam> scoreList)
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			foreach (NodeAttParam nodeAttParam in attList)
			{
				if (nodeAttParam.attType == GameEventAttType.Chips)
				{
					list.Add(nodeAttParam);
				}
			}
			if (list.Count > 0 || scoreList.Count > 0)
			{
				List<object> list2 = new List<object>();
				foreach (NodeAttParam nodeAttParam2 in attList)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["property_name"] = NodeAttParam.GetAttName_TGA(nodeAttParam2.attType, 0f);
					dictionary["count"] = nodeAttParam2.FinalCount;
					list2.Add(dictionary);
				}
				foreach (NodeScoreParam nodeScoreParam in scoreList)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2["property_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, nodeScoreParam.scoreNameId);
					dictionary2["count"] = nodeScoreParam.FinalCount;
					list2.Add(dictionary2);
				}
				if (list2.Count > 0)
				{
					dic[key] = list2;
				}
			}
		}

		public void SetGameEventNodeItemParam(Dictionary<string, object> dic, string key, List<NodeItemParam> itemList)
		{
			if (itemList.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (NodeItemParam nodeItemParam in itemList)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["item_id"] = nodeItemParam.itemId;
					dictionary["item_name"] = GameTGATools.Ins.GetItemName(nodeItemParam.itemId);
					dictionary["item_count"] = nodeItemParam.FinalCount;
					list.Add(dictionary);
				}
				dic[key] = list;
			}
		}

		public void SetGameEventSkillBuildData(Dictionary<string, object> dic, string key, List<GameEventSkillBuildData> skillList)
		{
			if (skillList.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (GameEventSkillBuildData gameEventSkillBuildData in skillList)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["skill_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, gameEventSkillBuildData.SkillConfig.nameID);
					dictionary["skill_rarity"] = GameTGATools.Ins.SkillQualityToColor(gameEventSkillBuildData.quality);
					list.Add(dictionary);
				}
				dic[key] = list;
			}
		}

		public void TryAddRewardDto_Item(RewardDto rewardDto, List<object> list)
		{
			if (rewardDto.Count == 0UL)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			ItemType itemType = (ItemType)elementById.itemType;
			if (itemType == ItemType.eEquip)
			{
				return;
			}
			if (itemType == ItemType.ePet)
			{
				return;
			}
			if (itemType == ItemType.eCollection)
			{
				return;
			}
			if (rewardDto.ConfigId == 1U || rewardDto.ConfigId == 2U || rewardDto.ConfigId == 4U)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = rewardDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)rewardDto.ConfigId);
			dictionary["item_count"] = rewardDto.Count;
			list.Add(dictionary);
		}

		public void TryAddCostDto_Item(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			ItemType itemType = (ItemType)elementById.itemType;
			if (itemType == ItemType.eEquip)
			{
				return;
			}
			if (itemType == ItemType.ePet)
			{
				return;
			}
			if (itemType == ItemType.eCollection)
			{
				return;
			}
			if (costDto.ConfigId == 1U || costDto.ConfigId == 2U || costDto.ConfigId == 4U)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = costDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)costDto.ConfigId);
			dictionary["item_count"] = Mathf.Abs((float)costDto.Count);
			list.Add(dictionary);
		}

		public void TryAddCostDto_AllItem(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId) == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = costDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)costDto.ConfigId);
			dictionary["item_count"] = Mathf.Abs((float)costDto.Count);
			list.Add(dictionary);
		}

		public void TryAddMailReward_Item(MailReward rewardDto, List<object> list)
		{
			int num = -1;
			if (!int.TryParse(rewardDto.id, out num) || num < 0)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			if (elementById == null)
			{
				return;
			}
			ItemType itemType = (ItemType)elementById.itemType;
			if (itemType == ItemType.eCurrency)
			{
				return;
			}
			if (itemType == ItemType.eEquip)
			{
				return;
			}
			if (itemType == ItemType.ePet)
			{
				return;
			}
			if (itemType == ItemType.eCollection)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = num;
			dictionary["item_name"] = this.GetItemName(num);
			dictionary["item_count"] = rewardDto.count;
			list.Add(dictionary);
		}

		public void TryAddRewardDto_AllItem(RewardDto rewardDto, List<object> list)
		{
			if (rewardDto.Count == 0UL)
			{
				return;
			}
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId) == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = rewardDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)rewardDto.ConfigId);
			dictionary["item_count"] = rewardDto.Count;
			list.Add(dictionary);
		}

		public void TryAddPetDto_AllItem(PetDto rewardDto, List<object> list)
		{
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId) == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = rewardDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)rewardDto.ConfigId);
			dictionary["item_count"] = 1;
			list.Add(dictionary);
		}

		public void TryAddMailReward_AllItem(MailReward rewardDto, List<object> list)
		{
			int num = -1;
			if (!int.TryParse(rewardDto.id, out num) || num < 0)
			{
				return;
			}
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num) == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = num;
			dictionary["item_name"] = this.GetItemName(num);
			dictionary["item_count"] = rewardDto.count;
			list.Add(dictionary);
		}

		public void TryAddRewardDto_Equipment(RewardDto rewardDto, List<object> list)
		{
			if (rewardDto.Count == 0UL)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (1 != elementById.itemType)
			{
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["equipment_id"] = rewardDto.ConfigId;
			dictionary["equipment_name"] = this.GetItemName((int)rewardDto.ConfigId);
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById3 != null)
			{
				dictionary["equipment_rarity"] = this.ItemQualityToColor(elementById3.quality);
			}
			if (elementById2 != null)
			{
				dictionary["equipment_grade"] = elementById2.composeId;
			}
			dictionary["equipment_level"] = 1;
			dictionary["count"] = rewardDto.Count;
			list.Add(dictionary);
		}

		public void TryAddEquipmentDto_Equipment(EquipmentDto equipmentDto, List<object> list)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)equipmentDto.EquipId);
			if (elementById == null)
			{
				return;
			}
			if (1 != elementById.itemType)
			{
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipmentDto.EquipId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["equipment_id"] = equipmentDto.EquipId;
			dictionary["equipment_name"] = this.GetItemName((int)equipmentDto.EquipId);
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)equipmentDto.EquipId);
			if (elementById3 != null)
			{
				dictionary["equipment_rarity"] = this.ItemQualityToColor(elementById3.quality);
			}
			if (elementById2 != null)
			{
				dictionary["equipment_grade"] = elementById2.composeId;
			}
			dictionary["equipment_level"] = equipmentDto.Level;
			dictionary["count"] = 1;
			list.Add(dictionary);
		}

		public void TryAddEquipData_Equipment(EquipData equipData, List<object> list)
		{
			if (equipData == null)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)equipData.id);
			if (elementById == null)
			{
				return;
			}
			if (1 != elementById.itemType)
			{
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipData.id);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["equipment_id"] = equipData.id;
			dictionary["equipment_name"] = this.GetItemName((int)equipData.id);
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)equipData.id);
			if (elementById3 != null)
			{
				dictionary["equipment_rarity"] = this.ItemQualityToColor(elementById3.quality);
			}
			if (elementById2 != null)
			{
				dictionary["equipment_grade"] = elementById2.composeId;
			}
			dictionary["equipment_level"] = equipData.level;
			dictionary["count"] = 1;
			list.Add(dictionary);
		}

		public void TryAddCostDto_Equipment(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (1 != elementById.itemType)
			{
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["equipment_id"] = costDto.ConfigId;
			dictionary["equipment_name"] = this.GetItemName((int)costDto.ConfigId);
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById3 != null)
			{
				dictionary["equipment_rarity"] = this.ItemQualityToColor(elementById3.quality);
			}
			if (elementById2 != null)
			{
				dictionary["equipment_grade"] = elementById2.composeId;
			}
			dictionary["equipment_level"] = 1;
			dictionary["count"] = Mathf.Abs((float)costDto.Count);
			list.Add(dictionary);
		}

		public void TryAddRewardDto_Pet(RewardDto rewardDto, List<object> list)
		{
			if (rewardDto.Count == 0UL)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (19 != elementById.itemType)
			{
				return;
			}
			Pet_pet elementById2 = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Quality_petQuality elementById3 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById2.quality);
			if (elementById3 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["pet_id"] = rewardDto.ConfigId;
			dictionary["pet_name"] = this.GetItemName((int)rewardDto.ConfigId);
			dictionary["pet_rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById3.nameID);
			dictionary["pet_level"] = 0;
			dictionary["pet_grade"] = 1;
			dictionary["pet_shards"] = rewardDto.Count;
			list.Add(dictionary);
		}

		public void TryAddCostDto_Pet(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (19 != elementById.itemType)
			{
				return;
			}
			Pet_pet elementById2 = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Quality_petQuality elementById3 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById2.quality);
			if (elementById3 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["pet_id"] = costDto.ConfigId;
			dictionary["pet_name"] = this.GetItemName((int)costDto.ConfigId);
			dictionary["pet_rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById3.nameID);
			dictionary["pet_level"] = 0;
			dictionary["pet_grade"] = 1;
			dictionary["pet_shards"] = costDto.Count;
			list.Add(dictionary);
		}

		public void TryAddRewardDto_Colliction(RewardDto rewardDto, List<object> list)
		{
			if (rewardDto.Count == 0UL)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (21 != elementById.itemType)
			{
				return;
			}
			Collection_collection elementById2 = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById((int)rewardDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = rewardDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)rewardDto.ConfigId);
			dictionary["item_rarity"] = CollectionHelper.GetRarityName(elementById2.rarity);
			dictionary["item_level"] = 0;
			list.Add(dictionary);
		}

		public void TryAddCostDto_Colliction(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (21 != elementById.itemType)
			{
				return;
			}
			Collection_collection elementById2 = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById((int)costDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = costDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)costDto.ConfigId);
			dictionary["item_rarity"] = CollectionHelper.GetRarityName(elementById2.rarity);
			dictionary["item_level"] = 0;
			list.Add(dictionary);
		}

		public void TryAddCollictionDto_Colliction(CollectionDto collectionDto, List<object> list)
		{
			if (collectionDto.CollecCount == 0U)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)collectionDto.ConfigId);
			if (elementById == null)
			{
				return;
			}
			if (21 != elementById.itemType)
			{
				return;
			}
			Collection_collection elementById2 = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById((int)collectionDto.ConfigId);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = collectionDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)collectionDto.ConfigId);
			dictionary["item_rarity"] = CollectionHelper.GetRarityName(elementById2.rarity);
			dictionary["item_level"] = 0;
			list.Add(dictionary);
		}

		public void TryAddMailReward_Equipment(MailReward rewardDto, List<object> list)
		{
			int num = -1;
			if (!int.TryParse(rewardDto.id, out num) || num < 0)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			if (elementById == null)
			{
				return;
			}
			if (1 != elementById.itemType)
			{
				return;
			}
			Equip_equip elementById2 = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(num);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["equipment_id"] = num;
			dictionary["equipment_name"] = this.GetItemName(num);
			Item_Item elementById3 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			if (elementById3 != null)
			{
				dictionary["equipment_rarity"] = this.ItemQualityToColor(elementById3.quality);
			}
			if (elementById2 != null)
			{
				dictionary["equipment_grade"] = elementById2.composeId;
			}
			dictionary["equipment_level"] = 1;
			dictionary["count"] = rewardDto.count;
			list.Add(dictionary);
		}

		public void TryAddMailReward_Pet(MailReward rewardDto, List<object> list)
		{
			int num = -1;
			if (!int.TryParse(rewardDto.id, out num) || num < 0)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			if (elementById == null)
			{
				return;
			}
			if (19 != elementById.itemType)
			{
				return;
			}
			Pet_pet elementById2 = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(num);
			if (elementById2 == null)
			{
				return;
			}
			Quality_petQuality elementById3 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById2.quality);
			if (elementById3 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["pet_id"] = num;
			dictionary["pet_name"] = this.GetItemName(num);
			dictionary["pet_rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById3.nameID);
			dictionary["pet_level"] = 0;
			dictionary["pet_grade"] = 1;
			dictionary["pet_shards"] = rewardDto.count;
			list.Add(dictionary);
		}

		public void TryAddMailReward_Colliction(MailReward rewardDto, List<object> list)
		{
			int num = -1;
			if (!int.TryParse(rewardDto.id, out num) || num < 0)
			{
				return;
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			if (elementById == null)
			{
				return;
			}
			if (21 != elementById.itemType)
			{
				return;
			}
			Collection_collection elementById2 = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetElementById(num);
			if (elementById2 == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = num;
			dictionary["item_name"] = this.GetItemName(num);
			dictionary["item_rarity"] = CollectionHelper.GetRarityName(elementById2.rarity);
			dictionary["item_level"] = 0;
			list.Add(dictionary);
		}

		public void TryAddPetDto(PetDto petDto, List<object> list)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["pet_id"] = petDto.ConfigId;
			dictionary["pet_name"] = this.GetItemName((int)petDto.ConfigId);
			Pet_pet elementById = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById((int)petDto.ConfigId);
			Quality_petQuality elementById2 = GameApp.Table.GetManager().GetQuality_petQualityModelInstance().GetElementById(elementById.quality);
			dictionary["pet_rarity"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById2.nameID);
			dictionary["pet_level"] = 0;
			dictionary["pet_grade"] = petDto.PetLv;
			dictionary["pet_shards"] = petDto.PetCount;
			list.Add(dictionary);
		}

		public void TryAddLegacyCostDto(CostDto costDto, List<object> list)
		{
			if (costDto.Count == 0L)
			{
				return;
			}
			if (GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)costDto.ConfigId) == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["item_id"] = costDto.ConfigId;
			dictionary["item_name"] = this.GetItemName((int)costDto.ConfigId);
			dictionary["item_count"] = costDto.Count;
			list.Add(dictionary);
		}

		public void TryAddLegacyRank(List<object> list)
		{
			TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			if (dataModule != null && dataModule.Top3User.Count > 0)
			{
				for (int i = 0; i < dataModule.Top3User.Count; i++)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary["player_uid"] = dataModule.Top3User[i].UserInfo.UserId.ToString();
					dictionary["nickname"] = dataModule.Top3User[i].UserInfo.NickName;
					dictionary["rank"] = dataModule.Top3User[i].Rank.ToString();
					list.Add(dictionary);
				}
			}
		}

		public void TryAddLegacyStudyFinishNode(List<object> list)
		{
			TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			if (dataModule != null)
			{
				TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = dataModule.OnGetTalentLegacyInfo();
				if (talentLegacyInfo != null)
				{
					foreach (TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo in talentLegacyInfo.CareerDic.Values)
					{
						Dictionary<string, object> dictionary = new Dictionary<string, object>();
						List<object> list2 = new List<object>();
						foreach (TalentLegacyDataModule.TalentLegacySkillInfo talentLegacySkillInfo in talentLegacyCareerInfo.SkillDic.Values)
						{
							if (dataModule.GetTalentLegacySkillCfgByLevel(talentLegacySkillInfo.TalentLegacyNodeId, talentLegacySkillInfo.Level + 1) == null)
							{
								list2.Add(talentLegacySkillInfo.TalentLegacyNodeId);
							}
						}
						if (list2.Count > 0)
						{
							TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(talentLegacyCareerInfo.CareerId);
							if (talentLegacy_career != null)
							{
								dictionary["skills"] = list2;
								dictionary["school"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, talentLegacy_career.nameID);
								list.Add(dictionary);
							}
						}
					}
				}
			}
		}

		public void TryAddLegacyEquipSkill(Dictionary<string, object> dic, string key)
		{
			List<object> list = new List<object>();
			TalentLegacyDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule);
			if (dataModule != null)
			{
				TalentLegacyDataModule.TalentLegacyInfo talentLegacyInfo = dataModule.OnGetTalentLegacyInfo();
				if (talentLegacyInfo != null)
				{
					foreach (TalentLegacyDataModule.TalentLegacyCareerInfo talentLegacyCareerInfo in talentLegacyInfo.CareerDic.Values)
					{
						if (talentLegacyInfo.SelectCareerId == talentLegacyCareerInfo.CareerId)
						{
							Dictionary<string, object> dictionary = new Dictionary<string, object>();
							List<object> list2 = new List<object>();
							for (int i = 0; i < talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList.Count; i++)
							{
								list2.Add(talentLegacyCareerInfo.AssemblyTalentLegacySkillIdList[i]);
							}
							if (list2.Count <= 0)
							{
								break;
							}
							TalentLegacy_career talentLegacy_career = GameApp.Table.GetManager().GetTalentLegacy_career(talentLegacyCareerInfo.CareerId);
							if (talentLegacy_career != null)
							{
								dictionary["skills"] = list2;
								dictionary["school"] = Singleton<LanguageManager>.Instance.GetInfoByID(talentLegacy_career.nameID);
								list.Add(dictionary);
								break;
							}
							break;
						}
					}
				}
			}
			if (list.Count > 0)
			{
				dic[key] = list;
			}
		}

		public string GetEventSizeTypeName(EventSizeType type)
		{
			if (type == EventSizeType.Normal)
			{
				return "普通";
			}
			if (type == EventSizeType.Fail)
			{
				return "运气不佳";
			}
			if (type == EventSizeType.MinorWin)
			{
				return "小吉";
			}
			if (type == EventSizeType.BigWin)
			{
				return "大吉";
			}
			if (type == EventSizeType.SuperWin)
			{
				return "SuperWin(未定义)";
			}
			if (type == EventSizeType.Activity)
			{
				return "惊喜活动";
			}
			return "";
		}

		public string ItemQualityToColor(int qualityColor)
		{
			if (qualityColor == 1)
			{
				return "白色";
			}
			if (qualityColor == 2)
			{
				return "绿色";
			}
			if (qualityColor == 3)
			{
				return "蓝色";
			}
			if (qualityColor == 4)
			{
				return "紫色";
			}
			if (qualityColor == 5)
			{
				return "橙色";
			}
			if (qualityColor == 6)
			{
				return "红色";
			}
			return "";
		}

		public string SkillQualityToColor(SkillBuildQuality quality)
		{
			if (quality == SkillBuildQuality.Gray)
			{
				return "白色";
			}
			if (quality == SkillBuildQuality.Gold)
			{
				return "金色";
			}
			if (quality == SkillBuildQuality.Red)
			{
				return "红色";
			}
			if (quality == SkillBuildQuality.Colorful)
			{
				return "彩色";
			}
			return "";
		}

		public string GetItemName(int id)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(id);
			if (elementById == null)
			{
				return "";
			}
			string nameID = elementById.nameID;
			LanguageRaft_languagetable elementById2 = GameApp.Table.GetManager().GetLanguageRaft_languagetableModelInstance().GetElementById(nameID);
			if (elementById2 == null)
			{
				return "";
			}
			return elementById2.chinesesimplified;
		}

		public void OnChapterStart()
		{
			this.ClearChapterTempData();
		}

		public void ClearChapterTempData()
		{
			this.ClearStageButtonContent();
			this.ClearStageSelectTempData();
			this.ChapterEndQuitType = 0;
		}

		public void SetStageButtonContent(int buttonIndex, string content)
		{
			string text = "(<color=.*?>|</color>)";
			string text2 = Regex.Replace(content, text, string.Empty);
			if (text2.Contains("休息"))
			{
				text2 = "休息";
			}
			if (!this.m_stageButtonContent.ContainsKey(buttonIndex))
			{
				this.m_stageButtonContent.Add(buttonIndex, text2);
				return;
			}
			this.m_stageButtonContent[buttonIndex] = text2;
		}

		public void SetStageButtonClickIndex(int buttonIndex)
		{
			this.m_stageButtonClickIndex = buttonIndex;
		}

		public bool CanGetStageButtonContent()
		{
			return this.m_stageButtonContent.Count > 0 && this.m_stageButtonContent.ContainsKey(this.m_stageButtonClickIndex);
		}

		public string GetStageButtonContent()
		{
			if (this.m_stageButtonClickIndex < 0)
			{
				return "";
			}
			if (this.m_stageButtonContent.ContainsKey(this.m_stageButtonClickIndex))
			{
				return this.m_stageButtonContent[this.m_stageButtonClickIndex];
			}
			return "";
		}

		public void ClearStageButtonContent()
		{
			this.m_stageButtonContent.Clear();
			this.m_stageButtonClickIndex = -1;
		}

		public void OnStageClickButton()
		{
			this.StageClickTempButtonName = this.GetStageButtonContent();
			this.ClearStageButtonContent();
		}

		public void SetStageClickTempSelectedSkillList(List<GameEventSkillBuildData> skillList)
		{
			this.StageClickTempSelectedSkillList.Clear();
			this.StageClickTempSelectedSkillList.AddRange(skillList);
		}

		public void AddStageClickTempAtt(List<NodeAttParam> attList, bool removeDuplicates = true)
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			List<NodeAttParam> list2 = new List<NodeAttParam>();
			foreach (NodeAttParam nodeAttParam in attList)
			{
				if (nodeAttParam.attType == GameEventAttType.AttackPercent || nodeAttParam.attType == GameEventAttType.RecoverHpRate || nodeAttParam.attType == GameEventAttType.HPMaxPercent || nodeAttParam.attType == GameEventAttType.DefencePercent || nodeAttParam.attType == GameEventAttType.CampHpRate)
				{
					list.Add(nodeAttParam);
				}
				else if (nodeAttParam.attType == GameEventAttType.Chips)
				{
					list2.Add(nodeAttParam);
				}
			}
			if (removeDuplicates)
			{
				using (List<NodeAttParam>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NodeAttParam att2 = enumerator.Current;
						if (this.StageClickTempAttList.FindIndex((NodeAttParam x) => x.attType == att2.attType && att2.baseCount == x.baseCount) < 0)
						{
							this.StageClickTempAttList.Add(att2);
						}
					}
				}
				using (List<NodeAttParam>.Enumerator enumerator = list2.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NodeAttParam att = enumerator.Current;
						if (this.StageClickTempChipAttList.FindIndex((NodeAttParam x) => x.attType == att.attType && att.baseCount == x.baseCount) < 0)
						{
							this.StageClickTempChipAttList.Add(att);
						}
					}
					return;
				}
			}
			this.StageClickTempAttList.AddRange(list);
			this.StageClickTempChipAttList.AddRange(list2);
		}

		public void AddStageClickTempScore(List<NodeScoreParam> attList, bool removeDuplicates = true)
		{
			if (removeDuplicates)
			{
				using (List<NodeScoreParam>.Enumerator enumerator = attList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NodeScoreParam att = enumerator.Current;
						if (this.StageScoreTempAttList.FindIndex((NodeScoreParam x) => x.scoreNameId == att.scoreNameId && att.score == x.score) < 0)
						{
							this.StageScoreTempAttList.Add(att);
						}
					}
					return;
				}
			}
			this.StageScoreTempAttList.AddRange(attList);
		}

		public void AddStageClickTempItem(List<NodeItemParam> itemList, bool removeDuplicates = true)
		{
			if (removeDuplicates)
			{
				using (List<NodeItemParam>.Enumerator enumerator = itemList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						NodeItemParam item = enumerator.Current;
						if (this.StageClickTempItemList.FindIndex((NodeItemParam x) => x.itemId == item.itemId && item.itemNum == x.itemNum) < 0)
						{
							this.StageClickTempItemList.Add(item);
						}
					}
					return;
				}
			}
			this.StageClickTempItemList.AddRange(itemList);
		}

		public void AddStageClickTempSkillShow(List<GameEventSkillBuildData> skillList)
		{
			this.StageClickTempSkillShowList.AddRange(skillList);
		}

		public void AddStageClickTempSkillSelect(List<GameEventSkillBuildData> skillList, bool removeDuplicates = true)
		{
			if (removeDuplicates)
			{
				using (List<GameEventSkillBuildData>.Enumerator enumerator = skillList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameEventSkillBuildData skill = enumerator.Current;
						if (this.StageClickTempSkillSelectList.FindIndex((GameEventSkillBuildData x) => x.skillId == skill.skillId) < 0)
						{
							this.StageClickTempSkillSelectList.Add(skill);
						}
					}
					return;
				}
			}
			this.StageClickTempSkillSelectList.AddRange(skillList);
		}

		public void ClearStageSelectTempData()
		{
			this.StageClickTempButtonName = "";
			this.StageClickTempAttList.Clear();
			this.StageClickTempChipAttList.Clear();
			this.StageScoreTempAttList.Clear();
			this.StageClickTempItemList.Clear();
			this.StageClickTempSkillShowList.Clear();
			this.StageClickTempSkillSelectList.Clear();
			this.StageClickTempBattleResult = -1;
			this.StageClickTempBattleRound = 0;
			this.StageClickTempEventSizeType = EventSizeType.Normal;
			this.StageClickTempEventID = 0;
			this.StageClickTempDay = 1;
			this.StageClickTempExp = 0;
			this.StageClickTempLevel = 1;
			this.StageClickTempATK = 0.0;
			this.StageClickTempDEF = 0.0;
			this.StageClickTempHP = 0.0;
			this.StageClickTempHP_MAX = 0.0;
			this.StageClickTempSelectedSkillList.Clear();
			this.StageClickTempEpicProgress = 0;
			this.StageClickTempNormalProgress = 0;
		}

		public string PreOrderIDToOrderID(int preOrderID)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dataModule == null)
			{
				return "";
			}
			return preOrderID.ToString() + "_" + dataModule.userId.ToString();
		}

		public static string GameTGACostCurrencyName(GameTGACostCurrency currency)
		{
			switch (currency)
			{
			case GameTGACostCurrency.Free:
				return "免费";
			case GameTGACostCurrency.Coin:
				return "金币";
			case GameTGACostCurrency.Gem:
				return "钻石";
			case GameTGACostCurrency.PetEgg:
				return "宠物蛋";
			case GameTGACostCurrency.ChestKey:
				return "宝箱钥匙";
			case GameTGACostCurrency.Ad:
				return "广告";
			default:
				return "";
			}
		}

		public static string GetSourceName(int id)
		{
			TGASource_Get elementById = GameApp.Table.GetManager().GetTGASource_GetModelInstance().GetElementById(id);
			if (elementById != null)
			{
				return elementById.source;
			}
			GameTGATools.TGADebugLogError(string.Format("[Track_Get_Cost]需要在TGASource_Get表中配置ID为{0}的数据", id));
			return "";
		}

		public static string CostSourceName(int id)
		{
			TGASource_Cost elementById = GameApp.Table.GetManager().GetTGASource_CostModelInstance().GetElementById(id);
			if (elementById != null)
			{
				return elementById.source;
			}
			GameTGATools.TGADebugLogError(string.Format("[Track_Get_Cost]需要在TGASource_Cost表中配置ID为{0}的数据", id));
			return "";
		}

		public static string ADSourceName(int adId)
		{
			TGASource_AD elementById = GameApp.Table.GetManager().GetTGASource_ADModelInstance().GetElementById(adId);
			if (elementById != null)
			{
				return elementById.source;
			}
			HLog.LogError("[Track_AD]需要在TGASource_AD表中配置id为", adId.ToString(), "的数据");
			return "";
		}

		public static void TGADebugLog(string log)
		{
		}

		public static void TGADebugLogError(string log)
		{
			Debug.LogError("[TGADebug]" + log);
		}

		public void ClearExtraADCount()
		{
			this._extraADCount = 0;
		}

		public void OnADSuccess()
		{
			this._extraADCount++;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.AD);
		}

		public int GetADCount()
		{
			AdDataModule dataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			if (dataModule != null)
			{
				return dataModule.AccountTotalADTimes + this._extraADCount;
			}
			return 0;
		}

		public double Timer { get; private set; }

		private IEnumerator TimerUpdate()
		{
			this.Timer = 0.0;
			for (;;)
			{
				this.Timer += (double)Time.unscaledDeltaTime;
				yield return null;
			}
			yield break;
		}

		public void OnOpenPage(string pageName)
		{
		}

		public void OnTrackPage(string pageName)
		{
		}

		public static double NumberOffset(long number, int digit = 10)
		{
			return (double)number / Math.Pow(10.0, (double)digit);
		}

		private static GameTGATools mToolIns;

		private LoginDataModule mLoginDataModule;

		private Dictionary<int, string> m_stageButtonContent = new Dictionary<int, string>();

		private int m_stageButtonClickIndex = -1;

		public int ChapterEndQuitType;

		public string StageClickTempButtonName = "";

		public EventSizeType StageClickTempEventSizeType;

		public int StageClickTempEventID;

		public int StageClickTempDay;

		public int StageClickTempExp;

		public int StageClickTempLevel;

		public double StageClickTempATK;

		public double StageClickTempDEF;

		public double StageClickTempHP;

		public double StageClickTempHP_MAX;

		public List<GameEventSkillBuildData> StageClickTempSelectedSkillList = new List<GameEventSkillBuildData>();

		public int StageClickTempEpicProgress;

		public int StageClickTempNormalProgress;

		public List<NodeAttParam> StageClickTempAttList = new List<NodeAttParam>();

		public List<NodeAttParam> StageClickTempChipAttList = new List<NodeAttParam>();

		public List<NodeScoreParam> StageScoreTempAttList = new List<NodeScoreParam>();

		public List<NodeItemParam> StageClickTempItemList = new List<NodeItemParam>();

		public List<GameEventSkillBuildData> StageClickTempSkillShowList = new List<GameEventSkillBuildData>();

		public List<GameEventSkillBuildData> StageClickTempSkillSelectList = new List<GameEventSkillBuildData>();

		public int StageClickTempBattleResult = -1;

		public int StageClickTempBattleRound;

		private int _extraADCount;

		public Dictionary<string, double> ViewOpenTimes = new Dictionary<string, double>();

		public string LastViewName = "";

		private Coroutine _timerCoroutine;

		private List<string> _openPageList = new List<string>();

		private List<string> _trackPageList = new List<string>();
	}
}
