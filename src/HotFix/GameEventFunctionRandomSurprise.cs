using System;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class GameEventFunctionRandomSurprise : GameEventFunctionBase
	{
		public GameEventFunctionRandomSurprise(GameEventDataFunction data)
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
				int randomSurprise = Singleton<GameEventController>.Instance.GetRandomSurprise(num, this.functionData.poolData.randomSeed);
				Chapter_surpriseBuild elementById = GameApp.Table.GetManager().GetChapter_surpriseBuildModelInstance().GetElementById(randomSurprise);
				if (elementById == null)
				{
					HLog.LogError(string.Format("Not found Chapter_surpriseBuild table, id ={0}", randomSurprise));
					return;
				}
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
