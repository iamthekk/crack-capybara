using System;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class GameEventFunctionFixSurprise : GameEventFunctionBase
	{
		public GameEventFunctionFixSurprise(GameEventDataFunction data)
			: base(data)
		{
		}

		public override GameEventFunctionBase.FunctionDoType GetDoType()
		{
			return GameEventFunctionBase.FunctionDoType.Immediately;
		}

		public override int GetDoOrder()
		{
			return 1;
		}

		public override void Create()
		{
		}

		public override async Task DoFunction()
		{
			int num;
			if (int.TryParse(this.functionData.functionParam, out num) && num > 0)
			{
				Chapter_surpriseBuild elementById = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetElementById(num);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Not found Chapter_surpriseBuild table, id ={0}", num));
					return;
				}
				Singleton<GameEventController>.Instance.SetCurrentSurpriseId(num);
				Chapter_eventPoint table = GameApp.Table.GetManager().GetChapter_eventPointModelInstance().GetElementById(elementById.eventPointId);
				if (table == null)
				{
					HLog.LogError(string.Format("未找到EventPoint，id={0}", elementById.eventPointId));
					return;
				}
				AsyncOperationHandle<GameObject> eventPoint = GameApp.Resources.LoadAssetAsync<GameObject>(table.path);
				await eventPoint.Task;
				EventMemberController.Instance.CreateEventPoint(eventPoint.Result, table);
				table = null;
				eventPoint = default(AsyncOperationHandle<GameObject>);
			}
			this.MarkDone();
		}
	}
}
