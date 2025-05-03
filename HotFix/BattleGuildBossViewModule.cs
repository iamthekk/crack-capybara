using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using Server;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleGuildBossViewModule : BaseViewModule
	{
		private int BossID
		{
			get
			{
				if (this.mCurBoss == null)
				{
					return 0;
				}
				return this.mCurBoss.BossID;
			}
		}

		public override void OnCreate(object data)
		{
			this.m_gameDataModule = GameApp.Data.GetDataModule(DataName.GameDataModule);
			this.m_battleBossDataModule = GameApp.Data.GetDataModule(DataName.BattleGuildBossDataModule);
			this.buttonJump.onClick.AddListener(new UnityAction(this.OnClickBtnJump));
		}

		public override void OnOpen(object data)
		{
			this.RefreshUIOnOpen();
			this.buttonSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvE);
			this.mBattleState = GameApp.State.GetState(StateName.BattleGuildBossState);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			this.buttonJump.onClick.RemoveListener(new UnityAction(this.OnClickBtnJump));
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnBattleStart));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_BattleDamageUpdate, new HandlerEvent(this.OnBattleDamageUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnRoundStartHandler));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleStart, new HandlerEvent(this.OnBattleStart));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_BattleDamageUpdate, new HandlerEvent(this.OnBattleDamageUpdate));
		}

		private void RefreshUIOnOpen()
		{
			this.m_tempCurDamage = 0L;
			this.Obj_Damage.SetActiveSafe(false);
			this.Obj_Round.SetActiveSafe(false);
			this.buttonJump.gameObject.SetActiveSafe(false);
			this.mBossInfo = GuildSDKManager.Instance.GuildActivity.GuildBoss;
			this.mCurBoss = this.mBossInfo.GetCurrentBoss();
			this.mTotalDamage = (long)this.m_battleBossDataModule.OldTotalDamage;
			this.m_curDamage = (long)this.m_battleBossDataModule.CurDamage;
			if (this.mCurBoss == null)
			{
				return;
			}
			this.BuildDamagePartData();
			if (this.mCurBoss == null)
			{
				HLog.LogError("mCurBoss is null");
				return;
			}
			List<CardData> guildBossEnemyCardDatas = GuildController.GetGuildBossEnemyCardDatas(GuildProxy.Table.TableMgr, this.mCurBoss.BossStep, -1L);
			if (guildBossEnemyCardDatas.Count > 0)
			{
				FP hpMax = guildBossEnemyCardDatas[0].m_memberAttributeData.GetHpMax();
				this.m_totalHp = hpMax.GetValue();
			}
			GuildBOSS_guildBoss elementById = GameApp.Table.GetManager().GetGuildBOSS_guildBossModelInstance().GetElementById(this.mCurBoss.BossID);
			if (elementById != null)
			{
				this.TextName.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.NameID);
				return;
			}
			this.TextName.text = "";
		}

		private void OnRefreshHpSlider()
		{
			this.Obj_Damage.SetActiveSafe(true);
			this.CheckDamagePart();
			long num;
			if (this.m_battleBossDataModule.BeforeBattleBossHP == -1L)
			{
				num = this.m_totalHp;
			}
			else
			{
				num = this.m_battleBossDataModule.BeforeBattleBossHP;
			}
			this.m_serverRemainHp = num;
			if (num <= 0L)
			{
				num = 0L;
			}
			this.TextHP.text = DxxTools.FormatNumber(num);
			double num2 = (double)num / (double)this.m_totalHp;
			this.SliderHP.value = (float)num2;
		}

		public void PlayOpenAni(Action aniFinish)
		{
			if (aniFinish != null)
			{
				aniFinish();
			}
		}

		private void OnBattleStart(object sender, int type, BaseEventArgs args)
		{
			this.OnRefreshHpSlider();
		}

		private void OnRoundStartHandler(object sender, int type, BaseEventArgs args)
		{
			this.Obj_Round.gameObject.SetActiveSafe(true);
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			int num = 0;
			Guild_guildConst guildConstTable = GuildProxy.Table.GetGuildConstTable(141);
			if (guildConstTable != null)
			{
				num = guildConstTable.TypeInt;
			}
			this.buttonJump.gameObject.SetActiveSafe(eventArgsRoundStart.CurRound >= num);
			this.TextRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitowerbattle_round", new object[] { eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound });
		}

		private void OnBattleDamageUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgBattleDamageUpdate eventArgBattleDamageUpdate = eventArgs as EventArgBattleDamageUpdate;
			if (eventArgBattleDamageUpdate != null)
			{
				this.mTotalDamage += eventArgBattleDamageUpdate.CurDamage;
				this.m_tempCurDamage += eventArgBattleDamageUpdate.CurDamage;
				this.CheckDamagePart();
				long num = eventArgBattleDamageUpdate.CurHP;
				if (num <= 0L)
				{
					num = 0L;
				}
				this.TextHP.text = DxxTools.FormatNumber(num);
				double num2 = (double)num / (double)this.m_totalHp;
				this.SliderHP.value = (float)num2;
			}
		}

		private void BuildDamagePartData()
		{
			this.mDamagePartList.Clear();
			this.mDamagePartIndex = 0;
			IList<GuildBOSS_guildBossBox> allElements = GameApp.Table.GetManager().GetGuildBOSS_guildBossBoxModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				GuildBOSS_guildBossBox guildBOSS_guildBossBox = allElements[i];
				if (guildBOSS_guildBossBox != null && guildBOSS_guildBossBox.BossId == this.BossID)
				{
					BattleGuildBossDamagePartData battleGuildBossDamagePartData = new BattleGuildBossDamagePartData();
					battleGuildBossDamagePartData.DamageMax = guildBOSS_guildBossBox.Damage;
					this.mDamagePartList.Add(battleGuildBossDamagePartData);
				}
			}
			this.mDamagePartList.Sort((BattleGuildBossDamagePartData x, BattleGuildBossDamagePartData y) => x.DamageMax.CompareTo(y.DamageMax));
			for (int j = 1; j < this.mDamagePartList.Count; j++)
			{
				BattleGuildBossDamagePartData battleGuildBossDamagePartData2 = this.mDamagePartList[j - 1];
				this.mDamagePartList[j].DamageMin = battleGuildBossDamagePartData2.DamageMax;
			}
		}

		private void CheckDamagePart()
		{
			for (int i = 0; i < this.mDamagePartList.Count; i++)
			{
				BattleGuildBossDamagePartData battleGuildBossDamagePartData = this.mDamagePartList[i];
				if (this.mTotalDamage < battleGuildBossDamagePartData.DamageMax)
				{
					this.mDamagePartIndex = i;
					this.mCurDamagePart = battleGuildBossDamagePartData;
					break;
				}
				if (this.mTotalDamage >= battleGuildBossDamagePartData.DamageMax)
				{
					this.mRewardCount++;
					this.mTotalDamage -= battleGuildBossDamagePartData.DamageMax;
				}
			}
			this.TextRewards.text = string.Format("x{0}", this.mRewardCount);
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		[Header("名称")]
		public CustomText TextName;

		[Header("血条")]
		public Slider SliderHP;

		public CustomText TextHP;

		public CustomText TextRewards;

		public GameObject Obj_Damage;

		[Header("回合")]
		public CustomText TextRound;

		public GameObject Obj_Round;

		[Header("底部操作")]
		public CustomButton buttonJump;

		public UISpeedButtonCtrl buttonSpeedUp;

		private GuildBossInfo mBossInfo;

		private GuildBossData mCurBoss;

		private List<BattleGuildBossDamagePartData> mDamagePartList = new List<BattleGuildBossDamagePartData>();

		private int mDamagePartIndex;

		private BattleGuildBossDamagePartData mCurDamagePart;

		private GameDataModule m_gameDataModule;

		private BattleGuildBossDataModule m_battleBossDataModule;

		private BattleGuildBossState mBattleState;

		private long mTotalDamage;

		private long m_curDamage;

		private int mRewardCount;

		private long m_totalHp;

		private long m_tempCurDamage;

		private long m_serverRemainHp;
	}
}
