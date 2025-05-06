using System;
using UnityEngine;

namespace HotFix
{
	public class UIGuideStyleBattleTips : UIGuideStyleNormal
	{
		public override UIGuideStyleNormal SetStyle(GuideStyleData styledata, GameObject obj)
		{
			this.Tips = obj.GetComponent<UIGuideTipsCtrl>();
			base.SetStyle(styledata, obj);
			return this;
		}

		protected override void InitArgs()
		{
			string arg = this.StyleData.GetArg(1);
			this.Tips.SetTips(arg);
			this.mStyleOffset = Vector2.zero;
			string arg2 = this.StyleData.GetArg(2);
			string arg3 = this.StyleData.GetArg(3);
			if (!string.IsNullOrEmpty(arg2) || !string.IsNullOrEmpty(arg3))
			{
				if (!float.TryParse(arg2, out this.mStyleOffset.x))
				{
					this.mStyleOffset.x = 0f;
				}
				if (!float.TryParse(arg3, out this.mStyleOffset.y))
				{
					this.mStyleOffset.y = 0f;
				}
			}
			this.CalcOffset();
		}

		public override void OnTargetAreaChange(Rect area)
		{
			Vector2 vector;
			vector..ctor(50f, 50f);
			this.RTFStyle.anchoredPosition = vector + this.mStyleOffset;
		}

		public UIGuideTipsCtrl Tips;
	}
}
