using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	[RuntimeDefaultSerializedProperty]
	public class FunctionData
	{
		public int OpenTimeSecond
		{
			get
			{
				return this.OpenTime * 24 * 3600;
			}
		}

		public bool IsFunctionOpened
		{
			get
			{
				return this.Status == FunctionOpenStatus.UnLocked;
			}
		}

		public void Init(Function_Function tabledata)
		{
			this.ID = tabledata.id;
			this.Status = FunctionOpenStatus.Lock;
			this.UnlockType = (FunctionUnlockType)tabledata.unlockType;
			this.UnlockArgs = tabledata.unlockArgs;
			this.IsShowOpenView = tabledata.showView != 0;
			this.ShowOpenViewIndex = tabledata.showIndex;
			this.OpenTime = tabledata.OpenTime;
		}

		public void SetStatus(FunctionOpenStatus status)
		{
			if (this.UnlockType == FunctionUnlockType.ForceLock)
			{
				this.Status = FunctionOpenStatus.Lock;
				return;
			}
			this.Status = status;
		}

		public static int Sort(FunctionData x, FunctionData y)
		{
			int num = x.ShowOpenViewIndex.CompareTo(y.ShowOpenViewIndex);
			if (num == 0)
			{
				num = x.ID.CompareTo(y.ID);
			}
			return num;
		}

		public int ID;

		public FunctionOpenStatus Status;

		public FunctionUnlockType UnlockType;

		public string UnlockArgs;

		public bool IsShowOpenView;

		public int ShowOpenViewIndex;

		public int OpenTime;
	}
}
