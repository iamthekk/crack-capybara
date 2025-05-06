using System;
using System.Collections.Generic;

namespace HotFix
{
	public class PetCollectionData
	{
		public int GetConfigIdByIndex(int index)
		{
			if (index + 1 > this.collectionIds.Count)
			{
				return 0;
			}
			return this.collectionIds[index];
		}

		public void Add2Ids(int id)
		{
			this.collectionIds.Add(id);
			if (this.collectionIds.IndexOf(id) == 0)
			{
				this.curConfigId = id;
			}
		}

		public void UpdateCurrentId(int id)
		{
			this.serverConfigId = id;
			int num = this.collectionIds.IndexOf(id);
			this.curIndex = ((num + 1 == this.collectionIds.Count) ? num : (num + 1));
			this.curConfigId = this.GetConfigIdByIndex(this.curIndex);
			this.isFull = num + 1 == this.collectionIds.Count;
		}

		public void CheckCondition()
		{
		}

		public void Reset()
		{
			this.groupId = -1;
			this.curConfigId = 0;
			this.collectionIds.Clear();
		}

		public int groupId = -1;

		public int curConfigId;

		public int curIndex;

		public bool isFull;

		public bool canUpgrade;

		public int serverConfigId;

		public int conditionStar;

		public List<int> requirePetIds = new List<int>();

		public List<int> collectionIds = new List<int>();
	}
}
