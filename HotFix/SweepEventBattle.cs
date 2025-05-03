using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework;

namespace HotFix
{
	public class SweepEventBattle : SweepEventBase
	{
		public override void OnInit()
		{
			GameApp.Data.GetDataModule(DataName.ChapterSweepDataModule);
			GameEventDataBattle gameEventDataBattle = this.currentData as GameEventDataBattle;
			if (gameEventDataBattle != null)
			{
				List<NodeItemParam> battleDrop = gameEventDataBattle.GetBattleDrop();
				if (battleDrop.Count > 0)
				{
					this.dropItemList.AddRange(battleDrop);
				}
				List<NodeItemParam> monsterDrop = gameEventDataBattle.GetMonsterDrop();
				if (monsterDrop.Count > 0)
				{
					this.dropItemList.AddRange(monsterDrop);
				}
				if (this.dropItemList.Count > 0)
				{
					Singleton<GameEventController>.Instance.AddDrops(this.dropItemList);
				}
			}
			base.DelayShowUI();
		}

		public override void OnDeInit()
		{
		}

		public override async Task OnEvent(GameEventPushType pushType, object param)
		{
			if (pushType == GameEventPushType.UIAniFinish)
			{
				List<NodeParamBase> list = new List<NodeParamBase>();
				for (int i = 0; i < this.dropItemList.Count; i++)
				{
					list.Add(this.dropItemList[i]);
				}
				float num = 0f;
				if (list.Count > 0)
				{
					EventArgFlyItems eventArgFlyItems = new EventArgFlyItems();
					eventArgFlyItems.SetData(list);
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_FlyItems, eventArgFlyItems);
					num = 1f;
				}
				base.GoNext(num);
			}
		}

		protected override void ShowUI()
		{
			GameEventUIData gameEventUIData = GameEventUIDataCreator.Create(this.currentData.poolData.tableId, this.stage, this.currentData.IsRoot, "GameEventData_25", null, null, this.dropItemList, null, null, null);
			EventArgAddEvent eventArgAddEvent = new EventArgAddEvent();
			eventArgAddEvent.uiData = gameEventUIData;
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_AddEvent, eventArgAddEvent);
		}

		protected List<NodeItemParam> dropItemList = new List<NodeItemParam>();
	}
}
