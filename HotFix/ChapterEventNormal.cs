using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ChapterEventNormal : ChapterEventBase
	{
		public bool IsMoveToNpcAutoClick { get; private set; }

		public override void OnInit()
		{
			this.CalcParams();
			this.GetItems();
			this.GetScore();
			this.CalcTriggerFatherFunction();
			this.ShowUI();
			this.isMergeAtt = false;
			this.isDoFunc = false;
			this.IsMoveToNpcAutoClick = false;
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.MoveToNpc)
			{
				this.isMoveToNpc = true;
			}
			else if (pushType == GameEventPushType.NpcArrived)
			{
				this.isMoveToNpc = false;
				this.IsMoveToNpcAutoClick = true;
				await this.OnClickButton(0);
				this.IsMoveToNpcAutoClick = false;
			}
			else if (pushType == GameEventPushType.UIAniFinish)
			{
				if (!this.isDoFunc)
				{
					this.isDoFunc = true;
					await base.DoFunctionImmediately(this.currentData);
					await base.TriggerFatherFunction(this.currentData);
				}
				if (!this.isMergeAtt)
				{
					this.isMergeAtt = true;
					base.MergerAttribute(this.calcParamList);
					List<NodeParamBase> list = new List<NodeParamBase>();
					for (int i = 0; i < this.showAttList.Count; i++)
					{
						if (this.showAttList[i].attType == GameEventAttType.Chips)
						{
							list.Add(this.showAttList[i]);
						}
					}
					for (int j = 0; j < this.showItemList.Count; j++)
					{
						list.Add(this.showItemList[j]);
					}
					for (int k = 0; k < this.scoreList.Count; k++)
					{
						list.Add(this.scoreList[k]);
					}
					if (list.Count > 0)
					{
						EventArgFlyItems eventArgFlyItems = new EventArgFlyItems();
						eventArgFlyItems.SetData(list);
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_FlyItems, eventArgFlyItems);
					}
				}
				if (!this.isMoveToNpc)
				{
					if (!base.isDone)
					{
						GameTGATools.Ins.AddStageClickTempAtt(this.showAttList, true);
						GameTGATools.Ins.AddStageClickTempItem(this.showItemList, true);
						GameTGATools.Ins.AddStageClickTempScore(this.scoreList, true);
					}
					base.MarkDone();
				}
			}
		}

		protected override void ShowUI()
		{
			GameEventDataNormal gameEventDataNormal = this.currentData as GameEventDataNormal;
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, gameEventDataNormal.contentId, null, this.showAttList, this.showItemList, this.showSkillList, this.showInfoList, this.scoreList);
			gameEventUIData.SetTipInfoId("UIGameEvent_115");
			gameEventUIData.SetInfo(gameEventDataNormal.titleId, gameEventDataNormal.summaryId);
			List<GameEventDataSelect> buttonDatas = base.GetButtonDatas();
			for (int i = 0; i < buttonDatas.Count; i++)
			{
				GameEventDataSelect gameEventDataSelect = buttonDatas[i];
				gameEventUIData.AddButton(i, gameEventDataSelect, gameEventDataSelect.IsUndoFunction(), gameEventDataSelect.GetUndoTip());
			}
			if (buttonDatas.Count == 1)
			{
				gameEventUIData.SetTipInfoId("GameEventData_129");
			}
			else if (buttonDatas.Count == 2)
			{
				gameEventUIData.SetTipInfoId("GameEventData_131");
			}
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
		}

		private void CalcParams()
		{
			List<NodeAttParam> paramList = (this.currentData as GameEventDataNormal).GetParamList();
			for (int i = 0; i < paramList.Count; i++)
			{
				NodeAttParam nodeAttParam = new NodeAttParam(paramList[i].attType, paramList[i].baseCount, ChapterDropSource.Event, this.currentData.poolData.serverRate);
				this.calcParamList.Add(nodeAttParam);
			}
			this.showAttList.AddRange(this.calcParamList);
		}

		private void GetItems()
		{
			GameEventDataNormal gameEventDataNormal = this.currentData as GameEventDataNormal;
			if (gameEventDataNormal != null)
			{
				List<NodeItemParam> itemList = gameEventDataNormal.GetItemList();
				for (int i = 0; i < itemList.Count; i++)
				{
					if (itemList[i].type == NodeItemType.ChapterEvent)
					{
						Singleton<GameEventController>.Instance.AddEventItem(itemList[i].itemId, (int)itemList[i].FinalCount, this.stage);
						Chapter_eventItem elementById = GameApp.Table.GetManager().GetChapter_eventItemModelInstance().GetElementById(itemList[i].itemId);
						if (elementById != null && elementById.showUI > 0)
						{
							this.showItemList.Add(itemList[i]);
						}
					}
				}
				if (gameEventDataNormal.isServerDrop)
				{
					List<NodeItemParam> serverDrop = gameEventDataNormal.GetServerDrop();
					if (serverDrop.Count > 0)
					{
						Singleton<GameEventController>.Instance.AddDrops(serverDrop);
						this.showItemList.AddRange(serverDrop);
					}
				}
			}
		}

		private void GetScore()
		{
			GameEventDataNormal gameEventDataNormal = this.currentData as GameEventDataNormal;
			if (gameEventDataNormal != null && gameEventDataNormal.poolData.IsActivity && gameEventDataNormal.isServerDrop)
			{
				Chapter_eventRes elementById = GameApp.Table.GetManager().GetChapter_eventResModelInstance().GetElementById(gameEventDataNormal.poolData.tableId);
				ChapterActivityDataModule dataModule = GameApp.Data.GetDataModule(DataName.ChapterActivityDataModule);
				ChapterBattlePassDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule);
				ChapterActivityWheelDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.ChapterActivityWheelDataModule);
				if (elementById != null)
				{
					int activityReward = elementById.activityReward;
					for (int i = 0; i < gameEventDataNormal.poolData.actRowIdArr.Length; i++)
					{
						ulong num = gameEventDataNormal.poolData.actRowIdArr[i];
						ChapterActivityData activityData = dataModule.GetActivityData(num);
						if (activityData != null && activityData.IsInProgress())
						{
							NodeScoreParam nodeScoreParam = new NodeScoreParam(num, ChapterDropSource.Event, activityReward);
							this.scoreList.Add(nodeScoreParam);
						}
						if (dataModule2.IsInProgress() && num == (ulong)dataModule2.BattlePassDto.RowId)
						{
							NodeScoreParam nodeScoreParam2 = new NodeScoreParam(num, ChapterDropSource.Event, activityReward);
							this.scoreList.Add(nodeScoreParam2);
						}
						if (dataModule3.IsActivityOpen((long)num))
						{
							NodeScoreParam nodeScoreParam3 = new NodeScoreParam(num, ChapterDropSource.Event, elementById.wheelReward);
							this.scoreList.Add(nodeScoreParam3);
						}
					}
				}
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

		private List<NodeAttParam> calcParamList = new List<NodeAttParam>();

		private List<NodeAttParam> showAttList = new List<NodeAttParam>();

		private List<NodeItemParam> showItemList = new List<NodeItemParam>();

		private List<NodeSkillParam> showSkillList = new List<NodeSkillParam>();

		private List<string> showInfoList = new List<string>();

		private List<NodeScoreParam> scoreList = new List<NodeScoreParam>();

		private bool isMergeAtt;

		private bool isDoFunc;
	}
}
