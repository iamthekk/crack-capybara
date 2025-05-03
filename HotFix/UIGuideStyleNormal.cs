using System;
using UnityEngine;

namespace HotFix
{
	public class UIGuideStyleNormal
	{
		public virtual UIGuideStyleNormal SetStyle(GuideStyleData styledata, GameObject obj)
		{
			this.ObjStyle = obj;
			this.StyleData = styledata;
			this.RTFStyle = obj.transform as RectTransform;
			if (this.StyleData != null)
			{
				this.InitArgs();
			}
			return this;
		}

		protected virtual void InitArgs()
		{
			this.mArrowPosition = this.StyleData.GetArg(1);
			string text = this.mArrowPosition;
			if (!(text == "1"))
			{
				if (!(text == "2"))
				{
					if (!(text == "3"))
					{
						if (text == "4")
						{
							this.mStyleEulerAngles = new Vector3(0f, 0f, 270f);
						}
					}
					else
					{
						this.mStyleEulerAngles = new Vector3(0f, 0f, 90f);
					}
				}
				else
				{
					this.mStyleEulerAngles = new Vector3(0f, 0f, 180f);
				}
			}
			else
			{
				this.mStyleEulerAngles = new Vector3(0f, 0f, 0f);
			}
			this.mStyleOffset = Vector2.zero;
			string arg = this.StyleData.GetArg(2);
			string arg2 = this.StyleData.GetArg(3);
			if (!string.IsNullOrEmpty(arg) || !string.IsNullOrEmpty(arg2))
			{
				if (!float.TryParse(arg, out this.mStyleOffset.x))
				{
					this.mStyleOffset.x = 0f;
				}
				if (!float.TryParse(arg2, out this.mStyleOffset.y))
				{
					this.mStyleOffset.y = 0f;
				}
			}
			this.CalcOffset();
		}

		protected virtual void CalcOffset()
		{
			if (this.mStyleOffset.x != 0f)
			{
				this.mStyleOffset.x = this.mStyleOffset.x / 1080f * (float)Screen.width;
			}
			if (this.mStyleOffset.y != 0f)
			{
				this.mStyleOffset.y = this.mStyleOffset.y / 1920f * (float)Screen.height;
			}
		}

		public virtual void OnTargetAreaChange(Rect area)
		{
			if (this.RTFStyle == null)
			{
				return;
			}
			string text = this.mArrowPosition;
			if (!(text == "1"))
			{
				if (!(text == "2"))
				{
					if (!(text == "3"))
					{
						if (text == "4")
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

		public GameObject ObjStyle;

		public RectTransform RTFStyle;

		public GuideStyleData StyleData;

		protected string mArrowPosition = "1";

		protected Vector3 mStyleEulerAngles;

		protected Vector2 mStyleOffset;
	}
}
