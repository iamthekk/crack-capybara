using System;

namespace HotFix
{
	public class GuideCondition
	{
		private GuideCondition(GuideConditionKind kind, string[] data)
		{
			this.Kind = kind;
			this.Agvs = data;
		}

		public static GuideCondition Create(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}
			string[] array = str.Split(':', StringSplitOptions.None);
			return new GuideCondition((GuideConditionKind)int.Parse(array[0]), array);
		}

		public bool GetInt(int index, out int ival)
		{
			ival = 0;
			return index >= 0 && this.Agvs != null && index <= this.Agvs.Length && int.TryParse(this.Agvs[index], out ival);
		}

		public GuideConditionKind Kind;

		public string[] Agvs;
	}
}
