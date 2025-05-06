using System;
using System.Collections.Generic;

namespace HotFix
{
	public class OneDayCarnivalTaskData
	{
		public bool IfAllFinished()
		{
			foreach (CarnivalTaskData carnivalTaskData in this.TaskDatas)
			{
				if (!carnivalTaskData.IsFinish && !carnivalTaskData.IsReceive)
				{
					return false;
				}
			}
			return true;
		}

		public OneDayCarnivalTaskData(CarnivalTaskData tasdData)
		{
			this.TaskDatas = new List<CarnivalTaskData> { tasdData };
		}

		public void SortData()
		{
			this.TaskDatas.Sort(delegate(CarnivalTaskData a, CarnivalTaskData b)
			{
				if (a.IsReceive && b.IsReceive)
				{
					if (a.Id < b.Id)
					{
						return -1;
					}
					return 1;
				}
				else
				{
					if (a.IsReceive && !b.IsReceive)
					{
						return 1;
					}
					if (!a.IsReceive && b.IsReceive)
					{
						return -1;
					}
					if (a.IsFinish && b.IsFinish)
					{
						if (a.Id < b.Id)
						{
							return -1;
						}
						return 1;
					}
					else
					{
						if (a.IsFinish && !b.IsFinish)
						{
							return -1;
						}
						if (!a.IsFinish && b.IsFinish)
						{
							return 1;
						}
						if (a.Id < b.Id)
						{
							return -1;
						}
						return 1;
					}
				}
			});
		}

		public int FinishCount;

		public List<CarnivalTaskData> TaskDatas;
	}
}
