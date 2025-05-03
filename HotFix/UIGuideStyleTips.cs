using System;
using UnityEngine;

namespace HotFix
{
	public class UIGuideStyleTips : UIGuideStyleNormal
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
			base.OnTargetAreaChange(area);
			if (this.RTFStyle == null)
			{
				return;
			}
			string mArrowPosition = this.mArrowPosition;
			if (!(mArrowPosition == "1"))
			{
				if (!(mArrowPosition == "2"))
				{
					if (!(mArrowPosition == "3"))
					{
						if (mArrowPosition == "4")
						{
							this.mStyleEulerAngles = new Vector3(0f, 0f, 270f);
							this.RTFStyle.localEulerAngles = this.mStyleEulerAngles;
						}
					}
					else
					{
						this.mStyleEulerAngles = new Vector3(0f, 0f, 90f);
						this.RTFStyle.localEulerAngles = this.mStyleEulerAngles;
					}
				}
				else
				{
					this.mStyleEulerAngles = new Vector3(0f, 0f, 180f);
					this.RTFStyle.localEulerAngles = this.mStyleEulerAngles;
				}
			}
			else
			{
				this.mStyleEulerAngles = new Vector3(0f, 0f, 0f);
				this.RTFStyle.localEulerAngles = this.mStyleEulerAngles;
			}
			this.RTFStyle.anchoredPosition = area.center + this.mStyleOffset;
		}

		public UIGuideTipsCtrl Tips;
	}
}
