using System;
using System.Collections.Generic;

namespace HotFix
{
	public class GuideStyleData
	{
		public void Init(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return;
			}
			this.Args = str.Split(':', StringSplitOptions.None);
			if (this.Args.Length < 1)
			{
				return;
			}
			int num;
			if (int.TryParse(this.Args[0], out num))
			{
				this.StyleKind = (GuideStyleKind)num;
			}
		}

		public string GetArg(int index)
		{
			if (this.Args == null || index < 0 || index >= this.Args.Length)
			{
				return "";
			}
			return this.Args[index];
		}

		public static List<GuideStyleData> CreateStyles(string[] args)
		{
			List<GuideStyleData> list = new List<GuideStyleData>();
			if (args == null || args.Length == 0)
			{
				return list;
			}
			for (int i = 0; i < args.Length; i++)
			{
				if (!string.IsNullOrEmpty(args[i]))
				{
					GuideStyleData guideStyleData = new GuideStyleData();
					guideStyleData.Init(args[i]);
					list.Add(guideStyleData);
				}
			}
			return list;
		}

		public GuideStyleKind StyleKind;

		public string[] Args;
	}
}
