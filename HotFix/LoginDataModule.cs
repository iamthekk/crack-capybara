using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dxx.Guild;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.Modules;
using LocalModels.Bean;
using Proto.Common;
using Proto.Mining;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class LoginDataModule : IDataModule
	{
		public bool IsNewUser { get; private set; }

		public int GetName()
		{
			return 105;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginData));
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, new HandlerEvent(this.OnEventRefreshLordAddSlaveData));
			FrameworkExpand.RegisterEvent(manager, 90002, new HandlerEvent(this.OnEventShowTip));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginData));
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, new HandlerEvent(this.OnEventRefreshLordAddSlaveData));
			FrameworkExpand.UnRegisterEvent(manager, 90002, new HandlerEvent(this.OnEventShowTip));
		}

		public void Reset()
		{
			this.userId = 0L;
			this.abVersion = 0U;
			this.userCurrency = null;
			this.userLevel = null;
			this.userMission = null;
			this.UserInfo = null;
		}

		private void OnEventShowTip(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgTips eventArgTips = eventArgs as EventArgTips;
			if (eventArgTips == null)
			{
				return;
			}
			string languageTips = eventArgTips.LanguageTips;
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID(languageTips);
			GameApp.View.ShowStringTip(infoByID);
		}

		private void OnLoginData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventLogin eventLogin = eventArgs as EventLogin;
			if (eventLogin == null)
			{
				return;
			}
			UserLoginResponse userLoginResponse = eventLogin.userLoginResponse;
			if (userLoginResponse == null)
			{
				return;
			}
			this.SetLoginDataBase(userLoginResponse);
			GameApp.Data.GetDataModule(DataName.HeroDataModule).SetLoginData(userLoginResponse);
			GameApp.Data.GetDataModule(DataName.ChapterDataModule).SetLoginData(userLoginResponse);
			GameApp.Data.GetDataModule(DataName.TalentDataModule).SetLoginTalentData(userLoginResponse.TalentsInfo);
			GameApp.Data.GetDataModule(DataName.ChestDataModule).UpdateChestInfo(userLoginResponse.ChestInfo);
			RedPointController.Instance.ReCalc("Main.ChapterReward", true);
			EventArgsSetEquipData instance = Singleton<EventArgsSetEquipData>.Instance;
			instance.SetData(userLoginResponse);
			GameApp.Event.DispatchNow(this, 146, instance);
			RedPointController.Instance.ReCalc("Equip.Hero", true);
			GameApp.Data.GetDataModule(DataName.PetDataModule).InitServerData(userLoginResponse.PetInfo);
			RedPointController.Instance.ReCalc("Equip.Pet", true);
			EventPropList instance2 = Singleton<EventPropList>.Instance;
			instance2.SetData(userLoginResponse.Items);
			GameApp.Event.DispatchNow(this, 113, instance2);
			EventArgsSetRelicData instance3 = Singleton<EventArgsSetRelicData>.Instance;
			instance3.SetData(userLoginResponse);
			GameApp.Event.DispatchNow(this, 180, instance3);
			RedPointController.Instance.ReCalc("Equip.Relic", true);
			EventArgsSetMainCityData instance4 = Singleton<EventArgsSetMainCityData>.Instance;
			instance4.SetData(userLoginResponse);
			GameApp.Event.DispatchNow(this, 112, instance4);
			EventArgsSetHeroLevelUpData instance5 = Singleton<EventArgsSetHeroLevelUpData>.Instance;
			instance5.SetData(userLoginResponse);
			GameApp.Event.DispatchNow(this, 114, instance5);
			GameApp.Data.GetDataModule(DataName.SevenDayCarnivalDataModule).OnInit();
			GameApp.Data.GetDataModule(DataName.TalentLegacyDataModule).OnInit();
			GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule).SetLoginData(userLoginResponse.ActiveInfo, userLoginResponse.RewardInfo);
			GameApp.Data.GetDataModule(DataName.DungeonDataModule).SetLoginData(userLoginResponse.DungeonInfo);
			GameApp.Data.GetDataModule(DataName.MountDataModule).SetLoginData(userLoginResponse.MountInfo, userLoginResponse.MountItemDtos);
			GameApp.Data.GetDataModule(DataName.ArtifactDataModule).SetLoginData(userLoginResponse.ArtifactInfo, userLoginResponse.ArtifactItemDtos);
			GameApp.Data.GetDataModule(DataName.HangUpDataModule).SetLoginData(userLoginResponse.HungUpInfoDto);
			GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).SetLoginData(userLoginResponse.HellStage, userLoginResponse.HellBattleStatus);
			NewWorldDataModule dataModule = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
			dataModule.SetLoginData(userLoginResponse.EnterNewWorld);
			GameApp.Data.GetDataModule(DataName.CollectionDataModule).InitServerData(userLoginResponse.CollectionInfo, userLoginResponse.UserStatisticInfo);
			RedPointController.Instance.ReCalc("Equip.Collection", true);
			EventArgsBool instance6 = Singleton<EventArgsBool>.Instance;
			instance6.SetData(false);
			GameApp.Event.DispatchNow(this, 145, instance6);
			GameApp.Data.GetDataModule(DataName.TicketDataModule).SetLoginData(userLoginResponse);
			GameApp.Data.GetDataModule(DataName.TicketDailyExchangeDataModule).SetLoginData(userLoginResponse);
			EventArgsFunctionOpenInit eventArgsFunctionOpenInit = new EventArgsFunctionOpenInit();
			eventArgsFunctionOpenInit.SetData(userLoginResponse.OpenModelIdList);
			GameApp.Event.DispatchNow(this, 135, eventArgsFunctionOpenInit);
			GlobalUpdater.Instance.RegisterUpdater(new HeartbeatCtrl());
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.BaseInfo);
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Login);
			GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule).SetLoginData(userLoginResponse.WheelInfo);
			NetworkUtils.PlayerData.SendUserGetAllPanelInfoRequest(delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_TaskDataModule_LoadTaskData, null);
			});
			NetworkUtils.Mining.DoGetMiningInfoRequest(delegate(bool isOk, GetMiningInfoResponse result)
			{
				if (isOk)
				{
					GameApp.Data.GetDataModule(DataName.MiningDataModule).AsyncCachePos();
				}
			});
			if (!dataModule.IsEnterNewWorld)
			{
				NetworkUtils.NewWorld.DoNewWorldInfoRequest(null);
			}
			GameApp.Data.GetDataModule(DataName.TVRewardDataModule).RequestTVInfos(true);
			NetworkUtils.ChainPacks.DoChainPacksTimeRequest(true, true, null, true);
			ChainPacksPushDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChainPacksPushDataModule);
			if (dataModule2 != null)
			{
				dataModule2.OnRefreshData(userLoginResponse.PushChainDto, true);
			}
		}

		private void OnEventRefreshLordAddSlaveData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshLordAddSlaveData eventArgsRefreshLordAddSlaveData = eventargs as EventArgsRefreshLordAddSlaveData;
			if (eventArgsRefreshLordAddSlaveData == null)
			{
				return;
			}
			this.OnRefreshLordAddSlave(eventArgsRefreshLordAddSlaveData.m_lordDto, eventArgsRefreshLordAddSlaveData.m_slaveCount);
		}

		public bool habbyMailBind { get; private set; }

		public string HabbyID { get; private set; }

		public int UserLiftMaxValue { get; private set; }

		public UserInfoDto UserInfo { get; private set; }

		public string NickName
		{
			get
			{
				if (this.UserInfo == null || string.IsNullOrEmpty(this.UserInfo.NickName))
				{
					return DxxTools.GetDefaultNick(this.userId);
				}
				return this.UserInfo.NickName;
			}
		}

		public string ServerSetNickName
		{
			get
			{
				if (this.UserInfo == null || string.IsNullOrEmpty(this.UserInfo.NickName))
				{
					return "";
				}
				return this.UserInfo.NickName;
			}
		}

		public int Avatar
		{
			get
			{
				if (this.m_Avatar == 0)
				{
					return Singleton<GameConfig>.Instance.AvatarDefaultId;
				}
				return this.m_Avatar;
			}
		}

		public int AvatarFrame
		{
			get
			{
				if (this.m_AvatarFrame == 0)
				{
					return Singleton<GameConfig>.Instance.AvatarDefaultFrameId;
				}
				return this.m_AvatarFrame;
			}
		}

		public int AvatarTitle
		{
			get
			{
				if (this.m_AvatarTitle == 0)
				{
					return Singleton<GameConfig>.Instance.AvatarDefaultTitleId;
				}
				return this.m_AvatarTitle;
			}
		}

		public ulong Power
		{
			get
			{
				if (this.UserInfo == null)
				{
					return 0UL;
				}
				return this.UserInfo.Power;
			}
		}

		public string GetGuildName(bool isShowNone)
		{
			if (GuildSDKManager.Instance.GuildInfo.GuildData != null && !string.IsNullOrEmpty(GuildSDKManager.Instance.GuildInfo.GuildData.GuildShowName))
			{
				return GuildSDKManager.Instance.GuildInfo.GuildData.GuildShowName;
			}
			if (!isShowNone)
			{
				return "";
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID("selfinfomation_guild_fullnone");
		}

		public LordDto LordData
		{
			get
			{
				return this.m_lordDto;
			}
		}

		public int SlaveCount
		{
			get
			{
				return this.m_slaveCount;
			}
		}

		public bool IsFullSlaveCount()
		{
			return this.SlaveCount >= Singleton<GameConfig>.Instance.SlaveMaxCount;
		}

		public long ServerOpenMidNightTimestamp
		{
			get
			{
				return DxxTools.Time.TimestampToMidNightTimestamp((long)this.ServerOpenTimestamp);
			}
		}

		public string LocalIMGroupId { get; private set; }

		public string ServerIMGroupId { get; private set; }

		public long LocalUTC
		{
			get
			{
				return this.GetLocalTime();
			}
		}

		public long ServerUTC
		{
			get
			{
				return this.LocalUTC + (long)LoginDataModule.synDeltaTime;
			}
		}

		public void SetDataNextDayRefreshTimestamp(ulong timestamp)
		{
			if (timestamp == 0UL)
			{
				DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime((double)this.ServerUTC).AddDays(1.0);
				timestamp = (ulong)DxxTools.Time.DateTimeToUnixTimestamp(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0, DateTimeKind.Utc));
			}
			this.DataNextDayRefreshTimestamp = timestamp;
		}

		public void SynTimestamp(ulong serverTimeNow, ulong registertimestamp)
		{
			this.registerTimestamp = registertimestamp;
			this.ServeTimestampNow = serverTimeNow;
			LoginDataModule.synLocalUTC = (ulong)this.LocalUTC;
			LoginDataModule.synDeltaTime = this.ServeTimestampNow - LoginDataModule.synLocalUTC;
		}

		public long GetLocalTime()
		{
			return Convert.ToInt64((DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds);
		}

		public ulong GuideMask { get; private set; }

		public int AccountTotalLoginDays { get; private set; }

		public static bool IsTestB()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return dataModule != null && dataModule.abVersion > 0U;
		}

		public void SetLoginDataBase(UserLoginResponse response)
		{
			if (response == null)
			{
				return;
			}
			this.abVersion = response.AbVersion;
			GameApp.NetWork.m_abVersion = this.abVersion;
			this.SetHabbyID(response.HabbyMailBind, response.HabbyId);
			this.habbyMailReward = response.HabbyMailReward;
			this.accountId = response.AccountKey;
			this.IsNewUser = response.IsNewUser;
			this.userId = response.UserId;
			GameApp.SDK.WebView.SetServerUserid((int)this.userId);
			GameApp.SDK.WebView.SetAccountID(this.accountId);
			this.userCurrency = response.UserCurrency;
			this.userLevel = response.UserLevel;
			this.userMission = response.UserMission;
			this.GuideMask = response.GuideMask;
			this.OnRefreshLordAddSlave(response.Lord, (int)response.SlaveCount);
			this.UserLiftMaxValue = Singleton<GameConfig>.Instance.APMax;
			this.ServerOpenTimestamp = response.OpenServerTime;
			this.ServerSetUserInfo(response.UserInfoDto, true);
			this.ServerIMGroupId = response.CrossServerIMGroupId;
			this.LocalIMGroupId = response.ServerIMGroupId;
			if (response.IntegralShops != null)
			{
				EventArgsRefreshShopData instance = Singleton<EventArgsRefreshShopData>.Instance;
				instance.ShopType = (ShopType)response.IntegralShops.ShopConfigId;
				instance.ShopInfo = response.IntegralShops;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, instance);
			}
			EventArgsSetCurTowerLevelIdData instance2 = Singleton<EventArgsSetCurTowerLevelIdData>.Instance;
			instance2.CompleteTowerLevelId = (int)response.Tower;
			instance2.ClaimedRewardTowerId = (int)response.TowerReward;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_TowerDataMoudule_SetCurTowerLevelIdData, instance2);
			DxxTools.Time.InitServerTimestamp((long)response.Timestamp, response.TimeZone);
			this.SynTimestamp(response.Timestamp, response.RegisterTimestamp);
			this.SetDataNextDayRefreshTimestamp(response.DataRefreshTimestamp);
			GuideController.OnCreate();
			GuideController.Instance.InitData(this.GuideMask);
			GameApp.Data.GetDataModule<RefreshDataModule>(107).StartRefreshCheck();
			if (response.TgaInfoDto != null)
			{
				this.AccountTotalLoginDays = response.TgaInfoDto.TotalLoginDays;
				this.AccountLastLoginTime = response.TgaInfoDto.PreLoginTime;
				this.AccountCreateTime = response.TgaInfoDto.CreateTime;
			}
		}

		public void SetHabbyMailBind(bool bind)
		{
			this.habbyMailBind = bind;
			RedPointController.Instance.ReCalc("Main.SelfInfo.HabbyId", true);
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.HabbyID);
		}

		public void SetHabbyID(bool bind, string habbyID = "")
		{
			this.habbyMailBind = bind;
			this.HabbyID = habbyID;
			RedPointController.Instance.ReCalc("Main.SelfInfo.HabbyId", true);
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.HabbyID);
		}

		private void PrintAvatarClothesData()
		{
			ClothesData selfClothesData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData;
			SceneSkinData selfSceneSkinData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfSceneSkinData;
		}

		public void UpdateCurrency(UpdateUserCurrency currency)
		{
			if (currency == null)
			{
				return;
			}
			if (currency.IsChange)
			{
				UserCurrency userCurrency = this.userCurrency;
				this.userCurrency = currency.UserCurrency;
				RedPointHelper.CurrencyChangeCheck(this.userCurrency, userCurrency);
				GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Login);
			}
		}

		public void UpdateUserLevel(UpdateUserLevel updateUserLevel)
		{
			if (updateUserLevel == null)
			{
				return;
			}
			if (updateUserLevel.IsChange)
			{
				this.userLevel = updateUserLevel.UserLevel;
			}
		}

		public void ServerSetUserInfo(UserInfoDto userinfo, bool isLogin = false)
		{
			if (userinfo == null)
			{
				return;
			}
			UserInfoDto userInfo = this.UserInfo;
			this.UserInfo = userinfo;
			if (isLogin)
			{
				this.unlockAllAvatarClotheScene = null;
			}
			this.UnlockAvatarList = this.UserInfo.UnlockAvatarList.ToList<UserUnlockAvatarDto>();
			this.UnlockAvatarFrameList = this.UserInfo.UnlockAvatarFrameList.ToList<UserUnlockAvatarDto>();
			this.UnlockAvatarTitleList = this.UserInfo.UnlockTitleList.ToList<UserUnlockAvatarDto>();
			this.UnlockSkinBodyList = this.UserInfo.UnlockSkinBodyList.ToList<UserUnlockAvatarDto>();
			this.UnlockSkinHeaddressList = this.UserInfo.UnlockSkinHeaddressList.ToList<UserUnlockAvatarDto>();
			this.UnlockSkinAccessoryList = this.UserInfo.UnlockSkinAccessoryList.ToList<UserUnlockAvatarDto>();
			this.UnlockBackGroundList = this.UserInfo.UnlockBackGroundList.ToList<UserUnlockAvatarDto>();
			if (this.unlockAllAvatarClotheScene == null)
			{
				this.UpdateUnlockAllAvatarClotheScene();
			}
			GameApp.Data.GetDataModule(DataName.ClothesDataModule).UpdateSelfClothesData(this.UserInfo, isLogin);
			if (userInfo == null || this.UserInfo.NickName != userInfo.NickName || (ulong)this.UserInfo.Avatar != (ulong)((long)this.m_Avatar) || (ulong)this.UserInfo.AvatarFrame != (ulong)((long)this.m_AvatarFrame) || (ulong)this.UserInfo.TitleId != (ulong)((long)this.m_AvatarTitle))
			{
				this.m_Avatar = (int)this.UserInfo.Avatar;
				this.m_AvatarFrame = (int)this.UserInfo.AvatarFrame;
				this.m_AvatarTitle = (int)this.UserInfo.TitleId;
				if (!isLogin)
				{
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_GameLoginData_UserInfoChange, null);
				}
			}
			if (isLogin)
			{
				GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.UpdateAvatarClothesTimeOut));
				GlobalUpdater.Instance.RegisterUpdater(new Action(this.UpdateAvatarClothesTimeOut));
				GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
				GameApp.Event.RegisterEvent(LocalMessageName.CC_Item_Update, new HandlerEvent(this.Event_ItemUpdate));
			}
			else
			{
				RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Avatar", true);
				RedPointController.Instance.ReCalc("Equip.Fashion", true);
			}
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Login);
		}

		public static void LogUserInfoUnlock(UserInfoDto userinfo)
		{
			if (userinfo == null)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("EmailSetUserInfo : {");
			if (userinfo.UnlockAvatarList != null && userinfo.UnlockAvatarList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockAvatarList:{");
				string text = "    ";
				for (int i = 0; i < userinfo.UnlockAvatarList.Count; i++)
				{
					text = text + userinfo.UnlockAvatarList[i].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockAvatarFrameList != null && userinfo.UnlockAvatarFrameList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockAvatarFrameList:{");
				string text2 = "    ";
				for (int j = 0; j < userinfo.UnlockAvatarFrameList.Count; j++)
				{
					text2 = text2 + userinfo.UnlockAvatarFrameList[j].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text2);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockTitleList != null && userinfo.UnlockTitleList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockTitleList:{");
				string text3 = "    ";
				for (int k = 0; k < userinfo.UnlockTitleList.Count; k++)
				{
					text3 = text3 + userinfo.UnlockTitleList[k].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text3);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockSkinBodyList != null && userinfo.UnlockSkinBodyList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockSkinBodyList:{");
				string text4 = "    ";
				for (int l = 0; l < userinfo.UnlockSkinBodyList.Count; l++)
				{
					text4 = text4 + userinfo.UnlockSkinBodyList[l].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text4);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockSkinHeaddressList != null && userinfo.UnlockSkinHeaddressList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockSkinHeaddressList:{");
				string text5 = "    ";
				for (int m = 0; m < userinfo.UnlockSkinHeaddressList.Count; m++)
				{
					text5 = text5 + userinfo.UnlockSkinHeaddressList[m].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text5);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockSkinAccessoryList != null && userinfo.UnlockSkinAccessoryList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockSkinAccessoryList:{");
				string text6 = "    ";
				for (int n = 0; n < userinfo.UnlockSkinAccessoryList.Count; n++)
				{
					text6 = text6 + userinfo.UnlockSkinAccessoryList[n].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text6);
				stringBuilder.AppendLine("  }");
			}
			if (userinfo.UnlockBackGroundList != null && userinfo.UnlockBackGroundList.Count > 0)
			{
				stringBuilder.AppendLine("  UnlockBackGroundList:{");
				string text7 = "    ";
				for (int num = 0; num < userinfo.UnlockBackGroundList.Count; num++)
				{
					text7 = text7 + userinfo.UnlockBackGroundList[num].ConfigId.ToString() + ",";
				}
				stringBuilder.AppendLine(text7);
				stringBuilder.AppendLine("  }");
			}
			stringBuilder.AppendLine("}");
		}

		public void EmailSetUserInfo(UserInfoDto userinfo)
		{
			LoginDataModule.LogUserInfoUnlock(userinfo);
			if (userinfo == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (userinfo.UnlockAvatarList != null && userinfo.UnlockAvatarList.Count > 0)
			{
				flag = true;
				this.UnlockAvatarList = userinfo.UnlockAvatarList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockAvatarFrameList != null && userinfo.UnlockAvatarFrameList.Count > 0)
			{
				flag = true;
				this.UnlockAvatarFrameList = userinfo.UnlockAvatarFrameList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockTitleList != null && userinfo.UnlockTitleList.Count > 0)
			{
				flag = true;
				this.UnlockAvatarTitleList = userinfo.UnlockTitleList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockSkinBodyList != null && userinfo.UnlockSkinBodyList.Count > 0)
			{
				flag2 = true;
				this.UnlockSkinBodyList = userinfo.UnlockSkinBodyList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockSkinHeaddressList != null && userinfo.UnlockSkinHeaddressList.Count > 0)
			{
				flag2 = true;
				this.UnlockSkinHeaddressList = userinfo.UnlockSkinHeaddressList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockSkinAccessoryList != null && userinfo.UnlockSkinAccessoryList.Count > 0)
			{
				flag2 = true;
				this.UnlockSkinAccessoryList = userinfo.UnlockSkinAccessoryList.ToList<UserUnlockAvatarDto>();
			}
			if (userinfo.UnlockBackGroundList != null && userinfo.UnlockBackGroundList.Count > 0)
			{
				flag3 = true;
				this.UnlockBackGroundList = userinfo.UnlockBackGroundList.ToList<UserUnlockAvatarDto>();
			}
			if (flag || flag2 || flag3)
			{
				this.unlockAllAvatarClotheScene = null;
				this.UpdateUnlockAllAvatarClotheScene();
				RedPointController.Instance.ReCalc("Equip.Fashion", true);
			}
			if (flag)
			{
				RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Avatar", true);
			}
			if (flag2)
			{
				RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Clothes", true);
			}
			if (flag3)
			{
				RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Scene", true);
			}
		}

		private void OnRefreshLordAddSlave(LordDto lordDto, int slaveCount)
		{
			this.m_lordDto = ((lordDto != null && lordDto.LordUid > 0L) ? lordDto : null);
			this.m_slaveCount = slaveCount;
		}

		public bool IsAvatarIconEquipped(int id)
		{
			return this.Avatar == id;
		}

		public bool IsAvatarFrameEquipped(int id)
		{
			return this.AvatarFrame == id;
		}

		public bool IsAvatarTitleEquipped(int id)
		{
			return this.AvatarTitle == id;
		}

		public bool IsAvatarIconUnlock(int id)
		{
			if (this.IsAvatarIconEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockAvatarList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockAvatarNeedItemNum(id) <= 0;
		}

		public bool IsAvatarFrameUnlock(int id)
		{
			if (this.IsAvatarFrameEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockAvatarFrameList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockAvatarNeedItemNum(id) <= 0;
		}

		public bool IsAvatarTitleUnlock(int id)
		{
			if (this.IsAvatarTitleEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockAvatarTitleList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockAvatarNeedItemNum(id) <= 0;
		}

		public bool IsAvatarIconCanUnlock(int id)
		{
			return !this.IsAvatarIconUnlock(id) && this.HasEnoughAvatarUnlockItem(id);
		}

		public bool IsAvatarFrameCanUnlock(int id)
		{
			return !this.IsAvatarFrameUnlock(id) && this.HasEnoughAvatarUnlockItem(id);
		}

		public bool IsAvatarTitleCanUnlock(int id)
		{
			return !this.IsAvatarTitleUnlock(id) && this.HasEnoughAvatarUnlockItem(id);
		}

		public int UnlockAvatarNeedItemNum(int id)
		{
			try
			{
				Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(id);
				if (avatar_Avatar != null && avatar_Avatar.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_Avatar.unlockItemId[0]))
				{
					return int.Parse(avatar_Avatar.unlockItemId[0].Split(',', StringSplitOptions.None)[1]);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			return 0;
		}

		private bool HasEnoughAvatarUnlockItem(int id)
		{
			try
			{
				Avatar_Avatar avatar_Avatar = GameApp.Table.GetManager().GetAvatar_Avatar(id);
				if (avatar_Avatar != null && avatar_Avatar.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_Avatar.unlockItemId[0]))
				{
					string[] array = avatar_Avatar.unlockItemId[0].Split(',', StringSplitOptions.None);
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)num)) < (long)num2)
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				return false;
			}
			return true;
		}

		public bool IsAvatarOrFrameRedNode()
		{
			IList<Avatar_Avatar> avatar_AvatarElements = GameApp.Table.GetManager().GetAvatar_AvatarElements();
			for (int i = 0; i < avatar_AvatarElements.Count; i++)
			{
				if (avatar_AvatarElements[i].type == 1)
				{
					if (this.IsAvatarIconCanUnlock(avatar_AvatarElements[i].id))
					{
						return true;
					}
				}
				else if (avatar_AvatarElements[i].type == 2)
				{
					if (this.IsAvatarFrameCanUnlock(avatar_AvatarElements[i].id))
					{
						return true;
					}
				}
				else if (avatar_AvatarElements[i].type == 7 && this.IsAvatarTitleCanUnlock(avatar_AvatarElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAvatarRedNode()
		{
			IList<Avatar_Avatar> avatar_AvatarElements = GameApp.Table.GetManager().GetAvatar_AvatarElements();
			for (int i = 0; i < avatar_AvatarElements.Count; i++)
			{
				if (avatar_AvatarElements[i].type == 1 && this.IsAvatarIconCanUnlock(avatar_AvatarElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAvatarFrameRedNode()
		{
			IList<Avatar_Avatar> avatar_AvatarElements = GameApp.Table.GetManager().GetAvatar_AvatarElements();
			for (int i = 0; i < avatar_AvatarElements.Count; i++)
			{
				if (avatar_AvatarElements[i].type == 2 && this.IsAvatarFrameCanUnlock(avatar_AvatarElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAvatarTitleRedNode()
		{
			IList<Avatar_Avatar> avatar_AvatarElements = GameApp.Table.GetManager().GetAvatar_AvatarElements();
			for (int i = 0; i < avatar_AvatarElements.Count; i++)
			{
				if (avatar_AvatarElements[i].type == 7 && this.IsAvatarTitleCanUnlock(avatar_AvatarElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public long GetAvatarIconEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public long GetAvatarFrameEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarFrameList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public long GetAvatarTitleEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarTitleList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public bool IsAvatarIconEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAvatarFrameEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarFrameList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsAvatarTitleEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockAvatarTitleList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesHeadEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinHeaddressList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesBodyEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinBodyList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesAccessoryEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinAccessoryList)
			{
				if (userUnlockAvatarDto.ConfigId == id && userUnlockAvatarDto.ExpireTime > 0UL && DxxTools.Time.ServerTimestamp >= (long)userUnlockAvatarDto.ExpireTime)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesRedNode()
		{
			IList<Avatar_Skin> avatar_SkinElements = GameApp.Table.GetManager().GetAvatar_SkinElements();
			for (int i = 0; i < avatar_SkinElements.Count; i++)
			{
				if (avatar_SkinElements[i].part == 2)
				{
					if (this.IsClothesHeadCanUnlock(avatar_SkinElements[i].id))
					{
						return true;
					}
				}
				else if (avatar_SkinElements[i].part == 1)
				{
					if (this.IsClothesBodyCanUnlock(avatar_SkinElements[i].id))
					{
						return true;
					}
				}
				else if (avatar_SkinElements[i].part == 3 && this.IsClothesAccessoriesCanUnlock(avatar_SkinElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesHeadRedNode()
		{
			IList<Avatar_Skin> avatar_SkinElements = GameApp.Table.GetManager().GetAvatar_SkinElements();
			for (int i = 0; i < avatar_SkinElements.Count; i++)
			{
				if (avatar_SkinElements[i].part == 2 && this.IsClothesHeadCanUnlock(avatar_SkinElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesBodyRedNode()
		{
			IList<Avatar_Skin> avatar_SkinElements = GameApp.Table.GetManager().GetAvatar_SkinElements();
			for (int i = 0; i < avatar_SkinElements.Count; i++)
			{
				if (avatar_SkinElements[i].part == 1 && this.IsClothesBodyCanUnlock(avatar_SkinElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsClothesAccessoryRedNode()
		{
			IList<Avatar_Skin> avatar_SkinElements = GameApp.Table.GetManager().GetAvatar_SkinElements();
			for (int i = 0; i < avatar_SkinElements.Count; i++)
			{
				if (avatar_SkinElements[i].part == 3 && this.IsClothesAccessoriesCanUnlock(avatar_SkinElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public long GetClothesHeadEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinHeaddressList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public long GetClothesBodyEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinBodyList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public long GetClothesAccessoryEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockSkinAccessoryList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public bool IsClotheHeadEquipped(int id)
		{
			return GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.HeadId == id;
		}

		public bool IsClotheBodyEquipped(int id)
		{
			return GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.BodyId == id;
		}

		public bool IsClotheAccessoryEquipped(int id)
		{
			return GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfClothesData.AccessoryId == id;
		}

		public bool IsClothesHeadUnlock(int id)
		{
			if (this.IsClotheHeadEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockSkinHeaddressList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockClothesNeedItemNum(id) <= 0;
		}

		public bool IsClothesBodyUnlock(int id)
		{
			if (this.IsClotheBodyEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockSkinBodyList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockClothesNeedItemNum(id) <= 0;
		}

		public bool IsClothesAccessoriesUnlock(int id)
		{
			if (this.IsClotheAccessoryEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockSkinAccessoryList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockClothesNeedItemNum(id) <= 0;
		}

		public bool IsClothesHeadCanUnlock(int id)
		{
			return !this.IsClothesHeadUnlock(id) && this.HasEnoughClothesUnlockItem(id);
		}

		public bool IsClothesBodyCanUnlock(int id)
		{
			return !this.IsClothesBodyUnlock(id) && this.HasEnoughClothesUnlockItem(id);
		}

		public bool IsClothesAccessoriesCanUnlock(int id)
		{
			return !this.IsClothesAccessoriesUnlock(id) && this.HasEnoughClothesUnlockItem(id);
		}

		public int UnlockClothesNeedItemNum(int id)
		{
			try
			{
				Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(id);
				if (avatar_Skin != null && avatar_Skin.unlockItemId != null && avatar_Skin.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_Skin.unlockItemId[0]))
				{
					return int.Parse(avatar_Skin.unlockItemId[0].Split(',', StringSplitOptions.None)[1]);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			return 0;
		}

		private bool HasEnoughClothesUnlockItem(int id)
		{
			try
			{
				Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(id);
				if (avatar_Skin != null && avatar_Skin.unlockItemId != null && avatar_Skin.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_Skin.unlockItemId[0]))
				{
					string[] array = avatar_Skin.unlockItemId[0].Split(',', StringSplitOptions.None);
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)num)) < (long)num2)
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				return false;
			}
			return true;
		}

		public bool IsEquipped(int part, int id)
		{
			if (part == 1)
			{
				return this.IsAvatarIconEquipped(id);
			}
			if (part == 2)
			{
				return this.IsAvatarFrameEquipped(id);
			}
			if (part == 7)
			{
				return this.IsAvatarTitleEquipped(id);
			}
			if (part == 3)
			{
				return this.IsClotheBodyEquipped(id);
			}
			if (part == 4)
			{
				return this.IsClotheHeadEquipped(id);
			}
			if (part == 5)
			{
				return this.IsClotheAccessoryEquipped(id);
			}
			return part == 6 && this.IsSceneSkinEquipped(id);
		}

		private void AddTimeOutPartId(Dictionary<int, List<int>> partTimeOuts, int part, int cfgId)
		{
			List<int> list;
			if (!partTimeOuts.TryGetValue(part, out list))
			{
				list = new List<int>();
				partTimeOuts.Add(part, list);
			}
			list.Add(cfgId);
		}

		public void UpdateAvatarClothesTimeOut()
		{
			if (this.UserInfo == null)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			ulong serverTimestamp = (ulong)DxxTools.Time.ServerTimestamp;
			this.partTimeOuts.Clear();
			for (int i = this.UnlockAvatarList.Count - 1; i >= 0; i--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto = this.UnlockAvatarList[i];
				if (userUnlockAvatarDto.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto.ExpireTime)
				{
					flag = true;
					this.UnlockAvatarList.RemoveAt(i);
					this.AddTimeOutPartId(this.partTimeOuts, 1, userUnlockAvatarDto.ConfigId);
					if (this.Avatar == userUnlockAvatarDto.ConfigId)
					{
						flag4 = true;
						this.m_Avatar = 0;
					}
				}
			}
			for (int j = this.UnlockAvatarFrameList.Count - 1; j >= 0; j--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto2 = this.UnlockAvatarFrameList[j];
				if (userUnlockAvatarDto2.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto2.ExpireTime)
				{
					flag = true;
					this.UnlockAvatarFrameList.RemoveAt(j);
					this.AddTimeOutPartId(this.partTimeOuts, 2, userUnlockAvatarDto2.ConfigId);
					if (this.AvatarFrame == userUnlockAvatarDto2.ConfigId)
					{
						flag4 = true;
						this.m_AvatarFrame = 0;
					}
				}
			}
			for (int k = this.UnlockAvatarTitleList.Count - 1; k >= 0; k--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto3 = this.UnlockAvatarTitleList[k];
				if (userUnlockAvatarDto3.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto3.ExpireTime)
				{
					flag = true;
					this.UnlockAvatarTitleList.RemoveAt(k);
					this.AddTimeOutPartId(this.partTimeOuts, 7, userUnlockAvatarDto3.ConfigId);
					if (this.AvatarTitle == userUnlockAvatarDto3.ConfigId)
					{
						flag4 = true;
						this.m_AvatarTitle = 0;
					}
				}
			}
			ClothesDataModule dataModule = GameApp.Data.GetDataModule(DataName.ClothesDataModule);
			ClothesData selfClothesData = dataModule.SelfClothesData;
			SceneSkinData selfSceneSkinData = dataModule.SelfSceneSkinData;
			for (int l = this.UnlockSkinBodyList.Count - 1; l >= 0; l--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto4 = this.UnlockSkinBodyList[l];
				if (userUnlockAvatarDto4.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto4.ExpireTime)
				{
					flag2 = true;
					this.UnlockSkinBodyList.RemoveAt(l);
					this.AddTimeOutPartId(this.partTimeOuts, 3, userUnlockAvatarDto4.ConfigId);
					if (selfClothesData.BodyId == userUnlockAvatarDto4.ConfigId)
					{
						flag5 = true;
						selfClothesData.BodyId = 0;
					}
				}
			}
			for (int m = this.UnlockSkinHeaddressList.Count - 1; m >= 0; m--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto5 = this.UnlockSkinHeaddressList[m];
				if (userUnlockAvatarDto5.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto5.ExpireTime)
				{
					flag2 = true;
					this.UnlockSkinHeaddressList.RemoveAt(m);
					this.AddTimeOutPartId(this.partTimeOuts, 4, userUnlockAvatarDto5.ConfigId);
					if (selfClothesData.HeadId == userUnlockAvatarDto5.ConfigId)
					{
						flag5 = true;
						selfClothesData.HeadId = 0;
					}
				}
			}
			for (int n = this.UnlockSkinAccessoryList.Count - 1; n >= 0; n--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto6 = this.UnlockSkinAccessoryList[n];
				if (userUnlockAvatarDto6.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto6.ExpireTime)
				{
					flag2 = true;
					this.UnlockSkinAccessoryList.RemoveAt(n);
					this.AddTimeOutPartId(this.partTimeOuts, 5, userUnlockAvatarDto6.ConfigId);
					if (selfClothesData.AccessoryId == userUnlockAvatarDto6.ConfigId)
					{
						flag5 = true;
						selfClothesData.AccessoryId = 0;
					}
				}
			}
			for (int num = this.UnlockBackGroundList.Count - 1; num >= 0; num--)
			{
				UserUnlockAvatarDto userUnlockAvatarDto7 = this.UnlockBackGroundList[num];
				if (userUnlockAvatarDto7.ExpireTime > 0UL && serverTimestamp >= userUnlockAvatarDto7.ExpireTime)
				{
					flag3 = true;
					this.UnlockBackGroundList.RemoveAt(num);
					this.AddTimeOutPartId(this.partTimeOuts, 6, userUnlockAvatarDto7.ConfigId);
					if (selfSceneSkinData.CurSkinId == userUnlockAvatarDto7.ConfigId)
					{
						flag6 = true;
						selfSceneSkinData.OnUpdateSkinId(0);
					}
				}
			}
			if (flag || flag2 || flag3)
			{
				this.unlockAllAvatarClotheScene = new bool?(false);
				EventArgClothesTimeOut eventArgClothesTimeOut = new EventArgClothesTimeOut(this.partTimeOuts);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_TimeOutUserAvatarClothesScene, eventArgClothesTimeOut);
				if (flag)
				{
					RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Avatar", true);
				}
				if (flag2)
				{
					RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Clothes", true);
				}
				if (flag3)
				{
					RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar.Scene", true);
				}
				RedPointController.Instance.ReCalc("Equip.Fashion", true);
			}
			if (flag4)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_GameLoginData_UserInfoChange, null);
			}
			if (flag5)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_ClothesData_SelfClothesChanged, null);
			}
			if (flag6)
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Change_SceneSkin, null);
			}
		}

		public long GetSceneSkinEndTime(int id)
		{
			foreach (UserUnlockAvatarDto userUnlockAvatarDto in this.UnlockBackGroundList)
			{
				if (userUnlockAvatarDto.ConfigId == id)
				{
					return (long)userUnlockAvatarDto.ExpireTime;
				}
			}
			return 0L;
		}

		public bool IsSceneSkinEquipped(int id)
		{
			SceneSkinData selfSceneSkinData = GameApp.Data.GetDataModule(DataName.ClothesDataModule).SelfSceneSkinData;
			return selfSceneSkinData != null && selfSceneSkinData.CurSkinId == id;
		}

		public bool IsSceneSkinUnlock(int id)
		{
			if (this.IsSceneSkinEquipped(id))
			{
				return true;
			}
			using (List<UserUnlockAvatarDto>.Enumerator enumerator = this.UnlockBackGroundList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ConfigId == id)
					{
						return true;
					}
				}
			}
			return this.UnlockSceneSkinNeedItemNum(id) <= 0;
		}

		public bool IsSceneSkinCanUnlock(int id)
		{
			return !this.IsSceneSkinUnlock(id) && this.HasEnoughSceneSkinUnlockItem(id);
		}

		public int UnlockSceneSkinNeedItemNum(int id)
		{
			try
			{
				Avatar_Skin avatar_Skin = GameApp.Table.GetManager().GetAvatar_Skin(id);
				if (avatar_Skin != null && avatar_Skin.unlockItemId != null && avatar_Skin.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_Skin.unlockItemId[0]))
				{
					return int.Parse(avatar_Skin.unlockItemId[0].Split(',', StringSplitOptions.None)[1]);
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			return 0;
		}

		private bool HasEnoughSceneSkinUnlockItem(int id)
		{
			try
			{
				Avatar_SceneSkin avatar_SceneSkin = GameApp.Table.GetManager().GetAvatar_SceneSkin(id);
				if (avatar_SceneSkin != null && avatar_SceneSkin.unlockItemId != null && avatar_SceneSkin.unlockItemId.Length != 0 && !string.IsNullOrEmpty(avatar_SceneSkin.unlockItemId[0]))
				{
					string[] array = avatar_SceneSkin.unlockItemId[0].Split(',', StringSplitOptions.None);
					int num = int.Parse(array[0]);
					int num2 = int.Parse(array[1]);
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)num)) < (long)num2)
					{
						return false;
					}
				}
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
				return false;
			}
			return true;
		}

		public bool IsSceneSkinRedNode()
		{
			IList<Avatar_SceneSkin> avatar_SceneSkinElements = GameApp.Table.GetManager().GetAvatar_SceneSkinElements();
			for (int i = 0; i < avatar_SceneSkinElements.Count; i++)
			{
				if (this.IsSceneSkinCanUnlock(avatar_SceneSkinElements[i].id))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsUnlockAllAvatarClotheScene()
		{
			return this.UnlockAvatarList.Count + this.UnlockAvatarFrameList.Count + this.UnlockAvatarTitleList.Count >= GameApp.Table.GetManager().GetAvatar_AvatarElements().Count && this.UnlockSkinBodyList.Count + this.UnlockSkinHeaddressList.Count + this.UnlockSkinAccessoryList.Count >= GameApp.Table.GetManager().GetAvatar_AvatarElements().Count && this.UnlockBackGroundList.Count >= GameApp.Table.GetManager().GetAvatar_SceneSkinElements().Count;
		}

		public void UpdateUnlockAllAvatarClotheScene()
		{
			if (this.unlockAllAvatarClotheScene != null && this.unlockAllAvatarClotheScene.Value)
			{
				return;
			}
			this.unlockAllAvatarClotheScene = new bool?(this.IsUnlockAllAvatarClotheScene());
		}

		public bool CheckAvatarClotheSceneItemRedFresh(int itemId)
		{
			if (this.unlockAllAvatarClotheScene != null && this.unlockAllAvatarClotheScene.Value)
			{
				return false;
			}
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(itemId);
			for (int i = 0; i < item_Item.redTypes.Length; i++)
			{
				if (item_Item.redTypes[i] == 1)
				{
					return true;
				}
			}
			return false;
		}

		private void Event_ItemUpdate(object sender, int eventid, BaseEventArgs eventArgs)
		{
			EventArgsItemUpdate eventArgsItemUpdate = eventArgs as EventArgsItemUpdate;
			if (eventArgsItemUpdate != null && GameApp.Data.GetDataModule(DataName.LoginDataModule).CheckAvatarClotheSceneItemRedFresh(eventArgsItemUpdate.itemId))
			{
				RedPointController.Instance.ReCalc("Main.SelfInfo.Avatar", true);
				RedPointController.Instance.ReCalc("Equip.Fashion", true);
			}
		}

		public string accountId;

		public long userId;

		public uint abVersion;

		public UserCurrency userCurrency;

		public UserLevel userLevel;

		public UserMission userMission;

		public bool habbyMailReward;

		private int m_Avatar;

		private int m_AvatarFrame;

		private int m_AvatarTitle;

		private List<UserUnlockAvatarDto> UnlockAvatarList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockAvatarFrameList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockAvatarTitleList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockSkinHeaddressList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockSkinBodyList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockSkinAccessoryList = new List<UserUnlockAvatarDto>();

		private List<UserUnlockAvatarDto> UnlockBackGroundList = new List<UserUnlockAvatarDto>();

		private LordDto m_lordDto;

		[SerializeField]
		private int m_slaveCount;

		public ulong registerTimestamp;

		public ulong ServeTimestampNow;

		public ulong DataNextDayRefreshTimestamp;

		public ulong ServerOpenTimestamp;

		private static ulong synLocalUTC;

		private static ulong synDeltaTime;

		public long AccountLastLoginTime;

		public long AccountCreateTime;

		private Dictionary<int, List<int>> partTimeOuts = new Dictionary<int, List<int>>();

		private bool? unlockAllAvatarClotheScene;
	}
}
