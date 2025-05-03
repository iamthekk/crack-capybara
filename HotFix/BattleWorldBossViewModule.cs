using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class BattleWorldBossViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.buttonJump.m_onClick = new Action(this.OnClickBtnJump);
		}

		public override void OnDelete()
		{
			this.buttonJump.m_onClick = null;
		}

		public override void OnOpen(object data)
		{
			this.worldBossData = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
			this.mTotalDamage = this.worldBossData.TotalDamage;
			this.mLastTotalDamage = this.mTotalDamage;
			this.TextName.text = Singleton<LanguageManager>.Instance.GetInfoByID(this.worldBossData.bossCfg.nameLanguageID);
			this.RefreshRound(0, 10);
			this.BuildDamagePartData();
			this.CheckDamagePart();
			this.FreshCurrentDamage();
			this.buttonSpeedUp.SetData(UISpeedButtonCtrl.SpeedType.PvE);
		}

		public override void OnClose()
		{
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnEventRefreshRound));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_BattleDamageUpdate, new HandlerEvent(this.OnBattleDamageUpdate));
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameView_RoundStart, new HandlerEvent(this.OnEventRefreshRound));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_BattleDamageUpdate, new HandlerEvent(this.OnBattleDamageUpdate));
		}

		private void OnEventRefreshRound(object sender, int type, BaseEventArgs args)
		{
			EventArgsRoundStart eventArgsRoundStart = args as EventArgsRoundStart;
			if (eventArgsRoundStart == null)
			{
				return;
			}
			this.RefreshRound(eventArgsRoundStart.CurRound, eventArgsRoundStart.MaxRound);
		}

		private void OnBattleDamageUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgBattleDamageUpdate eventArgBattleDamageUpdate = eventArgs as EventArgBattleDamageUpdate;
			if (eventArgBattleDamageUpdate != null)
			{
				this.mTotalDamage += eventArgBattleDamageUpdate.CurDamage;
				this.CheckDamagePart();
				this.FreshCurrentDamage();
			}
		}

		private void FreshCurrentDamage()
		{
			if (this.mCurDamagePart != null)
			{
				this.TextHP.text = DxxTools.FormatNumber(this.mTotalDamage) + "/" + DxxTools.FormatNumber(this.mCurDamagePart.DamageMax);
				double num = (double)(this.mTotalDamage - this.mCurDamagePart.DamageMin) / (double)(this.mCurDamagePart.DamageMax - this.mCurDamagePart.DamageMin);
				this.SliderHP.value = (float)num;
			}
		}

		private void BuildDamagePartData()
		{
			this.mDamagePartList.Clear();
			IList<WorldBoss_WorldBossBox> worldBoss_WorldBossBoxElements = GameApp.Table.GetManager().GetWorldBoss_WorldBossBoxElements();
			for (int i = 0; i < worldBoss_WorldBossBoxElements.Count; i++)
			{
				WorldBoss_WorldBossBox worldBoss_WorldBossBox = worldBoss_WorldBossBoxElements[i];
				if (worldBoss_WorldBossBox != null && worldBoss_WorldBossBox.BossId == this.worldBossData.ChapterId)
				{
					BattleWorldBossDamagePartData battleWorldBossDamagePartData = new BattleWorldBossDamagePartData();
					battleWorldBossDamagePartData.DamageMax = worldBoss_WorldBossBox.Damage;
					this.mDamagePartList.Add(battleWorldBossDamagePartData);
				}
			}
			this.mDamagePartList.Sort((BattleWorldBossDamagePartData x, BattleWorldBossDamagePartData y) => x.DamageMax.CompareTo(y.DamageMax));
			for (int j = 1; j < this.mDamagePartList.Count; j++)
			{
				BattleWorldBossDamagePartData battleWorldBossDamagePartData2 = this.mDamagePartList[j - 1];
				this.mDamagePartList[j].DamageMin = battleWorldBossDamagePartData2.DamageMax;
			}
		}

		private void CheckDamagePart()
		{
			this.mRewardCount = 0;
			for (int i = 0; i < this.mDamagePartList.Count; i++)
			{
				BattleWorldBossDamagePartData battleWorldBossDamagePartData = this.mDamagePartList[i];
				if (this.mTotalDamage < battleWorldBossDamagePartData.DamageMax)
				{
					this.mCurDamagePart = battleWorldBossDamagePartData;
					break;
				}
				if (this.mTotalDamage >= battleWorldBossDamagePartData.DamageMax && this.mLastTotalDamage < battleWorldBossDamagePartData.DamageMax)
				{
					this.mRewardCount++;
				}
			}
			this.TextRewards.text = string.Format("x{0}", this.mRewardCount);
		}

		private void OnClickBtnJump()
		{
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_Game_BattleJump, null);
		}

		private void RefreshRound(int current, int max)
		{
			this.TextRound.text = Singleton<LanguageManager>.Instance.GetInfoByID("uitowerbattle_round", new object[] { current, max });
		}

		[Header("名称")]
		public CustomText TextName;

		[Header("血条")]
		public Slider SliderHP;

		public CustomText TextHP;

		public CustomText TextRewards;

		[Header("回合")]
		public CustomText TextRound;

		[Header("底部操作")]
		public CustomButton buttonJump;

		public UISpeedButtonCtrl buttonSpeedUp;

		private WorldBossDataModule worldBossData;

		private List<BattleWorldBossDamagePartData> mDamagePartList = new List<BattleWorldBossDamagePartData>();

		private BattleWorldBossDamagePartData mCurDamagePart;

		private long mLastTotalDamage;

		private long mTotalDamage;

		private int mRewardCount;
	}
}
