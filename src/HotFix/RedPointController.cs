using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using HotFix.RedPoint.Calculators;
using UnityEngine;

namespace HotFix
{
	public class RedPointController
	{
		public static RedPointController Instance
		{
			get
			{
				if (RedPointController.mInstance == null)
				{
					HLog.LogError("[RedPointController] mInstance == null");
				}
				return RedPointController.mInstance;
			}
		}

		public static void OnCreate()
		{
			if (RedPointController.mInstance != null)
			{
				RedPointController.mInstance.UnOnInit();
				RedPointController.mInstance = null;
			}
			RedPointController.mInstance = new RedPointController();
			RedPointController.mInstance.OnInit();
		}

		public void OnDelect()
		{
			if (RedPointController.mInstance != null)
			{
				RedPointController.mInstance.UnOnInit();
				RedPointController.mInstance = null;
			}
			this.mRedPointDataModule = null;
			this.mSocketNetProxy = null;
		}

		public void OnInit()
		{
			this.mRedPointDataModule = GameApp.Data.GetDataModule(DataName.RedPointDataModule);
			this.mRedPointDataModule.OnInit();
			this.mSocketNetProxy = new RedPoint_SocketNetProxy();
			this.mSocketNetProxy.StartProxy();
			RedPointDataModule redPointDataModule = this.mRedPointDataModule;
			redPointDataModule.RegRecord("Chest", new RPTalent.Chest());
			redPointDataModule.RegRecord("Main", null);
			redPointDataModule.RegRecord("Main.Guild", null);
			redPointDataModule.RegRecord("Main.Shop", new RPMain.Shop());
			redPointDataModule.RegRecord("Main.Sociality", new RPMain.Sociality());
			redPointDataModule.RegRecord("Main.Mail", new RPMain.Mail());
			redPointDataModule.RegRecord("Main.Sign", new RPMain.Sign());
			redPointDataModule.RegRecord("Main.TVReward", new RPMain.TVReward());
			redPointDataModule.RegRecord("Main.Mission", new RPMain.Mission());
			redPointDataModule.RegRecord("Main.Carnival", new RPMain.Carnival());
			redPointDataModule.RegRecord("Main.Activity_Week", new RPMain.Activity_Week());
			redPointDataModule.RegRecord("Main.Activity_Slot", new RPMain.Activity_Slot());
			redPointDataModule.RegRecord("Main.Bag", new RPMain.Bag());
			redPointDataModule.RegRecord("Main.Setting", new RPMain.Setting());
			redPointDataModule.RegRecord("Main.Ranking", new RPMain.Ranking());
			redPointDataModule.RegRecord("Main.BlackMarket", new RPMain.BlackMarket());
			redPointDataModule.RegRecord("Main.ChapterReward", new RPMain.ChapterReward());
			redPointDataModule.RegRecord("Main.HangUp", new RPMain.HangUpReward());
			redPointDataModule.RegRecord("Main.SelfInfo", null);
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Avatar", new RPMain.Avatar_Avatar());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Avatar.Icon", new RPMain.Avatar_Avatar_Icon());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Avatar.Frame", new RPMain.Avatar_Avatar_Frame());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Avatar.Title", new RPMain.Avatar_Avatar_Title());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Clothes", new RPMain.Avatar_Clotes());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Clothes.Body", new RPMain.Avatar_Clotes_Body());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Clothes.Head", new RPMain.Avatar_Clotes_Head());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Clothes.Accessory", new RPMain.Avatar_Clotes_Accessory());
			redPointDataModule.RegRecord("Main.SelfInfo.Avatar.Scene", new RPMain.Avatar_Scene());
			redPointDataModule.RegRecord("Main.SelfInfo.Notice", new RPMain.Avatar_Notice());
			redPointDataModule.RegRecord("Main.SelfInfo.HabbyId", new RPMain.Avatar_Bind_HabbyId());
			redPointDataModule.RegRecord("Main.ChapterBattlePass", new RPMain.ChapterBattlePass());
			redPointDataModule.RegRecord("Main.NewWorld", new RPMain.NewWorld());
			redPointDataModule.RegRecord("Main.ChapterWheel", new RPMain.ChapterWheel());
			redPointDataModule.RegRecord("Talent", new RPTalent.Talent());
			redPointDataModule.RegRecord("Talent.TalentLegacy.TalentLegacyNode", new RPTalent.TalentLegacy());
			redPointDataModule.RegRecord("Equip.TalentLegacySkill", new RPTalent.TalentLegacySkill());
			redPointDataModule.RegRecord("Equip", null);
			redPointDataModule.RegRecord("Equip.Hero", new RPEquip.Equip());
			redPointDataModule.RegRecord("Equip.Hero.Compose", new RPEquip.Equip.EquipCompose());
			redPointDataModule.RegRecord("Equip.Pet", null);
			redPointDataModule.RegRecord("Equip.Pet.Ranch", new RPEquip.PetRanch());
			redPointDataModule.RegRecord("Equip.Pet.Ranch.Collection", new RPEquip.PetCollection());
			redPointDataModule.RegRecord("Equip.Pet.List", new RPEquip.PetList());
			redPointDataModule.RegRecord("Equip.Relic", new RPEquip.Relic());
			redPointDataModule.RegRecord("Equip.Pictorial", new RPEquip.Pictorial());
			redPointDataModule.RegRecord("Equip.Collection", new RPEquip.Collection());
			redPointDataModule.RegRecord("Equip.Collection.Main", new RPEquip.CollectionTabMain());
			redPointDataModule.RegRecord("Equip.Collection.Suit", new RPEquip.CollectionTabSuit());
			redPointDataModule.RegRecord("Equip.Collection.StarUpgrade", new RPEquip.CollectionTabStarUpgrade());
			redPointDataModule.RegRecord("Equip.Mount", new RPEquip.Mount());
			redPointDataModule.RegRecord("Equip.Mount.RideTag", new RPEquip.Mount_RideTag());
			redPointDataModule.RegRecord("Equip.Mount.UpgradeTag", new RPEquip.Mount_UpgradeTag());
			redPointDataModule.RegRecord("Equip.Mount.AdvanceTag", new RPEquip.Mount_AdvanceTag());
			redPointDataModule.RegRecord("Equip.Artifact", new RPEquip.Artifact());
			redPointDataModule.RegRecord("Equip.Artifact.UpgradeTag", new RPEquip.Artifact_UpgradeTag());
			redPointDataModule.RegRecord("Equip.Artifact.AdvanceTag", new RPEquip.Artifact_AdvanceTag());
			redPointDataModule.RegRecord("Equip.Fashion", new RPEquip.Fashion());
			redPointDataModule.RegRecord("MainShop", null);
			redPointDataModule.RegRecord("MainShop.TabEquip", new RPMain.MainShop.TabEquip());
			redPointDataModule.RegRecord("MainShop.TabBalckMarket", new RPMain.MainShop.TabBlackMarket());
			redPointDataModule.RegRecord("MainShop.TabSuperValue", new RPMain.MainShop.TabSuperValue());
			redPointDataModule.RegRecord("MainShop.TabGiftShop", new RPMain.MainShop.TabGiftShop());
			redPointDataModule.RegRecord("MainShop.TabDiamondShop", new RPMain.MainShop.TabDiamondShop());
			redPointDataModule.RegRecord("IAPGift", null);
			redPointDataModule.RegRecord("IAPGift.First", new RPMain.MainShop.Gift_First()).UseClickToHideRedPoint = false;
			redPointDataModule.RegRecord("IAPGift.OpenServer", new RPMain.MainShop.Gift_OpenServer()).SetUseLocalHideTime(true).OnceOneDay = true;
			redPointDataModule.RegRecord("IAPGift.Limited", new RPMain.MainShop.Gift_Limited()).SetUseLocalHideTime(true).OnceOneDay = true;
			redPointDataModule.RegRecord("IAPGift.Chatper", null);
			redPointDataModule.RegRecord("IAPGift.Meeting", new RPMain.MainShop.Gift_Meeting());
			redPointDataModule.RegRecord("IAPRechargeGift", null);
			redPointDataModule.RegRecord("IAPRechargeGift.Fund", new RPMain.IAPRechargeGift.Fund());
			redPointDataModule.RegRecord("IAPRechargeGift.Fund.BattlePass", new RPMain.IAPRechargeGift.BattlePass());
			redPointDataModule.RegRecord("IAPRechargeGift.Fund.TalentFund", new RPMain.IAPRechargeGift.TalentFund());
			redPointDataModule.RegRecord("IAPRechargeGift.Fund.TowerFund", new RPMain.IAPRechargeGift.TowerFund());
			redPointDataModule.RegRecord("IAPRechargeGift.Fund.RogueDungeonFund", new RPMain.IAPRechargeGift.RogueDungeonFund());
			redPointDataModule.RegRecord("IAPPrivileggeCard", null);
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card", new RPMain.IAPPrivilegeCard.Card());
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card.Free", new RPMain.IAPPrivilegeCard.CardFree());
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card.NoAd", new RPMain.IAPPrivilegeCard.CardNoAd());
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card.Month", new RPMain.IAPPrivilegeCard.CardMonth());
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card.Lifetime", new RPMain.IAPPrivilegeCard.CardLifetime());
			redPointDataModule.RegRecord("IAPPrivileggeCard.Card.AutoMining", new RPMain.IAPPrivilegeCard.CardAutoMining());
			redPointDataModule.RegRecord("Currency.Energy", new RPMain.Energy());
			redPointDataModule.RegRecord("ChainPacks", new RPMain.ChainPacks());
			redPointDataModule.RegRecord("Guild", null);
			redPointDataModule.RegRecord("Guild.Hall", null);
			redPointDataModule.RegRecord("Guild.Hall.ApplyJoin", new RPGuild.Guild_ApplyJoin()).UseClickToHideRedPoint = true;
			redPointDataModule.RegRecord("Guild.Hall.Donation", new Guild_Donation());
			redPointDataModule.RegRecord("Guild.Activity", null);
			redPointDataModule.RegRecord("Guild.Activity.Race", new RPGuild.Guild_Race());
			redPointDataModule.RegRecord("Guild.Boss.Challenge", new RPGuild.Guild_Boss());
			redPointDataModule.RegRecord("Guild.Boss.Task", new RPGuild.Guild_BossTask());
			redPointDataModule.RegRecord("Guild.Boss.BoxReward", new RPGuild.Guild_BossBoxReward());
			redPointDataModule.RegRecord("DailyActivity", null);
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag", null);
			redPointDataModule.RegRecord("DailyActivity.DungeonTag", null);
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.Tower", new RPDailyActivity.DailyActivity_Tower()).SetUseLocalHideTime(true).OnceOneDay = true;
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.WorldBoss", new RPDailyActivity.DailyActivity_WorldBoss());
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.CrossArena", new RPDailyActivity.DailyActivity_CrossArena());
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.RogueDungeon", new RPDailyActivity.DailyActivity_RogueDungeon());
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.Mining", null);
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.Mining.Ore", new RPDailyActivity.Mining.Ore());
			redPointDataModule.RegRecord("DailyActivity.ChallengeTag.Mining.Draw", new RPDailyActivity.Mining.Draw());
			redPointDataModule.RegRecord("DailyActivity.DungeonTag.DragonLair", new RPDailyActivity.DailyActivity_DragonLair());
			redPointDataModule.RegRecord("DailyActivity.DungeonTag.AstralTree", new RPDailyActivity.DailyActivity_AstralTree());
			redPointDataModule.RegRecord("DailyActivity.DungeonTag.SwordIsland", new RPDailyActivity.DailyActivity_SwordIsland());
			redPointDataModule.RegRecord("DailyActivity.DungeonTag.DeepSeaRuins", new RPDailyActivity.DailyActivity_DeepSeaRuins());
			redPointDataModule.LinkRecord("Main.Guild", "Guild");
			Singleton<RedNodeManager>.Instance.m_onAdd = new Action<RedNodeOneCtrl>(this.AutoRegRedNodeManager);
			Singleton<RedNodeManager>.Instance.m_onRemove = new Action<RedNodeOneCtrl>(this.AutoUnRegRedNodeManager);
		}

		private void AutoRegRedNodeManager(RedNodeOneCtrl ui)
		{
			if (ui == null || string.IsNullOrEmpty(ui.Key))
			{
				return;
			}
			this.RegRecordChange(ui.Key, new Action<RedNodeListenData>(ui.OnRefresh));
		}

		private void AutoUnRegRedNodeManager(RedNodeOneCtrl ui)
		{
			if (ui == null || string.IsNullOrEmpty(ui.Key))
			{
				return;
			}
			this.UnRegRecordChange(ui.Key, new Action<RedNodeListenData>(ui.OnRefresh));
		}

		public void UnOnInit()
		{
			if (this.mSocketNetProxy != null)
			{
				this.mSocketNetProxy.StopProxy();
			}
			this.mSocketNetProxy = null;
			Singleton<RedNodeManager>.Instance.m_onAdd = null;
			Singleton<RedNodeManager>.Instance.m_onRemove = null;
		}

		public void ReBuildCalcHero()
		{
		}

		public void AddHero(RedPointDataRecord record, long serverid)
		{
		}

		public void ReBuildCalcChapterGift(bool isReCalc)
		{
			RedPointDataRecord record = this.GetRecord("IAPGift.Chatper");
			if (record == null)
			{
				return;
			}
			record.RemoveAllChild();
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			if (dataModule == null || dataModule.ChapterGift == null)
			{
				return;
			}
			foreach (IAPChapterGift.Data data in dataModule.ChapterGift.GetEnableData())
			{
				if (data != null)
				{
					string text = data.Id.ToString();
					record.AddRecordPath(text, new RPMain.MainShop.Gift_Chatper_Node
					{
						m_id = data.Id
					}).SetUseLocalHideTime(true).OnceOneDay = true;
				}
			}
			if (isReCalc)
			{
				record.ReCalc(true, false);
			}
		}

		public string GetChapterGiftPath(int mID)
		{
			return "IAPGift.Chatper." + mID.ToString();
		}

		public void ReCalcAsync(string path)
		{
			if (this.cacheReCalcPath.ContainsKey(path))
			{
				return;
			}
			this.cacheReCalcPath.Add(path, Time.realtimeSinceStartup);
			DelayCall.Instance.CallOnce(1000, delegate
			{
				if (this.cacheReCalcPath.ContainsKey(path))
				{
					this.cacheReCalcPath.Remove(path);
				}
				this.ReCalc(path, true);
			});
		}

		public void ReCalc(string path, bool calcsub = true)
		{
			RedPointDataRecord record = this.mRedPointDataModule.GetRecord(path);
			if (record == null)
			{
				HLog.LogError("[redpoint]想要计算不存在的红点 : " + path);
				return;
			}
			record.ReCalc(calcsub, true);
		}

		public void RemoveRedPointHideTime(string path)
		{
			RedPointDataRecord record = this.mRedPointDataModule.GetRecord(path);
			if (record == null)
			{
				return;
			}
			record.RemoveHideTime();
		}

		public void ReCalcAll()
		{
			this.ReBuildCalcHero();
			this.mRedPointDataModule.ReCalcAll();
		}

		public RedPointDataRecord GetRecord(string path)
		{
			return this.mRedPointDataModule.GetRecord(path);
		}

		public void ClickRecord(string path)
		{
			RedPointDataRecord record = this.GetRecord(path);
			if (record != null)
			{
				record.ClickRecord();
			}
		}

		public void RegRecordChange(string path, Action<RedNodeListenData> action)
		{
			if (action == null || string.IsNullOrEmpty(path))
			{
				return;
			}
			RedPointDataRecord record = this.mRedPointDataModule.GetRecord(path);
			if (record == null)
			{
				HLog.LogError("[redpoint]RegRecordChange 未定义的红点:" + path);
				return;
			}
			record.RegRecordChange(action);
		}

		public void UnRegRecordChange(string path, Action<RedNodeListenData> action)
		{
			if (action == null || string.IsNullOrEmpty(path))
			{
				return;
			}
			RedPointDataRecord record = this.mRedPointDataModule.GetRecord(path);
			if (record == null)
			{
				HLog.LogError("[redpoint]UnRegRecordChange 未定义的红点:" + path);
				return;
			}
			record.UnRegRecordChange(action);
		}

		public void GotoMain()
		{
			this.ReCalcAll();
		}

		private static RedPointController mInstance;

		private RedPointDataModule mRedPointDataModule;

		private RedPoint_SocketNetProxy mSocketNetProxy;

		public Dictionary<string, float> cacheReCalcPath = new Dictionary<string, float>();
	}
}
