using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.GameTestTools;
using Framework.Platfrom;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class FunctionDataModule : IDataModule
	{
		public int GetName()
		{
			return 108;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_Function_InitData, new HandlerEvent(this.OnInitData));
			manager.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_Function_InitData, new HandlerEvent(this.OnInitData));
			manager.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void Reset()
		{
			this.mFunctionData.Clear();
			this.mOpeningList.Clear();
			this.mDelaySaveFunctionID.Clear();
			this.mIsWaittingSaveFunctionID = false;
		}

		public void InitFromServer(List<uint> openlist)
		{
			this.mLocalCache.ReadLocalData();
			if (this.mFunctionData.Count <= 0)
			{
				IList<Function_Function> allElements = GameApp.Table.GetManager().GetFunction_FunctionModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					FunctionData functionData = new FunctionData();
					functionData.Init(allElements[i]);
					this.mFunctionData[functionData.ID] = functionData;
				}
			}
			for (int j = 0; j < openlist.Count; j++)
			{
				FunctionData functionData2;
				if (this.mFunctionData.TryGetValue((int)openlist[j], out functionData2))
				{
					functionData2.SetStatus(FunctionOpenStatus.UnLocked);
				}
			}
			List<int> list = this.mLocalCache.UnlockedFunctionList;
			List<uint> list2 = new List<uint>();
			for (int k = 0; k < list.Count; k++)
			{
				FunctionData functionData3;
				if (this.mFunctionData.TryGetValue(list[k], out functionData3) && functionData3.Status < FunctionOpenStatus.UnLocked)
				{
					functionData3.SetStatus(FunctionOpenStatus.UnLocked);
					list2.Add((uint)functionData3.ID);
				}
			}
			this.DelaySaveFunctionOpen(list2);
			list = this.mLocalCache.UnlockingFunctionList;
			for (int l = 0; l < list.Count; l++)
			{
				FunctionData functionData4;
				if (this.mFunctionData.TryGetValue(list[l], out functionData4) && functionData4.Status < FunctionOpenStatus.UnLocking)
				{
					functionData4.SetStatus(FunctionOpenStatus.UnLocking);
				}
			}
			this.mLocalCache.Save(this.mFunctionData);
		}

		public void SetUnlockedImmediately(List<FunctionData> dataList)
		{
			if (dataList == null)
			{
				return;
			}
			List<uint> list = new List<uint>();
			for (int i = 0; i < dataList.Count; i++)
			{
				FunctionData functionData = this.GetFunctionData(dataList[i].ID);
				if (functionData.Status != FunctionOpenStatus.UnLocked)
				{
					functionData.SetStatus(FunctionOpenStatus.UnLocked);
					list.Add((uint)functionData.ID);
					EventArgsFunctionOpen eventArgsFunctionOpen = new EventArgsFunctionOpen();
					eventArgsFunctionOpen.FunctionID = functionData.ID;
					GameApp.Event.DispatchNow(this, LocalMessageName.CC_Function_Open, eventArgsFunctionOpen);
				}
			}
			this.DelaySaveFunctionOpen(list);
		}

		public bool IsFunctionOpened(int function)
		{
			FunctionData functionData;
			return this.CheckCloudDataOpen(function) && !this.IsServerClose(function) && this.mFunctionData.TryGetValue(function, out functionData) && functionData.IsFunctionOpened;
		}

		public bool CheckCloudDataOpen(int functionId)
		{
			bool cloudDataValue = GameApp.SDK.GetCloudDataValue<bool>("IfShowPay", true);
			return (functionId != 1 && functionId != 201 && functionId != 301 && functionId != 60 && functionId != 103 && functionId != 101 && functionId != 102 && functionId != 104 && functionId != 105 && functionId != 106 && functionId != 59) || cloudDataValue;
		}

		public bool IsFunctionOpened(FunctionID function)
		{
			return this.IsFunctionOpened((int)function);
		}

		public FunctionData GetFunctionData(int function)
		{
			FunctionData functionData;
			if (this.mFunctionData.TryGetValue(function, out functionData))
			{
				return functionData;
			}
			return null;
		}

		public FunctionData GetFunctionData(FunctionID function)
		{
			return this.GetFunctionData((int)function);
		}

		public List<FunctionData> GetUnlockList()
		{
			List<FunctionData> list = new List<FunctionData>();
			foreach (KeyValuePair<int, FunctionData> keyValuePair in this.mFunctionData)
			{
				if (keyValuePair.Value.Status != FunctionOpenStatus.UnLocked)
				{
					list.Add(keyValuePair.Value);
				}
			}
			return list;
		}

		public void SetUnlockingList(List<FunctionData> openinglist)
		{
			openinglist.Sort(new Comparison<FunctionData>(FunctionData.Sort));
			this.mOpeningList.Clear();
			this.mOpeningList.AddRange(openinglist);
		}

		public bool HasNewFunctionOpen()
		{
			return this.mOpeningList != null && this.mOpeningList.Count > 0;
		}

		public FunctionData GetNextUnlockingData()
		{
			if (this.mOpeningList == null || this.mOpeningList.Count <= 0)
			{
				return null;
			}
			return this.mOpeningList[0];
		}

		private async void DelaySaveFunctionOpen(List<uint> idlist)
		{
			if (idlist != null)
			{
				for (int i = 0; i < idlist.Count; i++)
				{
					if (!this.mDelaySaveFunctionID.Contains(idlist[i]))
					{
						this.mDelaySaveFunctionID.Add(idlist[i]);
					}
				}
				if (!this.mIsWaittingSaveFunctionID)
				{
					this.mIsWaittingSaveFunctionID = true;
					await TaskExpand.Delay(5000);
					if (Application.isPlaying)
					{
						this.mLocalCache.Save(this.mFunctionData);
						List<uint> savelist = new List<uint>();
						for (int j = 0; j < this.mDelaySaveFunctionID.Count; j++)
						{
							savelist.Add(this.mDelaySaveFunctionID[j]);
						}
						this.mDelaySaveFunctionID.Clear();
						this.mIsWaittingSaveFunctionID = false;
						NetworkUtils.PlayerData.UserOpenModelRequest(savelist, delegate(bool result, UserOpenModelResponse resp)
						{
							if (!result)
							{
								StringBuilder stringBuilder = new StringBuilder();
								for (int k = 0; k < savelist.Count; k++)
								{
									stringBuilder.Append(string.Format("{0},", savelist[k]));
								}
								HLog.LogError("[Function]保存新功能状态到服务器失败：Function ID = " + stringBuilder.ToString());
							}
							if (savelist.Contains(202U))
							{
								NetworkUtils.Chapter.DoGetHangUpInfoRequest(null);
							}
							if (savelist.Contains(201U))
							{
								NetworkUtils.PlayerData.SendUserGetIapInfoRequest(null);
							}
						});
					}
				}
			}
		}

		public void CommonUpdateServerCloseFunction(RepeatedField<int> closeFunctionIds)
		{
			if (closeFunctionIds == null)
			{
				return;
			}
			this.ServerCloseFunctionIds = closeFunctionIds;
		}

		public bool IsServerClose(int function)
		{
			return this.ServerCloseFunctionIds != null && this.ServerCloseFunctionIds.Contains(function);
		}

		private void OnInitData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpenInit eventArgsFunctionOpenInit = eventArgs as EventArgsFunctionOpenInit;
			if (eventArgsFunctionOpenInit == null)
			{
				return;
			}
			this.InitFromServer(eventArgsFunctionOpenInit.OpenList);
			Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen();
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			int functionID = eventArgsFunctionOpen.FunctionID;
			if (functionID == 0)
			{
				return;
			}
			FunctionData functionData = null;
			for (int i = 0; i < this.mOpeningList.Count; i++)
			{
				if (this.mOpeningList[i].ID == functionID)
				{
					functionData = this.mOpeningList[i];
					this.mOpeningList.RemoveAt(i);
					break;
				}
			}
			if (functionData != null)
			{
				functionData.SetStatus(FunctionOpenStatus.UnLocked);
			}
			this.DelaySaveFunctionOpen(new List<uint> { (uint)functionID });
		}

		[GameTestMethod("GM", "解锁全部功能", "", 501)]
		private static void OpenGameEventDemon()
		{
			List<FunctionData> unlockList = GameApp.Data.GetDataModule(DataName.FunctionDataModule).GetUnlockList();
			for (int i = 0; i < unlockList.Count; i++)
			{
				unlockList[i].SetStatus(FunctionOpenStatus.UnLocking);
			}
			Singleton<GameFunctionController>.Instance.CheckNewFunctionOpen();
		}

		private Dictionary<int, FunctionData> mFunctionData = new Dictionary<int, FunctionData>();

		private List<FunctionData> mOpeningList = new List<FunctionData>();

		private FunctionLocalCache mLocalCache = new FunctionLocalCache();

		private List<uint> mDelaySaveFunctionID = new List<uint>();

		private bool mIsWaittingSaveFunctionID;

		private RepeatedField<int> ServerCloseFunctionIds;
	}
}
