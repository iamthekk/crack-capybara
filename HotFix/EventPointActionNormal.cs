using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace HotFix
{
	public class EventPointActionNormal : EventPointBase
	{
		private protected bool IsArrived { protected get; private set; }

		public override void SetData(Chapter_eventPoint table)
		{
			base.SetData(table);
			EventPointActionUtility.ParseDefaultActionData(table, out this.defaultActionData);
			EventPointActionUtility.ParseActionData(table, out this.doActionDataList);
		}

		protected sealed override async void OnInit()
		{
			base.OnInit();
			await base.Load();
			this.IsArrived = false;
			this.IsLeaved = false;
			this.InitPoint();
			float num;
			this.OnAction(this.defaultActionData.createActionId, out num);
		}

		protected sealed override void OnDeInit()
		{
			base.OnDeInit();
			this.sequencePool.Clear(false);
			this.DeInitPoint();
		}

		protected virtual void InitPoint()
		{
		}

		protected virtual void DeInitPoint()
		{
		}

		public void OnArrived()
		{
			this.IsArrived = true;
			float num;
			this.OnAction(this.defaultActionData.arrivedActionId, out num);
		}

		public void OnLeave()
		{
			if (!this.IsArrived)
			{
				return;
			}
			this.IsLeaved = true;
			float num;
			this.OnAction(this.defaultActionData.leaveActionId, out num);
		}

		public void OnAction(int actionId, out float duration)
		{
			duration = 0f;
			EventPointActionData eventPointActionData = this.doActionDataList.Find((EventPointActionData act) => act.actionId == actionId);
			if (eventPointActionData == null || eventPointActionData.actionId < 0)
			{
				return;
			}
			duration = eventPointActionData.duration;
			this.DoAction(actionId);
		}

		protected virtual void DoAction(int actionId)
		{
		}

		protected EventPointDefaultActionData defaultActionData;

		protected List<EventPointActionData> doActionDataList;

		protected EventPointPlayerArriveActionData playerArriveActionData;

		protected bool IsLeaved;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
