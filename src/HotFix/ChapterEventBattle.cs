using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using Proto.Chapter;
using Server;

namespace HotFix
{
	public class ChapterEventBattle : ChapterEventBase
	{
		public override void OnInit()
		{
			this.battleData = this.currentData as GameEventDataBattle;
			if (this.battleData == null)
			{
				return;
			}
			this.sysRandom = new XRandom(this.battleData.poolData.randomSeed);
			this.monsterCfgId = this.battleData.poolData.monsterCfgId;
			if (this.battleData.groupIndex > 0)
			{
				int groupIndex = this.battleData.groupIndex;
				int difficult = Singleton<GameEventController>.Instance.GetDifficult(this.stage);
				int num = Singleton<GameEventController>.Instance.RandomPoolIdByGroup(groupIndex, difficult, this.battleData.poolData.randomSeed);
				if (num > 0)
				{
					this.monsterCfgId = num;
				}
				else
				{
					HLog.LogError(string.Format("特殊战斗获取失败，groupId={0}, difficult={1}", groupIndex, difficult));
				}
			}
			bool flag = EventMemberController.Instance.IsCheckBattle;
			int totalStage = Singleton<GameEventController>.Instance.GetTotalStage();
			if (this.stage == totalStage || this.battleData.poolData.eventType == GameEventType.BattleElite || this.battleData.poolData.eventType == GameEventType.BattleBoss)
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
				List<int> list = new List<int> { this.monsterCfgId };
				NetworkUtils.Chapter.DoEndChapterCheckRequest(chapterID, this.stage, checkAttribute, battleSkills, list, value, reviveCount, delegate(bool result, EndChapterCheckResponse response)
				{
					if (result)
					{
						this.DoBattle(this.battleData);
					}
				});
				return;
			}
			dataModule.SetBattleSeed(this.battleData.poolData.randomSeed);
			this.DoBattle(this.battleData);
		}

		public override void OnDeInit()
		{
		}

		private void DoBattle(GameEventDataBattle battleData)
		{
			bool flag = battleData.IsHaveChildType(GameEventNodeType.Box);
			EventArgsAddEnemy instance = Singleton<EventArgsAddEnemy>.Instance;
			instance.SetData(this.monsterCfgId, battleData.poolData.atkUpgrade, battleData.poolData.hpUpgrade, flag);
			EventMemberController.Instance.EventAddEnemy(new List<int> { this.monsterCfgId });
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

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.EndBattle)
			{
				this.ShowUI();
				this.CalcEndBattleAttr();
				this.isShowResult = true;
			}
			else if (this.isShowResult && pushType == GameEventPushType.UIAniFinish)
			{
				if (!base.isDone)
				{
					GameTGATools.Ins.AddStageClickTempAtt(this.dropAttList, true);
					GameTGATools.Ins.AddStageClickTempItem(this.dropItemList, true);
				}
				base.MarkDone();
			}
		}

		protected virtual void CreateParam()
		{
			this.dropAttList.Clear();
			this.dropItemList.Clear();
			List<int> positionMonsterIds = base.GetPositionMonsterIds(this.monsterCfgId);
			for (int i = 0; i < positionMonsterIds.Count; i++)
			{
				int num = positionMonsterIds[i];
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
			if (this.battleData != null)
			{
				List<NodeItemParam> battleDrop = this.battleData.GetBattleDrop();
				if (battleDrop.Count > 0)
				{
					this.dropItemList.AddRange(battleDrop);
				}
				List<NodeItemParam> monsterDrop = this.battleData.GetMonsterDrop();
				if (monsterDrop.Count > 0)
				{
					this.dropItemList.AddRange(monsterDrop);
				}
			}
		}

		protected void CalcEndBattleAttr()
		{
			List<NodeAttParam> list = new List<NodeAttParam>();
			BattleChapterPlayerData playerData = Singleton<GameEventController>.Instance.PlayerData;
			int num = playerData.AttributeData.EffectiveTimes.FloorToInt();
			if (num > 0 && playerData.BattleEndEffectActiveNum < num)
			{
				playerData.AddBattleEndEffectActiveNum();
				float num2 = playerData.AttributeData.AttackBattleEnd.AsFloat();
				float num3 = playerData.AttributeData.DefenseBattleEnd.AsFloat();
				if (num2 > 0f)
				{
					NodeAttParam nodeAttParam = new NodeAttParam(GameEventAttType.AttackPercent, (double)num2, ChapterDropSource.Battle, 1);
					list.Add(nodeAttParam);
				}
				if (num3 > 0f)
				{
					NodeAttParam nodeAttParam2 = new NodeAttParam(GameEventAttType.DefencePercent, (double)num3, ChapterDropSource.Battle, 1);
					list.Add(nodeAttParam2);
				}
				int num4 = num - playerData.BattleEndEffectActiveNum;
				if (list.Count > 0)
				{
					Singleton<GameEventController>.Instance.MergerAttribute(list);
					this.ShowEndBattleAttrUI(list, num4);
				}
			}
		}

		protected override void ShowUI()
		{
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "GameEventData_25", null, this.dropAttList, this.dropItemList, null, null, null);
			GameEventData next = this.currentData.GetNext(0);
			if (next != null)
			{
				GameEventDataSelect gameEventDataSelect = new GameEventDataSelect(this.currentData.poolData, "GameEventData_Continue", "", GameEventButtonType.Normal, GameEventButtonColorEnum.Green, 0, 0, "", "");
				gameEventDataSelect.AddChild(next);
				gameEventUIData.AddButton(0, gameEventDataSelect, false, "");
			}
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
		}

		protected void ShowEndBattleAttrUI(List<NodeAttParam> attList, int lave)
		{
			if (attList.Count > 0)
			{
				GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "GameEventData_142", new object[] { lave }, attList, null, null, null, null);
				EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
				eventArgAddEvent.uiData = gameEventUIData;
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
			}
		}

		protected List<NodeAttParam> dropAttList = new List<NodeAttParam>();

		protected List<NodeItemParam> dropItemList = new List<NodeItemParam>();

		protected int monsterCfgId;

		private XRandom sysRandom;

		private bool isShowResult;

		private GameEventDataBattle battleData;
	}
}
