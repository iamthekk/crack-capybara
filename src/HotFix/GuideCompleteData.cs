using System;

namespace HotFix
{
	public class GuideCompleteData
	{
		public static GuideCompleteData Create(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return null;
			}
			string[] array = str.Split(':', StringSplitOptions.None);
			if (array.Length < 1)
			{
				return null;
			}
			int num;
			if (!int.TryParse(array[0], out num))
			{
				return null;
			}
			GuideCompleteData guideCompleteData = new GuideCompleteData();
			guideCompleteData.GuideKind = (GuideCompleteKind)num;
			if (array.Length >= 2)
			{
				guideCompleteData.Argstr = array[1];
			}
			else
			{
				guideCompleteData.Argstr = "";
			}
			return guideCompleteData;
		}

		public GuideCompleteKind GuideKind;

		public string Argstr;
	}
}
