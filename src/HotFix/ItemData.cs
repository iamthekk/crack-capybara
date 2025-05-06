using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class ItemData
	{
		public int ID { get; private set; }

		public long Count { get; private set; }

		public ItemData()
		{
			this.needReCalc = true;
		}

		public ItemData(int id, long count)
		{
			this.SetID(id);
			this.SetCount(count);
		}

		public void SetID(uint id)
		{
			this.SetID((int)id);
		}

		public void SetID(int id)
		{
			this.ID = id;
			this.needReCalc = true;
		}

		public void SetID(long id)
		{
			this.ID = (int)id;
			this.needReCalc = true;
		}

		public void SetCount(long count)
		{
			this.Count = count;
			this.needReCalc = true;
		}

		public void SetReCalc()
		{
			this.needReCalc = true;
		}

		public long TotalCount
		{
			get
			{
				if (this.needReCalc)
				{
					this.UpdateTotalCount();
				}
				return this.finalCount;
			}
		}

		public void Combine(ItemData itemData)
		{
			if (itemData == null)
			{
				return;
			}
			if (this.ID != itemData.ID)
			{
				return;
			}
			this.Count += itemData.Count;
			this.needReCalc = true;
		}

		public ItemData Copy()
		{
			return new ItemData(this.ID, this.Count);
		}

		public string ToStringItem()
		{
			return string.Format("{0},{1}", this.ID, this.Count);
		}

		private void UpdateTotalCount()
		{
			this.finalCount = ChapterDataModule.CalcDynamicDrop(this.ID, this.Count, 0f, false);
			this.needReCalc = false;
		}

		public Item_Item Data
		{
			get
			{
				return GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.ID);
			}
		}

		public ItemType itemType
		{
			get
			{
				return (ItemType)this.Data.itemType;
			}
		}

		public BaseItemData ToBaseItemData()
		{
			return new BaseItemData
			{
				id = (uint)this.ID,
				count = (ulong)((uint)this.finalCount)
			};
		}

		public override string ToString()
		{
			return string.Format("ItemData id:{0},count:{1} type:{2}", this.ID, this.finalCount, this.itemType);
		}

		public static int CommonStringToList(string str, List<ItemData> list)
		{
			if (string.IsNullOrEmpty(str))
			{
				return 0;
			}
			if (list == null)
			{
				return 0;
			}
			return ItemData.CommonStringArrayToList(str.Split('|', StringSplitOptions.None), list);
		}

		public static int CommonStringArrayToList(string[] strarr, List<ItemData> list)
		{
			if (strarr == null || strarr.Length == 0)
			{
				return 0;
			}
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < strarr.Length; i++)
			{
				if (!string.IsNullOrEmpty(strarr[i]))
				{
					string[] array = strarr[i].Split(',', StringSplitOptions.None);
					int num2;
					int num3;
					if (array.Length == 2 && int.TryParse(array[0], out num2) && int.TryParse(array[1], out num3))
					{
						ItemData itemData = new ItemData
						{
							ID = num2,
							Count = (long)num3
						};
						list.Add(itemData);
						num++;
					}
				}
			}
			return num;
		}

		private long finalCount;

		private bool needReCalc = true;
	}
}
