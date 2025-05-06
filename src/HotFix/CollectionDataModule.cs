using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public class CollectionDataModule : IDataModule
	{
		public long GetConditionHistoryTriggerCount(int conditionId)
		{
			long num;
			if (this.userStatisticInfo.TryGetValue(conditionId, out num))
			{
				return num;
			}
			return 0L;
		}

		public void Add2AddAttributeData(List<MergeAttributeData> mergeAttributeDataList)
		{
			if (mergeAttributeDataList == null)
			{
				return;
			}
			for (int i = 0; i < mergeAttributeDataList.Count; i++)
			{
				this.Add2AddAttributeData(mergeAttributeDataList[i]);
			}
			this.SetCollectionAttributeDataDirty();
		}

		public void Add2AddAttributeData(MergeAttributeData mergeAttributeData)
		{
			if (mergeAttributeData == null)
			{
				return;
			}
			this.m_addAttributeData.m_attributeDatas.Add(mergeAttributeData);
		}

		public int GetName()
		{
			return 149;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionMerge, new HandlerEvent(this.OnEventCollectionMerge));
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, new HandlerEvent(this.OnEventCollectionLevelUp));
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, new HandlerEvent(this.OnEventCollectionStarUpgrade));
			manager.RegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionUpdate, new HandlerEvent(this.OnEventCollectionUpdate));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionMerge, new HandlerEvent(this.OnEventCollectionMerge));
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionLevelUp, new HandlerEvent(this.OnEventCollectionLevelUp));
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionStarUpgrade, new HandlerEvent(this.OnEventCollectionStarUpgrade));
			manager.UnRegisterEvent(LocalMessageName.CC_CollectionDataModule_CollectionUpdate, new HandlerEvent(this.OnEventCollectionUpdate));
		}

		public void Reset()
		{
			this.m_addAttributeData = new AddAttributeData();
			this.collectionDict.Clear();
			this.collectionDataMap.Clear();
			this.collectionSuitDict.Clear();
		}

		private void OnEventCollectionUpdate(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsCollectionUpdate eventArgsCollectionUpdate = eventArgs as EventArgsCollectionUpdate;
			if (eventArgsCollectionUpdate.dtos != null)
			{
				this.UpdateCollectionData(eventArgsCollectionUpdate.dtos.ToList<CollectionDto>());
			}
			this.ReCalcSuitData();
		}

		private void OnEventCollectionMerge(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsCollectionMerge eventArgsCollectionMerge = eventArgs as EventArgsCollectionMerge;
			if (eventArgsCollectionMerge.collectionDtoList != null)
			{
				this.UpdateCollectionData(eventArgsCollectionMerge.collectionDtoList);
			}
			this.ReCalcSuitData();
		}

		private void OnEventCollectionLevelUp(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsCollectionLevelUp eventArgsCollectionLevelUp = eventArgs as EventArgsCollectionLevelUp;
			if (eventArgsCollectionLevelUp.collectionDto != null)
			{
				this.UpdateCollectionData(eventArgsCollectionLevelUp.collectionDto);
			}
			this.ReCalcSuitData();
		}

		private void OnEventCollectionStarUpgrade(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsCollectionStarUpgrade eventArgsCollectionStarUpgrade = eventArgs as EventArgsCollectionStarUpgrade;
			if (eventArgsCollectionStarUpgrade.collectionDtoList != null)
			{
				this.UpdateCollectionData(eventArgsCollectionStarUpgrade.collectionDtoList);
			}
			this.ReCalcSuitData();
		}

		public void UpdateUserStatisticInfo(UserStatisticInfo sUserStatisticInfo)
		{
			bool flag = false;
			if (sUserStatisticInfo != null && sUserStatisticInfo.DataMap != null)
			{
				foreach (KeyValuePair<uint, ulong> keyValuePair in sUserStatisticInfo.DataMap)
				{
					int key = (int)keyValuePair.Key;
					long value = (long)keyValuePair.Value;
					this.userStatisticInfo[key] = value;
					foreach (CollectionData collectionData in this.collectionDict.Values)
					{
						if (collectionData.collectionType == 1U && collectionData.conditionChecker.conditionId == key && collectionData.TryTriggerCondition(value))
						{
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				GameApp.Event.DispatchNow(null, 145, null);
			}
		}

		public void InitServerData(CollectionInfo sCollectionInfo, UserStatisticInfo sUserStatisticInfo)
		{
			try
			{
				this.userStatisticInfo.Clear();
				this.collectionDict.Clear();
				this.collectionDataMap.Clear();
				if (sUserStatisticInfo != null && sUserStatisticInfo.DataMap != null)
				{
					foreach (KeyValuePair<uint, ulong> keyValuePair in sUserStatisticInfo.DataMap)
					{
						this.userStatisticInfo.Add((int)keyValuePair.Key, (long)keyValuePair.Value);
					}
				}
				IList<Collection_collection> allElements = GameApp.Table.GetManager().GetCollection_collectionModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					CollectionData collectionData = new CollectionData(allElements[i]);
					this.collectionDict.Add(collectionData.itemId, collectionData);
				}
				if (sCollectionInfo != null && sCollectionInfo.CollectionList != null)
				{
					this.UpdateCollectionData(sCollectionInfo.CollectionList.ToList<CollectionDto>());
				}
				this.OnInitSuitData();
			}
			catch (Exception ex)
			{
				HLog.LogException(ex);
			}
			finally
			{
				this.isColelctionDataInit = true;
			}
		}

		public int GetCollectionCount(int collectionId)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(collectionId);
			if (elementById == null)
			{
				return 0;
			}
			int num = 0;
			int itemType = elementById.itemType;
			foreach (CollectionData collectionData in this.collectionDict.Values)
			{
				if (itemType == 21)
				{
					if (collectionData.itemId.Equals(collectionId) && collectionData.collectionType == 1U)
					{
						num++;
						break;
					}
				}
				else if (itemType == 22)
				{
					if (collectionData.fragmentId.Equals(collectionId))
					{
						num += collectionData.fragMentCount;
						break;
					}
				}
				else if (itemType == 23 && collectionData.quality == elementById.quality && collectionData.IsFullStar)
				{
					num += collectionData.fragMentCount;
				}
			}
			return num;
		}

		public CollectionData GetCollectionData(int collectionId)
		{
			CollectionData collectionData;
			this.collectionDict.TryGetValue(collectionId, out collectionData);
			return collectionData;
		}

		public List<CollectionData> GetCollectionList(int collectionFilterType)
		{
			List<CollectionData> list = new List<CollectionData>();
			foreach (CollectionData collectionData in this.collectionDict.Values)
			{
				if (collectionFilterType == 0)
				{
					list.Add(collectionData);
				}
				else if (collectionFilterType == 1)
				{
					if (collectionData.rowId <= 0UL)
					{
						list.Add(collectionData);
					}
				}
				else if (collectionFilterType == 2)
				{
					if (collectionData.IsCanMerge)
					{
						list.Add(collectionData);
					}
				}
				else if (collectionFilterType == 3)
				{
					if (collectionData.collectionType == 2U)
					{
						list.Add(collectionData);
					}
				}
				else if (collectionFilterType == 4 && collectionData.collectionType == 1U)
				{
					list.Add(collectionData);
				}
			}
			return list;
		}

		public void UpdateCollectionData(List<CollectionDto> collectionList)
		{
			for (int i = 0; i < collectionList.Count; i++)
			{
				CollectionDto collectionDto = collectionList[i];
				this.UpdateCollectionData(collectionDto);
			}
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Collection);
		}

		public void UpdateCollectionData(CollectionDto dto)
		{
			if (dto == null)
			{
				return;
			}
			int collectionId = dto.GetCollectionId();
			if (this.collectionDict.ContainsKey(collectionId))
			{
				this.collectionDict[collectionId].DtoUpdate(dto);
			}
			else
			{
				HLog.LogError("UpdateCollectionData error, not found collectionId = " + collectionId.ToString());
			}
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Collection);
		}

		public CollectionSuitData GetCollectionSuitData(int suitId)
		{
			CollectionSuitData collectionSuitData;
			this.collectionSuitDict.TryGetValue(suitId, out collectionSuitData);
			return collectionSuitData;
		}

		private void ReCalcSuitData()
		{
			foreach (CollectionSuitData collectionSuitData in this.collectionSuitDict.Values)
			{
				collectionSuitData.UpdateSuitData();
			}
		}

		private void OnInitSuitData()
		{
			this.collectionSuitDict.Clear();
			IList<Collection_collectionSuit> allElements = GameApp.Table.GetManager().GetCollection_collectionSuitModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Collection_collectionSuit collection_collectionSuit = allElements[i];
				CollectionSuitData collectionSuitData;
				if (!this.collectionSuitDict.TryGetValue(collection_collectionSuit.suitId, out collectionSuitData))
				{
					collectionSuitData = new CollectionSuitData(collection_collectionSuit.suitId);
					this.collectionSuitDict.Add(collection_collectionSuit.suitId, collectionSuitData);
				}
				collectionSuitData.AddTableData(collection_collectionSuit);
			}
			foreach (CollectionSuitData collectionSuitData2 in this.collectionSuitDict.Values)
			{
				collectionSuitData2.UpdateSuitData();
			}
		}

		private void SetCollectionAttributeDataDirty()
		{
			if (!this.isColelctionDataInit)
			{
				return;
			}
			if (this.isAttributeChanged)
			{
				return;
			}
			this.isAttributeChanged = true;
			DelayCall.Instance.CallOnce(1000, new DelayCall.CallAction(this.TryCalcAttribute));
		}

		public void TryCalcAttribute()
		{
			if (this.isAttributeChanged)
			{
				this.isAttributeChanged = false;
				GameApp.Event.DispatchNow(null, 145, null);
			}
		}

		public AddAttributeData m_addAttributeData = new AddAttributeData();

		public Dictionary<uint, uint> collectionDataMap = new Dictionary<uint, uint>();

		public Dictionary<int, CollectionData> collectionDict = new Dictionary<int, CollectionData>();

		public Dictionary<int, CollectionSuitData> collectionSuitDict = new Dictionary<int, CollectionSuitData>();

		public Dictionary<int, long> userStatisticInfo = new Dictionary<int, long>();

		private bool isColelctionDataInit;

		private bool isAttributeChanged;
	}
}
