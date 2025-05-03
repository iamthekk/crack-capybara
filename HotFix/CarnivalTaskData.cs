using System;
using System.Collections.Generic;
using Framework;
using Proto.Common;

namespace HotFix
{
	public class CarnivalTaskData : IEquatable<CarnivalTaskData>
	{
		public bool Unlock
		{
			get
			{
				return this.Day <= GameApp.Data.GetDataModule<SevenDayCarnivalDataModule>(141).UnLockDay;
			}
		}

		public bool Equals(CarnivalTaskData other)
		{
			return other != null && (this == other || this.Id == other.Id);
		}

		public void RefreshData(SevenDayTaskDto updateTask)
		{
			if (this.ProgressType == 0)
			{
				this.Progress = (updateTask.IsFinish ? 1 : 0);
			}
			else
			{
				this.Progress = (int)updateTask.Process;
			}
			this.IsFinish = updateTask.IsFinish;
			this.IsReceive = updateTask.IsReceive;
		}

		public override string ToString()
		{
			return string.Format("Id:{0}, Progress:{1}/{2} IsFinish:{3} IsReceive:{4}", new object[] { this.Id, this.Progress, this.TotalProgressNeed, this.IsFinish, this.IsReceive });
		}

		public int Id;

		public int Day;

		public int TaskType;

		public int Progress;

		public int TotalProgressNeed;

		public bool IsFinish;

		public bool IsReceive;

		public string DescribeId;

		public int ProgressType;

		public int JumpType;

		public List<ItemData> DropItem = new List<ItemData>();
	}
}
