using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPChapterGift
	{
		public IAPChapterGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
		}

		public void SetData(MapField<uint, ulong> chapterDataVal, bool isClear, bool isNewData)
		{
			if (isClear)
			{
				this.dataDic.Clear();
			}
			List<int> list = new List<int>();
			foreach (int num in this.dataDic.Keys)
			{
				if (!chapterDataVal.ContainsKey((uint)num))
				{
					list.Add(num);
				}
			}
			foreach (int num2 in list)
			{
				this.dataDic.Remove(num2);
			}
			foreach (KeyValuePair<uint, ulong> keyValuePair in chapterDataVal)
			{
				int key = (int)keyValuePair.Key;
				long value = (long)keyValuePair.Value;
				IAPChapterGift.Data data;
				if (value - this.loginDataModule.ServerUTC <= 0L)
				{
					this.dataDic.Remove(key);
				}
				else if (this.dataDic.TryGetValue(key, out data))
				{
					data.EndTime = (long)keyValuePair.Value;
				}
				else
				{
					data = new IAPChapterGift.Data
					{
						Id = key,
						EndTime = value,
						LoginDataModule = this.loginDataModule,
						CommonData = this.commonData,
						IsNewData = isNewData
					};
					this.dataDic[data.Id] = data;
				}
			}
			RedPointController.Instance.ReBuildCalcChapterGift(false);
		}

		public List<IAPChapterGift.Data> GetEnableData()
		{
			List<IAPChapterGift.Data> list = new List<IAPChapterGift.Data>();
			foreach (KeyValuePair<int, IAPChapterGift.Data> keyValuePair in this.dataDic)
			{
				IAPChapterGift.Data value = keyValuePair.Value;
				if (value != null && value.IsEnable())
				{
					list.Add(value);
				}
			}
			return list;
		}

		public List<IAPChapterGift.Data> CheckAndResetNewEnableData()
		{
			List<IAPChapterGift.Data> list = new List<IAPChapterGift.Data>();
			foreach (KeyValuePair<int, IAPChapterGift.Data> keyValuePair in this.dataDic)
			{
				IAPChapterGift.Data value = keyValuePair.Value;
				if (value != null && value.CheckAndResetNewEnable())
				{
					list.Add(value);
				}
			}
			return list;
		}

		public IAPChapterGift.Data GetData(int id)
		{
			IAPChapterGift.Data data;
			this.dataDic.TryGetValue(id, out data);
			return data;
		}

		private readonly PurchaseCommonData commonData;

		private readonly LoginDataModule loginDataModule;

		private readonly Dictionary<int, IAPChapterGift.Data> dataDic = new Dictionary<int, IAPChapterGift.Data>();

		public class Data
		{
			public int Id { get; set; }

			public long EndTime { get; set; }

			public PurchaseCommonData CommonData { get; set; }

			public LoginDataModule LoginDataModule { get; set; }

			public bool IsNewData { get; set; }

			public bool IsEnable()
			{
				return GameApp.Purchase.IsEnable && this.GetPurchaseData() != null && !this.IsTimeOut();
			}

			public bool CheckAndResetNewEnable()
			{
				if (this.IsNewData && this.IsEnable())
				{
					this.IsNewData = false;
					return true;
				}
				return false;
			}

			public bool IsTimeOut()
			{
				return this.GetLastTime() < 0L;
			}

			public long GetLastTime()
			{
				long num;
				if (this.TryFinishedTime(out num))
				{
					return num - DxxTools.Time.ServerTimestamp;
				}
				return -1L;
			}

			public string GetLastTimeString()
			{
				long lastTime = this.GetLastTime();
				if (lastTime < 0L)
				{
					return string.Empty;
				}
				return Singleton<LanguageManager>.Instance.GetTime(lastTime);
			}

			public bool TryFinishedTime(out long time)
			{
				time = this.EndTime;
				return true;
			}

			public PurchaseCommonData.PurchaseData GetPurchaseData()
			{
				return this.CommonData.GetPurchaseDataByID(this.Id);
			}

			public int GetPriority()
			{
				IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(this.Id);
				if (elementById == null)
				{
					return -1;
				}
				return elementById.priority;
			}
		}
	}
}
