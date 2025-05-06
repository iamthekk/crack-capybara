using System;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIRewardTitleInfoNode : UIBoxInfoNode
	{
		public void SetTitleInfo(string title, string info)
		{
			this.textTitle.text = title;
			this.textInfo.text = info;
		}

		public CustomText textTitle;

		public CustomText textInfo;
	}
}
