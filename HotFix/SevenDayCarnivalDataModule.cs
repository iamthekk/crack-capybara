using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.SevenDayTask;

namespace HotFix
{
	public class SevenDayCarnivalDataModule : IDataModule
	{
		public void SetConnectFailed()
		{
			this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.ConnectFailed);
		}

		public SevenDayCarnivalDataModule.CarnivalMgrState CurState
		{
			get
			{
				return this._curState;
			}
		}

		public List<ActiveOneItem> ActiveOneItems
		{
			get
			{
				return this._activeItems;
			}
		}

		public int ActivePower
		{
			get
			{
				return this._activePower;
			}
		}

		public int UnLockDay
		{
			get
			{
				return this._unLockDay;
			}
		}

		public long TaskEndTimeStamp
		{
			get
			{
				return this._taskEndTimeStamp;
			}
		}

		public long EndTimeStamp
		{
			get
			{
				return this._endTimeStamp;
			}
		}

		public int GetName()
		{
			return 141;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DayChange_SevenDayCarnival_DataPull, new HandlerEvent(this.OnEventDayChangeSevenDayCarnivalDataPull));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DayChange_SevenDayCarnival_DataPull, new HandlerEvent(this.OnEventDayChangeSevenDayCarnivalDataPull));
		}

		public void Reset()
		{
		}

		private void OnEventDayChangeSevenDayCarnivalDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			this.RequestCarnivalGetInfo(delegate
			{
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_SevenDayCarnival_Refresh, null);
			}, null);
		}

		public bool IfTimeOut
		{
			get
			{
				if (this._curState == SevenDayCarnivalDataModule.CarnivalMgrState.Off)
				{
					return true;
				}
				if (this.EndTimeStamp == 0L || this._hasTimeOut)
				{
					return true;
				}
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				if (this.EndTimeStamp <= serverTimestamp)
				{
					this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.Off);
					return true;
				}
				return false;
			}
		}

		public bool IfTaskTimeOut
		{
			get
			{
				if (this._curState == SevenDayCarnivalDataModule.CarnivalMgrState.Off)
				{
					return true;
				}
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				return this.TaskEndTimeStamp <= serverTimestamp;
			}
		}

		public bool IfActiveStageGot(int id)
		{
			return (this._activeLog & (1 << id)) > 0;
		}

		public bool IfCarnivalOpen
		{
			get
			{
				switch (this._curState)
				{
				case SevenDayCarnivalDataModule.CarnivalMgrState.On:
					return true;
				case SevenDayCarnivalDataModule.CarnivalMgrState.Off:
					return false;
				case SevenDayCarnivalDataModule.CarnivalMgrState.OnConnecting:
				case SevenDayCarnivalDataModule.CarnivalMgrState.ConnectFailed:
				{
					long serverTimestamp = DxxTools.Time.ServerTimestamp;
					if (this.EndTimeStamp > serverTimestamp)
					{
						return true;
					}
					break;
				}
				}
				return false;
			}
		}

		public bool IfCanShowRedDot()
		{
			if (!this.IfCarnivalOpen)
			{
				return false;
			}
			bool flag = false;
			foreach (KeyValuePair<int, OneDayCarnivalTaskData> keyValuePair in this.TaskDic)
			{
				if (keyValuePair.Key > this._unLockDay)
				{
					break;
				}
				if (keyValuePair.Value.FinishCount > 0)
				{
					flag = true;
				}
			}
			flag |= this.IfAnyFreePay();
			flag |= this.IfActiveRewardCanGet();
			return flag || this.IfCanShowRedNode;
		}

		public bool IfAnyFreePay()
		{
			for (int i = 1; i <= this.UnLockDay; i++)
			{
				if (SevenDayCarnivalDataModule.IsTaskOneShowRed(this, SevenDayCarnivalDataModule.ViewType.Pay, i))
				{
					return true;
				}
			}
			return false;
		}

		public static uint GetTaskOneBuyCount(SevenDayCarnivalDataModule dataModule, IAPDataModule iapDataModule, SevenDay_SevenDayPay cfg)
		{
			uint num;
			if (!dataModule.FreeRewardRecord.TryGetValue((uint)cfg.id, ref num))
			{
				if (cfg.PurchaseId > 0)
				{
					PurchaseCommonData.PurchaseData purchaseDataByID = iapDataModule.Common.GetPurchaseDataByID(cfg.PurchaseId);
					if (purchaseDataByID != null)
					{
						num = purchaseDataByID.m_tolBuyCount;
					}
					else
					{
						num = 0U;
					}
				}
				else
				{
					num = 0U;
				}
			}
			return num;
		}

		public static float GetTaskOneBuyPrice(SevenDay_SevenDayPay cfg)
		{
			float num = 0f;
			if (cfg.PurchaseId > 0)
			{
				IAP_Purchase elementById = GameApp.Table.GetManager().GetIAP_PurchaseModelInstance().GetElementById(cfg.PurchaseId);
				if (elementById != null)
				{
					num = elementById.price1;
				}
			}
			return num;
		}

		public static bool IsTaskOneShowRed(SevenDayCarnivalDataModule dataModule, SevenDayCarnivalDataModule.ViewType viewType, int day)
		{
			if (day > dataModule.UnLockDay)
			{
				return false;
			}
			if (viewType == SevenDayCarnivalDataModule.ViewType.Reward)
			{
				OneDayCarnivalTaskData oneDayCarnivalTaskData;
				if (dataModule.TaskDic.TryGetValue(day, out oneDayCarnivalTaskData))
				{
					return oneDayCarnivalTaskData.FinishCount > 0;
				}
			}
			else if (viewType == SevenDayCarnivalDataModule.ViewType.Pay)
			{
				IList<SevenDay_SevenDayPay> allElements = GameApp.Table.GetManager().GetSevenDay_SevenDayPayModelInstance().GetAllElements();
				IAPDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.IAPDataModule);
				for (int i = 0; i < allElements.Count; i++)
				{
					SevenDay_SevenDayPay sevenDay_SevenDayPay = allElements[i];
					if (sevenDay_SevenDayPay.Day == day && SevenDayCarnivalDataModule.GetTaskOneBuyPrice(sevenDay_SevenDayPay) <= 0f)
					{
						if (sevenDay_SevenDayPay.objToplimit <= 0)
						{
							return true;
						}
						if ((ulong)SevenDayCarnivalDataModule.GetTaskOneBuyCount(dataModule, dataModule2, sevenDay_SevenDayPay) < (ulong)((long)sevenDay_SevenDayPay.objToplimit))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		public bool IfActiveRewardCanGet()
		{
			int num = 0;
			for (int i = 0; i < this._activeItems.Count; i++)
			{
				if (!this.IfActiveStageGot(i + 1))
				{
					num = i;
					break;
				}
				if (i == this._activeItems.Count - 1)
				{
					return false;
				}
			}
			return num < this._activeItems.Count && this._activeItems[num].ActiveNeed <= this._activePower;
		}

		public bool IFGetCarnivalSeverData()
		{
			return this.TaskEndTimeStamp > 0L;
		}

		public void OnInit()
		{
			this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.Off);
			this.InitActiveItemData();
			this.RefreshCarnivalGetInfo();
		}

		private void InitActiveItemData()
		{
			if (this._activeItems.Count > 0)
			{
				return;
			}
			foreach (SevenDay_SevenDayActiveReward sevenDay_SevenDayActiveReward in GameApp.Table.GetManager().GetSevenDay_SevenDayActiveRewardModelInstance().GetAllElements())
			{
				SevenDay_SevenDayActiveReward sevenDay_SevenDayActiveReward2 = sevenDay_SevenDayActiveReward;
				if (sevenDay_SevenDayActiveReward2 != null && sevenDay_SevenDayActiveReward2.ID > 0)
				{
					List<ItemData> list = sevenDay_SevenDayActiveReward2.Reward.ToItemDataList();
					this._activeItems.Add(new ActiveOneItem
					{
						Id = sevenDay_SevenDayActiveReward.ID,
						ActiveNeed = sevenDay_SevenDayActiveReward2.NeedActive,
						DropItems = list,
						IfEquipOne = (sevenDay_SevenDayActiveReward2.IfEquip == 1)
					});
				}
			}
		}

		private void ReceiveTaskReward(int taskId)
		{
			OneDayCarnivalTaskData oneDayCarnivalTaskData = this.TaskDic[this.GetTaskDay(taskId)];
			List<CarnivalTaskData> taskDatas = oneDayCarnivalTaskData.TaskDatas;
			oneDayCarnivalTaskData.FinishCount--;
			for (int i = 0; i < taskDatas.Count; i++)
			{
				if (taskDatas[i].Id == taskId)
				{
					taskDatas[i].IsReceive = true;
					return;
				}
			}
		}

		public void RefreshCarnivalGetInfo()
		{
			this.RequestCarnivalGetInfo(null, null);
		}

		public void RequestCarnivalGetInfo(Action onFinish, Action<int> onError)
		{
			this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.OnConnecting);
			NetworkUtils.SevenDayCarnival.RequestGetSevenDayInfo(delegate(SevenDayTaskGetInfoResponse resp)
			{
				if (resp == null || resp.SevenDayDto == null || resp.SevenDayDto.EndTimestamp == 0UL || this._hasTimeOut)
				{
					this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.Off);
					this.TaskDic.Clear();
					this.FreeRewardRecord.Clear();
				}
				else
				{
					this.OnRefreshActivityData(resp.SevenDayDto);
					this.FreeRewardRecord = resp.FreeRewardRecord;
					this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.On);
				}
				EventArgsInt eventArgsInt = new EventArgsInt();
				eventArgsInt.SetData(0);
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_CarnivalPanel_RefreshTasks, eventArgsInt);
				Action onFinish2 = onFinish;
				if (onFinish2 == null)
				{
					return;
				}
				onFinish2();
			}, delegate(int errorMsg)
			{
				HLog.LogError(errorMsg.ToString());
				this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.Off);
				Action<int> onError2 = onError;
				if (onError2 == null)
				{
					return;
				}
				onError2(errorMsg);
			});
		}

		public void RequestGetCarnivalActiveReward(int configId, int selectedRewardIdx, Action<SevenDayTaskActiveRewardResponse> onSuccess, Action<int> onError)
		{
			NetworkUtils.SevenDayCarnival.RequestGetCarnivalActiveReward(configId, selectedRewardIdx, delegate(SevenDayTaskActiveRewardResponse resp)
			{
				this._activeLog = (int)resp.ActiveLog;
				foreach (SevenDayTaskDto sevenDayTaskDto in resp.UpdateTaskDto)
				{
					this.UpdateTaskData(sevenDayTaskDto);
				}
				Action<SevenDayTaskActiveRewardResponse> onSuccess2 = onSuccess;
				if (onSuccess2 != null)
				{
					onSuccess2(resp);
				}
				GameApp.Event.DispatchNow(this, LocalMessageName.CC_CarnivalPanel_RefreshActiveItemState, null);
				GameApp.SDK.Analyze.Track_Carnival_Roadmap(configId, resp.CommonData.Reward);
			}, delegate(int errorMsg)
			{
				Action<int> onError2 = onError;
				if (onError2 == null)
				{
					return;
				}
				onError2(errorMsg);
			});
		}

		public void RequestGetCarnivalTaskReward(int taskId, Action<SevenDayTaskRewardResponse> onSuccess, Action<int> onError)
		{
			NetworkUtils.SevenDayCarnival.RequestGetSevenDayTaskReward(taskId, delegate(SevenDayTaskRewardResponse resp)
			{
				int num = (int)(resp.Active - (uint)this._activePower);
				this._activePower = (int)resp.Active;
				this.ReceiveTaskReward(taskId);
				foreach (SevenDayTaskDto sevenDayTaskDto in resp.UpdateTaskDto)
				{
					this.UpdateTaskData(sevenDayTaskDto);
				}
				int taskDay = this.GetTaskDay(taskId);
				this.TaskDic[taskDay].SortData();
				Action<SevenDayTaskRewardResponse> onSuccess2 = onSuccess;
				if (onSuccess2 != null)
				{
					onSuccess2(resp);
				}
				GameApp.Event.DispatchNow(this, 270, new EventArgsInt
				{
					m_count = taskDay
				});
				GameApp.Event.DispatchNow(this, 275, new EventArgsInt
				{
					m_count = taskDay
				});
				GameApp.Event.DispatchNow(this, 272, null);
				GameApp.Event.DispatchNow(this, 271, new EventArgsInt
				{
					m_count = num
				});
				GameApp.SDK.Analyze.Track_Carnival_Task(taskDay, taskId, resp.CommonData.Reward);
			}, delegate(int errorMsg)
			{
				Action<int> onError2 = onError;
				if (onError2 == null)
				{
					return;
				}
				onError2(errorMsg);
			});
		}

		public void RequestSevenDayFreeReward(int configId, int taskDay, Action<SevenDayFreeRewardResponse> onSuccess, Action<int> onError)
		{
			NetworkUtils.SevenDayCarnival.RequestSevenDayFreeReward(configId, delegate(SevenDayFreeRewardResponse resp)
			{
				this.FreeRewardRecord = resp.FreeRewardRecord;
				if (resp.CommonData != null && resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
				{
					DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
				}
				Action<SevenDayFreeRewardResponse> onSuccess2 = onSuccess;
				if (onSuccess2 != null)
				{
					onSuccess2(resp);
				}
				GameApp.Event.DispatchNow(this, 270, new EventArgsInt
				{
					m_count = taskDay
				});
				GameApp.Event.DispatchNow(this, 275, new EventArgsInt
				{
					m_count = taskDay
				});
			}, onError);
		}

		public void OnRefreshActivityData(SevenDayDto resp)
		{
			if (resp == null || resp.EndTimestamp == 0UL)
			{
				this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.Off);
				this.TaskDic.Clear();
				return;
			}
			this.SetDataByResp(resp);
			this.SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState.On);
		}

		private void UpdateTaskData(SevenDayTaskDto updateTask)
		{
			int taskDay = this.GetTaskDay((int)updateTask.Id);
			OneDayCarnivalTaskData oneDayCarnivalTaskData;
			if (!this.TaskDic.TryGetValue(taskDay, out oneDayCarnivalTaskData))
			{
				HLog.LogError(string.Format("Do Not Find Task In CarnivalMgr.TaskDic Please Check If The Connect About GetActivity Fail tempIndex:{0} curDay:{1}", taskDay, oneDayCarnivalTaskData));
				return;
			}
			List<CarnivalTaskData> taskDatas = oneDayCarnivalTaskData.TaskDatas;
			for (int i = 0; i < taskDatas.Count; i++)
			{
				if ((long)taskDatas[i].Id == (long)((ulong)updateTask.Id))
				{
					if (updateTask.IsFinish && !taskDatas[i].IsFinish)
					{
						oneDayCarnivalTaskData.FinishCount++;
					}
					taskDatas[i].RefreshData(updateTask);
				}
			}
			oneDayCarnivalTaskData.SortData();
		}

		public void UpdateTaskDatas(RepeatedField<SevenDayTaskDto> datas)
		{
			if (!this.IFGetCarnivalSeverData() || this.IfTimeOut)
			{
				return;
			}
			foreach (SevenDayTaskDto sevenDayTaskDto in datas)
			{
				this.UpdateTaskData(sevenDayTaskDto);
			}
			RedPointController.Instance.ReCalc("Main.Carnival", true);
		}

		private void SetCurState(SevenDayCarnivalDataModule.CarnivalMgrState state)
		{
			this._curState = state;
		}

		private void SortAllTasks()
		{
			for (int i = 1; i <= this._unLockDay; i++)
			{
				OneDayCarnivalTaskData oneDayCarnivalTaskData;
				if (this.TaskDic.TryGetValue(i, out oneDayCarnivalTaskData))
				{
					oneDayCarnivalTaskData.SortData();
				}
				else
				{
					HLog.LogError(string.Format("SortAllTasks:[{0}] is null.", i));
				}
			}
		}

		private void SetDataByResp(SevenDayDto resp)
		{
			this._activePower = (int)resp.Active;
			this._unLockDay = Utility.Math.Clamp((int)resp.Days, SevenDayCarnivalDataModule.MinDays, SevenDayCarnivalDataModule.MaxDays);
			this._taskEndTimeStamp = (long)resp.TaskEndTimestamp;
			this._endTimeStamp = (long)resp.EndTimestamp;
			this._activeLog = (int)resp.ActiveLog;
			this.TaskDic.Clear();
			foreach (SevenDayTaskDto sevenDayTaskDto in resp.Tasks)
			{
				CarnivalTaskData carnivalTaskData = new CarnivalTaskData();
				carnivalTaskData.Id = (int)sevenDayTaskDto.Id;
				carnivalTaskData.Progress = (int)sevenDayTaskDto.Process;
				carnivalTaskData.IsFinish = sevenDayTaskDto.IsFinish;
				carnivalTaskData.IsReceive = sevenDayTaskDto.IsReceive;
				this.GetDataFromTable(carnivalTaskData, sevenDayTaskDto);
				this.AddToTaskDic(carnivalTaskData);
			}
			this.SortAllTasks();
		}

		private void AddToTaskDic(CarnivalTaskData taskData)
		{
			int day = taskData.Day;
			OneDayCarnivalTaskData oneDayCarnivalTaskData;
			if (this.TaskDic.TryGetValue(day, out oneDayCarnivalTaskData))
			{
				oneDayCarnivalTaskData.TaskDatas.Add(taskData);
			}
			else
			{
				oneDayCarnivalTaskData = new OneDayCarnivalTaskData(taskData);
				this.TaskDic.Add(day, oneDayCarnivalTaskData);
			}
			if (taskData.IsFinish && !taskData.IsReceive)
			{
				oneDayCarnivalTaskData.FinishCount++;
			}
		}

		private void GetDataFromTable(CarnivalTaskData taskData, SevenDayTaskDto task)
		{
			SevenDay_SevenDayTask elementById = GameApp.Table.GetManager().GetSevenDay_SevenDayTaskModelInstance().GetElementById((int)task.Id);
			if (elementById == null)
			{
				HLog.LogError(string.Format("CarnivalManager.GetDataFromTable:{0} taskTableData == null", task.Id));
				return;
			}
			taskData.Day = elementById.Day;
			taskData.TaskType = elementById.TaskType;
			taskData.DescribeId = elementById.Describe.ToString();
			taskData.ProgressType = elementById.ProgressType;
			taskData.JumpType = elementById.Jump;
			taskData.TotalProgressNeed = elementById.Need;
			taskData.DropItem = elementById.Reward.ToItemDataList();
		}

		private int GetTaskDay(int taskId)
		{
			return GameApp.Table.GetManager().GetSevenDay_SevenDayTaskModelInstance().GetElementById(taskId)
				.Day;
		}

		public int GetOpenIndex()
		{
			if (this._curState == SevenDayCarnivalDataModule.CarnivalMgrState.Off)
			{
				return 1;
			}
			int num = -1;
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, OneDayCarnivalTaskData> keyValuePair in this.TaskDic)
			{
				if (this._unLockDay >= keyValuePair.Key)
				{
					list.Add(keyValuePair.Key);
				}
			}
			list.Sort(delegate(int a, int b)
			{
				if (a < b)
				{
					return -1;
				}
				return 1;
			});
			if (list.Count > 0)
			{
				for (int i = list.Count - 1; i >= 0; i--)
				{
					int num2 = list[i];
					if (this.TaskDic[num2].FinishCount > 0)
					{
						num = num2;
						break;
					}
				}
				if (num < 0)
				{
					for (int j = list.Count - 1; j >= 0; j--)
					{
						int num3 = list[j];
						if (!this.TaskDic[list[j]].IfAllFinished())
						{
							num = num3;
							break;
						}
					}
				}
			}
			if (num < 0)
			{
				num = this._unLockDay;
			}
			return num;
		}

		private SevenDayCarnivalDataModule.CarnivalMgrState _curState;

		private List<ActiveOneItem> _activeItems = new List<ActiveOneItem>();

		public Dictionary<int, OneDayCarnivalTaskData> TaskDic = new Dictionary<int, OneDayCarnivalTaskData>();

		public MapField<uint, uint> FreeRewardRecord = new MapField<uint, uint>();

		public bool IfCanShowRedNode;

		private int _activePower;

		private int _unLockDay;

		private long _taskEndTimeStamp;

		private long _endTimeStamp;

		private bool _hasTimeOut;

		private int _activeLog;

		public static int MinDays = 1;

		public static int MaxDays = 7;

		public enum ViewType
		{
			Reward,
			Pay
		}

		public enum CarnivalMgrState
		{
			On,
			Off,
			OnConnecting,
			ConnectFailed
		}
	}
}
