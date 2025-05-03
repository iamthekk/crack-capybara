using System;
using System.Threading.Tasks;
using Framework;
using HotFix.Client;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace HotFix
{
	public class GameEventFunctionAddEventPoint : GameEventFunctionBase
	{
		public GameEventFunctionAddEventPoint(GameEventDataFunction data)
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
			if (int.TryParse(this.functionData.functionParam, out num))
			{
				Chapter_eventPoint table = GameApp.Table.GetManager().GetChapter_eventPointModelInstance().GetElementById(num);
				if (table == null)
				{
					HLog.LogError(string.Format("未找到EventPoint，id={0}", num));
				}
				else
				{
					AsyncOperationHandle<GameObject> eventPoint = GameApp.Resources.LoadAssetAsync<GameObject>(table.path);
					await eventPoint.Task;
					EventMemberController.Instance.CreateEventPoint(eventPoint.Result, table);
					this.MarkDone();
					table = null;
					eventPoint = default(AsyncOperationHandle<GameObject>);
				}
			}
		}
	}
}
