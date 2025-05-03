using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Dxx.Guild;
using Framework;
using Framework.SDKManager;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Habby.Mail.Data;
using HotFix.TGA;
using LocalModels.Bean;
using NetWork;
using Proto.Collection;
using Proto.Common;
using Proto.Equip;
using Proto.Pet;
using Server;
using UnityEngine;

namespace HotFix
{
	public static class GameTGAExtend
	{
		public static void UserSet(this SDKManager.SDKTGA sdk, GameTGAUserSetType setType)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			if (setType == GameTGAUserSetType.Login)
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule != null)
				{
					dictionary["uid"] = dataModule.userId;
					dictionary["gems_hold"] = dataModule.userCurrency.Diamonds;
					dictionary["gold_hold"] = dataModule.userCurrency.Coins;
					dictionary["nick_name"] = dataModule.NickName;
					dictionary["total_login_days"] = dataModule.AccountTotalLoginDays;
					dictionary["ab_group"] = ((dataModule.abVersion == 0U) ? "A" : "B");
					if (dataModule.AccountCreateTime > 0L)
					{
						DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime((double)DxxTools.Time.lastServerTimestamp);
						DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)dataModule.AccountCreateTime);
						int num = (int)(dateTime - dateTime2).TotalDays;
						dictionary["register_days"] = num + 1;
					}
					dictionary["title"] = dataModule.AvatarTitle;
				}
				if (GameApp.NetWork != null && GameApp.NetWork.m_serverID > 0U)
				{
					dictionary["server"] = GameApp.NetWork.m_serverID;
				}
			}
			if (setType == GameTGAUserSetType.Ticket)
			{
				TicketDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.TicketDataModule);
				if (dataModule2 != null)
				{
					dictionary["stamina_hold"] = (int)dataModule2.GetTicket(UserTicketKind.UserLife).NewNum;
				}
			}
			if (setType == GameTGAUserSetType.Attribute)
			{
				AddAttributeDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				if (dataModule3 != null)
				{
					dictionary["battle_power"] = GameTGATools.NumberOffset((long)dataModule3.Combat, 10);
				}
			}
			if (setType == GameTGAUserSetType.Hero || setType == GameTGAUserSetType.Attribute)
			{
				AddAttributeDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				HeroDataModule dataModule5 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
				if (dataModule5 != null && dataModule4 != null)
				{
					CardData cardData = new CardData();
					cardData.CloneFrom(dataModule5.MainCardData);
					cardData.UpdateAttribute(dataModule4.AttributeDatas);
					long hpMax4UI = cardData.m_memberAttributeData.GetHpMax4UI();
					long attack4UI = cardData.m_memberAttributeData.GetAttack4UI();
					long defence4UI = cardData.m_memberAttributeData.GetDefence4UI();
					dictionary["current_atk"] = GameTGATools.NumberOffset(attack4UI, 10);
					dictionary["current_hp"] = GameTGATools.NumberOffset(hpMax4UI, 10);
					dictionary["current_def"] = GameTGATools.NumberOffset(defence4UI, 10);
				}
			}
			if (setType == GameTGAUserSetType.IAP)
			{
				IAPDataModule dataModule6 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				if (dataModule6 != null)
				{
					dictionary["is_ad_free"] = dataModule6.MonthCard.IsActivation(IAPMonthCardData.CardType.NoAd);
					dictionary["is_month"] = dataModule6.MonthCard.IsActivation(IAPMonthCardData.CardType.Month);
					dictionary["total_iap"] = dataModule6.AccountTotalCharge;
					if (dataModule6.AccountFirstPayTime > 0L)
					{
						dictionary["first_pay_time"] = DxxTools.Time.UnixTimestampToDateTime((double)dataModule6.AccountFirstPayTime).AddHours(8.0).ToString("yyyy-MM-dd HH:mm:ss.fff");
					}
				}
			}
			if (setType == GameTGAUserSetType.Talent)
			{
				TalentDataModule dataModule7 = GameApp.Data.GetDataModule(DataName.TalentDataModule);
				if (dataModule7 != null)
				{
					dictionary["talent_level"] = dataModule7.talentProgressData.curLevel;
				}
			}
			if (setType == GameTGAUserSetType.Chapter)
			{
				ChapterDataModule dataModule8 = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
				if (dataModule8 != null)
				{
					int num2 = dataModule8.ChapterID * 100 + dataModule8.MaxStage;
					dictionary["max_chapter_day"] = num2;
				}
			}
			if (setType == GameTGAUserSetType.Prop && !GameTGATools.Ins.SetAllBagItems(dictionary, "inventory"))
			{
				List<object> list = new List<object>();
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["item_id"] = -1;
				list.Add(dictionary2);
				dictionary["inventory"] = list;
			}
			if (setType == GameTGAUserSetType.Equipment && !GameTGATools.Ins.SetAllEquipments(dictionary, "current_equipment"))
			{
				List<object> list2 = new List<object>();
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3["equipment_id"] = -1;
				list2.Add(dictionary3);
				dictionary["current_equipment"] = list2;
			}
			if (setType == GameTGAUserSetType.Pet && !GameTGATools.Ins.SetAllPets(dictionary, "pets_hold"))
			{
				List<object> list3 = new List<object>();
				Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
				dictionary4["pet_id"] = -1;
				list3.Add(dictionary4);
				dictionary["pets_hold"] = list3;
			}
			if (setType == GameTGAUserSetType.Collection && !GameTGATools.Ins.SetAllCollections(dictionary, "collection"))
			{
				List<object> list4 = new List<object>();
				Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
				dictionary5["item_id"] = -1;
				list4.Add(dictionary5);
				dictionary["collection"] = list4;
			}
			if (setType == GameTGAUserSetType.AD)
			{
				AdDataModule dataModule9 = GameApp.Data.GetDataModule(DataName.AdDataModule);
				if (dataModule9 != null)
				{
					dictionary["total_ad"] = GameTGATools.Ins.GetADCount();
				}
			}
			if (setType == GameTGAUserSetType.GuildActivity)
			{
				GuildShareDetailData guildDetailData = GuildSDKManager.Instance.GuildInfo.GuildDetailData;
				if (guildDetailData != null && !string.IsNullOrEmpty(guildDetailData.GuildID))
				{
					dictionary["guild_id"] = guildDetailData.GuildID;
				}
				else
				{
					dictionary["guild_id"] = "";
				}
			}
			if (setType == GameTGAUserSetType.HabbyID)
			{
				LoginDataModule dataModule10 = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				if (dataModule10 != null)
				{
					if (dataModule10.habbyMailBind)
					{
						string habbyID = dataModule10.HabbyID;
						if (!string.IsNullOrEmpty(habbyID))
						{
							dictionary["habby_id"] = habbyID;
						}
					}
					else
					{
						dictionary["habby_id"] = "";
					}
				}
			}
			sdk.User_set(dictionary, setType == GameTGAUserSetType.BaseInfo);
		}

		public static void TGALogin(this SDKManager.SDKTGA sdk, string id)
		{
			sdk.Login(id);
		}

		public static void Track_HabbyIDBind(this SDKManager.SDKTGA sdk, string eventName, Dictionary<string, object> dic)
		{
			if (dic != null)
			{
				GameTGATools.Ins.SetPublicInfo(dic);
			}
			sdk.Track(eventName, dic, true);
		}

		public static void Track_HabbyIDShow(this SDKManager.SDKTGA sdk)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "habbyid_show";
			sdk.Track("overseas_habby_register", dictionary, true);
		}

		public static void Track_FirstActive(this SDKManager.SDKTGA sdk, string accountID)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			sdk.TrackFirstEvent(GameTGAEventName.first_active.ToString(), dictionary, accountID);
		}

		public static void Track_Reactive(this SDKManager.SDKTGA sdk)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			sdk.Track(GameTGAEventName.re_activate.ToString(), dictionary, true);
		}

		public static void Track_AppStart(this SDKManager.SDKTGA sdk)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			sdk.AppStart(dictionary);
		}

		public static void Track_AppEnd(this SDKManager.SDKTGA sdk)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			GameTGATools.Ins.SetAllBagItems(dictionary, "inventory");
			GameTGATools.Ins.SetAllCollections(dictionary, "collection");
			GameTGATools.Ins.SetAllPets(dictionary, "pets_hold");
			GameTGATools.Ins.SetAllEquipments(dictionary, "equipment");
			GameTGATools.Ins.SetAllMounts(dictionary, "mount");
			GameTGATools.Ins.SetAllArtifacts(dictionary, "legend");
			sdk.AppEnd(dictionary);
		}

		public static void Track_Login(this SDKManager.SDKTGA sdk, string step)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			sdk.TrackLogin(step, dictionary);
		}

		public static void Track_Guide(this SDKManager.SDKTGA sdk, int guideId, int step)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["guide_id"] = guideId;
			string text = "";
			switch (step)
			{
			case 0:
				text = "show";
				break;
			case 1:
				text = "click";
				break;
			case 2:
				text = "finish";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				dictionary["step"] = text;
			}
			sdk.Track(GameTGAEventName.guide.ToString(), dictionary, true);
		}

		public static void OnMessageCommonData(IMessage message, CommonData commonData)
		{
			if (message == null || commonData == null)
			{
				return;
			}
			GameTGAExtend.OnMessageCommonData_Collection(message, commonData);
			ushort messageId = PackageFactory.GetMessageId(message);
			if (messageId == 20210)
			{
				PetDrawResponse petDrawResponse = message as PetDrawResponse;
				if (petDrawResponse != null)
				{
					GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName((int)messageId), petDrawResponse.CommonData.Reward, petDrawResponse.ShowPet, null, null, null);
				}
				else
				{
					Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 PetDrawResponse", messageId));
				}
			}
			else if (messageId == 20222)
			{
				CollecComposeResponse collecComposeResponse = message as CollecComposeResponse;
				if (collecComposeResponse != null)
				{
					GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName((int)messageId), collecComposeResponse.CommonData.Reward, null, null, null, null);
				}
				else
				{
					Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 CollecComposeResponse", messageId));
				}
			}
			else if (messageId == 11104)
			{
				EquipComposeResponse equipComposeResponse = message as EquipComposeResponse;
				if (equipComposeResponse != null)
				{
					GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName((int)messageId), equipComposeResponse.CommonData.Reward, null, null, equipComposeResponse.CommonData.Equipment, null);
				}
				else
				{
					Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 EquipComposeResponse", messageId));
				}
			}
			else if (messageId == 11114)
			{
				EquipDecomposeResponse equipDecomposeResponse = message as EquipDecomposeResponse;
				if (equipDecomposeResponse != null)
				{
					GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName((int)messageId), equipDecomposeResponse.CommonData.Reward, null, null, equipDecomposeResponse.CommonData.Equipment, null);
				}
				else
				{
					Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 EquipDecomposeResponse", messageId));
				}
			}
			else
			{
				bool flag = false;
				if (commonData.Reward != null && commonData.Reward.Count != 0)
				{
					foreach (RewardDto rewardDto in commonData.Reward)
					{
						if (rewardDto != null && rewardDto.ConfigId > 0U && rewardDto.Count != 0UL)
						{
							flag = true;
							break;
						}
					}
				}
				if (messageId == 11208 || messageId == 11010 || messageId == 11018 || messageId == 11204 || messageId == 12902 || messageId == 11218)
				{
					flag = false;
				}
				if (flag)
				{
					GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName((int)messageId), commonData.Reward, null, null, null, null);
				}
			}
			if (messageId == 11104)
			{
				EquipComposeResponse equipComposeResponse2 = message as EquipComposeResponse;
				if (equipComposeResponse2 != null)
				{
					EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
					List<EquipData> list = new List<EquipData>();
					if (equipComposeResponse2.DelEquipRowId != null && equipComposeResponse2.DelEquipRowId.Count > 0)
					{
						foreach (long num in equipComposeResponse2.DelEquipRowId)
						{
							if (dataModule.m_equipDatas.ContainsKey((ulong)num))
							{
								EquipData equipData = dataModule.m_equipDatas[(ulong)num];
								list.Add(equipData);
							}
						}
					}
					GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName((int)messageId), commonData.CostDto, list);
					return;
				}
				Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 EquipComposeResponse", messageId));
				return;
			}
			else
			{
				if (messageId != 11114)
				{
					bool flag2 = false;
					if (commonData.CostDto != null && commonData.CostDto.Count != 0)
					{
						foreach (CostDto costDto in commonData.CostDto)
						{
							if (costDto != null && costDto.ConfigId > 0U && costDto.Count != 0L)
							{
								flag2 = true;
								break;
							}
						}
					}
					if (messageId == 11208 || messageId == 11204 || messageId == 12902 || messageId == 11218)
					{
						flag2 = false;
					}
					if (flag2)
					{
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName((int)messageId), commonData.CostDto, null);
					}
					return;
				}
				if (message is EquipDecomposeResponse)
				{
					GameApp.Data.GetDataModule(DataName.EquipDataModule);
					List<EquipData> list2 = new List<EquipData>();
					GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName((int)messageId), commonData.CostDto, list2);
					return;
				}
				Debug.LogError(string.Format("[TGA OnMessageCommonData]messageID {0} 不是 EquipDecomposeResponse", messageId));
				return;
			}
		}

		public static void Track_Get(this SDKManager.SDKTGA sdk, string source, RepeatedField<RewardDto> rewardDtos, RepeatedField<PetDto> extraPetDtos = null, RepeatedField<CollectionDto> extraCollectionDtos = null, RepeatedField<EquipmentDto> extraEquipmentDtos = null, List<MailRewardObject> mails = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			if (!string.IsNullOrEmpty(source))
			{
				dictionary["source"] = source;
			}
			long num = 0L;
			long num2 = 0L;
			List<object> list = new List<object>();
			List<object> list2 = new List<object>();
			List<object> list3 = new List<object>();
			List<object> list4 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					if (rewardDto.ConfigId == 1U || rewardDto.ConfigId == 4U)
					{
						num += (long)rewardDto.Count;
					}
					else if (rewardDto.ConfigId == 2U)
					{
						num2 += (long)rewardDto.Count;
					}
					else
					{
						GameTGATools.Ins.TryAddRewardDto_Item(rewardDto, list2);
						GameTGATools.Ins.TryAddRewardDto_Equipment(rewardDto, list3);
						GameTGATools.Ins.TryAddRewardDto_Pet(rewardDto, list);
						GameTGATools.Ins.TryAddRewardDto_Colliction(rewardDto, list4);
					}
				}
			}
			if (extraPetDtos != null && extraPetDtos.Count > 0)
			{
				foreach (PetDto petDto in extraPetDtos)
				{
					GameTGATools.Ins.TryAddPetDto(petDto, list);
				}
			}
			if (extraEquipmentDtos != null && extraEquipmentDtos.Count > 0)
			{
				foreach (EquipmentDto equipmentDto in extraEquipmentDtos)
				{
					GameTGATools.Ins.TryAddEquipmentDto_Equipment(equipmentDto, list3);
				}
			}
			if (extraCollectionDtos != null && extraCollectionDtos.Count > 0)
			{
				foreach (CollectionDto collectionDto in extraCollectionDtos)
				{
					GameTGATools.Ins.TryAddCollictionDto_Colliction(collectionDto, list4);
				}
			}
			if (mails != null && mails.Count > 0)
			{
				foreach (MailRewardObject mailRewardObject in mails)
				{
					if (mailRewardObject.mail != null && mailRewardObject.mail.rewards != null)
					{
						foreach (MailReward mailReward in mailRewardObject.mail.rewards)
						{
							int num3 = -1;
							if (int.TryParse(mailReward.id, out num3) && num3 >= 0)
							{
								if (num3 == 1 || num3 == 4)
								{
									num += (long)mailReward.count;
								}
								else if (num3 == 2)
								{
									num2 += (long)mailReward.count;
								}
								else
								{
									GameTGATools.Ins.TryAddMailReward_Item(mailReward, list2);
									GameTGATools.Ins.TryAddMailReward_Equipment(mailReward, list3);
									GameTGATools.Ins.TryAddMailReward_Pet(mailReward, list);
									GameTGATools.Ins.TryAddMailReward_Colliction(mailReward, list4);
								}
							}
						}
					}
				}
			}
			dictionary["gold"] = num;
			dictionary["gems"] = num2;
			if (list.Count > 0)
			{
				dictionary["pets_hold"] = list;
			}
			if (list2.Count > 0)
			{
				dictionary["item_list"] = list2;
			}
			if (list3.Count > 0)
			{
				dictionary["equipment"] = list3;
			}
			if (list4.Count > 0)
			{
				dictionary["collection"] = list4;
			}
			if (dictionary.ContainsKey("item_list"))
			{
				string text = string.Format("[Track_Get_Cost]Items.Count::{0},  ", (dictionary["item_list"] as List<object>).Count);
				foreach (object obj in (dictionary["item_list"] as List<object>))
				{
					Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
					text += string.Format("({0},  {1},  {2})，  ", dictionary2["item_id"], dictionary2["item_name"], dictionary2["item_count"]);
				}
			}
			if (dictionary.ContainsKey("equipment"))
			{
				string text2 = string.Format("[Track_Get_Cost]Equipments.Count::{0},  ", (dictionary["equipment"] as List<object>).Count);
				foreach (object obj2 in (dictionary["equipment"] as List<object>))
				{
					Dictionary<string, object> dictionary3 = obj2 as Dictionary<string, object>;
					text2 = text2 + string.Format("({0},  {1},  {2},  ", dictionary3["equipment_id"], dictionary3["equipment_name"], dictionary3["equipment_rarity"]) + string.Format("{0},  {1},  {2})，  ", dictionary3["equipment_grade"], dictionary3["equipment_level"], dictionary3["count"]);
				}
			}
			if (dictionary.ContainsKey("pets_hold"))
			{
				string text3 = string.Format("[Track_Get_Cost]Pets.Count::{0},  ", (dictionary["pets_hold"] as List<object>).Count);
				foreach (object obj3 in (dictionary["pets_hold"] as List<object>))
				{
					Dictionary<string, object> dictionary4 = obj3 as Dictionary<string, object>;
					text3 = text3 + string.Format("({0},  {1},  {2},  ", dictionary4["pet_id"], dictionary4["pet_name"], dictionary4["pet_rarity"]) + string.Format("{0}，  {1}，  {2})，  ", dictionary4["pet_level"], dictionary4["pet_grade"], dictionary4["pet_shards"]);
				}
			}
			if (dictionary.ContainsKey("collection"))
			{
				string text4 = string.Format("[Track_Get_Cost]Collections.Count::{0},  ", (dictionary["collection"] as List<object>).Count);
				foreach (object obj4 in (dictionary["collection"] as List<object>))
				{
					Dictionary<string, object> dictionary5 = obj4 as Dictionary<string, object>;
					text4 += string.Format("({0},  {1},  {2}，  {3})，  ", new object[]
					{
						dictionary5["item_id"],
						dictionary5["item_name"],
						dictionary5["item_rarity"],
						dictionary5["item_level"]
					});
				}
			}
			if ((long)dictionary["gold"] > 0L || (long)dictionary["gems"] > 0L || list.Count > 0 || list2.Count > 0 || list3.Count > 0 || list4.Count > 0)
			{
				sdk.Track(GameTGAEventName.get.ToString(), dictionary, true);
			}
		}

		public static void Track_Cost(this SDKManager.SDKTGA sdk, string source, RepeatedField<CostDto> costDtos, List<EquipData> extraEquipDatas = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			if (!string.IsNullOrEmpty(source))
			{
				dictionary["source"] = source;
			}
			long num = 0L;
			long num2 = 0L;
			List<object> list = new List<object>();
			List<object> list2 = new List<object>();
			List<object> list3 = new List<object>();
			List<object> list4 = new List<object>();
			if (costDtos != null && costDtos.Count > 0)
			{
				foreach (CostDto costDto in costDtos)
				{
					if (costDto.ConfigId == 1U || costDto.ConfigId == 4U)
					{
						num += costDto.Count;
						num = (long)Mathf.Abs((float)num);
					}
					else if (costDto.ConfigId == 2U)
					{
						num2 += costDto.Count;
						num2 = (long)Mathf.Abs((float)num2);
					}
					else
					{
						GameTGATools.Ins.TryAddCostDto_Item(costDto, list2);
						GameTGATools.Ins.TryAddCostDto_Equipment(costDto, list3);
						GameTGATools.Ins.TryAddCostDto_Pet(costDto, list);
						GameTGATools.Ins.TryAddCostDto_Colliction(costDto, list4);
					}
				}
			}
			if (extraEquipDatas != null && extraEquipDatas.Count > 0)
			{
				foreach (EquipData equipData in extraEquipDatas)
				{
					GameTGATools.Ins.TryAddEquipData_Equipment(equipData, list3);
				}
			}
			dictionary["gold"] = num;
			dictionary["gems"] = num2;
			if (list.Count > 0)
			{
				dictionary["pets_hold"] = list;
			}
			if (list2.Count > 0)
			{
				dictionary["item_list"] = list2;
			}
			if (list3.Count > 0)
			{
				dictionary["equipment"] = list3;
			}
			if (list4.Count > 0)
			{
				dictionary["collection"] = list4;
			}
			if (dictionary.ContainsKey("item_list"))
			{
				string text = string.Format("[Track_Get_Cost]Items.Count::{0},  ", (dictionary["item_list"] as List<object>).Count);
				foreach (object obj in (dictionary["item_list"] as List<object>))
				{
					Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
					text += string.Format("({0},  {1},  {2})，  ", dictionary2["item_id"], dictionary2["item_name"], dictionary2["item_count"]);
				}
			}
			if (dictionary.ContainsKey("equipment"))
			{
				string text2 = string.Format("[Track_Get_Cost]Equipments.Count::{0},  ", (dictionary["equipment"] as List<object>).Count);
				foreach (object obj2 in (dictionary["equipment"] as List<object>))
				{
					Dictionary<string, object> dictionary3 = obj2 as Dictionary<string, object>;
					text2 = text2 + string.Format("({0},  {1},  {2},  ", dictionary3["equipment_id"], dictionary3["equipment_name"], dictionary3["equipment_rarity"]) + string.Format("{0},  {1},  {2})，  ", dictionary3["equipment_grade"], dictionary3["equipment_level"], dictionary3["count"]);
				}
			}
			if (dictionary.ContainsKey("pets_hold"))
			{
				string text3 = string.Format("[Track_Get_Cost]Pets.Count::{0},  ", (dictionary["pets_hold"] as List<object>).Count);
				foreach (object obj3 in (dictionary["pets_hold"] as List<object>))
				{
					Dictionary<string, object> dictionary4 = obj3 as Dictionary<string, object>;
					text3 = text3 + string.Format("({0},  {1},  {2},  ", dictionary4["pet_id"], dictionary4["pet_name"], dictionary4["pet_rarity"]) + string.Format("{0}，  {1}，  {2})，  ", dictionary4["pet_level"], dictionary4["pet_grade"], dictionary4["pet_shards"]);
				}
			}
			if (dictionary.ContainsKey("collection"))
			{
				string text4 = string.Format("[Track_Get_Cost]Items.Count::{0},  ", (dictionary["collection"] as List<object>).Count);
				foreach (object obj4 in (dictionary["collection"] as List<object>))
				{
					Dictionary<string, object> dictionary5 = obj4 as Dictionary<string, object>;
					text4 += string.Format("({0},  {1},  {2}，  {3})，  ", new object[]
					{
						dictionary5["item_id"],
						dictionary5["item_name"],
						dictionary5["item_rarity"],
						dictionary5["item_level"]
					});
				}
			}
			if ((long)dictionary["gold"] > 0L || (long)dictionary["gems"] > 0L || list.Count > 0 || list2.Count > 0 || list3.Count > 0 || list4.Count > 0)
			{
				sdk.Track(GameTGAEventName.cost.ToString(), dictionary, true);
			}
		}

		public static void Track_AD(this SDKManager.SDKTGA sdk, string source, string step, string errorCode = "", RepeatedField<RewardDto> rewardDtos = null, RepeatedField<PetDto> extraPetDtos = null)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			if (!string.IsNullOrEmpty(source))
			{
				dictionary["source"] = source;
			}
			dictionary["step"] = step;
			if (step.Equals("SHOW ") || step.Equals("REWARD "))
			{
				dictionary["result"] = "SUCCESS";
			}
			else if (step.Equals("PLAYFAIL"))
			{
				dictionary["result"] = "FAIL";
			}
			else
			{
				step.Equals("CLICK ");
			}
			if (!string.IsNullOrEmpty(errorCode))
			{
				dictionary["error_code"] = errorCode;
			}
			List<object> list = new List<object>();
			if (rewardDtos != null)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (extraPetDtos != null && extraPetDtos.Count > 0)
			{
				foreach (PetDto petDto in extraPetDtos)
				{
					GameTGATools.Ins.TryAddPetDto_AllItem(petDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			GameTGATools.TGADebugLog("[Track_AD]-------------------------------------------------------------");
			string[] array = new string[8];
			array[0] = "[Track_AD]source::";
			array[1] = source;
			array[2] = ",  step::";
			array[3] = step;
			array[4] = ",  errorCode::";
			array[5] = errorCode;
			array[6] = ",  ";
			int num = 7;
			object obj = (dictionary.ContainsKey("result") ? dictionary["result"] : "");
			array[num] = ((obj != null) ? obj.ToString() : null);
			GameTGATools.TGADebugLog(string.Concat(array));
			if (dictionary.ContainsKey("reward_list"))
			{
				string text = string.Format("[Track_AD]Items.Count::{0},  ", (dictionary["reward_list"] as List<object>).Count);
				foreach (object obj2 in (dictionary["reward_list"] as List<object>))
				{
					Dictionary<string, object> dictionary2 = obj2 as Dictionary<string, object>;
					text += string.Format("({0},  {1},  {2})，  ", dictionary2["item_id"], dictionary2["item_name"], dictionary2["item_count"]);
				}
				GameTGATools.TGADebugLog(text);
			}
			GameTGATools.TGADebugLog("[Track_AD]-------------------------------------------------------------");
			sdk.Track(GameTGAEventName.ad.ToString(), dictionary, true);
		}

		public static void Track_TreasureOpen(this SDKManager.SDKTGA sdk, int chestType, int openAmt, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			string text = "";
			switch (chestType)
			{
			case 1:
				text = "青铜";
				break;
			case 2:
				text = "白银";
				break;
			case 3:
				text = "黄金";
				break;
			case 4:
				text = "宠物";
				break;
			case 5:
				text = "宝石";
				break;
			}
			dictionary["treasure_type"] = text;
			dictionary["open_amt"] = openAmt;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.treasure_open.ToString(), dictionary, true);
		}

		public static void Track_TreasurePoint_Collect(this SDKManager.SDKTGA sdk, long pointGet)
		{
			if (pointGet <= 0L)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "collect";
			dictionary["point_get"] = pointGet;
			ChestDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
			dictionary["point_after"] = dataModule.GetCurScore();
			dictionary["point_total"] = dataModule.maxScore;
			ChestList_ChestList elementById = GameApp.Table.GetManager().GetChestList_ChestListModelInstance().GetElementById(dataModule.curRewardConfigId);
			ItemData itemData = ((dataModule.curRewardType == 1) ? elementById.reward : elementById.rewardCircle).ToItemDataList()[0];
			string text = "";
			switch (itemData.ID)
			{
			case 71:
				text = "青铜";
				break;
			case 72:
				text = "白银";
				break;
			case 73:
				text = "黄金";
				break;
			case 74:
				text = "宠物";
				break;
			case 75:
				text = "宝石";
				break;
			}
			List<object> list = new List<object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["treasure_type"] = text;
			list.Add(dictionary2);
			dictionary["treasure_list"] = list;
			sdk.Track(GameTGAEventName.treasure_point.ToString(), dictionary, true);
		}

		public static void Track_TreasurePoint_Reward(this SDKManager.SDKTGA sdk, bool isGetAll, long pointCost, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = (isGetAll ? "reward_muti" : "reward");
			dictionary["point_cost"] = pointCost;
			ChestDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChestDataModule);
			dictionary["point_after"] = dataModule.GetCurScore();
			dictionary["point_total"] = dataModule.maxScore;
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (RewardDto rewardDto in rewardDtos)
				{
					string text = "";
					switch (rewardDto.ConfigId)
					{
					case 71U:
						text = "青铜";
						break;
					case 72U:
						text = "白银";
						break;
					case 73U:
						text = "黄金";
						break;
					case 74U:
						text = "宠物";
						break;
					case 75U:
						text = "宝石";
						break;
					}
					if (!string.IsNullOrEmpty(text))
					{
						for (int i = 0; i < (int)rewardDto.Count; i++)
						{
							Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
							dictionary2["treasure_type"] = text;
							list.Add(dictionary2);
						}
					}
				}
				dictionary["treasure_list"] = list;
			}
			sdk.Track(GameTGAEventName.treasure_point.ToString(), dictionary, true);
		}

		public static void Track_Mail(this SDKManager.SDKTGA sdk, List<MailRewardObject> mails)
		{
			if (mails == null || mails.Count == 0)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			List<object> list2 = new List<object>();
			foreach (MailRewardObject mailRewardObject in mails)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["mail_title"] = mailRewardObject.mail.mailTitle;
				list.Add(dictionary2);
				if (mailRewardObject.mail.rewards != null && mailRewardObject.mail.rewards.Length != 0)
				{
					foreach (MailReward mailReward in mailRewardObject.mail.rewards)
					{
						GameTGATools.Ins.TryAddMailReward_AllItem(mailReward, list2);
					}
				}
			}
			dictionary["mail_title"] = list;
			if (list2.Count > 0)
			{
				dictionary["reward_list"] = list2;
			}
			sdk.Track(GameTGAEventName.mail.ToString(), dictionary, true);
		}

		public static void Track_TalentUp(this SDKManager.SDKTGA sdk, string talentType, RepeatedField<CostDto> costDtos)
		{
			if (costDtos == null || costDtos.Count == 0)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			dictionary["talent_level"] = dataModule.talentData.TalentExp;
			if (talentType.Contains("Attack"))
			{
				dictionary["talent_type"] = "攻击";
			}
			else if (talentType.Contains("Defence"))
			{
				dictionary["talent_type"] = "防御";
			}
			else if (talentType.Contains("HP"))
			{
				dictionary["talent_type"] = "生命";
			}
			if (costDtos != null && costDtos.Count > 0)
			{
				dictionary["cost_gold_amt"] = Mathf.Abs((float)costDtos[0].Count);
			}
			sdk.Track(GameTGAEventName.talent_up.ToString(), dictionary, true);
		}

		public static void Track_Shop(this SDKManager.SDKTGA sdk, ShopType shopType, RepeatedField<CostDto> costDtos, RepeatedField<RewardDto> rewardDtos)
		{
			if (costDtos == null || costDtos.Count == 0)
			{
				return;
			}
			if (shopType != ShopType.BlackMarket && shopType != ShopType.Guild && shopType != ShopType.ManaCrystal)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			if (shopType == ShopType.BlackMarket)
			{
				dictionary["shop_name"] = "黑市";
			}
			else if (shopType == ShopType.Guild)
			{
				dictionary["shop_name"] = "公会商店";
			}
			else if (shopType == ShopType.ManaCrystal)
			{
				dictionary["shop_name"] = "魔晶商店";
			}
			dictionary["step"] = "buy";
			List<object> list = new List<object>();
			GameTGATools.Ins.TryAddCostDto_AllItem(costDtos[0], list);
			if (list.Count <= 0)
			{
				return;
			}
			Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[0];
			dictionary["cost_currency"] = dictionary2["item_name"];
			dictionary["cost_amt"] = dictionary2["item_count"];
			List<object> list2 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list2);
				}
			}
			if (list2.Count > 0)
			{
				dictionary["reward_list"] = list2;
			}
			sdk.Track(GameTGAEventName.shop.ToString(), dictionary, true);
		}

		public static void Track_AtiveTask_CollectPoint(this SDKManager.SDKTGA sdk)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "collect_point";
			TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			dictionary["daily_point"] = dataModule.DailyActive;
			dictionary["week_point"] = dataModule.WeeklyActive;
			sdk.Track(GameTGAEventName.active_task.ToString(), dictionary, true);
		}

		public static void Track_AtiveTask_CollectItem(this SDKManager.SDKTGA sdk, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "collect_item";
			TaskDataModule dataModule = GameApp.Data.GetDataModule(DataName.TaskDataModule);
			dictionary["daily_point"] = dataModule.DailyActive;
			dictionary["week_point"] = dataModule.WeeklyActive;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.active_task.ToString(), dictionary, true);
		}

		public static void Track_CapyRoulette(this SDKManager.SDKTGA sdk, int type, long coinRemains, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			string text = "";
			switch (type)
			{
			case 1:
				text = "1";
				break;
			case 2:
				text = "10";
				break;
			case 3:
				text = "collect";
				break;
			}
			dictionary["type_new"] = text;
			GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid(50UL);
			dictionary["coin_remains"] = coinRemains;
			ActivitySlotTrainDataModule dataModule = GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule);
			dictionary["total_times"] = dataModule.Count;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["item_list"] = list;
			sdk.Track(GameTGAEventName.capy_roulette.ToString(), dictionary, true);
		}

		public static void Track_TaskFinish(this SDKManager.SDKTGA sdk, string source, int activityId, int taskId, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = source;
			dictionary["activity_id"] = activityId;
			dictionary["task_id"] = taskId;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["item_list"] = list;
			sdk.Track(GameTGAEventName.task_finish.ToString(), dictionary, true);
		}

		public static void Track_Carnival_Task(this SDKManager.SDKTGA sdk, int day, int taskId, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = string.Format("day{0}", day);
			dictionary["task_id"] = taskId;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			sdk.Track(GameTGAEventName.carnival.ToString(), dictionary, true);
		}

		public static void Track_Carnival_Roadmap(this SDKManager.SDKTGA sdk, int taskId, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = "roadmap";
			dictionary["task_id"] = taskId;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			sdk.Track(GameTGAEventName.carnival.ToString(), dictionary, true);
		}

		public static void Track_MiningMine_Mine(this SDKManager.SDKTGA sdk, uint optType, RepeatedField<CostDto> costDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "挖矿";
			MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			dictionary["level"] = dataModule.MiningInfo.Stage;
			int num = 0;
			foreach (GridDto gridDto in dataModule.MiningInfo.Grids)
			{
				if (gridDto.Status != 0 || gridDto.Floors != 0)
				{
					num++;
				}
			}
			dictionary["slot_left"] = num;
			dictionary["is_auto"] = optType == 1U;
			List<object> list = new List<object>();
			if (costDtos != null && costDtos.Count > 0)
			{
				foreach (CostDto costDto in costDtos)
				{
					GameTGATools.Ins.TryAddCostDto_AllItem(costDto, list);
				}
			}
			dictionary["cost_item"] = list;
			sdk.Track(GameTGAEventName.mining_mine.ToString(), dictionary, true);
		}

		public static void Track_MiningMine_Capy(this SDKManager.SDKTGA sdk, RepeatedField<int> raritys, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "卡皮雕像";
			MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			dictionary["level"] = dataModule.MiningInfo.Stage;
			int num = 0;
			foreach (GridDto gridDto in dataModule.MiningInfo.Grids)
			{
				if (gridDto.Status != 0 || gridDto.Floors != 0)
				{
					num++;
				}
			}
			dictionary["slot_left"] = num;
			int grade = dataModule.GetTreasureFirstGridDto().Grade;
			Mining_oreQuality mining_oreQuality = GameApp.Table.GetManager().GetMining_oreQuality(grade);
			string text = ((mining_oreQuality != null) ? Singleton<LanguageManager>.Instance.GetInfoByID(2, mining_oreQuality.languageId) : "");
			text = Regex.Replace(text, "<[^>]*>", string.Empty);
			dictionary["capy_rarity"] = text;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			sdk.Track(GameTGAEventName.mining_mine.ToString(), dictionary, true);
		}

		public static void Track_MiningMine_Next(this SDKManager.SDKTGA sdk, List<RewardDto> rewardDtos, int slotLeft)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "下一章";
			MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			dictionary["level"] = dataModule.MiningInfo.Stage;
			dictionary["slot_left"] = slotLeft;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.mining_mine.ToString(), dictionary, true);
		}

		public static void Track_MiningMine_Reward(this SDKManager.SDKTGA sdk, RepeatedField<RewardDto> rewardDtos)
		{
			if (rewardDtos == null || rewardDtos.Count == 0)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = "主动领奖";
			MiningDataModule dataModule = GameApp.Data.GetDataModule(DataName.MiningDataModule);
			dictionary["level"] = dataModule.MiningInfo.Stage;
			int num = 0;
			foreach (GridDto gridDto in dataModule.MiningInfo.Grids)
			{
				if (gridDto.Status != 0 || gridDto.Floors != 0)
				{
					num++;
				}
			}
			dictionary["slot_left"] = num;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			sdk.Track(GameTGAEventName.mining_mine.ToString(), dictionary, true);
		}

		public static void Track_MiningTrain(this SDKManager.SDKTGA sdk, bool isFree, int freeTimes, int totalTimes, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["is_free"] = isFree;
			dictionary["free_times"] = freeTimes;
			dictionary["total_times"] = totalTimes;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			sdk.Track(GameTGAEventName.mining_train.ToString(), dictionary, true);
		}

		public static void Track_BattlePassGet_ChapterActivity(this SDKManager.SDKTGA sdk, string eventName, int bpLevel, RepeatedField<RewardDto> rewardDtos, int payID, List<int> freeReward, List<int> payReward, List<int> extraReward)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["bp_type"] = "活动通行证";
			dictionary["event_name"] = eventName;
			dictionary["bp_level"] = bpLevel;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (freeReward != null && freeReward.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (int num in freeReward)
				{
					list.Add(num);
				}
				dictionary2["free"] = list;
			}
			if (payReward != null && payReward.Count > 0)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(payID);
				if (elementById != null)
				{
					IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
					if (elementById2 != null)
					{
						List<object> list2 = new List<object>();
						foreach (int num2 in payReward)
						{
							list2.Add(num2);
						}
						string text = elementById2.price.ToString("F2");
						text = text.Replace('.', '_');
						text = "pay_" + text;
						dictionary2[text] = list2;
					}
				}
			}
			if (extraReward != null && extraReward.Count > 0)
			{
				List<object> list3 = new List<object>();
				foreach (int num3 in extraReward)
				{
					list3.Add(999);
				}
				dictionary2["extra"] = list3;
			}
			dictionary["bp_reward_id"] = dictionary2;
			List<object> list4 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list4);
				}
			}
			dictionary["reward_list"] = list4;
			sdk.Track(GameTGAEventName.battlepass_get.ToString(), dictionary, true);
		}

		public static void Track_BattlePassGet_BattlePass(this SDKManager.SDKTGA sdk, int bpLevel, RepeatedField<RewardDto> rewardDtos, int payID, List<int> freeReward, List<int> payReward, List<int> extraReward)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["bp_type"] = "通行证";
			dictionary["bp_level"] = bpLevel;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (freeReward != null && freeReward.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (int num in freeReward)
				{
					list.Add(num);
				}
				dictionary2["free"] = list;
			}
			if (payReward != null && payReward.Count > 0)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(payID);
				if (elementById != null)
				{
					IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
					if (elementById2 != null)
					{
						List<object> list2 = new List<object>();
						foreach (int num2 in payReward)
						{
							list2.Add(num2);
						}
						string text = elementById2.price.ToString("F2");
						text = text.Replace('.', '_');
						text = "pay_" + text;
						dictionary2[text] = list2;
					}
				}
			}
			if (extraReward != null && extraReward.Count > 0)
			{
				List<object> list3 = new List<object>();
				foreach (int num3 in extraReward)
				{
					list3.Add(999);
				}
				dictionary2["extra"] = list3;
			}
			dictionary["bp_reward_id"] = dictionary2;
			List<object> list4 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list4);
				}
			}
			dictionary["reward_list"] = list4;
			sdk.Track(GameTGAEventName.battlepass_get.ToString(), dictionary, true);
		}

		public static void Track_InvestGet(this SDKManager.SDKTGA sdk, string investName, int investId, int investLevel, RepeatedField<RewardDto> rewardDtos, int payID, List<int> freeReward, List<int> payReward)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["invest_type"] = investName;
			dictionary["invest_level"] = investLevel;
			dictionary["invest_id"] = investId;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["reward_list"] = list;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (freeReward != null && freeReward.Count > 0)
			{
				List<object> list2 = new List<object>();
				foreach (int num in freeReward)
				{
					list2.Add(num);
				}
				dictionary2["free"] = list2;
			}
			if (payReward != null && payReward.Count > 0)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(payID);
				if (elementById != null)
				{
					IAP_platformID elementById2 = GameApp.Table.GetManager().GetIAP_platformIDModelInstance().GetElementById(elementById.platformID);
					if (elementById2 != null)
					{
						List<object> list3 = new List<object>();
						foreach (int num2 in payReward)
						{
							list3.Add(num2);
						}
						string text = elementById2.price.ToString("F2");
						text = text.Replace('.', '_');
						text = "pay_" + text;
						dictionary2[text] = list3;
					}
				}
			}
			dictionary["invest_reward_id"] = dictionary2;
			sdk.Track(GameTGAEventName.invest_get.ToString(), dictionary, true);
		}

		public static void Track_ActivityPoint(this SDKManager.SDKTGA sdk, string pointType, int pointGet, int pointAfter)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["point_type"] = pointType;
			dictionary["point_get"] = pointGet;
			dictionary["point_after"] = pointAfter;
			sdk.Track(GameTGAEventName.activity_point.ToString(), dictionary, true);
		}

		public static void Track_Turntable(this SDKManager.SDKTGA sdk, int rate, int score, int playTimes, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["times"] = rate;
			dictionary["coin_remains"] = score;
			dictionary["total_times"] = playTimes;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			dictionary["item_list"] = list;
			sdk.Track(GameTGAEventName.turntable.ToString(), dictionary, true);
		}

		public static void Track_LegendUpgrade(this SDKManager.SDKTGA sdk, ArtifactInfo artifactInfo, RepeatedField<CostDto> costDtos, int preStage, int preLevel)
		{
			if (artifactInfo == null)
			{
				return;
			}
			ArtifactDataModule dataModule = GameApp.Data.GetDataModule(DataName.ArtifactDataModule);
			List<ArtifactBasicData> basicDataList = dataModule.GetBasicDataList();
			if ((ulong)artifactInfo.Stage > (ulong)((long)basicDataList.Count))
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			ArtifactBasicData artifactBasicData = basicDataList[(int)artifactInfo.Stage];
			dictionary2["legend_id"] = artifactBasicData.ItemConfig.id;
			dictionary2["legend_name"] = GameTGATools.Ins.GetItemName(artifactBasicData.ItemConfig.id);
			dictionary2["legend_stage"] = artifactInfo.Stage;
			dictionary2["legend_grade"] = artifactInfo.Level;
			list.Add(dictionary2);
			dictionary["legend_level_after"] = list;
			foreach (MergeAttributeData mergeAttributeData in dataModule.GetCurrentLevelInfo().attribute.GetMergeAttributeData())
			{
				if (mergeAttributeData.Header.Contains("HP") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["legend_hp"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Attack") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["legend_atk"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Defence") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["legend_def"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
			}
			if (costDtos != null && costDtos.Count > 0 && costDtos[0] != null)
			{
				dictionary["cost_currency"] = GameTGATools.Ins.GetItemName((int)costDtos[0].ConfigId);
				dictionary["cost_amt"] = Mathf.Abs((float)costDtos[0].Count);
			}
			dictionary["exp"] = dictionary["cost_amt"];
			IList<Artifact_artifactLevel> allElements = GameApp.Table.GetManager().GetArtifact_artifactLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Artifact_artifactLevel artifact_artifactLevel = allElements[i];
				if (artifact_artifactLevel.stage == preStage && artifact_artifactLevel.star == preLevel)
				{
					dictionary["total_exp"] = artifact_artifactLevel.levelCost;
					break;
				}
			}
			sdk.Track(GameTGAEventName.legend_upgrade.ToString(), dictionary, true);
		}

		public static void Track_RareLegendUpgrade(this SDKManager.SDKTGA sdk, int artifactConfigID, ArtifactItemDto artifactItemDto, RepeatedField<CostDto> costDtos)
		{
			if (artifactItemDto == null)
			{
				return;
			}
			if (artifactConfigID != artifactItemDto.ConfigId)
			{
				return;
			}
			Artifact_advanceArtifact elementById = GameApp.Table.GetManager().GetArtifact_advanceArtifactModelInstance().GetElementById(artifactConfigID);
			if (elementById == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["legend_id"] = elementById.itemId;
			dictionary["legend_name"] = GameTGATools.Ins.GetItemName(elementById.itemId);
			dictionary["star_after"] = artifactItemDto.Star;
			if (costDtos != null && costDtos.Count > 0 && costDtos[0] != null)
			{
				dictionary["cost_shards_amt"] = Mathf.Abs((float)costDtos[0].Count);
			}
			sdk.Track(GameTGAEventName.rare_legend_upgrade.ToString(), dictionary, true);
		}

		public static void Track_UnlockRareLegend(this SDKManager.SDKTGA sdk, int artifactConfigID)
		{
			Artifact_advanceArtifact elementById = GameApp.Table.GetManager().GetArtifact_advanceArtifactModelInstance().GetElementById(artifactConfigID);
			if (elementById == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["legend_id"] = elementById.itemId;
			dictionary["legend_name"] = GameTGATools.Ins.GetItemName(elementById.itemId);
			sdk.Track(GameTGAEventName.unlock_rare_legend.ToString(), dictionary, true);
		}

		public static void Track_ChapterStart(this SDKManager.SDKTGA sdk, int chapterId, string sessionId, int battleType, int rate, int challengeTimes)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["chapter_id"] = chapterId;
			dictionary["session_id"] = sessionId;
			GameTGATools.Ins.SetEquipPets(dictionary, "current_pet");
			GameTGATools.Ins.SetAllMounts(dictionary, "current_mount");
			GameTGATools.Ins.SetEquipEquipments(dictionary, "current_equipment");
			GameTGATools.Ins.SetAllArtifacts(dictionary, "current_legend");
			if (battleType == 0)
			{
				dictionary["battle_type"] = "主动战斗";
				dictionary["challenge_times"] = challengeTimes;
			}
			else if (battleType == 1)
			{
				dictionary["battle_type"] = "游历";
				dictionary["rate"] = rate;
				dictionary["challenge_times"] = 0;
			}
			GameTGATools.Ins.TryAddLegacyEquipSkill(dictionary, "current_inherit");
			sdk.Track(GameTGAEventName.chapter_start.ToString(), dictionary, true);
			GameTGATools.Ins.OnChapterStart();
		}

		public static void Track_ChapterEnd(this SDKManager.SDKTGA sdk, int chapterId, string sessionId, int battleType, long ingameAtk, long ingameHp, long ingameDef, int challengeTimes, List<GameEventSkillBuildData> selectSkills, int result, int maxDay, int rate, int duration, RepeatedField<RewardDto> rewardItem, int chip, List<NodeScoreParam> scores)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["chapter_id"] = chapterId;
			dictionary["session_id"] = sessionId;
			GameTGATools.Ins.SetEquipPets(dictionary, "current_pet");
			GameTGATools.Ins.SetAllMounts(dictionary, "current_mount");
			GameTGATools.Ins.SetEquipEquipments(dictionary, "current_equipment");
			GameTGATools.Ins.SetAllArtifacts(dictionary, "current_legend");
			if (result == 0)
			{
				dictionary["result"] = "success";
			}
			else if (result == 1)
			{
				dictionary["result"] = "fail";
			}
			else if (result == 2)
			{
				dictionary["result"] = "quit";
			}
			else if (result == 3)
			{
				dictionary["result"] = "resume_quit";
			}
			dictionary["max_day"] = maxDay;
			dictionary["duration"] = duration;
			List<object> list = new List<object>();
			if (rewardItem != null)
			{
				foreach (RewardDto rewardDto in rewardItem)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2["item_id"] = rewardDto.ConfigId;
					dictionary2["item_name"] = GameTGATools.Ins.GetItemName((int)rewardDto.ConfigId);
					dictionary2["item_count"] = rewardDto.Count;
					list.Add(dictionary2);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_item_new"] = list;
			}
			if (battleType == 0)
			{
				dictionary["battle_type"] = "主动战斗";
				dictionary["challenge_times"] = challengeTimes;
				dictionary["ingame_atk"] = GameTGATools.NumberOffset(ingameAtk, 10);
				dictionary["ingame_hp"] = GameTGATools.NumberOffset(ingameHp, 10);
				dictionary["ingame_def"] = GameTGATools.NumberOffset(ingameDef, 10);
				GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "selected_skills_new", selectSkills);
			}
			else if (battleType == 1)
			{
				dictionary["battle_type"] = "游历";
				dictionary["rate"] = rate;
				dictionary["challenge_times"] = 0;
			}
			GameTGATools.Ins.TryAddLegacyEquipSkill(dictionary, "current_inherit");
			List<object> list2 = new List<object>();
			if (chip > 0)
			{
				Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
				dictionary3["property_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, "GameEventData_chips");
				dictionary3["count"] = chip;
				list2.Add(dictionary3);
			}
			if (list2.Count > 0)
			{
				dictionary["reward_property"] = list2;
			}
			sdk.Track(GameTGAEventName.chapter_end.ToString(), dictionary, true);
		}

		public static void Track_ChapterBattleCheat(this SDKManager.SDKTGA sdk, int chapterId, int day, long ingameAtk, long ingameDef, long ingameHp, long ingameMaxeHP, List<GameEventSkillBuildData> selectSkills)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["chapter_id"] = chapterId;
			dictionary["day"] = day;
			dictionary["ingame_atk"] = GameTGATools.NumberOffset(ingameAtk, 10);
			dictionary["ingame_def"] = GameTGATools.NumberOffset(ingameDef, 10);
			dictionary["ingame_hp"] = GameTGATools.NumberOffset(ingameHp, 10);
			dictionary["ingame_hp_max"] = GameTGATools.NumberOffset(ingameMaxeHP, 10);
			GameTGATools.Ins.SetEquipPets(dictionary, "current_pet");
			GameTGATools.Ins.SetAllMounts(dictionary, "current_mount");
			GameTGATools.Ins.SetEquipEquipments(dictionary, "current_equipment");
			GameTGATools.Ins.SetAllArtifacts(dictionary, "current_legend");
			GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "selected_skills_new", selectSkills);
			sdk.Track(GameTGAEventName.chapter_battle_cheat.ToString(), dictionary, true);
		}

		public static void Track_StagetClickTest(this SDKManager.SDKTGA sdk, GameEventPoolData debugPoolData = null)
		{
			if (GameTGATools.Ins.StageClickTempEventID <= 0)
			{
				return;
			}
			string stageClickTempButtonName = GameTGATools.Ins.StageClickTempButtonName;
			if (string.IsNullOrEmpty(stageClickTempButtonName))
			{
				return;
			}
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			int stageClickTempDay = GameTGATools.Ins.StageClickTempDay;
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["click_button"] = stageClickTempButtonName;
			dictionary["chapter_id"] = dataModule.ChapterID;
			dictionary["session_id"] = dataModule.ChapterBattleKey;
			dictionary["day"] = stageClickTempDay;
			dictionary["ingame_exp"] = GameTGATools.Ins.StageClickTempExp;
			dictionary["ingame_level"] = GameTGATools.Ins.StageClickTempLevel;
			dictionary["ingame_atk"] = GameTGATools.Ins.StageClickTempATK;
			dictionary["ingame_hp"] = GameTGATools.Ins.StageClickTempHP;
			dictionary["ingame_hp_max"] = GameTGATools.Ins.StageClickTempHP_MAX;
			dictionary["ingame_def"] = GameTGATools.Ins.StageClickTempDEF;
			GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "selected_skills_new", GameTGATools.Ins.StageClickTempSelectedSkillList);
			dictionary["reward_rarity"] = GameTGATools.Ins.GetEventSizeTypeName(GameTGATools.Ins.StageClickTempEventSizeType);
			dictionary["event_id"] = GameTGATools.Ins.StageClickTempEventID;
			GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "skill_show", GameTGATools.Ins.StageClickTempSkillShowList);
			GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "skill_select", GameTGATools.Ins.StageClickTempSkillSelectList);
			GameTGATools.Ins.SetGameEventNodeAttParam(dictionary, "reward_buff", GameTGATools.Ins.StageClickTempAttList);
			GameTGATools.Ins.SetGameEventNodeItemParam(dictionary, "reward_item_new", GameTGATools.Ins.StageClickTempItemList);
			GameTGATools.Ins.SetGameRewardProperty(dictionary, "reward_property", GameTGATools.Ins.StageClickTempChipAttList, GameTGATools.Ins.StageScoreTempAttList);
			if (GameTGATools.Ins.StageClickTempBattleResult == 1)
			{
				dictionary["battle_result"] = "win";
				dictionary["battle_rounds"] = GameTGATools.Ins.StageClickTempBattleRound;
			}
			else if (GameTGATools.Ins.StageClickTempBattleResult == 0)
			{
				dictionary["battle_result"] = "lose";
				dictionary["battle_rounds"] = GameTGATools.Ins.StageClickTempBattleRound;
			}
			dictionary["epic_progress"] = GameTGATools.Ins.StageClickTempEpicProgress;
			dictionary["normal_progress"] = GameTGATools.Ins.StageClickTempNormalProgress;
			GameTGATools.TGADebugLog(string.Format("[Track_StageClick]day::{0},  content::{1},  reward_rarity::{2},  event_id::{3}", new object[]
			{
				stageClickTempDay,
				stageClickTempButtonName,
				dictionary["reward_rarity"],
				dictionary["event_id"]
			}));
			if (debugPoolData != null)
			{
				GameTGATools.TGADebugLog("[Track_StageClick]res::" + debugPoolData.path);
			}
			if (dictionary.ContainsKey("epic_progress"))
			{
				GameTGATools.TGADebugLog(string.Format("[Track_StageClick]eipc_progress::{0}", dictionary["epic_progress"]));
			}
			if (dictionary.ContainsKey("normal_progress"))
			{
				GameTGATools.TGADebugLog(string.Format("[Track_StageClick]normal_progress::{0}", dictionary["normal_progress"]));
			}
			if (dictionary.ContainsKey("battle_result"))
			{
				GameTGATools.TGADebugLog(string.Format("[Track_StageClick]battleResult::{0},  battleRounds::{1}", dictionary["battle_result"], dictionary["battle_rounds"]));
			}
			if (dictionary.ContainsKey("reward_buff"))
			{
				string text = string.Format("[Track_StageClick]Atts.Count::{0},  ", (dictionary["reward_buff"] as List<object>).Count);
				foreach (object obj in (dictionary["reward_buff"] as List<object>))
				{
					Dictionary<string, object> dictionary2 = obj as Dictionary<string, object>;
					text += string.Format("({0},  {1})，  ", dictionary2["buff_type"], dictionary2["name"]);
				}
				GameTGATools.TGADebugLog(text);
			}
			if (dictionary.ContainsKey("reward_item_new"))
			{
				string text2 = string.Format("[Track_StageClick]Items.Count::{0},  ", (dictionary["reward_item_new"] as List<object>).Count);
				foreach (object obj2 in (dictionary["reward_item_new"] as List<object>))
				{
					Dictionary<string, object> dictionary3 = obj2 as Dictionary<string, object>;
					text2 += string.Format("({0},  {1},  {2})，  ", dictionary3["item_id"], dictionary3["item_name"], dictionary3["item_count"]);
				}
				GameTGATools.TGADebugLog(text2);
			}
			if (dictionary.ContainsKey("reward_property"))
			{
				string text3 = string.Format("[Track_StageClick]Property.Count::{0},  ", (dictionary["reward_property"] as List<object>).Count);
				foreach (object obj3 in (dictionary["reward_property"] as List<object>))
				{
					Dictionary<string, object> dictionary4 = obj3 as Dictionary<string, object>;
					text3 += string.Format("({0},  {1})，  ", dictionary4["property_name"], dictionary4["count"]);
				}
				GameTGATools.TGADebugLog(text3);
			}
			if (dictionary.ContainsKey("skill_show"))
			{
				string text4 = string.Format("[Track_StageClick]SkillShow.Count::{0},  ", (dictionary["skill_show"] as List<object>).Count);
				foreach (object obj4 in (dictionary["skill_show"] as List<object>))
				{
					Dictionary<string, object> dictionary5 = obj4 as Dictionary<string, object>;
					text4 += string.Format("({0},  {1})，  ", dictionary5["skill_name"], dictionary5["skill_rarity"]);
				}
				GameTGATools.TGADebugLog(text4);
			}
			if (dictionary.ContainsKey("skill_select"))
			{
				string text5 = string.Format("[Track_StageClick]SkillSelect.Count::{0},  ", (dictionary["skill_select"] as List<object>).Count);
				foreach (object obj5 in (dictionary["skill_select"] as List<object>))
				{
					Dictionary<string, object> dictionary6 = obj5 as Dictionary<string, object>;
					text5 += string.Format("({0},  {1})，  ", dictionary6["skill_name"], dictionary6["skill_rarity"]);
				}
				GameTGATools.TGADebugLog(text5);
			}
			if (dictionary.ContainsKey("selected_skills_new"))
			{
				GameTGATools.TGADebugLog(string.Format("[Track_StageClick]SelectedSkills.Count::{0},  ", (dictionary["selected_skills_new"] as List<object>).Count));
			}
			GameTGATools.TGADebugLog("[Track_StageClick]-------------------------------------------------------------");
			sdk.Track(GameTGAEventName.stage_click.ToString(), dictionary, true);
			GameTGATools.Ins.ClearStageSelectTempData();
		}

		public static void Track_TowerBattleStart(this SDKManager.SDKTGA sdk, int towerId, int stageId, int towerTicketAmt)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["tower_id"] = towerId;
			dictionary["stage_id"] = stageId;
			dictionary["tower_ticket_amt"] = towerTicketAmt;
			sdk.Track(GameTGAEventName.tower_battle_start.ToString(), dictionary, true);
		}

		public static void Track_TowerBattleEnd(this SDKManager.SDKTGA sdk, int towerId, int stageId, int towerTicketAmt, int result, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["tower_id"] = towerId;
			dictionary["stage_id"] = stageId;
			dictionary["tower_ticket_amt"] = towerTicketAmt;
			if (result == 1)
			{
				dictionary["result"] = "success";
			}
			else
			{
				dictionary["result"] = "fail";
			}
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.tower_battle_end.ToString(), dictionary, true);
		}

		public static void Track_ChapterReward(this SDKManager.SDKTGA sdk, int chapterId, int day, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["chapter_id"] = chapterId;
			dictionary["day"] = day;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.chapter_reward.ToString(), dictionary, true);
		}

		public static void Track_IdleReward(this SDKManager.SDKTGA sdk, long idleDuration, bool isAD, RepeatedField<RewardDto> rewardDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["idle_duration"] = idleDuration;
			dictionary["is_ad"] = isAD;
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.idle_reward.ToString(), dictionary, true);
		}

		public static void Track_ArenaBattleStart(this SDKManager.SDKTGA sdk, CrossArenaRankMember enemy)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			dictionary["arena_grade"] = CrossArenaDataModule.GetCrossArenaDanName(dataModule.Dan, 2);
			dictionary["arena_point"] = dataModule.MyRankInfo.Score;
			dictionary["battle_point"] = GameTGATools.NumberOffset(dataModule.MyRankInfo.Power, 10);
			dictionary["arena_rank"] = dataModule.MyRankInfo.Rank;
			dictionary["arena_groupid"] = dataModule.GroupId;
			int ticketCount = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.CrossArena);
			dictionary["ticket_left"] = ticketCount;
			dictionary["enemy_battle_point"] = GameTGATools.NumberOffset(enemy.Power, 10);
			if (enemy.Rank > 0)
			{
				dictionary["enemy_rank"] = enemy.Rank;
			}
			dictionary["is_bot"] = enemy.UserID < 0L;
			sdk.Track(GameTGAEventName.arena_battle_start.ToString(), dictionary, true);
		}

		public static void Track_ArenaBattleEnd(this SDKManager.SDKTGA sdk, CrossArenaRankMember enemy, int result, int rank, int score)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			dictionary["arena_grade"] = CrossArenaDataModule.GetCrossArenaDanName(dataModule.Dan, 2);
			dictionary["arena_point"] = score;
			dictionary["battle_point"] = GameTGATools.NumberOffset(dataModule.MyRankInfo.Power, 10);
			dictionary["arena_rank"] = rank;
			dictionary["arena_groupid"] = dataModule.GroupId;
			int ticketCount = GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.CrossArena);
			dictionary["ticket_left"] = ticketCount;
			dictionary["enemy_battle_point"] = GameTGATools.NumberOffset(enemy.Power, 10);
			if (enemy.Rank > 0)
			{
				dictionary["enemy_rank"] = enemy.Rank;
			}
			dictionary["is_bot"] = enemy.UserID < 0L;
			dictionary["result"] = ((result == 1) ? "win" : "lose");
			sdk.Track(GameTGAEventName.arena_battle_end.ToString(), dictionary, true);
		}

		public static void Track_Raid(this SDKManager.SDKTGA sdk, int raidId, int raidDiffculty, bool isSweep, int result, RepeatedField<CostDto> costDtos, RepeatedField<RewardDto> rewardDtos)
		{
			Dungeon_DungeonBase elementById = GameApp.Table.GetManager().GetDungeon_DungeonBaseModelInstance().GetElementById(raidId);
			if (elementById == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["raid_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, elementById.name);
			dictionary["raid_difficulty"] = raidDiffculty;
			dictionary["type_new"] = (isSweep ? "扫荡" : "挑战");
			dictionary["challenge_result"] = ((result == 1) ? "win" : "lose");
			if (costDtos != null && costDtos.Count > 0)
			{
				List<object> list = new List<object>();
				GameTGATools.Ins.TryAddCostDto_AllItem(costDtos[0], list);
				if (list.Count <= 0)
				{
					return;
				}
				Dictionary<string, object> dictionary2 = (Dictionary<string, object>)list[0];
				dictionary["cost_currency"] = dictionary2["item_name"];
				dictionary["cost_amt"] = dictionary2["item_count"];
			}
			List<object> list2 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list2);
				}
			}
			if (list2.Count > 0)
			{
				dictionary["reward_list"] = list2;
			}
			sdk.Track(GameTGAEventName.raid.ToString(), dictionary, true);
		}

		public static void Track_EnterDungeon(this SDKManager.SDKTGA sdk, int result, long ingameAtk, long ingameHp, long ingameDef, List<GameEventSkillBuildData> selectSkills, RepeatedField<RewardDto> rewardItem)
		{
			if (result == -1)
			{
				return;
			}
			RogueDungeonDataModule dataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			if (dataModule == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			if (result == 0)
			{
				dictionary["result"] = "success";
			}
			else if (result == 1)
			{
				dictionary["result"] = "fail";
			}
			else if (result == 2)
			{
				dictionary["result"] = "quit";
			}
			else if (result == 3)
			{
				dictionary["result"] = "resume_quit";
			}
			if (dataModule.MonsterSkills != null && dataModule.MonsterSkills.Count > 0)
			{
				List<int> list = new List<int>();
				list.AddRange(dataModule.MonsterSkills);
				dictionary["entry"] = list;
			}
			GameTGATools.Ins.SetGameEventSkillBuildData(dictionary, "selected_skills_new", selectSkills);
			dictionary["ingame_atk"] = GameTGATools.NumberOffset(ingameAtk, 10);
			dictionary["ingame_hp"] = GameTGATools.NumberOffset(ingameHp, 10);
			dictionary["ingame_def"] = GameTGATools.NumberOffset(ingameDef, 10);
			uint currentFloorID = dataModule.CurrentFloorID;
			uint passFloorCount = dataModule.PassFloorCount;
			dictionary["chapter_id_start"] = currentFloorID - passFloorCount;
			dictionary["chapter_id_end"] = currentFloorID;
			List<object> list2 = new List<object>();
			if (rewardItem != null)
			{
				foreach (RewardDto rewardDto in rewardItem)
				{
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2["item_id"] = rewardDto.ConfigId;
					dictionary2["item_name"] = GameTGATools.Ins.GetItemName((int)rewardDto.ConfigId);
					dictionary2["item_count"] = rewardDto.Count;
					list2.Add(dictionary2);
				}
			}
			if (list2.Count > 0)
			{
				dictionary["reward_item_new"] = list2;
			}
			GameTGATools.Ins.TryAddLegacyEquipSkill(dictionary, "current_inherit");
			GameTGATools.Ins.SetEquipPets(dictionary, "current_pet");
			GameTGATools.Ins.SetAllMounts(dictionary, "current_mount");
			GameTGATools.Ins.SetEquipEquipments(dictionary, "current_equipment");
			GameTGATools.Ins.SetAllArtifacts(dictionary, "current_legend");
			sdk.Track(GameTGAEventName.enter_dungeon.ToString(), dictionary, true);
		}

		private static void OnMessageCommonData_Collection(IMessage message, CommonData commonData)
		{
			if (message == null || commonData == null)
			{
				return;
			}
			GameApp.SDK.Analyze.Track_Collection(commonData.Reward);
		}

		public static void Track_CollectionUpgrade(this SDKManager.SDKTGA sdk, int collectionId)
		{
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			if (!dataModule.collectionDict.ContainsKey(collectionId))
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			CollectionData collectionData = dataModule.collectionDict[collectionId];
			List<object> list = new List<object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			dictionary2["collection_id"] = collectionData.itemId;
			dictionary2["collection_name"] = GameTGATools.Ins.GetItemName(collectionData.itemId);
			dictionary2["collection_rarity"] = CollectionHelper.GetRarityName(collectionData.rarity);
			dictionary2["collection_grade"] = collectionData.collectionStar;
			list.Add(dictionary2);
			dictionary["collection_grade_after"] = list;
			List<object> list2 = new List<object>();
			foreach (CollectionSuitData collectionSuitData in dataModule.collectionSuitDict.Values)
			{
				if (collectionSuitData.CurIndex != 0)
				{
					string suitName = collectionSuitData.GetSuitName();
					Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
					dictionary3["set_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, suitName);
					list2.Add(dictionary3);
				}
			}
			if (list2.Count > 0)
			{
				dictionary["active_set_new"] = list2;
			}
			sdk.Track(GameTGAEventName.collection_upgrade.ToString(), dictionary, true);
		}

		public static void Track_Collection(this SDKManager.SDKTGA sdk, RepeatedField<RewardDto> rewardDtos)
		{
			if (rewardDtos == null || rewardDtos.Count == 0)
			{
				return;
			}
			List<object> list = new List<object>();
			List<uint> list2 = new List<uint>();
			foreach (RewardDto rewardDto in rewardDtos)
			{
				if (!list2.Contains(rewardDto.ConfigId))
				{
					GameTGATools.Ins.TryAddRewardDto_Colliction(rewardDto, list);
					list2.Add(rewardDto.ConfigId);
				}
			}
			if (list.Count == 0)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["get_collection"] = list;
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			List<object> list3 = new List<object>();
			foreach (CollectionSuitData collectionSuitData in dataModule.collectionSuitDict.Values)
			{
				if (collectionSuitData.CurIndex != 0)
				{
					string suitName = collectionSuitData.GetSuitName();
					Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
					dictionary2["set_name"] = Singleton<LanguageManager>.Instance.GetInfoByID(2, suitName);
					list3.Add(dictionary2);
				}
			}
			if (list3.Count > 0)
			{
				dictionary["active_set_new"] = list3;
			}
			string text = string.Format("[Track_Collection]Collections.Count::{0},  ", (dictionary["get_collection"] as List<object>).Count);
			foreach (object obj in (dictionary["get_collection"] as List<object>))
			{
				Dictionary<string, object> dictionary3 = obj as Dictionary<string, object>;
				text += string.Format("({0},  {1},  {2}，  {3})，  ", new object[]
				{
					dictionary3["item_id"],
					dictionary3["item_name"],
					dictionary3["item_rarity"],
					dictionary3["item_level"]
				});
			}
			sdk.Track(GameTGAEventName.collection.ToString(), dictionary, true);
		}

		public static void Track_EquipmentBoxOpen(this SDKManager.SDKTGA sdk, string boxType, GameTGACostCurrency costCurrency, int costAmt, RepeatedField<RewardDto> rewardDtos, List<TGACommonItemInfo> rewardPreview)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["box_type"] = boxType;
			if (costCurrency != GameTGACostCurrency.Free)
			{
				dictionary["cost_currency"] = GameTGATools.GameTGACostCurrencyName(costCurrency);
			}
			if (costCurrency != GameTGACostCurrency.Ad && costCurrency != GameTGACostCurrency.Free)
			{
				dictionary["cost_amt"] = costAmt;
			}
			List<object> list = new List<object>();
			List<object> list2 = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_Item(rewardDto, list);
					GameTGATools.Ins.TryAddRewardDto_Equipment(rewardDto, list2);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			if (list2.Count > 0)
			{
				dictionary["reward_equipment"] = list2;
			}
			if (rewardPreview != null && rewardPreview.Count > 0)
			{
				dictionary["up_optional"] = rewardPreview.ToJson();
			}
			sdk.Track(GameTGAEventName.equipment_box_open.ToString(), dictionary, true);
		}

		public static void Track_UpEquipmentOptional(this SDKManager.SDKTGA sdk, long time_left, List<TGACommonItemInfo> up_optional, TGACommonItemInfo equip)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["time_left"] = time_left;
			dictionary["up_optional"] = up_optional.ToJson();
			dictionary["equipment_get"] = equip.ToJson();
			sdk.Track(GameTGAEventName.up_equipment_optional.ToString(), dictionary, true);
		}

		public static void Track_EquipmentMerge(this SDKManager.SDKTGA sdk, RepeatedField<EquipmentDto> equipmentDtos, RepeatedField<long> deleteRowIDs, bool isAuto)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			if (equipmentDtos != null && equipmentDtos.Count > 0)
			{
				foreach (EquipmentDto equipmentDto in equipmentDtos)
				{
					GameTGATools.Ins.TryAddEquipmentDto_Equipment(equipmentDto, list);
				}
			}
			dictionary["merged_equipment"] = list;
			EquipDataModule dataModule = GameApp.Data.GetDataModule(DataName.EquipDataModule);
			List<EquipData> list2 = new List<EquipData>();
			if (deleteRowIDs != null && deleteRowIDs.Count > 0)
			{
				foreach (long num in deleteRowIDs)
				{
					if (dataModule.m_equipDatas.ContainsKey((ulong)num))
					{
						EquipData equipData = dataModule.m_equipDatas[(ulong)num];
						list2.Add(equipData);
					}
				}
			}
			List<object> list3 = new List<object>();
			if (list2 != null && list2.Count > 0)
			{
				foreach (EquipData equipData2 in list2)
				{
					GameTGATools.Ins.TryAddEquipData_Equipment(equipData2, list3);
				}
			}
			dictionary["cost_equipment"] = list3;
			dictionary["is_auto"] = isAuto;
			sdk.Track(GameTGAEventName.equipment_merge.ToString(), dictionary, true);
		}

		public static void Track_EquipmentLevel(this SDKManager.SDKTGA sdk, RepeatedField<EquipmentDto> equipmentDtos)
		{
			if (equipmentDtos == null || equipmentDtos.Count == 0 || equipmentDtos[0] == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			GameTGATools.Ins.TryAddEquipmentDto_Equipment(equipmentDtos[0], list);
			dictionary["leveled_equipment"] = list;
			sdk.Track(GameTGAEventName.equipment_level.ToString(), dictionary, true);
		}

		public static void Track_GuildActivity(this SDKManager.SDKTGA sdk, string step, TGA_GuildActivityData data, List<long> memberuids = null)
		{
			if (data == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["step"] = step;
			dictionary["guild_id"] = data.GuildID;
			dictionary["guild_level"] = data.GuildLevel;
			dictionary["guild_exp"] = data.GuildExp;
			dictionary["guild_power"] = GameTGATools.NumberOffset(data.GuildPower, 10);
			dictionary["guild_total_member"] = data.GuildMemberCount;
			dictionary["week_contribution"] = data.WeekActivity;
			if (memberuids != null && memberuids.Count > 0)
			{
				List<object> list = new List<object>();
				foreach (long num in memberuids)
				{
					list.Add(num);
				}
				dictionary["member_uid"] = list;
			}
			sdk.Track(GameTGAEventName.guild_activity.ToString(), dictionary, true);
		}

		public static void Track_GuildBossBattleEnd(this SDKManager.SDKTGA sdk, int bossId, int bossLevel, ulong damagePoint)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			GuildBossInfo guildBoss = GuildSDKManager.Instance.GuildActivity.GuildBoss;
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(140);
			if (guildConstTable != null)
			{
				if (bossLevel > guildConstTable.TypeInt)
				{
					dictionary["type_new"] = "server";
					dictionary["guild_boss_rank"] = guildBoss.GuildRank;
					dictionary["rank_stage"] = guildBoss.GuildDan.ToString();
				}
				else
				{
					dictionary["type_new"] = "single";
				}
			}
			dictionary["boss_id"] = bossId;
			dictionary["boss_level"] = bossLevel;
			if (guildBoss != null)
			{
				dictionary["personal_boss_rank"] = guildBoss.PersonRank;
				dictionary["damage_point_total"] = GameTGATools.NumberOffset(guildBoss.TotalPersonalDamage, 10);
				List<CardData> guildBossEnemyCardDatas = GuildController.GetGuildBossEnemyCardDatas(GuildProxy.Table.TableMgr, guildBoss.BossData.BossStep, -1L);
				long num = 0L;
				if (guildBossEnemyCardDatas.Count > 0)
				{
					num = guildBossEnemyCardDatas[0].m_memberAttributeData.GetHpMax().GetValue();
				}
				long num2 = 0L;
				if (guildBoss.BossData.CurHP == -1L)
				{
					if (guildBossEnemyCardDatas.Count > 0)
					{
						num2 = num;
					}
				}
				else
				{
					num2 = guildBoss.BossData.CurHP;
				}
				long num3 = num2;
				if (num3 < 0L)
				{
					num3 = 0L;
				}
				dictionary["health_remain"] = GameTGATools.NumberOffset(num3, 10);
			}
			dictionary["damage_point"] = GameTGATools.NumberOffset((long)damagePoint, 10);
			sdk.Track(GameTGAEventName.guild_boss_battle_end.ToString(), dictionary, true);
		}

		public static void Track_GuildBossReward(this SDKManager.SDKTGA sdk, RepeatedField<RewardDto> rewardDtos)
		{
			GuildBossInfo guildBoss = GuildSDKManager.Instance.GuildActivity.GuildBoss;
			if (guildBoss == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["boss_id"] = guildBoss.BossData.BossID;
			dictionary["boss_level"] = guildBoss.BossData.BossStep;
			List<CardData> guildBossEnemyCardDatas = GuildController.GetGuildBossEnemyCardDatas(GuildProxy.Table.TableMgr, guildBoss.BossData.BossStep, -1L);
			long num = 0L;
			if (guildBossEnemyCardDatas.Count > 0)
			{
				num = guildBossEnemyCardDatas[0].m_memberAttributeData.GetHpMax().GetValue();
			}
			dictionary["require_point"] = GameTGATools.NumberOffset(num, 10);
			List<object> list = new List<object>();
			if (rewardDtos != null && rewardDtos.Count > 0)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_list"] = list;
			}
			sdk.Track(GameTGAEventName.guild_boss_reward.ToString(), dictionary, true);
		}

		public static void Track_IAPShow(this SDKManager.SDKTGA sdk, GameTGAIAPParam param)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			sdk.Track(GameTGAEventName.iap_show.ToString(), dictionary, true);
		}

		public static void Track_IAPOrder(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			sdk.Track(GameTGAEventName.iap_order.ToString(), dictionary, true);
		}

		public static void Track_IAPFinishChannel(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			sdk.Track(GameTGAEventName.iap_finish_channel.ToString(), dictionary, true);
		}

		public static void Track_IAPSuccess(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID, int isSandBox, RepeatedField<RewardDto> rewardDtos)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			if (isSandBox == 1)
			{
				dictionary["is_sandbox"] = true;
			}
			else if (isSandBox == 0)
			{
				dictionary["is_sandbox"] = false;
			}
			List<object> list = new List<object>();
			if (rewardDtos != null)
			{
				foreach (RewardDto rewardDto in rewardDtos)
				{
					GameTGATools.Ins.TryAddRewardDto_AllItem(rewardDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["item_list"] = list;
			}
			sdk.Track(GameTGAEventName.iap_success.ToString(), dictionary, true);
		}

		public static void Track_IAPFail(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID, string failReason, int isSandBox)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			if (!string.IsNullOrEmpty(failReason))
			{
				dictionary["fail_reason"] = failReason;
			}
			if (isSandBox == 1)
			{
				dictionary["is_sandbox"] = true;
			}
			else if (isSandBox == 0)
			{
				dictionary["is_sandbox"] = false;
			}
			sdk.Track(GameTGAEventName.iap_fail.ToString(), dictionary, true);
		}

		public static void Track_IAPOrder(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID, int isSandBox)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			dictionary["is_sandbox"] = false;
			sdk.Track(GameTGAEventName.iap_order.ToString(), dictionary, true);
		}

		public static void Track_IAPFinishChannel(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID, int isSandBox)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			dictionary["is_sandbox"] = false;
			sdk.Track(GameTGAEventName.iap_finish_channel.ToString(), dictionary, true);
		}

		public static void Track_IAPFail(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string failReason, int isSandBox)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			if (!string.IsNullOrEmpty(failReason))
			{
				dictionary["fail_reason"] = failReason;
			}
			dictionary["is_sandbox"] = false;
			sdk.Track(GameTGAEventName.iap_fail.ToString(), dictionary, true);
		}

		public static void Track_IAPFail(this SDKManager.SDKTGA sdk, GameTGAIAPParam param, string orderID, string failReason)
		{
			if (param == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["source"] = param.Source;
			dictionary["product_id"] = param.ProductID;
			dictionary["product_id_int"] = param.ProductIDInt;
			dictionary["product_price"] = param.ProductPrice;
			dictionary["local_price"] = param.LocalPrice;
			dictionary["currency"] = param.Currency;
			dictionary["order_id"] = orderID;
			if (!string.IsNullOrEmpty(failReason))
			{
				dictionary["fail_reason"] = failReason;
			}
			dictionary["is_sandbox"] = false;
			sdk.Track(GameTGAEventName.iap_fail.ToString(), dictionary, true);
		}

		public static void Track_EquipMount(this SDKManager.SDKTGA sdk, MountInfo mountInfo)
		{
			if (mountInfo == null || mountInfo.ConfigType == 0U)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
			List<object> list = new List<object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (mountInfo.ConfigType == 1U)
			{
				using (List<MountBasicData>.Enumerator enumerator = dataModule.GetBasicDataList().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MountBasicData mountBasicData = enumerator.Current;
						if ((long)mountBasicData.ID == (long)((ulong)mountInfo.ConfigId))
						{
							dictionary2["mount_id"] = mountBasicData.MemberConfig.id;
							dictionary2["mount_name"] = GameTGATools.Ins.GetItemName(mountBasicData.MemberConfig.id);
							dictionary2["mount_stage"] = mountInfo.Stage;
							dictionary2["mount_grade"] = mountInfo.Level;
							break;
						}
					}
					goto IL_01C9;
				}
			}
			if (mountInfo.ConfigType == 2U)
			{
				foreach (MountAdvanceData mountAdvanceData in dataModule.GetAdvanceDataList())
				{
					if ((long)mountAdvanceData.ID == (long)((ulong)mountInfo.ConfigId))
					{
						dictionary2["mount_id"] = mountAdvanceData.MemberConfig.id;
						dictionary2["mount_name"] = GameTGATools.Ins.GetItemName(mountAdvanceData.MemberConfig.id);
						dictionary2["rare_star"] = mountAdvanceData.Star;
						dictionary2["mount_rarity"] = GameTGATools.Ins.ItemQualityToColor(mountAdvanceData.Config.quality);
						break;
					}
				}
			}
			IL_01C9:
			list.Add(dictionary2);
			dictionary["equip_mount"] = list;
			foreach (MergeAttributeData mergeAttributeData in dataModule.GetCurrentLevelInfo().attribute.GetMergeAttributeData())
			{
				if (mergeAttributeData.Header.Contains("HP") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_hp"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Attack") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_atk"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Defence") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_def"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
			}
			sdk.Track(GameTGAEventName.equip_mount.ToString(), dictionary, true);
		}

		public static void Track_MountLevel(this SDKManager.SDKTGA sdk, MountInfo mountInfo, RepeatedField<CostDto> costDtos, int preStage, int preLevel)
		{
			if (mountInfo == null)
			{
				return;
			}
			MountDataModule dataModule = GameApp.Data.GetDataModule(DataName.MountDataModule);
			List<MountBasicData> basicDataList = dataModule.GetBasicDataList();
			if ((ulong)mountInfo.Stage >= (ulong)((long)basicDataList.Count))
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			MountBasicData mountBasicData = basicDataList[(int)mountInfo.Stage];
			dictionary2["mount_id"] = mountBasicData.MemberConfig.id;
			dictionary2["mount_name"] = GameTGATools.Ins.GetItemName(mountBasicData.MemberConfig.id);
			dictionary2["mount_stage"] = mountInfo.Stage;
			dictionary2["mount_grade"] = mountInfo.Level;
			list.Add(dictionary2);
			dictionary["mount_level_after"] = list;
			foreach (MergeAttributeData mergeAttributeData in dataModule.GetCurrentLevelInfo().attribute.GetMergeAttributeData())
			{
				if (mergeAttributeData.Header.Contains("HP") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_hp"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Attack") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_atk"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
				else if (mergeAttributeData.Header.Contains("Defence") && mergeAttributeData.Value.GetValue() > 0L)
				{
					dictionary["mount_def"] = (float)mergeAttributeData.Value.GetValue() / 100f;
				}
			}
			if (costDtos != null && costDtos.Count > 0 && costDtos[0] != null)
			{
				dictionary["cost_currency"] = GameTGATools.Ins.GetItemName((int)costDtos[0].ConfigId);
				dictionary["cost_amt"] = Mathf.Abs((float)costDtos[0].Count);
			}
			dictionary["exp"] = dictionary["cost_amt"];
			IList<Mount_mountLevel> allElements = GameApp.Table.GetManager().GetMount_mountLevelModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Mount_mountLevel mount_mountLevel = allElements[i];
				if (mount_mountLevel.stage == preStage && mount_mountLevel.star == preLevel)
				{
					dictionary["total_exp"] = mount_mountLevel.levelCost;
					break;
				}
			}
			sdk.Track(GameTGAEventName.mount_level.ToString(), dictionary, true);
		}

		public static void Track_RareMountUpgrade(this SDKManager.SDKTGA sdk, int mountConfigID, MountItemDto mountItemDto, RepeatedField<CostDto> costDtos)
		{
			if (mountItemDto == null)
			{
				return;
			}
			if (mountConfigID != mountItemDto.ConfigId)
			{
				return;
			}
			Mount_advanceMount elementById = GameApp.Table.GetManager().GetMount_advanceMountModelInstance().GetElementById(mountConfigID);
			if (elementById == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["mount_id"] = elementById.memberId;
			dictionary["mount_name"] = GameTGATools.Ins.GetItemName(elementById.memberId);
			dictionary["star_after"] = mountItemDto.Star;
			if (costDtos != null && costDtos.Count > 0 && costDtos[0] != null)
			{
				dictionary["cost_shards_amt"] = Mathf.Abs((float)costDtos[0].Count);
			}
			sdk.Track(GameTGAEventName.rare_mount_upgrade.ToString(), dictionary, true);
		}

		public static void Track_MountUnlock(this SDKManager.SDKTGA sdk, int mountConfigID)
		{
			Mount_advanceMount elementById = GameApp.Table.GetManager().GetMount_advanceMountModelInstance().GetElementById(mountConfigID);
			if (elementById == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["mount_id"] = elementById.memberId;
			dictionary["mount_name"] = GameTGATools.Ins.GetItemName(elementById.memberId);
			sdk.Track(GameTGAEventName.mount_unlock.ToString(), dictionary, true);
		}

		public static void OnViewOpen(ViewName viewName)
		{
			GameTGAExtend.OnViewOpen(GameTGAExtend.PageViewName(viewName));
		}

		public static void OnViewClose(ViewName viewName)
		{
			GameTGAExtend.OnViewClose(GameTGAExtend.PageViewName(viewName));
		}

		public static void OnViewOpen(string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				return;
			}
			GameTGATools.Ins.OnOpenPage(viewName);
			if (!GameApp.SDK.Analyze.IsLogin)
			{
				return;
			}
			TGASource_Page elementById = GameApp.Table.GetManager().GetTGASource_PageModelInstance().GetElementById(viewName.ToString());
			if (elementById == null)
			{
				HLog.LogError("[Debug_TrackPage]OnViewOpen,  需要在TGASource_Page表里配置", viewName);
				return;
			}
			if (elementById.track == 0)
			{
				return;
			}
			GameTGATools.Ins.ViewOpenTimes[viewName] = GameTGATools.Ins.Timer;
		}

		public static void OnViewClose(string viewName)
		{
			if (string.IsNullOrEmpty(viewName))
			{
				return;
			}
			if (!GameTGATools.Ins.ViewOpenTimes.ContainsKey(viewName))
			{
				return;
			}
			TGASource_Page elementById = GameApp.Table.GetManager().GetTGASource_PageModelInstance().GetElementById(viewName.ToString());
			if (elementById == null)
			{
				HLog.LogError("[Debug_TrackPage]OnViewClose,  需要在TGASource_Page表里配置", viewName.ToString());
				GameTGATools.Ins.ViewOpenTimes.Remove(viewName);
				return;
			}
			double timer = GameTGATools.Ins.Timer;
			double num = GameTGATools.Ins.ViewOpenTimes[viewName];
			GameApp.SDK.Analyze.Track_PageSwitch(elementById.source, (int)(timer - num), viewName);
			GameTGATools.Ins.ViewOpenTimes.Remove(viewName);
			GameTGATools.Ins.OnTrackPage(viewName);
		}

		private static string PageViewName(ViewName viewName)
		{
			if (viewName == ViewName.MainViewModule || viewName == ViewName.IAPFundViewModule)
			{
				return "";
			}
			return viewName.ToString();
		}

		public static void Track_PageSwitch(this SDKManager.SDKTGA sdk, string pageName, int duration, string viewName)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["page_name"] = pageName;
			dictionary["duration"] = duration;
			sdk.Track(GameTGAEventName.page_switch.ToString(), dictionary, true);
		}

		public static void Track_PetBoxOpen(this SDKManager.SDKTGA sdk, int eggLevel, GameTGACostCurrency costCurrency, int costAmt, string source, RepeatedField<PetDto> petDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["egg_level"] = eggLevel;
			dictionary["cost_currency"] = GameTGATools.GameTGACostCurrencyName(costCurrency);
			if (costCurrency != GameTGACostCurrency.Ad && costCurrency != GameTGACostCurrency.Free)
			{
				dictionary["cost_amt"] = costAmt;
			}
			dictionary["source"] = source;
			List<object> list = new List<object>();
			if (petDtos != null && petDtos.Count > 0)
			{
				foreach (PetDto petDto in petDtos)
				{
					GameTGATools.Ins.TryAddPetDto(petDto, list);
				}
			}
			if (list.Count > 0)
			{
				dictionary["reward_pet_list"] = list;
			}
			sdk.Track(GameTGAEventName.pet_box_open.ToString(), dictionary, true);
		}

		public static void Track_PetLevel(this SDKManager.SDKTGA sdk, PetDto petDto, RepeatedField<CostDto> costDtos)
		{
			if (petDto == null)
			{
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			List<object> list = new List<object>();
			GameTGATools.Ins.TryAddPetDto(petDto, list);
			dictionary["level_pets_after"] = list;
			if (costDtos != null && costDtos.Count > 0 && costDtos[0] != null)
			{
				long num = 0L;
				foreach (CostDto costDto in costDtos)
				{
					if (costDto.ConfigId == 26U)
					{
						dictionary["cost_currency"] = GameTGATools.Ins.GetItemName((int)costDtos[0].ConfigId);
						num += costDto.Count;
					}
					dictionary["cost_amt"] = Mathf.Abs((float)num);
				}
			}
			sdk.Track(GameTGAEventName.pet_level.ToString(), dictionary, true);
		}

		public static void Track_TalentLegacyStudy(this SDKManager.SDKTGA sdk, int nodeId, RepeatedField<CostDto> costDtos)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			GameTGATools.Ins.SetPublicInfo(dictionary);
			dictionary["skill_id"] = nodeId;
			List<object> list = new List<object>();
			for (int i = 0; i < costDtos.Count; i++)
			{
				GameTGATools.Ins.TryAddLegacyCostDto(costDtos[i], list);
			}
			if (list.Count > 0)
			{
				dictionary["cost_item_list"] = list;
			}
			List<object> list2 = new List<object>();
			GameTGATools.Ins.TryAddLegacyRank(list2);
			if (list2.Count > 0)
			{
				dictionary["rank_top3"] = list2;
			}
			List<object> list3 = new List<object>();
			GameTGATools.Ins.TryAddLegacyStudyFinishNode(list3);
			if (list3.Count > 0)
			{
				dictionary["unlock_skills"] = list3;
			}
			sdk.Track(GameTGAEventName.hero_inheritance.ToString(), dictionary, true);
		}
	}
}
