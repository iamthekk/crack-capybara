using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.SevenDayTask;

namespace HotFix
{
	public class ActivitySlotTrainDataModule : IDataModule
	{
		public int TurntableId { get; private set; }

		public uint Count { get; private set; }

		public int BigGuaranteeItemConfigId { get; private set; }

		public uint BigGuaranteeItemNum { get; private set; }

		public MapField<int, int> SmallGuaranteeCount { get; private set; }

		public RepeatedField<int> TurntableRewardIds { get; private set; }

		public RepeatedField<DropItemDto> DropItemDtos { get; private set; }

		public RepeatedField<RewardDto> RewardItems { get; private set; }

		public int GetLimitLeftCount { get; private set; }

		public int BigGuaranteeCount { get; private set; }

		public MapField<int, int> TaskData { get; private set; }

		public RepeatedField<int> FinishedTaskId { get; private set; }

		public MapField<int, int> TurntablePayCount { get; private set; }

		public bool Inited { get; private set; }

		public long EndTime { get; private set; }

		public long LeftTime
		{
			get
			{
				long num = this.EndTime - DxxTools.Time.ServerTimestamp;
				if (num >= 0L)
				{
					return num;
				}
				return 0L;
			}
		}

		public MapField<int, int> LastSmallGuaranteeCount { get; private set; } = new MapField<int, int>();

