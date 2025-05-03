using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Framework;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using HotFix.Client;
using LocalModels.Bean;
using Proto.Tower;
using Server;

namespace HotFix
{
	public class GameEventStateRogueDungeon : GameEventStateBase
	{
		public GameEventStateRogueDungeon(int id)
			: base(id)
		{
		}

		public override void OnEnter()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Game_BattleChangeAttribute, new HandlerEvent(this.OnEventBattleChangeAttribute));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_GameOverRefreshAttr, new HandlerEvent(this.OnEventGameOver));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_SpecialChallenge_Close, new HandlerEvent(this.OnEventCloseSpecialChallenge));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEndBattle));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, new HandlerEvent(this.OnEventSaveSkillAndAttr));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEven_ArriveNpc, new HandlerEvent(this.OnEventArriveNpc));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIGameEvent_CloseBoxUI, new HandlerEvent(this.OnEventCloseBoxUI));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_Escape, new HandlerEvent(this.OnEventEscape));
			AddAttributeDataModule dataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			HeroDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.HeroDataModule);
			PetDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.mDataModule = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
			this.PlayerData = new BattleChapterPlayerData(dataModule2.MainCardData.m_memberID, dataModule.MemberAttributeData, dataModule.SkillIDs, dataModule3.GetFightPetCardData());
			this.skillBuildPool = new GameEventSkillBuildFactory();
			this.slotTrainPool = new GameEventSlotTrainFactory();
			this.skillBuildPool.Init(100);
			this.slotTrainPool.Init();
			if (this.mDataModule.IsBattleSign)
			{
				this.UserPlayerRecord();
			}
			this.isEscape = false;
			this.isBattle = false;
		}

		public override void OnUpdate(float deltaTime)
		{
		}

		public override void OnExit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Game_BattleChangeAttribute, new HandlerEvent(this.OnEventBattleChangeAttribute));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_GameOverRefreshAttr, new HandlerEvent(this.OnEventGameOver));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_SpecialChallenge_Close, new HandlerEvent(this.OnEventCloseSpecialChallenge));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_Battle_GameOver, new HandlerEvent(this.OnEndBattle));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UI_RogueDungeon_SaveSkillAndAttr, new HandlerEvent(this.OnEventSaveSkillAndAttr));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEven_ArriveNpc, new HandlerEvent(this.OnEventArriveNpc));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIGameEvent_CloseBoxUI, new HandlerEvent(this.OnEventCloseBoxUI));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_UIBattleRogueDungeon_Escape, new HandlerEvent(this.OnEventEscape));
			this.PlayerData.ClearData();
			this.PlayerData = null;
			this.skillBuildPool = null;
			this.slotTrainPool = null;
		}

		public override void StartEvent()
		{
			if (this.mDataModule.IsBattleSign)
			{
				this.UserSkillRecord();
			}
			if (this.mDataModule.EventID > 0U)
			{
				this.DoEventPoint();
				return;
			}
			SpecialChallengesViewModule.OpenData openData = new SpecialChallengesViewModule.OpenData();
			openData.monsterEntryIds = this.mDataModule.MonsterSkills.ToList<int>();
			openData.source = SpecialChallengesViewModule.Source.RogueDungeon;
			GameApp.View.OpenView(ViewName.SpecialChallengesViewModule, openData, 1, null, null);
			this.isShowSpecialChallenge = true;
		}

		public override void ContinueEvent()
		{
		}

		private void OnEventBattleChangeAttribute(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsGameChangeAttribute eventArgsGameChangeAttribute = eventArgs as EventArgsGameChangeAttribute;
			if (eventArgsGameChangeAttribute != null)
			{
				this.PlayerData.BattleChangeAttribute(eventArgsGameChangeAttribute.changeData);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			}
		}

		private void OnEventGameOver(object sender, int type, BaseEventArgs eventArgs)
		{
			this.PlayerData.SetEventAttributes();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		private void OnEventCloseSpecialChallenge(object sender, int type, BaseEventArgs eventArgs)
		{
			if (!this.isShowSpecialChallenge)
			{
				return;
			}
			this.isShowSpecialChallenge = false;
			bool flag = false;
			if (this.mDataModule.IsEnterSkill)
			{
				if (this.mDataModule.PassFloorCount > 0U && !this.mDataModule.IsRoundSkill)
				{
					flag = true;
					this.FloorSelectSkill();
				}
			}
			else
			{
				flag = true;
				RoundSelectSkillViewModule.OpenData openData = new RoundSelectSkillViewModule.OpenData();
				openData.Seed = this.mDataModule.FloorSeed;
				openData.SourceType = (SkillBuildSourceType)GameConfig.RogueDungeon_Start_SkillSourceID;
				openData.TotalRound = GameConfig.RogueDungeon_Start_SelectRound;
				openData.RandomSkillNum = GameConfig.RogueDungeon_Start_RandomSkillNum;
				openData.SelectSkillNum = GameConfig.RogueDungeon_Start_SelectSkillNum;
				openData.OnSaveSkillAction = new Action<Dictionary<int, int[]>>(this.OnSaveSkillAction);
				GameApp.View.OpenView(ViewName.RoundSelectSkillViewModule, openData, 1, null, null);
			}
			if (!flag)
			{
				this.DoEventBattle();
			}
		}

		private void OnEndBattle(object sender, int type, BaseEventArgs args)
		{
			this.isBattle = false;
			EventArgsBool eventArgsBool = new EventArgsBool();
			eventArgsBool.SetData(false);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_ShowHideRound, eventArgsBool);
			EventArgsGameEnd eventArgsGameEnd = args as EventArgsGameEnd;
			if (eventArgsGameEnd != null)
			{
				if (eventArgsGameEnd.m_gameOverType == GameOverType.Win)
				{
					RogueDungeon_rogueDungeon rogueDungeon_rogueDungeon = GameApp.Table.GetManager().GetRogueDungeon_rogueDungeonModelInstance().GetAllElements()
						.Last<RogueDungeon_rogueDungeon>();
					if ((ulong)this.mDataModule.CurrentFloorID > (ulong)((long)rogueDungeon_rogueDungeon.id))
					{
						GameApp.View.OpenView(ViewName.BattleRogueDungeonResultViewModule, null, 1, null, null);
						return;
					}
					if (!this.mDataModule.IsBattleEndRecoverDisabled())
					{
						NetworkUtils.RogueDungeon.DoHellRevertHpRequest(delegate(bool result, HellRevertHpResponse resp)
						{
							if (result && resp.Hp > 0L)
							{
								this.PlayerData.SetCurrentHp(resp.Hp);
								GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
							}
							this.<OnEndBattle>g__DoNext|15_1();
						});
						return;
					}
					this.<OnEndBattle>g__DoNext|15_1();
					return;
				}
				else
				{
					GameApp.View.OpenView(ViewName.BattleRogueDungeonResultViewModule, null, 1, null, null);
				}
			}
		}

		private void OnEventSaveSkillAndAttr(object sender, int type, BaseEventArgs args)
		{
			if (this.isDoEventPoint)
			{
				this.isDoEventPoint = false;
				EventMemberController.Instance.LeaveFromNpc();
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			EventArgSaveSkillAndAttr eventArgSaveSkillAndAttr = args as EventArgSaveSkillAndAttr;
			if (eventArgSaveSkillAndAttr != null && eventArgSaveSkillAndAttr.attrDic != null)
			{
				dictionary = eventArgSaveSkillAndAttr.attrDic;
			}
			List<int> eventSkillBuildIds = Singleton<GameEventController>.Instance.PlayerData.GetEventSkillBuildIds();
			NetworkUtils.RogueDungeon.DoHellSaveSkillRequest(this.PlayerData.CurrentHp.AsLong(), eventSkillBuildIds, dictionary, delegate(bool result, HellSaveSkillResponse response)
			{
				this.DoGoNextFloor();
			});
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_Refresh, null);
		}

		private void OnEventArriveNpc(object sender, int type, BaseEventArgs args)
		{
			if (this.isDoEventPoint && this.mDataModule.EventID > 0U)
			{
				RogueDungeon_endEvent rogueDungeon_endEvent = GameApp.Table.GetManager().GetRogueDungeon_endEvent((int)this.mDataModule.EventID);
				if (rogueDungeon_endEvent == null)
				{
					HLog.LogError(string.Format("RogueDungeon_endEvent 表未找到 id ={0}", this.mDataModule.EventID));
					return;
				}
				switch (rogueDungeon_endEvent.type)
				{
				case 1:
				{
					GameEventAngelViewModule.OpenData openData = new GameEventAngelViewModule.OpenData();
					openData.modelId = rogueDungeon_endEvent.modelId;
					openData.memberId = rogueDungeon_endEvent.memberId;
					openData.seed = this.mDataModule.FloorSeed;
					openData.sourceType = (SkillBuildSourceType)rogueDungeon_endEvent.buildId;
					GameApp.View.OpenView(ViewName.GameEventAngelViewModule, openData, 1, null, null);
					return;
				}
				case 2:
				{
					GameEventDemonViewModule.OpenData openData2 = new GameEventDemonViewModule.OpenData();
					openData2.modelId = rogueDungeon_endEvent.modelId;
					openData2.memberId = rogueDungeon_endEvent.memberId;
					openData2.seed = this.mDataModule.FloorSeed;
					openData2.sourceType = (SkillBuildSourceType)rogueDungeon_endEvent.buildId;
					GameApp.View.OpenView(ViewName.GameEventDemonViewModule, openData2, 1, null, null);
					return;
				}
				case 3:
				{
					SlotTrainViewModule.OpenData openData3 = new SlotTrainViewModule.OpenData();
					openData3.seed = this.mDataModule.FloorSeed;
					openData3.sourceType = (SkillBuildSourceType)rogueDungeon_endEvent.buildId;
					GameApp.View.OpenView(ViewName.SlotTrainViewModule, openData3, 1, null, null);
					break;
				}
				default:
					return;
				}
			}
		}

		private void OnEventCloseBoxUI(object sender, int type, BaseEventArgs args)
		{
			EventMemberController.Instance.RemoveBox();
		}

		private void OnEventEscape(object sender, int type, BaseEventArgs args)
		{
			this.isEscape = true;
		}

		private void UserPlayerRecord()
		{
			this.PlayerData.UserRecord(this.mDataModule.PlayerCurrentHp, this.mDataModule.PlayerAttMap, this.mDataModule.RevertCount);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
		}

		private void UserSkillRecord()
		{
			List<int> playerSkillIds = this.mDataModule.GetPlayerSkillIds();
			for (int i = 0; i < playerSkillIds.Count; i++)
			{
				GameEventSkillBuildData skillByID = this.skillBuildPool.GetSkillByID(playerSkillIds[i]);
				this.PlayerData.AddSkillBuild(skillByID, false);
				this.skillBuildPool.SelectSkill(skillByID, false);
				this.skillBuildPool.RecordRemoveLowLevelSkill(skillByID);
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_RefreshAttribute, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_Refresh, null);
		}

		private void FloorSelectSkill()
		{
			if (this.isEscape)
			{
				return;
			}
			RoundSelectSkillViewModule.OpenData openData = new RoundSelectSkillViewModule.OpenData();
			openData.Seed = this.mDataModule.FloorSeed;
			openData.SourceType = (SkillBuildSourceType)GameConfig.RogueDungeon_Floor_SkillSourceID;
			openData.TotalRound = GameConfig.RogueDungeon_Floor_SelectRound;
			openData.RandomSkillNum = GameConfig.RogueDungeon_Floor_RandomSkillNum;
			openData.SelectSkillNum = GameConfig.RogueDungeon_Floor_SelectSkillNum;
			openData.OnSaveSkillAction = new Action<Dictionary<int, int[]>>(this.OnSaveSkillAction);
			GameApp.View.OpenView(ViewName.RoundSelectSkillViewModule, openData, 1, null, null);
		}

		private void OnSaveSkillAction(Dictionary<int, int[]> skillDic)
		{
			NetworkUtils.RogueDungeon.DoHellEnterSelectSkillRequest(Singleton<GameEventController>.Instance.PlayerData.GetEventSkillBuildIds(), delegate(bool result, HellEnterSelectSkillResponse response)
			{
				this.DoEventBattle();
			});
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_Refresh, null);
		}

		private void DoEventBattle()
		{
			if (this.isEscape)
			{
				return;
			}
			this.isLastBattle = (ulong)this.mDataModule.CurrentWaveIndex == (ulong)((long)(this.mDataModule.MonsterCfgList.Count - 1));
			string key = "RogueDungeon";
			if ((ulong)this.mDataModule.CurrentWaveIndex < (ulong)((long)this.mDataModule.MonsterCfgList.Count))
			{
				NetworkUtils.RogueDungeon.DoBattleRequest(this.mDataModule.ServerFloorID, delegate(bool result, HellDoChallengeResponse response)
				{
					if (result)
					{
						DxxTools.UI.RemoveServerTimeClockCallback(key);
						if (this.isBattle)
						{
							return;
						}
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < response.MonsterCfgId.Count; i++)
						{
							stringBuilder.Append(response.MonsterCfgId[i]);
							if (i != response.MonsterCfgId.Count - 1)
							{
								stringBuilder.Append(", ");
							}
						}
						if (response.MonsterCfgId.Count <= 0)
						{
							HLog.LogError("无法进入战斗，服务器返回对阵组数量为0");
							return;
						}
						this.isBattle = true;
						EventMemberController.Instance.EventAddEnemy(response.MonsterCfgId.ToList<int>());
						bool flag = false;
						if (response.Result != 1)
						{
							flag = true;
						}
						if (flag)
						{
							int num = -1;
							if (response.Result == 0)
							{
								num = 1;
							}
							else if (response.Result == 1)
							{
								num = 0;
							}
							else if (response.Result == -1)
							{
								num = 2;
							}
							GameApp.SDK.Analyze.Track_EnterDungeon(num, this.PlayerData.AttributeData.Attack.GetValue(), this.PlayerData.AttributeData.Defence.GetValue(), this.PlayerData.AttributeData.HPMax.GetValue(), this.PlayerData.GetPlayerSkillBuildList(), response.CommonData.Reward);
							return;
						}
					}
					else
					{
						long serverTimestamp = DxxTools.Time.ServerTimestamp;
						DxxTools.UI.RemoveServerTimeClockCallback(key);
						DxxTools.UI.AddServerTimeCallback(key, new Action(this.DoEventBattle), serverTimestamp + 10L, 0);
						HLog.LogError("地牢战斗信息异常...");
					}
				});
			}
		}

		private void DoEventPoint()
		{
			GameEventStateRogueDungeon.<DoEventPoint>d__25 <DoEventPoint>d__;
			<DoEventPoint>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<DoEventPoint>d__.<>4__this = this;
			<DoEventPoint>d__.<>1__state = -1;
			<DoEventPoint>d__.<>t__builder.Start<GameEventStateRogueDungeon.<DoEventPoint>d__25>(ref <DoEventPoint>d__);
		}

		private void DoGoNextFloor()
		{
			EventMemberController.Instance.GoOutScreen(delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_NextFloorStart, null);
				DelayCall.Instance.CallOnce(1000, new DelayCall.CallAction(this.NextFloorAniFinish));
			});
		}

		private void NextFloorAniFinish()
		{
			if (this.isEscape)
			{
				return;
			}
			EventMemberController.Instance.ForceRemoveAllNpc();
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_RogueDungeon_NextFloorEnd, null);
			EventMemberController.Instance.GoInScreen(new Action(this.FloorSelectSkill));
		}

		[GameTestMethod("地牢", "进场", "", 503)]
		private static void OpenEnter()
		{
			EventMemberController.Instance.GoOutScreen(delegate
			{
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_RogueDungeon_NextFloorStart, null);
				DelayCall.Instance.CallOnce(1000, new DelayCall.CallAction(GameEventStateRogueDungeon.<OpenEnter>g__NextFloorAniFinish|28_1));
			});
		}

		[CompilerGenerated]
		private void <OnEndBattle>g__DoNext|15_1()
		{
			if (this.isLastBattle)
			{
				DelayCall.Instance.CallOnce(2000, new DelayCall.CallAction(this.DoEventPoint));
				return;
			}
			DelayCall.Instance.CallOnce(2000, delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIBattleRogueDungeon_Refresh, null);
				this.DoEventBattle();
			});
		}

		[CompilerGenerated]
		internal static void <OpenEnter>g__NextFloorAniFinish|28_1()
		{
			EventMemberController.Instance.ForceRemoveAllNpc();
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_RogueDungeon_NextFloorEnd, null);
			EventMemberController.Instance.GoInScreen(null);
		}

		private RogueDungeonDataModule mDataModule;

		private bool isLastBattle;

		private bool isDoEventPoint;

		private bool isShowSpecialChallenge;

		private bool isEscape;

		private bool isBattle;
	}
}
