using System;
using Framework.Logic.Component;
using Framework.Logic.UI;

namespace HotFix
{
	public class UIChapterActivityRankScoreItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(string atlas, string icon, string info, int score)
		{
			this.imageIcon.SetImage(atlas, icon);
			this.textInfo.text = info;
			this.textScore.text = string.Format("+{0}", score);
		}

		public CustomImage imageIcon;

		public CustomText textInfo;

		public CustomText textScore;
	}
}