		public int GetName()
		{
			return 139;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_ActivitySlotTrain_DataPull, new HandlerEvent(this.OnEvent_DayChangeDataPull));
			manager.RegisterEvent(LocalMessageName.CC_ActivitySlotTrain, new HandlerEvent(this.OnEventSetActivitySlotTrainData));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableExtract, new HandlerEvent(this.OnEventSetTurnTableExtractData));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableReceiveCumulativeReward, new HandlerEvent(this.OnEventSetTurnTableReceiveCumulativeRewardData));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableTaskReceiveReward, new HandlerEvent(this.OnEventSetTurnTableTaskReceiveRewardData));
			manager.RegisterEvent(LocalMessageName.CC_TurnTableSelectBigGuaranteeItem, new HandlerEvent(this.OnEventSetTurnTableSelectBigGuaranteeItemData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_ActivitySlotTrain_DataPull, new HandlerEvent(this.OnEvent_DayChangeDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_ActivitySlotTrain, new HandlerEvent(this.OnEventSetActivitySlotTrainData));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableExtract, new HandlerEvent(this.OnEventSetTurnTableExtractData));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableReceiveCumulativeReward, new HandlerEvent(this.OnEventSetTurnTableReceiveCumulativeRewardData));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableTaskReceiveReward, new HandlerEvent(this.OnEventSetTurnTableTaskReceiveRewardData));
			manager.UnRegisterEvent(LocalMessageName.CC_TurnTableSelectBigGuaranteeItem, new HandlerEvent(this.OnEventSetTurnTableSelectBigGuaranteeItemData));
		}

		public void Reset()
		{
			this.Clear();
		}

		public void Clear()
		{
			if (this.Inited)
			{
				this.Inited = false;
			}
		}

		private void OnEvent_DayChangeDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.Login.OnLoginGetActivitySlotTrainInfo(null);
		}

		private void OnEventSetActivitySlotTrainData(object sender, int type, BaseEventArgs eventargs)
		{
			this.netGetting = false;
			EventArgsActivitySlotTrainData eventArgsActivitySlotTrainData = eventargs as EventArgsActivitySlotTrainData;
			if (eventArgsActivitySlotTrainData == null)
			{
				GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
				return;
			}
			this.TurntableId = eventArgsActivitySlotTrainData.Response.TurntableId;
			this.Count = eventArgsActivitySlotTrainData.Response.Count;
			this.BigGuaranteeItemConfigId = eventArgsActivitySlotTrainData.Response.BigGuaranteeItemConfigId;
			this.BigGuaranteeItemNum = eventArgsActivitySlotTrainData.Response.BigGuaranteeItemNum;
			this.SmallGuaranteeCount = eventArgsActivitySlotTrainData.Response.SmallGuaranteeCount;
			this.TurntableRewardIds = eventArgsActivitySlotTrainData.Response.TurntableRewardIds;
			this.TaskData = eventArgsActivitySlotTrainData.Response.TaskData;
			this.FinishedTaskId = eventArgsActivitySlotTrainData.Response.FinishedTaskId;
			this.GetLimitLeftCount = eventArgsActivitySlotTrainData.Response.BigRewardCount;
			this.BigGuaranteeCount = eventArgsActivitySlotTrainData.Response.BigGuaranteeCount;
			if (eventArgsActivitySlotTrainData.Response.ActivityTimeLeft > 0L)
			{
				this.EndTime = DxxTools.Time.ServerTimestamp + eventArgsActivitySlotTrainData.Response.ActivityTimeLeft / 1000L;
			}
			else
			{
				this.EndTime = DxxTools.Time.ServerTimestamp;
			}
			this.Inited = this.TurntableId > 0 && this.EndTime > DxxTools.Time.ServerTimestamp;
			RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			if (this.TurntableId > 0 && !this.IsTimeOut())
			{
				GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
				GlobalUpdater.Instance.RegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
				return;
			}
			GlobalUpdater.Instance.UnRegisterUpdater(new Action(this.OnUpdateCheckRemainTimes));
		}

		private void OnEventSetTurnTableExtractData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTurnTableExtractData eventArgsTurnTableExtractData = eventargs as EventArgsTurnTableExtractData;
			if (eventArgsTurnTableExtractData == null)
			{
				return;
			}
			this.DropItemDtos = eventArgsTurnTableExtractData.Response.DropItemDtos;
			this.RewardItems = eventArgsTurnTableExtractData.Response.CommonData.Reward;
			this.Count = eventArgsTurnTableExtractData.Response.Count;
			this.SmallGuaranteeCount = eventArgsTurnTableExtractData.Response.SmallGuaranteeCount;
			this.GetLimitLeftCount = eventArgsTurnTableExtractData.Response.BigRewardCount;
			this.BigGuaranteeCount = eventArgsTurnTableExtractData.Response.BigGuaranteeCount;
			if (this.Inited)
			{
				RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			}
		}

		private void OnEventSetTurnTableReceiveCumulativeRewardData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTurnTableReceiveCumulativeRewardData eventArgsTurnTableReceiveCumulativeRewardData = eventargs as EventArgsTurnTableReceiveCumulativeRewardData;
			if (eventArgsTurnTableReceiveCumulativeRewardData == null)
			{
				return;
			}
			this.TurntableRewardIds = eventArgsTurnTableReceiveCumulativeRewardData.Response.TurntableRewardIds;
			if (this.Inited)
			{
				RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			}
		}

		private void OnEventSetTurnTableTaskReceiveRewardData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTurnTableTaskReceiveRewardData eventArgsTurnTableTaskReceiveRewardData = eventargs as EventArgsTurnTableTaskReceiveRewardData;
			if (eventArgsTurnTableTaskReceiveRewardData == null)
			{
				return;
			}
			this.TaskData = eventArgsTurnTableTaskReceiveRewardData.Response.TaskData;
			this.FinishedTaskId = eventArgsTurnTableTaskReceiveRewardData.Response.FinishedTaskId;
			if (this.Inited)
			{
				GameApp.Event.DispatchNow(null, 261, null);
				RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			}
		}

		private void OnEventSetTurnTableSelectBigGuaranteeItemData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsTurnTableSelectBigGuaranteeItemData eventArgsTurnTableSelectBigGuaranteeItemData = eventargs as EventArgsTurnTableSelectBigGuaranteeItemData;
			if (eventArgsTurnTableSelectBigGuaranteeItemData == null)
			{
				return;
			}
			this.BigGuaranteeItemConfigId = eventArgsTurnTableSelectBigGuaranteeItemData.Response.ItemId;
			this.BigGuaranteeItemNum = eventArgsTurnTableSelectBigGuaranteeItemData.Response.ItemNum;
		}

		public void CommonUpdateTurnTableTaskDto(RepeatedField<TurnTableTaskDto> taskData)
		{
			if (taskData == null)
			{
				return;
			}
			if (this.TaskData == null)
			{
				this.TaskData = new MapField<int, int>();
			}
			foreach (TurnTableTaskDto turnTableTaskDto in taskData)
			{
				this.TaskData[(int)turnTableTaskDto.Id] = (int)turnTableTaskDto.Process;
			}
			if (this.Inited)
			{
				GameApp.Event.DispatchNow(null, 261, null);
				RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			}
		}

		public void CommonUpdateTurntablePayCount(MapField<int, int> turntablePayCount)
		{
			if (turntablePayCount == null)
			{
				return;
			}
			this.TurntablePayCount = turntablePayCount;
			if (this.Inited)
			{
				GameApp.Event.DispatchNow(null, 262, null);
				RedPointController.Instance.ReCalc("Main.Activity_Slot", true);
			}
		}

		public bool IsTimeOut()
		{
			return this.EndTime <= 0L || DxxTools.Time.ServerTimestamp >= this.EndTime;
		}

		public void SaveLastSmallGuaranteeCount()
		{
			if (this.SmallGuaranteeCount != null)
			{
				foreach (KeyValuePair<int, int> keyValuePair in this.SmallGuaranteeCount)
				{
					this.LastSmallGuaranteeCount[keyValuePair.Key] = keyValuePair.Value;
				}
			}
		}

		public void ResetLastSmallGuaranteeCount(int count1, int count2)
		{
			this.LastSmallGuaranteeCount[1] = count1;
			this.LastSmallGuaranteeCount[14] = count2;
		}

		public bool PickedTurntableReward(ActivityTurntable_TurntableReward cfg)
		{
			return this.TurntableRewardIds != null && this.TurntableRewardIds.Contains(cfg.id);
		}

		public bool CanPickTurntableReward(ActivityTurntable_TurntableReward cfg)
		{
			return (ulong)this.Count >= (ulong)((long)cfg.point) && !this.PickedTurntableReward(cfg);
		}

		public int GetTaskScore(ActivityTurntable_TurntableQuest cfg)
		{
			int num = 0;
			if (this.IsFinishedTask(cfg))
			{
				num = cfg.Need;
			}
			else if (this.TaskData != null && this.TaskData.ContainsKey(cfg.ID))
			{
				num = this.TaskData[cfg.ID];
			}
			return num;
		}

		public bool IsFinishedTask(ActivityTurntable_TurntableQuest cfg)
		{
			return this.FinishedTaskId != null && this.FinishedTaskId.Contains(cfg.ID);
		}

		public bool CanPickTask(ActivityTurntable_TurntableQuest cfg)
		{
			return this.GetTaskScore(cfg) >= cfg.Need && !this.IsFinishedTask(cfg);
		}

		public int GetPayCount(ActivityTurntable_TurntablePay cfg)
		{
			if (this.TurntablePayCount != null && this.TurntablePayCount.ContainsKey(cfg.id))
			{
				return this.TurntablePayCount[cfg.id];
			}
			return 0;
		}

		public bool IsFinishedPay(ActivityTurntable_TurntablePay cfg)
		{
			return cfg.objToplimit > 0 && this.GetPayCount(cfg) >= cfg.objToplimit;
		}

		public bool CanFreePay(ActivityTurntable_TurntablePay cfg)
		{
			if (this.IsFinishedPay(cfg))
			{
				return false;
			}
			if (cfg.AdId > 0)
			{
				return GameApp.Data.GetDataModule(DataName.AdDataModule).CheckCloudDataAdOpen();
			}
			return GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(cfg.PurchaseId)
				.price1 <= 0f;
		}

		public bool ShowAnyRed()
		{
			return this.CanShow() && (this.ShowRedByTurntableReward() || this.ShowRedByQuest() || this.ShowRedByPay());
		}

		private bool ShowRedByTurntableReward()
		{
			if (!this.CanShow())
			{
				return false;
			}
			IList<ActivityTurntable_TurntableReward> activityTurntable_TurntableRewardElements = GameApp.Table.GetManager().GetActivityTurntable_TurntableRewardElements();
			for (int i = 0; i < activityTurntable_TurntableRewardElements.Count; i++)
			{
				if (this.CanPickTurntableReward(activityTurntable_TurntableRewardElements[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool ShowRedByQuest()
		{
			if (!this.CanShow())
			{
				return false;
			}
			IList<ActivityTurntable_TurntableQuest> activityTurntable_TurntableQuestElements = GameApp.Table.GetManager().GetActivityTurntable_TurntableQuestElements();
			for (int i = 0; i < activityTurntable_TurntableQuestElements.Count; i++)
			{
				if (this.CanPickTask(activityTurntable_TurntableQuestElements[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool ShowRedByPay()
		{
			if (!this.CanShow())
			{
				return false;
			}
			IList<ActivityTurntable_TurntablePay> activityTurntable_TurntablePayElements = GameApp.Table.GetManager().GetActivityTurntable_TurntablePayElements();
			for (int i = 0; i < activityTurntable_TurntablePayElements.Count; i++)
			{
				if (this.CanFreePay(activityTurntable_TurntablePayElements[i]))
				{
					return true;
				}
			}
			return false;
		}

		public bool CanShow()
		{
			return this.IsOpen() && this.TurntableId > 0 && !this.IsTimeOut();
		}

		private bool IsOpen()
		{
			return Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.Activity_SlotTrain, false);
		}

		private void OnUpdateCheckRemainTimes()
		{
			if (!this.netGetting && this.TurntableId > 0 && this.IsTimeOut())
			{
				this.netGetting = true;
				NetworkUtils.Login.OnLoginGetActivitySlotTrainInfo(delegate(int i, int i1)
				{
					this.netGetting = false;
				});
			}
		}

		public bool SkipAnimation;

		private bool netGetting;
	}
}
