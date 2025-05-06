using System;
using System.Collections.Generic;
using System.Text;
using Framework.Logic;

namespace HotFix
{
	public class FunctionLocalCache
	{
		public void ReadLocalData()
		{
			string userString = Utility.PlayerPrefs.GetUserString(LocalDataName.FunctionOpen_LocalCache, "");
			this.UnlockedFunctionList.Clear();
			this.UnlockingFunctionList.Clear();
			if (!string.IsNullOrEmpty(userString) && userString.Length > 0)
			{
				string[] array = userString.Split('|', StringSplitOptions.None);
				if (array.Length != 0 && !string.IsNullOrEmpty(array[0]))
				{
					string[] array2 = array[0].Split(',', StringSplitOptions.None);
					for (int i = 0; i < array2.Length; i++)
					{
						int num;
						if (int.TryParse(array2[i], out num))
						{
							this.UnlockedFunctionList.Add(num);
						}
					}
				}
				if (array.Length > 1 && !string.IsNullOrEmpty(array[1]))
				{
					string[] array3 = array[1].Split(',', StringSplitOptions.None);
					for (int j = 0; j < array3.Length; j++)
					{
						int num2;
						if (int.TryParse(array3[j], out num2))
						{
							this.UnlockingFunctionList.Add(num2);
						}
					}
				}
			}
		}

		public void Save(Dictionary<int, FunctionData> dic)
		{
			this.UnlockedFunctionList.Clear();
			this.UnlockingFunctionList.Clear();
			foreach (KeyValuePair<int, FunctionData> keyValuePair in dic)
			{
				if (keyValuePair.Value.Status == FunctionOpenStatus.UnLocked)
				{
					this.UnlockedFunctionList.Add(keyValuePair.Key);
				}
				if (keyValuePair.Value.Status == FunctionOpenStatus.UnLocking)
				{
					this.UnlockingFunctionList.Add(keyValuePair.Key);
				}
			}
			string text = this.MakeSaveString();
			Utility.PlayerPrefs.SetUserString(LocalDataName.FunctionOpen_LocalCache, text);
		}

		private string MakeSaveString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.UnlockedFunctionList.Count; i++)
			{
				stringBuilder.Append(this.UnlockedFunctionList[i].ToString());
				if (i + 1 < this.UnlockedFunctionList.Count)
				{
					stringBuilder.Append(',');
				}
			}
			stringBuilder.Append('|');
			for (int j = 0; j < this.UnlockingFunctionList.Count; j++)
			{
				stringBuilder.Append(this.UnlockingFunctionList[j].ToString());
				if (j + 1 < this.UnlockingFunctionList.Count)
				{
					stringBuilder.Append(',');
				}
			}
			return stringBuilder.ToString();
		}

		public List<int> UnlockedFunctionList = new List<int>();

		public List<int> UnlockingFunctionList = new List<int>();
	}
}
