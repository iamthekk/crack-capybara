using System;
using System.Collections.Generic;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public class CollectionConditionChecker
	{
		public CollectionConditionChecker(int collectionId, int collectionStarId, LocalModelManager localModelManager)
		{
			this.localModelManager = localModelManager;
			this.collectionId = collectionId;
			Collection_collection elementById = localModelManager.GetCollection_collectionModelInstance().GetElementById(collectionId);
			if (elementById == null)
			{
				HLog.LogError(string.Format("collectionId:{0} is not exist in collection_table", collectionId));
			}
			this.conditionId = elementById.passiveType;
			Collection_collectionStar elementById2 = localModelManager.GetCollection_collectionStarModelInstance().GetElementById(collectionStarId);
			if (elementById2 == null)
			{
				HLog.LogError(string.Format("collectionStarId:{0} is not exist in collection_star_table", collectionStarId));
			}
			this.conditionJson = new CollectionConditionJson();
			string text;
			FP fp;
			elementById2.effectAttributeEx.GetAttributeKV(out text, out fp);
			this.conditionJson.Cost = (long)elementById.passivePara;
			this.conditionJson.Attribute = text;
			this.conditionJson.Value = fp;
			this.conditionJson.Limit = elementById2.effectTimes;
		}

		public bool MatchCondition(out Dictionary<string, FP> addAttributeDict, uint collectionType, long dataCount)
		{
			if (this.conditionId == 1)
			{
				addAttributeDict = null;
				return false;
			}
			addAttributeDict = null;
			if (this.isConditionFull)
			{
				return false;
			}
			if (dataCount < this.nextConditionValue)
			{
				return false;
			}
			long num;
			this.triggerCount = this.GetCanTriggerCount(collectionType, dataCount, out num);
			this.isConditionFull = this.triggerCount >= this.conditionJson.Limit;
			this.nextConditionValue = (long)(this.triggerCount + 1) * this.conditionJson.Cost;
			addAttributeDict = new Dictionary<string, FP>();
			addAttributeDict.Add(this.conditionJson.Attribute, this.conditionJson.Value * this.triggerCount);
			return true;
		}

		public List<MergeAttributeData> GetPassiveAttributeList(int collectionStarId, uint collectionType, long dataCount)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (KeyValuePair<string, FP> keyValuePair in this.GetPassiveAttributeDict(collectionStarId, collectionType, dataCount))
			{
				list.Add(new MergeAttributeData(string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value.AsDouble()), null, null));
			}
			return list;
		}

		public Dictionary<string, FP> GetPassiveAttributeDict(int collectionStarId, uint collectionType, long dataCount)
		{
			Collection_collectionStar elementById = this.localModelManager.GetCollection_collectionStarModelInstance().GetElementById(collectionStarId);
			string text;
			FP fp;
			elementById.effectAttributeEx.GetAttributeKV(out text, out fp);
			this.conditionJson.Value = fp;
			this.conditionJson.Limit = elementById.effectTimes;
			this.triggerCount = this.GetCanTriggerCount(collectionType, dataCount, out this.triggerCurValue);
			if (collectionType == 1U)
			{
				this.isConditionFull = this.triggerCount >= this.conditionJson.Limit;
				this.nextConditionValue = (long)(this.triggerCount + 1) * this.conditionJson.Cost;
			}
			Dictionary<string, FP> dictionary = new Dictionary<string, FP>();
			if (this.conditionJson.Value * this.triggerCount > 0)
			{
				dictionary.Add(text, this.conditionJson.Value * this.triggerCount);
			}
			return dictionary;
		}

		private int GetCanTriggerCount(uint collectionType, long dataCount, out long triggerCurValue)
		{
			triggerCurValue = 0L;
			if (collectionType != 1U)
			{
				return 0;
			}
			if (this.conditionId != 1)
			{
				int num = (int)(dataCount / this.conditionJson.Cost);
				triggerCurValue = dataCount % this.conditionJson.Cost;
				return Math.Min(num, this.conditionJson.Limit);
			}
			if (this.conditionJson.Limit <= 0)
			{
				return 1;
			}
			return this.conditionJson.Limit;
		}

		public int collectionId;

		public int conditionId;

		public long nextConditionValue;

		public int triggerCount;

		public long triggerCurValue;

		public bool isConditionFull;

		public CollectionConditionJson conditionJson;

		private LocalModelManager localModelManager;
	}
}
