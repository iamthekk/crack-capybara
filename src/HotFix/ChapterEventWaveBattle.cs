using System;
using System.Collections.Generic;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using Proto.Chapter;
using Server;

namespace HotFix
{
	public class ChapterEventWaveBattle : ChapterEventBattle
	{
		public override void OnInit()
		{
			this.waveBattleData = this.currentData as GameEventDataWaveBattle;
			if (this.waveBattleData == null)
			{
				return;
			}
			this.sysRandom = new XRandom(this.waveBattleData.poolData.randomSeed);
			for (int i = 0; i < this.waveBattleData.groupList.Count; i++)
			{
				int num = this.waveBattleData.groupList[i];
				int difficult = Singleton<GameEventController>.Instance.GetDifficult(this.stage);
				int num2 = this.sysRandom.NextInt();
				int num3 = Singleton<GameEventController>.Instance.RandomPoolIdByGroup(num, difficult, num2);
				if (num3 > 0)
				{
					this.monsterCfgList.Add(num3);
				}
			}
			if (this.monsterCfgList.Count <= 0)
			{
				return;
			}
			this.monsterCfgId = this.monsterCfgList[0];
			bool flag = EventMemberController.Instance.IsCheckBattle;
			int totalStage = Singleton<GameEventController>.Instance.GetTotalStage();
			if (this.stage == totalStage || this.waveBattleData.poolData.eventType == GameEventType.BattleElite || this.waveBattleData.poolData.eventType == GameEventType.BattleBoss)
			{
				flag = true;
			}
			ChapterDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			if (flag)
			{
				int chapterID = dataModule.ChapterID;
				string checkAttribute = Singleton<GameEventController>.Instance.GetCheckAttribute();
				List<int> battleSkills = Singleton<GameEventController>.Instance.GetBattleSkills();
				long value = Singleton<GameEventController>.Instance.PlayerData.CurrentHp.GetValue();
				int reviveCount = Singleton<GameEventController>.Instance.PlayerData.ReviveCount;
				NetworkUtils.Chapter.DoEndChapterCheckRequest(chapterID, this.stage, checkAttribute, battleSkills, this.monsterCfgList, value, reviveCount, delegate(bool result, EndChapterCheckResponse response)
				{
					if (result)
					{
						this.DoBattle(this.waveBattleData);
					}
				});
				return;
			}
			dataModule.SetBattleSeed(this.waveBattleData.poolData.randomSeed);
			this.DoBattle(this.waveBattleData);
		}

		public override void OnDeInit()
		{
		}

		private void DoBattle(GameEventDataWaveBattle waveBattleData)
		{
			bool flag = waveBattleData.IsHaveChildType(GameEventNodeType.Box);
			EventArgsAddEnemy instance = Singleton<EventArgsAddEnemy>.Instance;
			instance.SetData(this.monsterCfgId, waveBattleData.poolData.atkUpgrade, waveBattleData.poolData.hpUpgrade, flag);
			EventMemberController.Instance.EventAddEnemy(this.monsterCfgList);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEnemy, instance);
			this.CreateParam();
			List<NodeParamBase> list = new List<NodeParamBase>();
			if (this.dropAttList.Count > 0)
			{
				list.AddRange(this.dropAttList);
			}
			if (this.dropItemList.Count > 0)
			{
				list.AddRange(this.dropItemList);
			}
			if (list.Count > 0)
			{
				EventArgDropInfo eventArgDropInfo = new EventArgDropInfo();
				eventArgDropInfo.SetData(list);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_DropInfo, eventArgDropInfo);
			}
		}

		protected override void CreateParam()
		{
			this.dropAttList.Clear();
			this.dropItemList.Clear();
			for (int i = 0; i < this.monsterCfgList.Count; i++)
			{
				List<int> positionMonsterIds = base.GetPositionMonsterIds(this.monsterCfgId);
				for (int j = 0; j < positionMonsterIds.Count; j++)
				{
					int num = positionMonsterIds[j];
					GameMember_member elementById = GameApp.Table.GetManager().GetGameMember_memberModelInstance().GetElementById(num);
					if (elementById == null)
					{
						HLog.LogError(string.Format("GameEventBattle.CreateParam: Not found member id={0}", num));
					}
					else
					{
						int num2 = base.RandomDrop(this.sysRandom, elementById.dropExp);
						NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.Exp, (double)num2, ChapterDropSource.Battle, 1);
						this.dropAttList.Add(nodeAttParam);
					}
				}
			}
			if (this.waveBattleData != null)
			{
				List<NodeItemParam> battleDrop = this.waveBattleData.GetBattleDrop();
				if (battleDrop.Count > 0)
				{
					this.dropItemList.AddRange(battleDrop);
				}
				List<NodeItemParam> monsterDrop = this.waveBattleData.GetMonsterDrop();
				if (monsterDrop.Count > 0)
				{
					this.dropItemList.AddRange(monsterDrop);
				}
			}
		}

		private List<int> monsterCfgList = new List<int>();

		private XRandom sysRandom;

		private GameEventDataWaveBattle waveBattleData;
	}
}
