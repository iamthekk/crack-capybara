using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterEventMiniGame : ChapterEventBase
	{
		public override void OnInit()
		{
			GameEventDataMiniGame gameEventDataMiniGame = this.currentData as GameEventDataMiniGame;
			if (gameEventDataMiniGame != null)
			{
				this.OpenMiniGameUI(gameEventDataMiniGame);
			}
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.CloseTurntable)
			{
				await base.TriggerFatherFunction(this.currentData);
				this.eventParam = param;
				this.attLanguageId = "turntable_result";
				this.defaultLanguageId = "turntable_result_noreward";
				this.ShowUI();
				base.MarkDone();
			}
			else if (pushType == GameEventPushType.CloseCardFlipping)
			{
				await base.TriggerFatherFunction(this.currentData);
				this.eventParam = param;
				this.attLanguageId = "card_flipping_result";
				this.defaultLanguageId = "card_flipping_result_noreward";
				this.ShowUI();
				base.MarkDone();
			}
			else if (pushType == GameEventPushType.CloseSlotMachine)
			{
				await base.TriggerFatherFunction(this.currentData);
				this.eventParam = param;
				this.attLanguageId = "slot_machine_result";
				this.defaultLanguageId = "slot_machine_result_noreward";
				this.ShowUI();
				base.MarkDone();
			}
			else if (pushType == GameEventPushType.ClosePaySlot)
			{
				await base.TriggerFatherFunction(this.currentData);
				this.eventParam = param;
				this.defaultLanguageId = "pay_slot_result";
				this.ShowUI();
				base.MarkDone();
			}
		}

		protected override void ShowUI()
		{
			ChapterActivityDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
			ChapterActivityRankData chapterActivityRankData = null;
			for (int i = 0; i < this.currentData.poolData.actRowIdArr.Length; i++)
			{
				ulong num = this.currentData.poolData.actRowIdArr[i];
				ChapterActivityData activityData = dataModule.GetActivityData(num);
				if (activityData != null && activityData.Kind == ChapterActivityKind.Rank && activityData.IsInProgress())
				{
					chapterActivityRankData = activityData as ChapterActivityRankData;
					break;
				}
			}
			int num2 = 0;
			List<GameEventMiniGameData> list = this.eventParam as List<GameEventMiniGameData>;
			if (list != null)
			{
				if (list.Count == 0)
				{
					this.ShowDefaultUI();
					return;
				}
				for (int j = 0; j < list.Count; j++)
				{
					GameEventMiniGameData gameEventMiniGameData = list[j];
					if (gameEventMiniGameData != null)
					{
						string text = null;
						object[] array = null;
						List<NodeScoreParam> list2 = new List<NodeScoreParam>();
						List<NodeParamBase> list3 = new List<NodeParamBase>();
						GameEventSkillBuildData gameEventSkillBuildData = gameEventMiniGameData.Reward as GameEventSkillBuildData;
						if (gameEventSkillBuildData != null)
						{
							text = "GameEventData_35";
							array = new object[] { gameEventSkillBuildData.skillName };
						}
						else
						{
							NodeAttParam nodeAttParam = gameEventMiniGameData.Reward as NodeAttParam;
							if (nodeAttParam != null)
							{
								text = this.attLanguageId;
								List<NodeAttParam> list4 = new List<NodeAttParam>();
								List<NodeAttParam> fatherFunctionAttr = this.currentData.GetFatherFunctionAttr();
								if (fatherFunctionAttr.Count > 0)
								{
									list4.AddRange(fatherFunctionAttr);
								}
								list4.Add(nodeAttParam);
								list3.Add(nodeAttParam);
								this.showAttList.AddRange(list4);
							}
							else
							{
								NodeItemParam nodeItemParam = gameEventMiniGameData.Reward as NodeItemParam;
								if (nodeItemParam != null)
								{
									text = this.attLanguageId;
									List<NodeItemParam> list5 = new List<NodeItemParam>();
									list5.Add(nodeItemParam);
									list3.Add(nodeItemParam);
									this.showItemList.AddRange(list5);
								}
							}
						}
						if (gameEventMiniGameData.TableId > 0 && gameEventMiniGameData.GameType == MiniGameType.Turntable)
						{
							ChapterMiniGame_turntableReward elementById = GameApp.Table.GetManager().GetChapterMiniGame_turntableRewardModelInstance().GetElementById(gameEventMiniGameData.TableId);
							if (elementById != null)
							{
								text = elementById.resultTextId;
							}
						}
						if (text == null)
						{
							text = this.defaultLanguageId;
						}
						int num3 = 0;
						if (chapterActivityRankData != null && gameEventMiniGameData.GameType == MiniGameType.MiniSlot && chapterActivityRankData.Config.parameter.Length != 0)
						{
							List<int> listInt = chapterActivityRankData.Config.parameter[0].GetListInt(',');
							if (gameEventMiniGameData.Result == MiniGameResult.GearTwo && listInt.Count >= 2)
							{
								num3 = listInt[0];
							}
							else if (gameEventMiniGameData.Result == MiniGameResult.GearThree && listInt.Count >= 2)
							{
								num3 = listInt[1];
							}
							if (num3 > 0)
							{
								NodeScoreParam nodeScoreParam = new NodeScoreParam(chapterActivityRankData.RowId, ChapterDropSource.TinySlot, num3);
								list2.Add(nodeScoreParam);
								list3.Add(nodeScoreParam);
							}
						}
						else if (chapterActivityRankData != null && gameEventMiniGameData.GameType == MiniGameType.CardFlipping && chapterActivityRankData.Config.parameter.Length > 1)
						{
							List<int> listInt2 = chapterActivityRankData.Config.parameter[1].GetListInt(',');
							switch (gameEventMiniGameData.Result)
							{
							case MiniGameResult.GearOne:
								num3 = ((listInt2.Count > 0) ? listInt2[0] : 0);
								break;
							case MiniGameResult.GearTwo:
								num3 = ((listInt2.Count > 1) ? listInt2[1] : 0);
								break;
							case MiniGameResult.GearThree:
								num3 = ((listInt2.Count > 2) ? listInt2[2] : 0);
								break;
							}
							if (num3 > 0)
							{
								NodeScoreParam nodeScoreParam2 = new NodeScoreParam(chapterActivityRankData.RowId, ChapterDropSource.CardFlipping, num3);
								list2.Add(nodeScoreParam2);
								list3.Add(nodeScoreParam2);
							}
						}
						num2 += num3;
						this.CalcTriggerFatherFunction();
						GameTGATools.Ins.AddStageClickTempScore(list2, false);
						GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, text, array, this.showAttList, this.showItemList, this.showSkillList, this.showInfoList, list2);
						EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
						eventArgAddEvent.uiData = gameEventUIData;
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
						if (list3.Count > 0)
						{
							EventArgFlyItems eventArgFlyItems = new EventArgFlyItems();
							eventArgFlyItems.SetData(list3);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_FlyItems, eventArgFlyItems);
						}
					}
				}
			}
			else
			{
				this.ShowDefaultUI();
			}
			if (chapterActivityRankData != null && num2 > 0)
			{
				this.isAsyncScore = true;
			}
		}

		private void ShowDefaultUI()
		{
			this.CalcTriggerFatherFunction();
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, this.defaultLanguageId, null, this.showAttList, this.showItemList, this.showSkillList, this.showInfoList, null);
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
		}

		protected void OpenMiniGameUI(GameEventDataMiniGame openMiniGame)
		{
			int randomSeed = this.currentData.poolData.randomSeed;
			int num = this.currentData.poolData.serverRate;
			if (randomSeed == 0)
			{
				HLog.LogError("Event Seed is 0");
			}
			if (num < 1)
			{
				num = 1;
			}
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_OpenMiniGameUI, null);
			bool flag = Singleton<GameEventController>.Instance.IsState(GameEventStateName.Sweep);
			switch (openMiniGame.miniGameType)
			{
			case MiniGameType.MiniSlot:
			{
				SlotMachineViewModule.OpenData openData = new SlotMachineViewModule.OpenData();
				openData.slotMachineId = openMiniGame.param;
				openData.randSeed = randomSeed;
				openData.rewardRate = num;
				openData.isInSweep = flag;
				openData.autoStartDelay = 3f;
				GameApp.View.OpenView(ViewName.SlotMachineViewModule, openData, 1, null, null);
				return;
			}
			case MiniGameType.CardFlipping:
			{
				CardFlippingViewModule.OpenData openData2 = new CardFlippingViewModule.OpenData();
				openData2.cardFlippingId = openMiniGame.param;
				openData2.randSeed = randomSeed;
				openData2.rewardRate = num;
				openData2.isInSweep = flag;
				openData2.autoStartDelay = 3f;
				GameApp.View.OpenView(ViewName.CardFlippingViewModule, openData2, 1, null, null);
				return;
			}
			case MiniGameType.Turntable:
			{
				FortuneWheelViewModule.OpenData openData3 = new FortuneWheelViewModule.OpenData();
				openData3.turntableId = openMiniGame.param;
				openData3.randSeed = randomSeed;
				openData3.rewardRate = num;
				GameApp.View.OpenView(ViewName.FortuneWheelViewModule, openData3, 1, null, null);
				return;
			}
			case MiniGameType.PaySlot:
			{
				PaySlotViewModule.OpenData openData4 = new PaySlotViewModule.OpenData();
				openData4.slotId = openMiniGame.param;
				openData4.seed = randomSeed;
				GameApp.View.OpenView(ViewName.PaySlotViewModule, openData4, 1, null, null);
				return;
			}
			default:
				return;
			}
		}

		private void CalcTriggerFatherFunction()
		{
			List<NodeAttParam> fatherFunctionAttr = this.currentData.GetFatherFunctionAttr();
			if (fatherFunctionAttr.Count > 0)
			{
				this.showAttList.AddRange(fatherFunctionAttr);
			}
			List<NodeItemParam> fatherFunctionItems = this.currentData.GetFatherFunctionItems();
			if (fatherFunctionItems.Count > 0)
			{
				this.showItemList.AddRange(fatherFunctionItems);
			}
			List<NodeSkillParam> fatherFunctionSkills = this.currentData.GetFatherFunctionSkills();
			if (fatherFunctionSkills.Count > 0)
			{
				this.showSkillList.AddRange(fatherFunctionSkills);
			}
			List<string> fatherFunctionInfos = this.currentData.GetFatherFunctionInfos();
			if (fatherFunctionInfos.Count > 0)
			{
				this.showInfoList.AddRange(fatherFunctionInfos);
			}
		}

		private object eventParam;

		private string attLanguageId;

		private string defaultLanguageId;

		protected bool isAsyncScore;

		private List<NodeAttParam> showAttList = new List<NodeAttParam>();

		private List<NodeItemParam> showItemList = new List<NodeItemParam>();

		private List<NodeSkillParam> showSkillList = new List<NodeSkillParam>();

		private List<string> showInfoList = new List<string>();
	}
}
