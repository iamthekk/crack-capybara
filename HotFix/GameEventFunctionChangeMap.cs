using System;
using System.Threading.Tasks;
using Framework;
using Framework.Platfrom;
using UnityEngine;

namespace HotFix
{
	public class GameEventFunctionChangeMap : GameEventFunctionBase
	{
		public GameEventFunctionChangeMap(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 100000;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			int mapId;
			if (int.TryParse(this.functionData.functionParam, out mapId))
			{
				EventArgChangeMapPause eventArgChangeMapPause = new EventArgChangeMapPause();
				eventArgChangeMapPause.SetData(true, mapId);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_NewScene_Pause, eventArgChangeMapPause);
				Action <>9__1;
				GameApp.View.OpenView(ViewName.LoadingMapViewModule, null, 2, null, delegate(GameObject obj)
				{
					LoadingViewModule viewModule = GameApp.View.GetViewModule(ViewName.LoadingMapViewModule);
					Action action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate
						{
							EventArgChangeMap instance = Singleton<EventArgChangeMap>.Instance;
							instance.SetData(mapId);
							GameApp.Event.DispatchNow(this, LocalMessageName.CC_UIGameEvent_ChangeMap, instance);
						});
					}
					viewModule.PlayShow(action);
				});
			}
			await TaskExpand.Delay(2000);
			this.MarkDone();
		}
	}
}
