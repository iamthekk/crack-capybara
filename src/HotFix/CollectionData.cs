using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public class CollectionData
	{
		public bool IsCanMerge
		{
			get
			{
				return this.collectionType == 2U && this.fragMentCount >= this.MergeNeedFragment;
			}
		}

		public int MergeNeedFragment { get; private set; }

		public int ConditionTalentId { get; private set; }

		public bool IsFullStar { get; private set; }

		public List<ItemData> StarUpgradeItemCost { get; private set; } = new List<ItemData>();

		public CollectionData(Collection_collection collectionTable)
		{
			this.itemId = collectionTable.id;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(collectionTable.id);
			if (elementById == null)
			{
				this.quality = 1;
				HLog.LogError(string.Format("collectionId:{0} can't find in item table", collectionTable.id));
			}
			else
			{
				this.quality = elementById.quality;
			}
			this.tagId = collectionTable.tagId;
			this.nameId = collectionTable.nameId;
			this.atlasId = elementById.atlasID;
			this.iconName = elementById.icon;
			this.rarity = collectionTable.rarity;
			this.suitId = collectionTable.suitId;
			this.passiveTriggerType = collectionTable.passiveType;
			this.MergeNeedFragment = collectionTable.needFragment;
			this.collectionType = 0U;
			this.fragMentCount = 0;
			this.collectionStar = 0;
			this.fragmentId = int.Parse(collectionTable.toFragment.Split(',', StringSplitOptions.None)[0]);
			this.conditionChecker = new CollectionConditionChecker(this.itemId, this.GetStarId(0), GameApp.Table.GetManager());
		}

		public void DtoUpdate(CollectionDto dto)
		{
			if (dto.CollecType == 1U)
			{
				this.rowId = dto.RowId;
				int num = this.collectionStar;
				this.collectionStar = (int)dto.CollecStar;
				int num2 = this.collectionStar;
				uint num3 = this.collectionType;
				this.collectionType = dto.CollecType;
				uint num4 = this.collectionType;
				this.UpdateCanStarUpgradeData();
				this.UpdateAddAttributeData(num3, num4, num, num2);
				return;
			}
			if (dto.CollecType == 2U)
			{
				this.collectionType = ((this.rowId > 0UL) ? 1U : 2U);
				this.fragmentRowId = dto.RowId;
				this.fragMentCount = (int)dto.CollecCount;
			}
		}

		private void UpdateAddAttributeData(uint oldCollectionType, uint newCollectionType, int oldStar, int newStar)
		{
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			int num = newStar - oldStar;
			if ((oldCollectionType != 1U && newCollectionType == 1U) || num > 0)
			{
				CollectionSuitData collectionSuitData = dataModule.GetCollectionSuitData(this.suitId);
				if (collectionSuitData != null)
				{
					collectionSuitData.SetDirty(true);
				}
				Dictionary<string, FP> passiveAttributes = this.GetPassiveAttributes(newStar);
				Dictionary<string, FP> basicAttributes = this.GetBasicAttributes(newStar);
				this.Add2AddAttributeData(this.basicAttributeDict, basicAttributes);
				this.Add2AddAttributeData(this.passiveAttributeDict, passiveAttributes);
				this.passiveAttributeDict = passiveAttributes;
				this.basicAttributeDict = basicAttributes;
			}
		}

		public bool TryTriggerCondition(long dataCount)
		{
			Dictionary<string, FP> dictionary;
			if (this.conditionChecker.MatchCondition(out dictionary, this.collectionType, dataCount))
			{
				bool flag = this.Add2AddAttributeData(this.passiveAttributeDict, dictionary);
				this.passiveAttributeDict = dictionary;
				return flag;
			}
			return false;
		}

		private bool Add2AddAttributeData(Dictionary<string, FP> oldAttributeDict, Dictionary<string, FP> newAttributeDict)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (KeyValuePair<string, FP> keyValuePair in newAttributeDict)
			{
				FP fp = (oldAttributeDict.ContainsKey(keyValuePair.Key) ? (newAttributeDict[keyValuePair.Key] - oldAttributeDict[keyValuePair.Key]) : newAttributeDict[keyValuePair.Key]);
				oldAttributeDict[keyValuePair.Key] = fp;
				if (fp != 0)
				{
					list.Add(new MergeAttributeData(string.Format("{0}={1}", keyValuePair.Key, fp.AsDouble()), null, null));
				}
			}
			if (list.Count > 0)
			{
				GameApp.Data.GetDataModule(DataName.CollectionDataModule).Add2AddAttributeData(list);
				return true;
			}
			return false;
		}

		public Dictionary<string, FP> GetBasicAttributes(int star)
		{
			int starId = this.GetStarId(star);
			Collection_collectionStar elementById = GameApp.Table.GetManager().GetCollection_collectionStarModelInstance().GetElementById(starId);
			if (elementById == null)
			{
				return new Dictionary<string, FP>();
			}
			return elementById.basicAttribute.GetAttributeDict();
		}

		public Dictionary<string, FP> GetPassiveAttributes(int star)
		{
			int starId = this.GetStarId(star);
			long conditionHistoryTriggerCount = GameApp.Data.GetDataModule(DataName.CollectionDataModule).GetConditionHistoryTriggerCount(this.conditionChecker.conditionId);
			return this.conditionChecker.GetPassiveAttributeDict(starId, this.collectionType, conditionHistoryTriggerCount);
		}

		private void UpdateCanStarUpgradeData()
		{
			int num = this.collectionStar + this.tagId * 100;
			Collection_collectionStar elementById = GameApp.Table.GetManager().GetCollection_collectionStarModelInstance().GetElementById(num);
			Collection_collectionStar elementById2 = GameApp.Table.GetManager().GetCollection_collectionStarModelInstance().GetElementById(num + 1);
			this.ConditionTalentId = elementById.conditionTalentId;
			this.StarUpgradeItemCost = elementById.starItemCost.ToItemDataList();
			this.IsFullStar = elementById2 == null;
		}

		public bool IsMatchStarUpgradeCondition()
		{
			TalentDataModule dataModule = GameApp.Data.GetDataModule(DataName.TalentDataModule);
			int num = ((dataModule != null) ? dataModule.TalentStage : 0);
			return this.collectionType == 1U && !this.IsFullStar && num >= this.ConditionTalentId && this.IsStarUpgradeCostEnough();
		}

		public bool IsStarUpgradeCostEnough()
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			for (int i = 0; i < this.StarUpgradeItemCost.Count; i++)
			{
				long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)this.StarUpgradeItemCost[i].ID));
				long totalCount = this.StarUpgradeItemCost[i].TotalCount;
				if (itemDataCountByid < totalCount)
				{
					return false;
				}
			}
			return true;
		}

		public int GetStarId(int star)
		{
			if (star < 0)
			{
				return this.tagId * 100;
			}
			return this.tagId * 100 + star;
		}

		public ulong rowId;

		public ulong fragmentRowId;

		public uint collectionType;

		public int itemId;

		public int fragmentId;

		public CollectionConditionChecker conditionChecker;

		public int fragMentCount;

		public int collectionStar;

		public int tagId;

		public int rarity;

		public int quality;

		public int suitId;

		public string nameId;

		public int atlasId;

		public string iconName;

		public int passiveTriggerType;

		public Dictionary<string, FP> basicAttributeDict = new Dictionary<string, FP>();

		public Dictionary<string, FP> passiveAttributeDict = new Dictionary<string, FP>();
	}
}
