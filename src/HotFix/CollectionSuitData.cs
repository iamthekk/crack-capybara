using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public class CollectionSuitData
	{
		public bool IsDirty { get; set; } = true;

		public int SuitId { get; private set; }

		public List<Collection_collectionSuit> GroupConfigList { get; private set; } = new List<Collection_collectionSuit>();

		public int CurIndex { get; private set; }

		public bool CurIndexConditionMatch { get; private set; }

		public bool IsSuitActiveFull { get; private set; }

		public CollectionSuitData(int suitId)
		{
			this.SuitId = suitId;
		}

		public void AddTableData(Collection_collectionSuit collectionSuitTable)
		{
			this.GroupConfigList.Add(collectionSuitTable);
		}

		public int[] GetCurCollectionIds()
		{
			return this.GroupConfigList[this.CurIndex].collectionId;
		}

		public string GetSuitName()
		{
			return this.GroupConfigList[this.CurIndex].suitNameId;
		}

		public void GetSuitProgress(out int curValue, out int maxValue)
		{
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			int[] collectionId = this.GroupConfigList[this.CurIndex].collectionId;
			curValue = 0;
			maxValue = collectionId.Length;
			foreach (int num in collectionId)
			{
				CollectionData collectionData = dataModule.GetCollectionData(num);
				if (collectionData != null && collectionData.collectionType == 1U)
				{
					curValue++;
				}
			}
		}

		public bool CheckSuitActive(Collection_collectionSuit config)
		{
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			int[] collectionId = config.collectionId;
			int conditionType = config.conditionType;
			int conditionParam = config.conditionParam;
			for (int i = 0; i < collectionId.Length; i++)
			{
				CollectionData collectionData = dataModule.GetCollectionData(collectionId[i]);
				if (collectionData == null || collectionData.collectionType != 1U)
				{
					return false;
				}
				if (conditionType == 1 && collectionData.collectionStar < conditionParam)
				{
					return false;
				}
				if (conditionType == 2)
				{
					return false;
				}
			}
			return true;
		}

		public void UpdateSuitData()
		{
			if (!this.IsDirty)
			{
				return;
			}
			if (this.IsSuitActiveFull)
			{
				this.SetDirty(false);
				return;
			}
			CollectionDataModule dataModule = GameApp.Data.GetDataModule(DataName.CollectionDataModule);
			for (int i = this.CurIndex; i < this.GroupConfigList.Count; i++)
			{
				this.CurIndex = i;
				if (!this.CheckSuitActive(this.GroupConfigList[i]))
				{
					this.CurIndexConditionMatch = false;
					break;
				}
				this.CurIndexConditionMatch = true;
				List<MergeAttributeData> mergeAttributeData = this.GroupConfigList[this.CurIndex].attributes.GetMergeAttributeData();
				dataModule.Add2AddAttributeData(mergeAttributeData);
			}
			this.IsSuitActiveFull = this.CurIndex == this.GroupConfigList.Count - 1 && this.CurIndexConditionMatch;
			this.SetDirty(false);
		}

		public void SetDirty(bool isDirty)
		{
			this.IsDirty = isDirty;
		}
	}
}
