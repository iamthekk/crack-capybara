using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class SweepEventNormal : SweepEventBase
	{
		public override void OnInit()
		{
			this.GetItems();
			this.GetScore();
			this.ShowUI();
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.UIAniFinish)
			{
				List<NodeParamBase> list = new List<NodeParamBase>();
				for (int i = 0; i < this.showItemList.Count; i++)
				{
					list.Add(this.showItemList[i]);
				}
				for (int j = 0; j < this.scoreList.Count; j++)
				{
					list.Add(this.scoreList[j]);
				}
				if (list.Count > 0)
				{
					EventArgFlyItems eventArgFlyItems = new EventArgFlyItems();
					eventArgFlyItems.SetData(list);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_FlyItems, eventArgFlyItems);
					this.delayEx = 1f;
				}
			}
		}

		protected override void CheckFinish()
		{
			base.GoNext(this.delayEx);
		}

		public override void ResumeHangUp()
		{
			base.ResumeHangUp();
			this.OnClickButton(0);
		}

		protected override void ShowUI()
		{
			GameEventDataNormal gameEventDataNormal = this.currentData as GameEventDataNormal;
			GameTGATools.Ins.AddStageClickTempScore(this.scoreList, false);
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, gameEventDataNormal.contentId, null, null, this.showItemList, null, null, this.scoreList);
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

		private void GetItems()
		{
			GameEventDataNormal gameEventDataNormal = this.currentData as GameEventDataNormal;
			if (gameEventDataNormal != null && gameEventDataNormal.isServerDrop)
			{
				List<NodeItemParam> serverDrop = gameEventDataNormal.GetServerDrop();
				if (serverDrop.Count > 0)
				{
					Singleton<GameEventController>.Instance.AddDrops(serverDrop);
					this.showItemList.AddRange(serverDrop);
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

		private List<NodeItemParam> showItemList = new List<NodeItemParam>();

		private List<NodeScoreParam> scoreList = new List<NodeScoreParam>();

		private float delayEx;
	}
}
